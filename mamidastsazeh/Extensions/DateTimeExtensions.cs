using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToPersianInterval(this DateTime created)
        {
            var interval = DateTime.Now - created;

            if (interval.TotalDays < 1)
            {
                if (interval.TotalHours < 1)
                {
                    if (interval.TotalMinutes < 1)
                    {
                        return "لحظاتی پیش";
                    }
                    else
                    {
                        var minutes = ((int)interval.TotalMinutes).ToString().ToPersianDigits();
                        return $"{minutes} دقیقه پیش";
                    }
                }
                else
                {
                    var hours = ((int)interval.TotalHours).ToString().ToPersianDigits();
                    return $"{hours} ساعت پیش";
                }
            }
            else if (interval.TotalDays < 30)
            {
                var days = ((int)interval.TotalDays).ToString().ToPersianDigits();
                return $"{days} روز پیش";
            }
            else if (interval.TotalDays < 365)
            {
                var months = ((int)(interval.TotalDays / 30)).ToString().ToPersianDigits();
                return $"{months} ماه پیش";
            }
            else
            {
                var years = ((int)(interval.TotalDays / 365)).ToString().ToPersianDigits();
                return $"{years} سال پیش";
            }
        }
    }
}
