using System.Security.Claims;
using FluentValidation;
using IT_DeskServer.Business;
using IT_DeskServer.DataAccess;
using IT_DeskServer.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddBusiness();

builder.Services.AddCors(config =>
{
    config.AddDefaultPolicy(opt =>
    {
        opt.AllowAnyHeader() // get post put delet
            .AllowAnyMethod() // header kısmında istediğim tip ile çalışma
            .AllowAnyOrigin(); // belli bir site adresi
    });
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecuritySheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecuritySheme, Array.Empty<string>() }
    });
}); // bu kısım swagger üzerinde token ile authentication olabilmek için

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

CreateDeveloperUserMiddleware.CreateFirstUser(app);

app.UseCors();

app.UseHttpsRedirection();

app.MapControllers() 
    .RequireAuthorization(policy => 
    {
        policy.RequireClaim(ClaimTypes.NameIdentifier); // en az 1 require claim gerekiyor.
        policy.AddAuthenticationSchemes("Bearer");
    }); // default olarak allowano atmadığım tüm controllerlarda giriş yapmadığım takdirde 401 atıcak.


app.Run();