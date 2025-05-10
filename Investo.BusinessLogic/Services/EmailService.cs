using Investo.BusinessLogic.Interfaces;
using Investo.BusinessLogic.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace Investo.BusinessLogic.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings emailSettings;
    private readonly IEmailTemplateProviderService templateProviderService;
    private readonly IEmailSender emailSender;

    public EmailService(IOptions<EmailSettings> options, IEmailTemplateProviderService templateProviderService, IEmailSender emailSender)
    {
        this.emailSettings = options.Value;
        this.templateProviderService = templateProviderService;
        this.emailSender = emailSender;
    }

    public async Task SendResetCodeAsync(string receiver, string code)
    {
        var body = templateProviderService.GetResetPasswordTemplate(code);

        var message = new MailMessage
        {
            From = new MailAddress(emailSettings.From, "Investo"),
            Subject = "Password Reset Code - Investo",
            Body = body,
            IsBodyHtml = true,
        };
        message.To.Add(new MailAddress(receiver));

        await emailSender.SendASync(message);
    }
}
