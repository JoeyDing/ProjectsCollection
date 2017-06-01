using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.Core.Service
{
    public class SendEmailService
    {
        private readonly string smtpServer;
        private readonly string emailFrom;
        private readonly List<string> emailTo;
        private readonly List<string> ccTo;

        public SendEmailService(
            string smtpServer,
            string emailFrom,
            List<string> emailTo,
            List<string> ccTo = null)
        {
            this.smtpServer = smtpServer;
            this.emailFrom = emailFrom;
            this.emailTo = emailTo;
            this.ccTo = ccTo;
        }

        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string[] Attachments { get; set; }

        public void SendEmail()
        {
            if (!string.IsNullOrEmpty(emailFrom))
            {
                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress(emailFrom.Trim());
                if (emailTo != null)
                {
                    foreach (var email in emailTo)
                    {
                        mailMsg.To.Add(new MailAddress(email.Trim()));
                    }
                }

                mailMsg.Subject = EmailSubject.Trim();
                mailMsg.Body = EmailBody.Trim();
                mailMsg.IsBodyHtml = true;

                if (Attachments != null)
                {
                    foreach (string att in Attachments)
                    {
                        System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                        contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                        contentType.Name = att;
                        mailMsg.Attachments.Add(new Attachment(att, contentType));
                    }
                }

                if (ccTo != null)
                {
                    foreach (var email in ccTo)
                    {
                        mailMsg.CC.Add(new MailAddress(email.Trim()));
                    }
                }

                ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate,
                 X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };
                //Smtpclient to send the mail message
                SmtpClient SmtpMail = new SmtpClient(smtpServer);
                SmtpMail.UseDefaultCredentials = true;
                SmtpMail.EnableSsl = true;
                SmtpMail.Send(mailMsg);
            }
        }
    }
}