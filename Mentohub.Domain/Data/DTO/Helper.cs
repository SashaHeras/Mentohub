using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO
{
    public static class Helper
    {
        public static string GetTimeSinceDate(DateTime date)
        {
            DateTime currentDate = DateTime.Now;
            TimeSpan timeDifference = currentDate - date;

            int years = currentDate.Year - date.Year;
            int months = currentDate.Month - date.Month;
            int days = currentDate.Day - date.Day;
            int hours = currentDate.Hour - date.Hour;

            if (years > 0)
            {
                return $"{years} year{(years > 1 ? "s" : "")} ago";
            }
            else if (months > 0)
            {
                return $"{months} month{(months > 1 ? "s" : "")} ago";
            }
            else if (days > 0)
            {
                return $"{days} day{(days > 1 ? "s" : "")} ago";
            }
            else
            {
                if (hours == 0)
                {
                    return $"right now";
                }
                return $"{hours} hour{(hours > 1 ? "s" : "")} ago";
            }
        }
    }
}
