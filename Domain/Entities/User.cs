using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser
{

    // Not "useful" stuff

    public string? Name { get; set; }

    public int? Age { get; set; }


    // "Useful" stuff

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiration { get; set; }

}

