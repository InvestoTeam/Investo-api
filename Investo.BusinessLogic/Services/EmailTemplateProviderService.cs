using Investo.BusinessLogic.Interfaces;

namespace Investo.BusinessLogic.Services;

public class EmailTemplateProviderService : IEmailTemplateProviderService
{
    public string GetResetPasswordTemplate(string code)
    {
        return $"""
            <p>Hello!</p>
            <p>You have requested a password reset for your <strong>Investo</strong> account.</p>
            <p>Your password reset code is:</p>
            <h2 style=\"color: #2d89ef;\">{code}</h2>
            <p>This code is valid for 10 minutes. If you did not make this request, please ignore this email.</p>
            <br />
            <p style=\"font-size: 0.9em; color: #777;\">Best regards,<br />The Investo Team<br />noreply.investo@gmail.com</p>
            """;
    }
}
