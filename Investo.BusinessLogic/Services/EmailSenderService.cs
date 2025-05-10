using Investo.BusinessLogic.Interfaces;
using Investo.BusinessLogic.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Investo.BusinessLogic.Services;

public class EmailSenderService : IEmailSender
{
    private readonly EmailSettings settings;
    public EmailSenderService(IOptions<EmailSettings> options)
    {
        this.settings = options.Value;
    }
    public async Task SendAsync(MailMessage message)
    {
        using var smtpClient = new SmtpClient(this.settings.Host, this.settings.Port)
        {
            Credentials = new NetworkCredential(this.settings.UserName, this.settings.Password),
            EnableSsl = this.settings.EnableSsl
        };
        await smtpClient.SendMailAsync(message);
    }
}
