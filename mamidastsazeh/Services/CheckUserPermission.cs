using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mamidastsazeh.Abstractions;

namespace mamidastsazeh.Services
{
    public class CheckUserPermission:ICheckUserPermission
    {
        public CheckUserPermission()
        {

        }
        public bool AllowedHtml(string username)
        {
            if(username=="09124078476") return true;
            return false;
        }
        public bool AllowFullVideo(string username)
        {
            if (username == "09124078476") return true;
            return false;
        }
    }
}
