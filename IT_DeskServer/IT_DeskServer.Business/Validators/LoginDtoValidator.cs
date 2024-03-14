using FluentValidation;
using IT_DeskServer.Business.DTOs;

namespace IT_DeskServer.Business.Validators;

public sealed class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .NotNull().WithMessage("Kullanıcı boş olamaz")
            .NotEmpty().WithMessage("Kullanıcı boş olamaz")
            .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter içermelidir");
        RuleFor(x => x.Password)
            .NotNull().WithMessage("Şifre boş olamaz")
            .NotEmpty().WithMessage("Şifre boş olamaz")
            .Matches(@"^(?=.*[a-z])").WithMessage("Şifre en az bir küçük harf içermelidir")
            .Matches(@"^(?=.*[A-Z])").WithMessage("Şifre en az bir büyük harf içermelidir")
            .Matches(@"^(?=.*\d)").WithMessage("Şifre en az bir rakam içermelidir")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter içermelidir");
    }
}