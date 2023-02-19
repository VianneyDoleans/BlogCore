using BlogCoreAPI.Constants;
using BlogCoreAPI.Models.DTOs.User;
using FluentValidation;

namespace BlogCoreAPI.Validators.User
{
    public class UserDtoValidator : AbstractValidator<IUserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(p => p).NotNull().WithMessage(p => UserMessage.CannotBeNull(nameof(p)));
            RuleFor(p => p.UserName).NotNull().NotEmpty().WithMessage(p => UserMessage.CannotBeNullOrEmpty(nameof(p.UserName)));
            RuleFor(p => p.UserName).Matches(@"^(?!.*[._()\[\]-]{2})[A-Za-z0-9._()\[\]-]{3,20}$")
                .WithMessage(p => nameof(p.UserName) + 
                " must consist of between 3 to 20 allowed characters (A-Z, a-z, 0-9, .-_()[]) and cannot contain two consecutive symbols.");
            RuleFor(p => p.Email).NotNull().NotEmpty().WithMessage(p => UserMessage.CannotBeNullOrEmpty(nameof(p.Email)));
            RuleFor(p => p.Email).Length(1, 320).WithMessage(p => UserMessage.CannotExceed(nameof(p.Email), 320));
            RuleFor(p => p.Email)
                .Matches(@"\A[a-zA-Z0-9.!\#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*\z")
                .WithMessage(p => "Email address is invalid.");
            RuleFor(p => p.Password).NotNull().NotEmpty().WithMessage(p => UserMessage.CannotBeNullOrEmpty(nameof(p.Password)));
            RuleFor(p => p.UserDescription).Length(1, 1000).When(p => p != null)
                .WithMessage(p => UserMessage.CannotExceed(nameof(p.UserDescription), 1000));
        }
    }
}
