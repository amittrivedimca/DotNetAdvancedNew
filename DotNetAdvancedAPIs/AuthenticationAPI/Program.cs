
// https://www.youtube.com/watch?v=ExQJljpj1lY&list=PLOeFnOV9YBa4yaz-uIi5T4ZW3QQGHJQXi&index=1
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-8.0

using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var services = builder.Services;
services.AddHttpContextAccessor();
services.AddAuthentication().AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
    opt => { 

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();

app.MapControllers();

app.Run();
