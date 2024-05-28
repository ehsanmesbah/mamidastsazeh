using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Abstractions;
using System.Security.Policy;
using RestSharp;
using Newtonsoft.Json;
using mamidastsazeh.ViewModels;
using mamidastsazeh.Models;

namespace mamidastsazeh.Controllers
{
    public class TestController :Controller
    {
        private readonly IMainCategoryRepository _maincategories;
        private readonly IsmsTokenRepository _smsToken;
        private readonly IsmsRepository _smss;
        private readonly IPostRepository _posts;
        public TestController(IPostRepository posts,IMainCategoryRepository mainCategory,IsmsTokenRepository smsToken,IsmsRepository smss)
        {
            _maincategories=mainCategory;
            _smsToken = smsToken;
            _smss = smss;
            _posts = posts;
        }
        public IActionResult Index(int page = 1,int limit=10)
        {
            var posts = _posts.Posts.Where(p=>p.ID < 200);
            PostsPaginationViewModel model = new PostsPaginationViewModel()
            {
                Limit = limit,
                Page = page,
                Posts = posts
            };
            return View(model);
        }
        public IActionResult SendSms()
        {

            var client = new RestClient("https://RestfulSms.com/api/UltraFastSend");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-sms-ir-secure-token", "aHg3aUQxeHJNZ2lwNWo1SUpndG1Hd2x3M2JRUzZSMXVReFo2K0dVMHVUeUdMVFRQbFpzL0tpbXBDUjlJWnd3VFdhRWhTb0pRU09VK0Z0eWo3TllMd2V2WGwrWUh6VHU4RFU5cVVLaThTRFdHUmNIeGxtZXVUMVhlSEdIaE1yM0NRM0VlbEszSmhHY3haQ1hReWZ5T3V2dSt0Y2xRTE1vY1Vsa1FhM0oyd2ZjQS9mTU9FOUloZzZKVkd2aEJ4YUVZ");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n \"ParameterArray\":[\r\n{ \"Parameter\": \"VerificationCode\",\"ParameterValue\": \"33333\"}\r\n],\r\n\"Mobile\":\"09123104698\",\r\n\"TemplateId\":\"34738\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var decodeResponse = JsonConvert.DeserializeObject<SendSmsApiResult>(response.Content);
            var sms = new Sms
            {

                IsSuccessful = decodeResponse.IsSuccessful,
                SendTime = DateTime.Now,
                ServerMessage = decodeResponse.Message,
                Validate = false,
                ValidateTime = DateTime.Now,
                VerificationCodeId = decodeResponse.VerificationCodeId
            };
            _smss.Add(sms);
            _smss.SaveChanges();

            return View();
            
        }
    }
}
