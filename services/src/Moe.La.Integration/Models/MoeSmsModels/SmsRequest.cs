namespace Moe.La.Integration.Models
{
    public class SmsRequest
    {
        public string Sender { get; set; }
        public string Text { get; set; }
        public bool IsArabic { get; set; }
        public bool CheckActivation { get; set; }
        public SMSToList ToList { get; set; }
    }
}
