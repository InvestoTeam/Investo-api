using System.Net.Mail;

namespace Investo.BusinessLogic.Interfaces;

public interface IEmailSender
{
    Task SendAsync(MailMessage message);
}
