namespace ScBootstrap.Logic.Services
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Sitecore.Diagnostics;

    public class MailService : IMailService
    {
        public bool SendMail(string from, string recipients, string subject, string htmlContent, string textContent)
        {
            var ci = new CultureInfo("en-GB");
            var smtp = new SmtpClient();
            try
            {
                // Assign a sender, recipient
                using (var message = new MailMessage(from, recipients))
                {
                    message.Subject = subject;

                    // Set encoding to UTF8
                    message.SubjectEncoding = message.BodyEncoding = Encoding.UTF8;

                    // Set the Content-Language header
                    message.Headers.Add("Content-Language", ci.TwoLetterISOLanguageName);

                    // Define the html alternate view and add to message
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlContent, Encoding.UTF8, MediaTypeNames.Text.Html);
                    message.AlternateViews.Add(htmlView);

                    // Define the plain text alternate view and add to message
                    AlternateView plainTextView = AlternateView.CreateAlternateViewFromString(textContent, Encoding.UTF8, MediaTypeNames.Text.Plain);
                    message.AlternateViews.Add(plainTextView);

                    // Send the message
                    smtp.Send(message);
                    Log.Info(String.Format("A new email has been sent from {0} to {1}", from, recipients), this);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Info(string.Format("Message not sent! {0} | {1}", ex.Message, ex.StackTrace), this);
                Log.Info(string.Format("Message contents: from:{0}; to:{1}; subject:{2}; body:{3}", from, recipients, subject, htmlContent), this);
                return false;
            }
        }

        public string GetHtmlFromTemplate(string path, ListDictionary replacements)
        {
            // Define the html alternate view and add to message
            var md = new MailDefinition
            {
                BodyFileName = HttpContext.Current.Server.MapPath(path),
                From = "me@somewhere.com",
                IsBodyHtml = true,
                Subject = "Subject"
            };

            MailMessage msg = md.CreateMailMessage("you@somewhere.com", replacements, new Control());
            return msg.Body;
        }
    }
}
