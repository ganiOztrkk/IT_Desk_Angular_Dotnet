using FluentValidation;
using IT_DeskServer.DataAccess;
using IT_DeskServer.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccess(builder.Configuration);

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

CreateDeveloperUserMiddleware.CreateFirstUser(app);

app.UseCors();

app.UseHttpsRedirection();

app.MapControllers();


app.Run();