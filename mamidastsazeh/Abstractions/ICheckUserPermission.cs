using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Abstractions
{
    public interface ICheckUserPermission
    {
        bool AllowedHtml(string username);
        bool AllowFullVideo(string username);

    }
}
