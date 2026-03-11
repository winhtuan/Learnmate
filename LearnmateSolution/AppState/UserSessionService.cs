namespace LearnmateSolution.AppState;

/// <summary>
/// Per-circuit (Blazor Server) auth state.
/// Set after login; persists in sessionStorage via JS interop (keys: lm_email, lm_role, lm_token, lm_avatar, lm_userid).
/// </summary>
public sealed class UserSessionService
{
    public bool    IsAuthenticated { get; private set; }
    public string? Role            { get; private set; }
    public string? Email           { get; private set; }
    public string? AccessToken     { get; private set; }
    public string? AvatarUrl       { get; private set; }
    public long    UserId          { get; private set; }
    public string? FullName        { get; private set; }

    public void SetSession(string email, string role, string accessToken, string? avatarUrl = null, long userId = 0, string? fullName = null)
    {
        Email           = email;
        Role            = role;
        AccessToken     = accessToken;
        AvatarUrl       = avatarUrl;
        UserId          = userId;
        FullName        = fullName;
        IsAuthenticated = true;
    }

    public void UpdateAvatar(string? avatarUrl)   => AvatarUrl = avatarUrl;
    public void UpdateFullName(string? fullName)  => FullName  = fullName;

    public void Clear()
    {
        Email           = null;
        Role            = null;
        AccessToken     = null;
        AvatarUrl       = null;
        UserId          = 0;
        FullName        = null;
        IsAuthenticated = false;
    }
}
