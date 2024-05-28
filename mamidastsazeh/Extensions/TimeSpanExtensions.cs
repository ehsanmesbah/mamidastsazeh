using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Extensions
{
    public static class TimeSpanExtensions
    {
        const string TimeSeperator = ":";
        public static string ToDisplayDuration(this TimeSpan ts, bool persianDigit = true)
        {
            var output = "";

            bool hasHours = false;
            if (ts.Hours > 0)
            {
                output += persianDigit ? ts.Hours.ToString().ToPersianDigits() : ts.Hours.ToString();
                output += TimeSeperator;
                hasHours = true;
            }

            var minutes = hasHours ? ts.Minutes.ToString().PadLeft(2, '0') : ts.Minutes.ToString();
            output += persianDigit ? minutes.ToPersianDigits() : minutes;
            output += TimeSeperator;

            var seconds = ts.Seconds.ToString().PadLeft(2, '0');
            output += persianDigit ? seconds.ToPersianDigits() : seconds.ToString();

            return output;
        }
    }
}
