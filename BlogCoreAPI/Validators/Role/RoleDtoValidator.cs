using BlogCoreAPI.Constants;
using BlogCoreAPI.DTOs.Role;
using FluentValidation;

namespace BlogCoreAPI.Validators.Role
{
    public class RoleDtoValidator : AbstractValidator<IRoleDto>
    {
        public RoleDtoValidator()
        {
            RuleFor(p => p).NotNull().WithMessage(p => UserMessage.CannotBeNull(nameof(p)));
            RuleFor(p => p.Name).NotNull().NotEmpty().WithMessage(p => UserMessage.CannotBeNullOrEmpty(nameof(p.Name)));
            RuleFor(p => p.Name).Length(1, 20).WithMessage(p => UserMessage.CannotExceed(nameof(p.Name), 20));
        }
    }
}
