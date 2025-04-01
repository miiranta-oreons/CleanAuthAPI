using Microsoft.AspNetCore.Identity;
using Domain.Constants;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddInfrastructureServices();
builder.AddApplicationServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder =>
    {
        var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(",");
        if (allowedOrigins is null) return;

        policyBuilder.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins(allowedOrigins);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c => { });
    app.UseSwaggerUI(c => { });

    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Create Roles from Role Manager from domain constants
// Should this be in Application?
using (var scope = app.Services.CreateScope())
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

app.Run();
