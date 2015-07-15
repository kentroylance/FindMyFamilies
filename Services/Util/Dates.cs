using System;
using System.Globalization;

namespace FindMyFamilies.Util {
    /// <summary>
    /// This class contains all the functions that allow us to work with dates, time, datetime, character dates etc. 
    /// Class contains methods that return Date(), DateTime(), Time(), Day(), Month(), Year(), CDOW(), Quarter(),
    /// Sec(), CMonth() etc. It also contains functions such as MDY(), DMY() and GoMonth()
    /// </summary>
    public class Dates {
        public static DateTime? GetDateTime(string dateTime) {
            try {
                DateTime dateTimeResult;
                DateTime.TryParseExact(dateTime, new[] {"dd MMM yyyy", "dd MMMM yyyy", "d MMMM yyyy", "d MMM yyyy", "yyyyMMdd", "MMMM dd, yyyy", "MMM dd, yyyy", "yyyy"}, null, DateTimeStyles.None, out dateTimeResult);
                return dateTimeResult;
            } catch (Exception) {
            }
            return null;
        }

        public bool IsDate(string dateString) {
            DateTime dateTime;
            bool valid = true;
            try {
                dateTime = DateTime.Parse(dateString);
            } catch (FormatException e) {
                // the Parse method failed => the string strDate cannot be converted to a date.
                valid = false;
            }
            return valid;
        }

        public static bool IsEmpty(DateTime dateTime) {
            bool isEmpty = false;

            if (!dateTime.Equals(null)) {
                if (dateTime.ToString(Constants.DATE_FORMAT).Equals(Constants.EMPTY_DATE)) {
                    isEmpty = true;
                }
            }
            return isEmpty;
        }

        public int GetWeekdayMonthStartsOn(int month, int year) {
            var dateTime = new DateTime(year, month, 1);
            return Convert.ToInt16(dateTime.DayOfWeek);
            //          return Microsoft.VisualBasic.DateAndTime.WeekDay(DateTime.Parse(month + "/1/" + year));
        }

        public string GetMonthName(int theMonth) {
            var info = new DateTimeFormatInfo();
            string month = info.MonthNames[theMonth - 1];
            return month;
        }

        public int GetDaysInMonth(int month, int year) {
            switch (month) {
                case 4:
                    return 30;
                case 6:
                    return 30;
                case 9:
                    return 30;
                case 11:
                    return 30;
                case 2:
                    if (IsDate("February 29, " + Convert.ToString(year))) {
                        return 29;
                    } else {
                        return 28;
                    }
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
            } else {
                month = month + 1;
            }

            if (day > DateTime.DaysInMonth(year, month)) {
                day = DateTime.DaysInMonth(year, month);
            }

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
            } else {
                month = month - 1;
            }

            if (day > DateTime.DaysInMonth(year, month)) {
                day = DateTime.DaysInMonth(year, month);
            }

            return new DateTime(year, month, day);
        }

        public string getDateSpelledOut(DateTime date) {
            string dateSpelledOut = null;

            dateSpelledOut = date.ToString("MMMMM") + " " + date.Day + ", " + date.Year;

            return dateSpelledOut;
        }

        /// <summary>
        /// Receives a date in string format as a parameter and converts it to a DateTime format
        /// <pre>
        /// Example:
        /// string lcDate = "4/12/01";
        /// DateTime MyDate = DateProblems.CTOT(lcDate);   //converts the string to a DateTime value
        /// </pre>
        /// </summary>
        /// <returns></returns>
        public static DateTime ConvertDateNameToDate(string dateTime) {
            return DateTime.Parse(dateTime);
        }

        /// <summary>
        /// Receives a date in string format as a parameter and returns the current day of week as a string
        /// <pre>
        /// Example:
        /// DateTime tDateTime = DateTime.Now;
        /// string lcCurrentDay = DateProblems.CDOW(tDateTime);    //returns "Wednesday"
        /// </pre>
        /// </summary>
        /// <param name="dDate"></param>
        /// <returns></returns>
        public static string DayOfWeek(DateTime date) {
            return date.DayOfWeek.ToString();
        }

