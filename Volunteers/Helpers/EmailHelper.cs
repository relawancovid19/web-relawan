using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Volunteers.Helpers
{
    public class EmailHelper
    {
        public static async Task<bool> Send(string subject, string destinationEmailAddress, string destinationFullName, string htmlContent)
        {
            try
            {
                var fromEmailAddress = ConfigurationManager.AppSettings["SenderAddress"];
                var fromSender = ConfigurationManager.AppSettings["SenderDisplayName"];
                var apiKey = ConfigurationManager.AppSettings["SendGridKey"];
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(fromEmailAddress, fromSender);
                var to = new EmailAddress(destinationEmailAddress, destinationFullName);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
            return false;
        }
    }
}