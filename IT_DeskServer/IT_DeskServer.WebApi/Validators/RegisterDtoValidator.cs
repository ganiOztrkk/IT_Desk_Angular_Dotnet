using FluentValidation;
using IT_DeskServer.WebApi.DTOs;

namespace IT_DeskServer.WebApi.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(15)
            .WithMessage("Ad alanı boş olamaz ve en az 3 en fazla 15 karakter olmalı.");
        
        RuleFor(x => x.Lastname)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(15)
            .WithMessage("Soyad alanı boş olamaz ve en az 3 en fazla 15 karakter olmalı.");
    }
}