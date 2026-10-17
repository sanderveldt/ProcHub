using FluentValidation;
using ProcHub.Application.Authorization;
using ProcHub.Application.Common.Constants;

namespace ProcHub.Application.Features.Users.Create;

public sealed class CreateUserCommandValidator
    : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("User email is required.")
            .EmailAddress()
            .WithMessage("Invalid email address.")
            .MaximumLength(UserConstants.EmailMaxLength)
            .WithMessage(
                "User email cannot exceed " + 
                $"{UserConstants.EmailMaxLength} characters.");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("Password is required.");

        RuleFor(u => u.Role)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Role is required.")
            .Must(AppRoles.IsValid)
            .WithMessage("Invalid user role.");
    }
}