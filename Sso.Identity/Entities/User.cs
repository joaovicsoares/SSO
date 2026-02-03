using System;

public class User
{
    public long Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid Guid { get; private set; }

    protected User() { }

    public User(string email, string passwordHash, string name)
    {
        Name = name;
        PasswordHash = passwordHash;
        Email = email;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        Guid = Guid.NewGuid();
    }
}
