namespace Investo.Api.ViewModels;

public class UserRefreshTokenViewModel
{
    public string OldToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;
}
