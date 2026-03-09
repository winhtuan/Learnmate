namespace LearnmateSolution.AppState;

/// <summary>
/// Per-circuit (Blazor Server) auth state.
/// Set after login; persists in sessionStorage via JS interop (keys: lm_email, lm_role, lm_token, lm_avatar).
/// </summary>
public sealed class UserSessionService
{
    public bool    IsAuthenticated { get; private set; }
    public string? Role            { get; private set; }
    public string? Email           { get; private set; }
    public string? AccessToken     { get; private set; }
    public string? AvatarUrl       { get; private set; }

    public void SetSession(string email, string role, string accessToken, string? avatarUrl = null)
    {
        Email           = email;
        Role            = role;
        AccessToken     = accessToken;
        AvatarUrl       = avatarUrl;
        IsAuthenticated = true;
    }

    public void Clear()
    {
        Email       = null;
        Role        = null;
        AccessToken = null;
        AvatarUrl   = null;
        IsAuthenticated = false;
    }
}