        /// <summary>
        /// Receives a DateTime as a parameter and returns the current month formatted as a string
        /// <pre>
        /// Example:
        /// DateTime tDateTime = DateTime.Now;
        /// string lcDate = DateProblems.CMonth(tDateTime);    //returns "May"
        /// </pre>
        /// </summary>
        /// <param name="dDate"></param>
        /// <returns></returns>
        public static string MonthName(DateTime date) {
            return date.ToString("MMMM");
        }

        /// <summary>
        /// Receives a date in string format as a parameter and converts it to a DateTime format
        /// <pre>
        /// Example:
        /// string lcDate = "4/12/01";
        /// DateTime MyDate = DateProblems.CTOT(lcDate);   //converts the string to a DateTime value
        /// </pre>
        /// </summary>
        /// <param name="cDate"></param>
        /// <returns></returns>
        public static DateTime ConvertToDate(string date) {
            return DateTime.Parse(date);
        }

        /// <summary>
        /// Returns the current Date in System.DateTime format. (Use System.DateTime.Today instead)
        /// </summary>
        /// <returns></returns>
        public static DateTime ToDaysDate() {
            return DateTime.Today;
        }

        /// <summary>
        /// Returns the current DateTime in System.DateTime format. (Use System.DateTime.Now instead)
        /// </summary>
        /// <returns></returns>
        public static DateTime TodaysDateTime() {
            return DateTime.Now;
        }

        /// <summary>
        /// Returns the current Day from a DateTime (Use MyDate.Day instead)
        /// </summary>
        /// <returns></returns>
        public static int Day(DateTime dDate) {
            return dDate.Day;
        }

        /// <summary>
        /// Receives a DateTime as a parameter and returns a string formatted as a DMY
        /// <pre>
        /// Example:
        /// DateTime tDateTime = DateTime.Now;
        /// string cDate = DateProblems.CMonth(tDateTime); //returns "09 May 01"
        /// </pre>
        /// </summary>
        /// <param name="dDate"></param>
        /// <returns></returns>
        public static string DMY(DateTime dDate) {
            return dDate.ToString("dd MMM yy");
        }

        /// <summary>
        /// Receives a Date as a parameter and returns a string of that date. (Use MyDate.ToShortDateString() instead)
        /// </summary>
        /// <param name="dDate"></param>
        /// <returns></returns>
        public static string DTOC(DateTime dDate) {
            return dDate.ToShortDateString();
        }

        /// <summary>
        /// Receives a DateTime as a parameter and returns a DTOS formatted string
        /// </summary>
        /// <example>
        /// Example:
        /// DateTime tDateTime = DateTime.Now;
        /// string cDate = DateProblems.DTOS(tDateTime);   //returns "20010509"
        /// </example>
        /// <param name="dDate"></param>
        /// <returns></returns>
        public static string DTOS(DateTime dDate) {
            return dDate.ToString("yyyyMMdd");
        }

        /// <summary>
        /// This is simply a placeholder. VFP had Date and DateTime as separate datatypes. Now there
        /// is no difference here as there is only a single datatype; DateTime.
        /// </summary>
        /// <returns></returns>
        public static DateTime DTOT(DateTime tDateTime) {
            //The date is the same as the datetime. Return the same value back :)
            return tDateTime;
        }

        /// <summary>
        /// Receives a date as a parameter and returns an integer specifying the day of the week for that date
        /// <pre>
        /// Example:
        /// DateTime tDateTime = DateTime.Now;
        /// int nDow = DateProblems.DOW(tDateTime);
        /// </pre>
        /// </summary>
        /// <param name="dDate"></param>
        /// <returns></returns>
        public static int DOW(DateTime dDate) {
            return (int) dDate.DayOfWeek;
        }

        /// <summary>
        /// Receives a date and number of months as parameters and adds that many months to the date and returns the new date.
        /// <pre>
        /// Example:
        /// DateTime myNewDate;
        /// myNewDate = DateProblems.GoMonth(DateTime.Now, 2);     //returns a date after adding 2 months
        /// </pre>
        /// </summary>
        /// <returns></returns>
        public static DateTime GoMonth(DateTime dDate, int nMonths) {
            return dDate.AddMonths(nMonths);
        }

        /// <summary>
        /// Returns the current Hour from a DateTime (Use MyDate.Hour instead)
        /// </summary>
        /// <returns></returns>
        public static int Hour(DateTime dDate) {
            return dDate.Hour;
        }

