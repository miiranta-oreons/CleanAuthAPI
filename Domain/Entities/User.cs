using Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser
{

    // Not "useful" stuff

    public string? Name { get; set; }

    public int? Age { get; set; }


    // "Useful" stuff

    public required string Email { get; set; } // Still useful with identity?

    public required string PasswordHash { get; set; } // Still useful with identity?

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiration { get; set; }

}

