using Microsoft.AspNetCore.Http;
using Moe.La.Core.Entities;
using Moe.La.Core.Integration;
using System;
using System.Net;
using System.Net.Mail;

namespace Moe.La.Integration
{
    public enum ContactEmailType
    {
        Gmail = 1,
        Hotmail = 2,
        Other = 3
    }

    public class EmailNotification : IEmailNotification
    {
        private readonly string displayName;
        private readonly int emailType;
        private readonly string from;
        private readonly string password;
        private readonly string smtpAttributes;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EmailNotification(AppSettings appSettings)
        {
            displayName = appSettings.SystemName;
            emailType = appSettings.EmailType;
            from = appSettings.EmailAccount;
            password = appSettings.EmailPassword;
            smtpAttributes = appSettings.SMTP_Attributes;
        }

        public EmailNotification(AppSettings appSettings, IHttpContextAccessor httpContextAccessor)
            : this(appSettings)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public bool Send(string to, string subject, string message)
        {
            return Send(new string[] { to }, null, subject, message, null);
        }

        public bool Send(string[] to, string subject, string message)
        {
            return Send(to, null, subject, message, null);
        }

        public bool Send(string[] to, string subject, string message, IFormFile fuAttachment)
        {
            return Send(to, null, subject, message, fuAttachment);
        }

        public bool Send(string[] to, string displayName, string subject, string message)
        {
            return Send(to, null, displayName, subject, message, null);
        }

        public bool Send(string[] to, string[] bcc, string subject, string message)
        {
            return Send(to, bcc, subject, message, null);
        }

        public bool Send(string[] to, string[] bcc, string displayName, string subject, string message, string from, string password)
        {
            return Send(to, bcc, displayName, subject, message, null);
        }

        public bool Send(string[] to, string[] bcc, string subject, string message, IFormFile fuAttachment)
        {
            return Send(to, bcc, displayName, subject, message, fuAttachment);
        }

        public bool Send(string[] to, string[] bcc, string displayName, string subject, string message, IFormFile fuAttachment)
        {
            string smtp_host = "mail.smart-fingers.sa";
            int smtp_port = 25;
            bool enable_ssl = false;

            if (emailType == (int)ContactEmailType.Gmail)
            {
                smtp_host = "smtp.gmail.com";
                smtp_port = 587;
                enable_ssl = true;
            }
            else if (emailType == (int)ContactEmailType.Hotmail)
            {
                smtp_host = "smtp.live.com";
                smtp_port = 25;
                enable_ssl = true;
            }
            else if (emailType == (int)ContactEmailType.Other)
            {
                if (!String.IsNullOrEmpty(smtpAttributes))
                {
                    string[] Attributes = smtpAttributes.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (Attributes.Length == 3)
                    {
                        smtp_host = Attributes[0];
                        smtp_port = Convert.ToInt32(Attributes[1]);
                        enable_ssl = Convert.ToBoolean(Attributes[2]);
                    }
                    return false;
                }
                return false;
            }

            return Send(to, bcc, displayName, subject, message, from, password, smtp_host, smtp_port, enable_ssl, fuAttachment);
        }

        public bool Send(string[] to, string[] bcc, string displayName, string subject, string message, string from, string password, string smtp_host, int smtp_port, bool enable_ssl, IFormFile fuAttachment)
        {
            try
            {
                SmtpClient serv = new SmtpClient();
                MailMessage msgMail = new MailMessage();
                MailAddress fromAddress = new MailAddress(from, displayName);

                msgMail.Subject = subject;
                if (to != null)
                {
                    for (int i = 0; i < to.Length; i++)
                    {
                        msgMail.To.Add(to[i]);
                    }
                }
                if (bcc != null)
                {
                    for (int i = 0; i < bcc.Length; i++)
                    {
                        msgMail.Bcc.Add(bcc[i]);

                    }
                }

                msgMail.From = fromAddress;

                msgMail.Body = message;
                if (fuAttachment != null)
                {
                    //if (fuAttachment.HasFile)
                    //{
                    //    for (int i = 0; i < fuAttachment.PostedFiles.Count; i++)
                    //    {
                    //        string FileName = Path.GetFileName(fuAttachment.PostedFiles[i].FileName);
                    //        msgMail.Attachments.Add(new System.Net.Mail.Attachment(fuAttachment.PostedFiles[i].InputStream, FileName));
                    //    }
                    //}
                }

                msgMail.BodyEncoding = System.Text.Encoding.UTF8;
                msgMail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtp_host;
                smtp.Port = smtp_port;
                smtp.EnableSsl = enable_ssl;
                ////
                NetworkCredential NetworkCred = new NetworkCredential(from, password);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Send(msgMail);

                return true;

            }
            catch (Exception)
            {
                //Utilities.LogError(ex); // To check the error when the email does not sending
                return false;
            }
        }

