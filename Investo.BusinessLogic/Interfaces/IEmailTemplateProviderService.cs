namespace Investo.BusinessLogic.Interfaces;

public interface IEmailTemplateProviderService
{
    string GetResetPasswordTemplate(string code);
}
