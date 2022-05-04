using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;

namespace InvestOA.DataManager
{
    public class EmailService
    {
        public void SendEmail(string email, string subject, string message, IConfiguration configuration)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(MailboxAddress.Parse(configuration["Email:Login"]));
            emailMessage.To.Add(MailboxAddress.Parse(email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587);

            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            client.Authenticate(configuration["Email:Login"], configuration["Email:Password"]);
            client.Send(emailMessage);

            client.Disconnect(true);
        }
    }
}
