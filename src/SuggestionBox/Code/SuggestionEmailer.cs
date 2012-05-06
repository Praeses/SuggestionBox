using System.Net.Mail;
using System.Web.Configuration;
using SuggestionBox.Data;

namespace SuggestionBox.Code
{
	public static class SuggestionEmailer
	{
		public static void SendEmail(Suggestion suggestion)
		{
			var appName = WebConfigurationManager.AppSettings["AppName"];
			var appUrl = WebConfigurationManager.AppSettings["AppUrl"];
			var smtpServer = WebConfigurationManager.AppSettings["SmtpServer"];
			var fromAddress = WebConfigurationManager.AppSettings["EmailFrom"];
			var toAddress = WebConfigurationManager.AppSettings["EmailTo"];

			var subject = string.Format("{0}: {1}", appName, suggestion.Title);
			var body = string.Format(@"""{0}""  has been submitted to {1}.

""{2}""

You can view the suggestion here: {3}Suggestions/View/{4}", suggestion.Title, appName, suggestion.Body, appUrl, suggestion.Id);

			var message = new MailMessage(fromAddress, toAddress)
			{
				Subject = subject,
				Body = body
			};

			var client = new SmtpClient(smtpServer);

			client.Send(message);
		}
	}
}