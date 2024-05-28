using mamidastsazeh.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HashidsNet;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Security.Policy;
using RestSharp;
using Newtonsoft.Json;
using mamidastsazeh.ViewModels;
using mamidastsazeh.Models;


namespace mamidastsazeh.Services
{
    public class SmsManager: ISmsManager
    {
        public readonly IsmsRepository _smss;
        public readonly IsmsTokenRepository _smsToken;
        private readonly IConfiguration _configuration;

        public SmsManager(IsmsRepository smss,IsmsTokenRepository smstoken, IConfiguration configuration)
        {
            _smss = smss;
            _smsToken = smstoken;
            _configuration = configuration;

        }
        public string SendSms(string MobileNumber,string Ip,string Type)
        {
            try
            {
               // string token = "success";
                var lasttoken = _smsToken.SmsTokens.Where(s => s.GetDate > DateTime.Now.AddMinutes(-20)).OrderByDescending(s => s.GetDate).FirstOrDefault();
                string token;
                if (lasttoken == null)
                {

                    token = GetToken();
                }
                else token = lasttoken.TokenKey;

                if (token != "fail")
                {
                    Random r = new Random();
                    int code = r.Next(11001, 99997);
                    Sms sms = new Sms
                    {
                        MobileNumber = MobileNumber,
                        Type = Type,
                        SendTime = DateTime.Now,
                        Code = code.ToString(),
                        Ip = Ip
                       
                    };

                    _smss.Add(sms);
                    _smss.SaveChanges();
                    var client = new RestClient("https://RestfulSms.com/api/UltraFastSend");
                    client.Timeout = -1;
                   
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("x-sms-ir-secure-token", token);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", "{\r\n \"ParameterArray\":[\r\n{ \"Parameter\": \"VerificationCode\",\"ParameterValue\": \"" + code.ToString() + "\"}\r\n],\r\n\"Mobile\":\"" + MobileNumber + "\",\r\n\"TemplateId\":\"34738\"\r\n}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    var decodeResponse = JsonConvert.DeserializeObject<SendSmsApiResult>(response.Content);
                    
                    sms.IsSuccessful = decodeResponse.IsSuccessful;
                    sms.ServerMessage = decodeResponse.Message;
                    sms.VerificationCodeId = decodeResponse.VerificationCodeId;
                    _smss.SaveChanges();
                    return "success";
                }
                return "fail";
            }
            catch (Exception e)
            {
                return "fail";
            }

        }
        private string GetToken()
        {
            var client = new RestClient("https://RestfulSms.com/api/Token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n\t\"UserApiKey\":\""+ _configuration["SKey:Api"] + "\",\r\n\t\"SecretKey\":\""+ _configuration["SKey:Scode"] + "\"\r\n}\r\n", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var decodeResponse = JsonConvert.DeserializeObject<ApiResult>(response.Content);
            if (decodeResponse != null && decodeResponse.Message != null)
            {
                SmsToken token = new SmsToken
                {
                    Message = decodeResponse.Message,
                    GetDate = DateTime.Now,
                    IsSuccessful = decodeResponse.IsSuccessful,
                    TokenKey = decodeResponse.TokenKey
                };
                _smsToken.Add(token);
                _smsToken.SaveChanges();
                return decodeResponse.TokenKey;
            }
            else
            {
                SmsToken token = new SmsToken
                {
                    Message = "عدم اتصال به سرور",
                    GetDate = DateTime.Now,
                    IsSuccessful = false,

                };
                _smsToken.Add(token);
                _smsToken.SaveChanges();
                return "fail";
            }
        }

      
    }
}