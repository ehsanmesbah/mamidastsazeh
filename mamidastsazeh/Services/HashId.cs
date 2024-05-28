using mamidastsazeh.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HashidsNet;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace mamidastsazeh.Services
{
    public class HashId : IHashId
    {
        private readonly Hashids _hashId;
        public HashId(IConfiguration configuration)
        {
            var config = configuration.GetSection("HashId");
            var salt = config.GetValue<string>("Salt");
            var minlength = config.GetValue<int>("MinLength");
            _hashId = new Hashids(salt, minlength);

        }
        public int Decode(string hashId)
        {
         
            var val = _hashId.Decode(hashId);
            if (val.Length > 0)
            {
                return val[0];
            }
            else return 0;

        }

        public string Encode(int id)
        {
           
            return _hashId.Encode(id);
        }
        public long DecodeLong(string hashId)
        {

            var val = _hashId.DecodeLong(hashId);
            if (val.Length > 0)
            {
                return val[0];
            }
            else return 0;

        }

        public string EncodeLong(long id)
        {

            return _hashId.EncodeLong(id);
        }
    }
}