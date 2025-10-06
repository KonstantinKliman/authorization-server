using AuthorizationServer.DataAccess.Dtos;
using AuthorizationServer.DataAccess.Dtos.Users;
using FluentValidation;

namespace AuthorizationServer.BusinessLogic.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя пользователя не должно быть пустым");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль не должен быть пустым")
            .MinimumLength(8).WithMessage("Пароль должен иметь минимум 8 символов")
            .MaximumLength(100).WithMessage("Максимальная длина пароля 100 символов")
            .Matches("[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
            .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру");
    }
}