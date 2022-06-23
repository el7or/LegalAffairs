namespace Moe.La.Integration.Models
{
    public class EmailResponse
    {
        public string ResponseId { get; set; }
        public string HasWrongAddresses { get; set; }
        public WrongEmailAddresses WrongEmailAddresses { get; set; }
    }
}
