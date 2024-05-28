using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Extensions
{
    public static class StringExtensions
    {
        public static string ToPersianDigits(this string str)
        {
            int helpint;
            Int32.TryParse(str,out helpint);
            string help = String.Format("{0:n0}", helpint);
            return help.Replace("0", "۰")
                      .Replace("1", "۱")
                      .Replace("2", "۲")
                      .Replace("3", "۳")
                      .Replace("4", "۴")
                      .Replace("5", "۵")
                      .Replace("6", "۶")
                      .Replace("7", "۷")
                      .Replace("8", "۸")
                      .Replace("9", "۹");
        }
    }
}
