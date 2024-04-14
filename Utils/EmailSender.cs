using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}
namespace Gbc_Travel_Group63.Utils{
public class EmailSender : IEmailSender
{
    private readonly EmailSenderOptions _emailSenderOptions;

    public EmailSender(IOptions<EmailSenderOptions> emailSenderOptions)
    {
        _emailSenderOptions = emailSenderOptions.Value;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        System.Diagnostics.Debug.WriteLine("Hello! " + email);
        System.Diagnostics.Debug.WriteLine("Email sent to " + email);
        return Execute(_emailSenderOptions.SmtpServer, _emailSenderOptions.Port, _emailSenderOptions.UseSsl,
            _emailSenderOptions.UserName, _emailSenderOptions.Password, email, subject, htmlMessage);
    }

    private async Task Execute(string smtpServer, int port, bool useSsl, string userName, string password, string email, string subject, string htmlMessage)
    {
        using (var client = new SmtpClient(smtpServer, port))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(userName, password);
            client.EnableSsl = useSsl;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(userName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
            System.Diagnostics.Debug.WriteLine("Email sent to " + email);
        }
    }
}

public class EmailSenderOptions
{
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public bool UseSsl { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
}