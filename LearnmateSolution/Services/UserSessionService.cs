namespace LearnmateSolution.Services;

/// <summary>
/// Per-circuit (Blazor Server) auth state.
/// Set after login; persists in sessionStorage via JS interop.
/// </summary>
public sealed class UserSessionService
{
    public bool   IsAuthenticated { get; private set; }
    public string? Role           { get; private set; }
    public string? Email          { get; private set; }

    public void SetSession(string email, string role)
    {
        Email           = email;
        Role            = role;
        IsAuthenticated = true;
    }

    public void Clear()
    {
        Email           = null;
        Role            = null;
        IsAuthenticated = false;
    }
}