        public bool SendEmailWithFile(string to, string subject, string message, string path)
        {
            string smtp_host = "smtp.gmail.com"; // "mail.smart-fingers.sa";
            int smtp_port = 587; // 25;
            bool enable_ssl = true; // false;

            if (emailType == (int)ContactEmailType.Gmail)
            {
                smtp_host = "smtp.gmail.com";
                smtp_port = 587;
                enable_ssl = true;
            }
            else if (emailType == (int)ContactEmailType.Hotmail)
            {
                smtp_host = "smtp.live.com";
                smtp_port = 25;
                enable_ssl = true;
            }
            else if (emailType == (int)ContactEmailType.Other)
            {
                if (!String.IsNullOrEmpty(smtpAttributes))
                {
                    string[] Attributes = smtpAttributes.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (Attributes.Length == 3)
                    {
                        smtp_host = Attributes[0];
                        smtp_port = Convert.ToInt32(Attributes[1]);
                        enable_ssl = Convert.ToBoolean(Attributes[2]);
                    }
                    else
                        return false;
                }
                else
                    return false;
            }

            return SendEmailWithFile(new string[] { to }, null, displayName, subject, message, from, password, smtp_host, smtp_port, enable_ssl, path);
        }

        public bool SendEmailWithFile(string[] to, string[] bcc, string displayName, string subject, string message, string from, string password, string smtp_host, int smtp_port, bool enable_ssl, string my_path)
        {

            try
            {

                SmtpClient serv = new SmtpClient();
                MailMessage msgMail = new MailMessage();
                MailAddress fromAddress = new MailAddress(from, displayName);

                msgMail.Subject = subject;
                //////////////////////////
                if (to != null)
                {
                    for (int i = 0; i < to.Length; i++)
                    {
                        msgMail.To.Add(to[i]);
                    }
                }
                //////////////////////////
                if (bcc != null)
                {
                    for (int i = 0; i < bcc.Length; i++)
                    {
                        msgMail.Bcc.Add(bcc[i]);

                    }
                }
                //////////////////////////

                msgMail.From = fromAddress;

                msgMail.Body = message;

                if (my_path != null)
                {
                    msgMail.Attachments.Add(new System.Net.Mail.Attachment(my_path));
                }

                msgMail.BodyEncoding = System.Text.Encoding.UTF8;
                msgMail.IsBodyHtml = true;


                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtp_host;
                smtp.Port = smtp_port;
                smtp.EnableSsl = enable_ssl;
                ////

                NetworkCredential NetworkCred = new NetworkCredential(from, password);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Send(msgMail);


                //serv.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                //serv.Credentials = new NetworkCredential(from, password);
                //serv.Send(msgMail);

                return true;

            }
            catch (Exception)
            {
                //Utilities.LogError(ex); // To check the error when the email does not sending
                return false;
            }
        }

        public string GetTemplate(string message)
        {
            var request = httpContextAccessor.HttpContext.Request;
            var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
            ///
            string logo = absoluteUri + "/assets/images/logo.png";
            string copyright = absoluteUri + "/assets/images/copyright.png";

            string template = @"

 
       <table dir='rtl'width='90%' border='0' cellspacing='0' cellpadding='0' align='center' 
style='font-family:tahoma;background-color:#3E9223;margin-right:auto;margin-left:auto'>
            <tr>
                <td colspan='3' style='height:100px;'> </td>
            </tr>

            <tr>
                <td style='width:12%;'></td>
                <td style='-moz-box-shadow: inset 0 0 0 #888;-webkit-box-shadow: inset 0 0 0 #888;box-shadow: inset 0 0 0 #888;display: block;background-color: white;border-radius: 5px;'>
                    <br>
                    " + message + @"
                    <br>
                </td>
                <td style='width:12%;'></td>
            </tr>
                 
            <tr>
                <td style='vertical-align:bottom;text-align:right;height:100px;' colspan='2'>
                  <img src='" + copyright + @"' alt='Logo' style='display:block;max-height:40px;' width='40%'>
                </td>
                <td style='vertical-align:bottom;text-align:left;'>
                  <img src='" + logo + @"' alt='Logo' style='display:block;' width='100%'>
                </td>
            </tr>
    </table>";
            return template;
        }

    }
}
