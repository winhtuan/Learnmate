namespace LearnmateSolution.Services;

/// <summary>
/// Per-circuit (Blazor Server) auth state.
/// Set after login; persists in sessionStorage via JS interop (keys: lm_email, lm_role, lm_token).
/// </summary>
public sealed class UserSessionService
{
    public bool    IsAuthenticated { get; private set; }
    public string? Role            { get; private set; }
    public string? Email           { get; private set; }
    public string? AccessToken     { get; private set; }

    public void SetSession(string email, string role, string accessToken)
    {
        Email           = email;
        Role            = role;
        AccessToken     = accessToken;
        IsAuthenticated = true;
    }

    public void Clear()
    {
        Email       = null;
        Role        = null;
        AccessToken = null;
        IsAuthenticated = false;
    }
}
