using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.ViewModels
{
    public class UserRolesEditViewModel
    {
        public IEnumerable<string> UserInRoles { get; set; }
        public IEnumerable<string> UserOutRoles { get; set; }
    }
}