        /// <summary>
        /// Receives a DateTime as a parameter and returns a formatted string in MDY format
        /// <pre>
        /// Example:
        /// DateTime tDateTime = DateTime.Now;
        /// string cDate = DateProblems.MDY(tDateTime);    //returns "May 09 2001"
        /// </pre>
        /// </summary>
        /// <param name="dDate"></param>
        /// <returns></returns>
        public static string MDY(DateTime dDate) {
            return dDate.ToString("MMMM dd yyyy");
        }

        /// <summary>
        /// Returns the current Minute from a DateTime (Use MyDate.Minute instead)
        /// </summary>
        /// <returns></returns>
        public static int Minute(DateTime dDate) {
            return dDate.Minute;
        }

        /// <summary>
        /// Returns the current Month from a DateTime (Use MyDate.Month instead)
        /// </summary>
        /// <returns></returns>
        public static int Month(DateTime dDate) {
            return dDate.Month;
        }

        /// <summary>
        /// Receives a date time as a parameter and returns an integer with the quarter the date
        /// falls in
        /// <pre>
        /// Example:
        /// int nCurrentQuarter = DateProblems.Quarter(DateTime.Now);      //returns 2
        /// </pre>
        /// </summary>
        /// <param name="dDate"></param>
        /// <returns></returns>
        public static int Quarter(DateTime dDate) {
            //Get the current month
            int i = dDate.Month;

            //Based on the current month return the quarter
            if (i <= 3) {
                return 1;
            } else if (i >= 4 && i <= 6) {
                return 2;
            } else if (i >= 7 && i <= 9) {
                return 3;
            } else if (i >= 10 && i <= 12) {
                return 4;
            } else {
                //Something probably is wrong 
                return 0;
            }
        }

        /// <summary>
        /// Returns the current second from a DateTime (Use MyDate.Second instead)
        /// </summary>
        /// <returns></returns>
        public static int Sec(DateTime dDate) {
            return dDate.Second;
        }

        /// <summary>
        /// Returns the number of seconds since midnight
        /// <example>
        /// Example:
        /// double nTotalSeconds = DateProblems.Seconds();
        /// </example>
        /// </summary>
        /// <returns></returns>
        public static double Seconds() {
            //Create the timespan object get the time between the dates
            TimeSpan st = DateTime.Now.Subtract(DateTime.Today);

            //Return the number of seconds
            return st.Duration().TotalMilliseconds / 1000;
        }

        /// <summary>
        /// Returns the current time in string format from a DateTime.
        /// <pre>
        /// Example:
        /// string MyTime = DateProblems.Time();   //returns "2:33 AM"
        /// </pre>
        /// </summary>
        /// <returns></returns>
        public static string Time() {
            return DateTime.Now.ToShortTimeString();
        }

        /// <summary>
        /// Receives a Date as a parameter and returns a string of that date and time. 
        /// (Use MyDate.ToShortDateString() and MyDate.ToShortTimeString() instead)
        /// </summary>
        /// <param name="dDate"></param>
        /// <returns></returns>
        public static string TTOC(DateTime dDate) {
            return String.Concat(dDate.ToShortDateString(), " ", dDate.ToShortTimeString());
        }

        /// <summary>
        /// Converts a DateTime expression to a short date string
        /// <pre>
        /// Example:
        /// DateTime tDateTime = DateTime.Now;
        /// string cDate = DateProblems.TTOD(tDateTime);
        /// </pre>
        /// </summary>
        /// <param name="tDateTime"></param>
        /// <returns></returns>
        public static string TTOD(DateTime tDateTime) {
            //Call tDateTime.ToShortDateString() which is a string to get this value
            return tDateTime.ToShortDateString();
        }

        /// <summary>
        /// Returns the current Year from a DateTime (Use MyDate.Year instead)
        /// </summary>
        /// <returns></returns>
        public static int Year(DateTime dDate) {
            return dDate.Year;
        }

        /// <summary>
        /// Receives a DateTime as a parameter and returns the week of the year
        /// </summary>
        /// <example>
        /// int nCurrentWeek = DateProblems.Week(DateTime.Now);
        /// </example>
        public static int Week() {
            var d = new DateTimeFormatInfo();

            //Receives the DateTime, Rule to start first day and first starting day (Mon, tue etc)
            return d.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, System.DayOfWeek.Monday);
        }

        ///<summary>
        ///</summary>
        //End of DateProblems class
    }
}