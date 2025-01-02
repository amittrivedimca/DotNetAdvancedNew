using Application;
using Asp.Versioning;
using ProductDomain.ExternalServiceInterfaces;
using Microsoft.OpenApi.Models;
using Persistence;
using System.Reflection;
using AzureMessageBroker;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CommonUtils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string authScheme = "MyAuth";
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // https://www.c-sharpcorner.com/article/api-versioning-best-practices-in-net-8/
    // Define Swagger documents for different versions
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Cart API",
        Description = "API documentation for version 1",
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
    options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v2",
        Title = "Cart API v2",
        Description = "API documentation for version 2",
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

    // Use the ConflictingActionsResolver workaround
    options.ResolveConflictingActions(apiDescriptions =>
    {
        // Your conflict resolution strategy here
        // Example: Choose the first action
        return apiDescriptions.First();
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

    //// using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
var services = builder.Services;

// https://www.youtube.com/watch?v=i6kkKBsHEJs
services.AddApiVersioning(o => {
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1,0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("X-Version")
        //,new QueryStringApiVersionReader("api-version"),        
        //new MediaTypeApiVersionReader("ver")
        );
}).AddApiExplorer( opt => {
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;

});


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
services.AddCartMessageBrokerServices(builder.Configuration);
services.AddScoped<UserService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Define the endpoints for each version
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cart API v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Cart API v2");
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
