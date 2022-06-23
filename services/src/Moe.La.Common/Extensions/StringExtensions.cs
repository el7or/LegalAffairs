using System;
using System.Globalization;
using System.Linq;

namespace Moe.La.Common.Extensions
{
    public static class StringExtensions
    {
        public static string Shorten(this string str, int numberOfWords)
        {
            if (numberOfWords < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfWords), "numberOfWords should be grater than or equal to zero.");
            }

            if (numberOfWords == 0)
            {
                return "";
            }

            var words = str.Split(' ');

            if (words.Length < numberOfWords)
            {
                return str;
            }

            return string.Join("", words.Take(numberOfWords));
        }

        public static string SharedHeader()
        {
            var Header = @"<div style='padding-top: 30px;text-align:right;padding-bottom:10px;direction:rtl;font-size: 1.6em; font-family:""Traditional Arabic""'>
                            <div style='width:28%;height:7rem;float: right;text-align:center;font-size: 1.3em;'>
                                <div>المملكة العربية السعودية</div>
                                <div>وزارة التعليم</div>
                                <div>الإدارة العامة للشؤون القانونية</div>
                            </div>
                            <div style='width:42%;height:7rem; display: inline-block; text-align:center;'>
                                <img src=""wwwroot/images/logo-icon.png"" />                                       
                            </div>
                            <div style='width:28%;height:7rem;left: 0rem;float: left'>
                                <div><b>التاريخ: </b>" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "م " + DateTimeHelper.GetHigriDateForPrint(DateTime.Now).ToString() + "هـ" + @"</div>
                                <div><b>الوقت:  </b>" + DateTimeHelper.GetTime(DateTime.Now) + @"</div>
                                </div>
                          </div>";

            return Header;
        }

    }
}
