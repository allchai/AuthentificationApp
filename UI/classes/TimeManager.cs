using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.classes
{
    public enum DayPart
    {
        Morning, Day, Evening, Night
    }

    internal class TimeManager
    {
        public static DayPart GetDayPart()
        {
            int current = DateTime.Now.Hour;

            if (current > 10 && current < 12)
                return DayPart.Morning;
            else if (current >= 12 && current < 17)
                return DayPart.Day;
            else if (current >= 17 && current < 19)
                return DayPart.Evening;
            else
                return DayPart.Night;
        }

        public static bool IsWorkHour()
        {
            int current = DateTime.Now.Hour;

            if (current >= 10 && current <= 19)
                return true;
            else
                return false;
        }
    }
}
