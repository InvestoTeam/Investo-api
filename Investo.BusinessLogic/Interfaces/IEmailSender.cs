using System.Net.Mail;

namespace Investo.BusinessLogic.Interfaces;

public interface IEmailSender
{
    Task SendASync(MailMessage message);
}
