namespace Moe.La.Integration.Models
{
    public class EmailRequest
    {
        public string FromAddress { get; set; }
        public ToAddresses ToAddresses { get; set; }
        public CCAddresses CCAddresses { get; set; }
        public BCCAddresses BCCAddresses { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailRequestAttachments Attachments { get; set; }
        public EmailRequestEmbeddedImages EmbeddedImages { get; set; }
    }
}
