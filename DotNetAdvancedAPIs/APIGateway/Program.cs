using CommonUtils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using OcelotAPIGateway;
using System.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    //var jwtSecurityScheme = new OpenApiSecurityScheme
    //{
    //    BearerFormat = "JWT",
    //    Name = "Gateway JWT Authentication",
    //    In = ParameterLocation.Header,
    //    Type = SecuritySchemeType.Http,
    //    Scheme = JwtBearerDefaults.AuthenticationScheme,
    //    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

    //    Reference = new OpenApiReference
    //    {
    //        Id = JwtBearerDefaults.AuthenticationScheme,
    //        Type = ReferenceType.SecurityScheme
    //    }
    //};

    //options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    //options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    { jwtSecurityScheme, Array.Empty<string>() }
    //});

    //// using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

var services = builder.Services;

services.AddScoped<UserService>();
string authScheme = "MyAuth";
services.AddAuthentication().AddScheme<MyAuthenticationOptions, MyAuthenticationHandler>(authScheme, opt => { });

services.AddAuthorization(builder =>
{
    builder.AddPolicy("auth-policy", p =>
    {
        p.RequireAuthenticatedUser();
        p.AddAuthenticationSchemes(authScheme);
    });
});

services.AddOcelot(builder.Configuration)
    .AddCacheManager(x =>
    {
        x.WithDictionaryHandle();
    });

// https://github.com/Burgyn/MMLib.SwaggerForOcelot
// https://medium.com/@sanchit.bhardwaj31/using-swagger-with-ocelot-api-gateway-11a5fa89ff12
// https://mahedee.net/configure-swagger-on-api-gateway-using-ocelot-in-asp.net-core-application/

services.AddSwaggerForOcelot(builder.Configuration, o => {
    //o.GenerateDocsForGatewayItSelf = true;
    o.GenerateDocsDocsForGatewayItSelf(opt =>
    {        
        opt.GatewayDocsTitle = "My Gateway";
        opt.GatewayDocsOpenApiInfo = new()
        {
            Title = "My Gateway",
            Version = "v1",
        };
    });

});

var app = builder.Build();


app.UseSwaggerForOcelotUI(opt =>
{
    opt.ReConfigureUpstreamSwaggerJson = OcelotSwaggerUtils.AlterUpstreamSwaggerJson;
    opt.PathToSwaggerGenerator = "/swagger/docs";
});
await app.UseOcelot();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
