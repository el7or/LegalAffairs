using System;
using System.Globalization;

namespace Moe.La.Common
{
    public static class DateTimeHelper
    {
        public static string GetArDate(DateTime date)
        {
            return date.Day + " " + GetArMonth(date) + " " + date.Year;
        }


        //////////////////////////////////////////Arabic Date /////////////////////////////////////////     
        public static string GetArDay(object obj)
        {
            string ar_day = "";
            DateTime date = Convert.ToDateTime(obj);

            if (date.DayOfWeek == DayOfWeek.Saturday) ar_day = "السبت";
            else if (date.DayOfWeek == DayOfWeek.Sunday) ar_day = "الأحد";
            else if (date.DayOfWeek == DayOfWeek.Monday) ar_day = "الاثنين";
            else if (date.DayOfWeek == DayOfWeek.Tuesday) ar_day = "الثلاثاء";
            else if (date.DayOfWeek == DayOfWeek.Wednesday) ar_day = "الأربعاء";
            else if (date.DayOfWeek == DayOfWeek.Thursday) ar_day = "الخميس";
            else if (date.DayOfWeek == DayOfWeek.Friday) ar_day = "الجمعة";

            return ar_day;
        }
        public static string GetArMonth(object obj)
        {
            string date_format = "";
            DateTime d = Convert.ToDateTime(obj);

            if (d.Month == 1)
                date_format += "يناير";
            else if (d.Month == 2)
                date_format += "فبراير";
            else if (d.Month == 3)
                date_format += "مارس";
            else if (d.Month == 4)
                date_format += "أبريل";
            else if (d.Month == 5)
                date_format += "مايو";
            else if (d.Month == 6)
                date_format += "يونيو";
            else if (d.Month == 7)
                date_format += "يوليو";
            else if (d.Month == 8)
                date_format += "أغسطس";
            else if (d.Month == 9)
                date_format += "سبتمبر";
            else if (d.Month == 10)
                date_format += "أكتوبر";
            else if (d.Month == 11)
                date_format += "نوفمبر";
            else if (d.Month == 12)
                date_format += "ديسمبر";

            return date_format;
        }
        ///////
        public static string GetArTime(object obj)
        {
            try
            {
                if (obj != null)
                {
                    if (obj.ToString().Contains("ص")
                        || obj.ToString().Contains("م"))
                        return obj + "";

                    return Convert.ToDateTime(obj).ToString("h:mm tt").Replace("AM", "ص").Replace("PM", "م");
                }
                return "";
            }
            catch
            {
                return obj + "";
            }
        }
        public static string GetTime(object obj)
        {
            return Convert.ToDateTime(obj).ToString("hh:mm tt").Replace("AM", "صباحا").Replace("PM", "مساء");
        }

        public static string GetHigriDate(this DateTime? dateTime)
        {
            try
            {
                if (dateTime == null)
                    return "---------";

                DateTimeFormatInfo HijriDTFI = new CultureInfo("ar-Sa", false).DateTimeFormat;
                HijriDTFI.Calendar = new UmAlQuraCalendar();
                HijriDTFI.ShortDatePattern = "dd/MM/yyyy";

                return dateTime.Value.Date.ToString("yyyy-MM-dd", HijriDTFI);
            }
            catch
            {
                return "";
            }
        }

        public static string GetHigriDateForPrint(this DateTime? dateTime)
        {
            if (dateTime == null)
                return "---------";

            DateTimeFormatInfo HijriDTFI = new CultureInfo("ar-Sa", false).DateTimeFormat;
            HijriDTFI.Calendar = new UmAlQuraCalendar();
            HijriDTFI.ShortDatePattern = "dd/MM/yyyy";

            return dateTime.Value.Date.ToString("dd-MM-yyyy", HijriDTFI);
        }
        public static int GetHigriDateInt(this DateTime? dateTime)
        {
            if (dateTime == null)
                return 00000000;

            DateTimeFormatInfo HijriDTFI = new CultureInfo("ar-Sa", false).DateTimeFormat;
            HijriDTFI.Calendar = new UmAlQuraCalendar();
            HijriDTFI.ShortDatePattern = "dd/MM/yyyy";

            var date = dateTime.Value.Date.ToString("dd-mm-yyyy", HijriDTFI);
            String[] values = date.Split("-");
            int day = int.Parse(values[0]);
            int month = int.Parse(values[1]);
            int year = int.Parse(values[2]);

            return day + month + year;
        }
        public static int GetHigriYearInt(this DateTime? dateTime)
        {
            if (dateTime == null)
                return 00000000;

            DateTimeFormatInfo HijriDTFI = new CultureInfo("ar-Sa", false).DateTimeFormat;
            HijriDTFI.Calendar = new UmAlQuraCalendar();
            HijriDTFI.ShortDatePattern = "dd/MM/yyyy";

            var date = dateTime.Value.Date.ToString("dd-mm-yyyy", HijriDTFI);
            string[] values = date.Split("-");
            int year = int.Parse(values[2]);

            return year;
        }

        public static string GetEnDate(object obj)
        {
            if (obj is DBNull) return "غير محدد";
            DateTime date = Convert.ToDateTime(obj);
            return date.Day + "/" + date.Month.ToString("00") + "/" + date.Year;
        }

        public static string GetEnDate_2(object obj)
        {
            if (obj is DBNull) return "غير محدد";
            DateTime date = Convert.ToDateTime(obj);
            return date.Day + " " + GetArMonth(obj) + " " + date.Year;
        }

        public static string GetEnDay(object obj)
        {
            string date_format = "";
            DateTime d = Convert.ToDateTime(obj);
            date_format = d.DayOfWeek.ToString();
            return date_format;
        }
        public static string GetEnMonth(object obj)
        {
            string date_format = "";
            DateTime d = Convert.ToDateTime(obj);
            date_format = d.Month.ToString();
            return date_format;
        }
        public static string GetEnTime(object obj)
        {
            return (Convert.ToDateTime(obj).ToString("hh:mm") + " Mecca");
        }
        public static string GetFullTime(object obj)
        {
            return (Convert.ToDateTime(obj).ToString("h:mm tt", new CultureInfo("ar-SA")));
        }
        public static DateTime ToRiyadhTimeZone(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Arab Standard Time");
        }

        public static DateTime FromHijriToGoergian(string higriDate)
        {
            string[] allFormats ={"yyyy/MM/dd","yyyy/M/d",
                    "dd/MM/yyyy","d/M/yyyy",
                    "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
                    "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
                    "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
                    "yyyy M d","dd MM yyyy","d M yyyy",
                    "dd M yyyy","d MM yyyy"};


            return DateTime.ParseExact(higriDate, allFormats, new CultureInfo("ar-SA").DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
        }
    }
}
