namespace MyBlazorApp.Components.State;

public class SessionState
{
    public bool IsAuthenticated { get; private set; }
    public UserProfile? User { get; private set; }

    public void Set(UserProfile user)
    {
        User = user;
        IsAuthenticated = true;
    }

    public void Clear()
    {
        User = null;
        IsAuthenticated = false;
    }
}

public class UserProfile
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
}