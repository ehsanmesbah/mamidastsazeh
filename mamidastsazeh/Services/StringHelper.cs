using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Services
{
    public class StringHelper
    {
        public StringHelper()
        {

        }
        public int Check(){
            return 0;
        }
        public string Refresh(string text)
        {
            text = text.Trim();
            text = text.Replace(">", "").Replace("<", "");
            text = text.Replace('ك', 'ک').Replace('ي', 'ی');
            return text;
        }
    }
}
