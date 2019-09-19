using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace CerberusMultiBranch.Support
{
    public class MailSender
    {
        
        public List<string> CC { get;  set; }

        public string FromAddress { get; private set; }

        public string FromTitle { get; private set; }

        public string Password { get; private set; }

        public  string OutsideFrom { get; set; }

        private string host;

        private int port;

        public MailSender(string fromAddress, string password,string host, int port, string fromTitle = "")
        {
            FromAddress = fromAddress;
            FromTitle = fromTitle;
            Password = password;
            this.host = host;
            this.port = port;
            this.CC = new List<string>();
        }

       
        public void SendMail(List<string> to, string body, string subject, Dictionary<string, string> embeddedImage = null)
        {
            // smtpClient.EnableSsl = true;
            MailMessage message = new MailMessage();
            message.Body = body;
            message.IsBodyHtml = true;

            if (embeddedImage != null)
            {
                foreach (var img in embeddedImage)
                {
                    var att = new Attachment(img.Value);
                    att.ContentId = img.Key;
                    message.Attachments.Add(att);
                }
            }

            message.From = new MailAddress(string.IsNullOrEmpty(this.OutsideFrom) ? FromAddress : this.OutsideFrom, FromTitle);

            foreach (var t in to)
                message.To.Add(new MailAddress(t));

            foreach (var c in CC)
                message.CC.Add(c);

            message.Subject = subject;

            SmtpClient smtpClient = new SmtpClient(host, port);


            smtpClient.Credentials = new System.Net.NetworkCredential(FromAddress, Password);


            smtpClient.Send(message);
        }
    }
}