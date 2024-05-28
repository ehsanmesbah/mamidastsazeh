using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Abstractions
{
    public interface ISmsManager
    {
        string SendSms(string MobileNumber,string Ip,string Type);

    }
}
