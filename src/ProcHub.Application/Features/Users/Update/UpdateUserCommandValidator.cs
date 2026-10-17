using FluentValidation;
using ProcHub.Application.Authorization;
using ProcHub.Application.Common.Constants;

namespace ProcHub.Application.Features.Users.Update;

public sealed class UpdateUserCommandValidator
    : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(u => u.UserName)
            .NotEmpty()
            .WithMessage("User name is required.")
            .MaximumLength(UserConstants.UserNameMaxLength)
            .WithMessage(
                "User name cannot exceed " +
                $"{UserConstants.UserNameMaxLength} characters.");
        
        RuleFor(u => u.Role)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Role is required.")
            .Must(AppRoles.IsValid)
            .WithMessage("Invalid user role.");

        RuleFor(u => u.DisplayName)
            .MaximumLength(UserConstants.DisplayNameMaxLength)
            .WithMessage(
                "Display name cannot exceed " +
                $"{UserConstants.DisplayNameMaxLength} characters.");
    }
}