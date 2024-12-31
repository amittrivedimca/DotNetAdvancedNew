
// Authentication Basics - https://www.youtube.com/watch?v=ExQJljpj1lY&list=PLOeFnOV9YBa4yaz-uIi5T4ZW3QQGHJQXi&index=1
// JWT - https://www.youtube.com/watch?v=8FvN5bhVYxY&list=PLOeFnOV9YBa4yaz-uIi5T4ZW3QQGHJQXi&index=12
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-8.0

using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.DataProtection;
using System.Text;
using AuthenticationAPI.Classes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Authentication API",
        Description = "API documentation for version 1"
    });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var services = builder.Services;
services.AddHttpContextAccessor();

string defaultAuth = JwtBearerDefaults.AuthenticationScheme;
var authn = services.AddAuthentication(defaultAuth);

// add cookie authentication
authn.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
    opt => { });

// add jwt authentication
authn.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, 
    opt => {
        opt.TokenValidationParameters = UserLogin.TokenValidationParams;
    });

services.AddAuthorization(builder => {
    builder.AddPolicy("customer-policy", p =>
    {
        p.RequireAuthenticatedUser();
        p.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
        p.RequireClaim(ClaimTypes.Role, "StoreCustomer");
    });

    builder.AddPolicy("manager-policy", p =>
    {
        p.RequireAuthenticatedUser();
        p.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
        p.RequireClaim(ClaimTypes.Role, "Manager");
    });

    // for jwt
    builder.AddPolicy("bearer-policy", p =>
    {
        p.RequireAuthenticatedUser();
        p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI( o => {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication API v1");        
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
