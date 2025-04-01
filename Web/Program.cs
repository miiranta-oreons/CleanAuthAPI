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

await app.AddPostBuildApplicationConfigurationAsync();

app.MapControllers();

app.Run();
