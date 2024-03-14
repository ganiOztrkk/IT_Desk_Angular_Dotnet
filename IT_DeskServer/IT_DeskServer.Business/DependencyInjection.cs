using FluentValidation;
using FluentValidation.AspNetCore;
using IT_DeskServer.Business.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace IT_DeskServer.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        //services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(typeof(LoginDtoValidator).Assembly); AddFluentValidationAutoValidation() metodu validasyonları otomatikleştirir. ben hata kontrolünü kendim yapmak istedim.
        
        services.AddValidatorsFromAssembly(typeof(LoginDtoValidator).Assembly);
        return services;
    }
}