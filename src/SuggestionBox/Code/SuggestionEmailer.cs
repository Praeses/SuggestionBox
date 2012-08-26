using System.Net.Mail;
using System.Web.Configuration;
using MarkdownSharp;
using SuggestionBox.Data;

namespace SuggestionBox.Code
{
    public static class SuggestionEmailer
    {
        public static void SendEmail(Suggestion suggestion)
        {
            var markdown = new Markdown();
            var appName = WebConfigurationManager.AppSettings["AppName"];
            var appUrl = WebConfigurationManager.AppSettings["AppUrl"];
            var smtpServer = WebConfigurationManager.AppSettings["SmtpServer"];
            var fromAddress = WebConfigurationManager.AppSettings["EmailFrom"];
            var toAddress = WebConfigurationManager.AppSettings["EmailTo"];

            var subject = string.Format("{0}: {1}", appName, suggestion.Title);
            var body = string.Format(@"<p>""{0}""  has been submitted to {1}.</p>
{2}
<p>You can view the suggestion <a href=""{3}Suggestions/View/{4}"">here</a>.</p>", suggestion.Title, appName, markdown.Transform(suggestion.Body), appUrl, suggestion.Id);

            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            var client = new SmtpClient(smtpServer);

            client.Send(message);
        }
    }
}