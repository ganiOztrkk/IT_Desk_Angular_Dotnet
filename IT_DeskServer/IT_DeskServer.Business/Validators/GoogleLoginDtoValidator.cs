using FluentValidation;
using IT_DeskServer.Business.DTOs;

namespace IT_DeskServer.Business.Validators;

public class GoogleLoginDtoValidator : AbstractValidator<GoogleLoginDto>
{
    public GoogleLoginDtoValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Google ile giriş başarısız.");
        RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Google ile giriş başarısız.");
        RuleFor(x => x.IdToken).NotNull().NotEmpty().WithMessage("Google ile giriş başarısız.");
    }
}