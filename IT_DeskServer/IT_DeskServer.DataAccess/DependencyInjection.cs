using System.Reflection;
using System.Text;
using IT_DeskServer.DataAccess.Context;
using IT_DeskServer.Entity.Models;
using IT_DeskServer.Entity.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scrutor;

namespace IT_DeskServer.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        #region jwt injection ve options pattern uygulaması

        services.Configure<Jwt>(configuration.GetSection("Jwt"));
        var serviceProvider = services.BuildServiceProvider();
        var jwtConfiguration = serviceProvider.GetRequiredService<IOptions<Jwt>>().Value;

        services
            .AddAuthentication()
            .AddJwtBearer(cfr =>
            {
                cfr.TokenValidationParameters = new()
                {
                    ValidateIssuer = true, //uygulamanın kime ya da hangi siteye ait olduğunun kontrolünü sağlar
                    ValidateAudience = true, //uygulamanın kimlere ya da hangi siteye açıldığının kontolü sağlar
                    ValidateIssuerSigningKey =
                        true, //verdiğimiz güvenlik anahtarının doğrulanmasını isteyip istemediğimizi sağlar
                    ValidateLifetime = true, //üretilen tokenin süreli olup olmayacağını sağlar
                    ValidIssuer = jwtConfiguration.Issuer, //valid issuer değeri
                    ValidAudience = jwtConfiguration.Audience, //valid audience değeri
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey))
                };
            });

        #endregion


        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options
                .UseSqlServer(configuration.GetConnectionString("db"));
        });

        services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireDigit = true;
                opt.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddHttpContextAccessor();

        services.Scan(action =>
        {
            action
                .FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(publicOnly: false)
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });
        return services;
    }
}