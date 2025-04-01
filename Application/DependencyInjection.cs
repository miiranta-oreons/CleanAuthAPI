using System.Reflection;
using System.Text;
using Application.Services.TokenService;
using Domain.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITokenService, TokenService>();

        // Should that be here?
        builder.Services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
        );

        // Should that be here?
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["TokenSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["TokenSettings:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenSettings:Token"]!)),
            };
        });

        // Should that be here?
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    }

    public static async Task AddPostBuildApplicationConfigurationAsync(this IApplicationBuilder app)
    {
        
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = typeof(RoleTypes).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(x => x.IsLiteral && !x.IsInitOnly)
                .Select(x => x.GetRawConstantValue()?.ToString())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role!))
                {
                    await roleManager.CreateAsync(new IdentityRole(role!));
                }
            }
        }
    }
}
