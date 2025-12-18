namespace MyBlazorApp.Components.State;

public class CurrentUserState
{
    public int? UserId { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Role { get; private set; }

    public bool IsLoggedIn => UserId.HasValue;

    public string DisplayName =>
        $"{FirstName} {LastName}".Trim();

    public void Set(int userId, string firstName, string lastName, string role)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
    }

    public void Clear()
    {
        UserId = null;
        FirstName = null;
        LastName = null;
        Role = null;
    }
}