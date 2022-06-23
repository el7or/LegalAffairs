namespace Moe.La.Core.Entities
{
    public class AppSettings
    {
        public string SystemName { get; set; }

        public string AdminEmail { get; set; }

        public string AdminMobile { get; set; }

        public string MobilyUserName { get; set; }

        public string MobilyPassword { get; set; }

        public string MobilySender { get; set; }

        public int EmailType { get; set; }

        public string EmailAccount { get; set; }

        public string EmailPassword { get; set; }

        public string SMTP_Attributes { get; set; }

        public string IsLawFirmOffice { get; set; }

        public string AgainstCasesRequiresRating { get; set; }
    }
}
