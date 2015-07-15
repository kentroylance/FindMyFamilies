using System;
using System.Globalization;

namespace FindMyFamilies.Util {
    /// <summary>
    /// Summary description for DateHelper.
    /// </summary>
    public class DateHelper {
        public DateHelper() {
        }

        public bool IsDate(string dateString) {
            DateTime dateTime;
            bool valid = true;
            try {
                dateTime = DateTime.Parse(dateString);
            }
            catch (FormatException e) {
                // the Parse method failed => the string strDate cannot be converted to a date.
                valid = false;
            }
            return valid;
        }


        public int GetWeekdayMonthStartsOn(int month, int year) {
            DateTime dateTime = new DateTime(year, month, 1);
            return Convert.ToInt16(dateTime.DayOfWeek);
            //          return Microsoft.VisualBasic.DateAndTime.WeekDay(DateTime.Parse(month + "/1/" + year));
        }

        public string GetMonthName(int theMonth) {
            DateTimeFormatInfo info = new DateTimeFormatInfo();
            string month = info.MonthNames[theMonth - 1];
            return month;
        }

        public int GetDaysInMonth(int month, int year) {
            switch(month) {         
                case 4:
                    return 30;
                case 6:
                    return 30;
                case 9:
                    return 30;
                case 11:
                    return 30;
                case 2:
                    if (IsDate("February 29, " + Convert.ToString(year))) 
                        return 29;
                    else
                        return 28;
                default:
                    return 31;
            }
        }

        public DateTime AddOneMonth(DateTime dateTime) {
            int day;
            int month;
            int year;
            day = dateTime.Day;
            month = dateTime.Month;
            year = dateTime.Year;
            if (month == 12) {
                month = 1;
                year = year + 1;
            } else
                month = month + 1;

            if (day > DateTime.DaysInMonth(year, month))
                day = DateTime.DaysInMonth(year, month);

            return new DateTime(year, month, day);
        }

        public DateTime SubtractOneMonth(DateTime dateTime) {
            int day;
            int month;
            int year;
            day = dateTime.Day;
            month = dateTime.Month;
            year = dateTime.Year;
            if (month == 1) {
                month = 12;
                year = year - 1;
            } else
                month = month - 1;

            if (day > DateTime.DaysInMonth(year, month))
                day = DateTime.DaysInMonth(year, month);

            return new DateTime(year, month, day);
        }
    }
}