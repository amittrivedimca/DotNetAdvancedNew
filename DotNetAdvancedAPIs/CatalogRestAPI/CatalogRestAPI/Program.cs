using Application;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.DB;
using System.Reflection;
using AzureMessageBroker;
using CatalogRestAPI.Classes;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string authScheme = JwtBearerDefaults.AuthenticationScheme; //"MyAuth";
builder.Services.AddControllers()
    .AddJsonOptions(opt => {
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Calalog API",
        Description = "An ASP.NET Core Web API for managing Calalog items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
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

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var services = builder.Services;

//services.AddAuthentication().AddRemoteScheme<MyAuthenticationOptions, MyAuthenticationHandler>("MyAuth", "MyAuth", options =>
//{    
//    options.CallbackPath = new PathString("/callback");
//    options.MyAuthUrl = "http://localhost:5046/api/users";
//    options.RemoteAuthenticationTimeout = TimeSpan.FromMinutes(2);    
//});

services.AddAuthentication().AddScheme<MyAuthenticationOptions, MyAuthenticationHandler>(authScheme, opt => { });

services.AddAuthorization(builder =>
{
    builder.AddPolicy("auth-policy", p =>
    {
        p.RequireAuthenticatedUser();
        p.AddAuthenticationSchemes(authScheme);
    });
});

services.AddApplicationServices();
services.AddPersistenceServices(builder.Configuration);
services.AddCatalogMessageBrokerServices(builder.Configuration);
services.AddScoped<UserService>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var srv = scope.ServiceProvider;
    var context = srv.GetRequiredService<CatalogDB>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseAuthorization();

app.MapControllers();

app.Run();
