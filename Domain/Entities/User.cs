using Domain.Enums;

namespace Domain.Entities;

public class User
{

    // Not "useful" stuff

    public string? Name { get; set; }

    public int? Age { get; set; }


    // "Useful" stuff

    public Guid Id { get; set; }

    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public List<EntityRole> Roles { get; set; } = new List<EntityRole>() { 
        new EntityRole { Name = RoleType.Default }
    };

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiration { get; set; }

}

