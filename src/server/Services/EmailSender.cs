using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace invoicing.server.Services {
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            System.Console.WriteLine("Sending email to "+email+": " + subject + "\r\n" + htmlMessage);
            return Task.FromResult(true);
        }
    }
}