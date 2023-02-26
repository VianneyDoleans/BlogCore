using BlogCoreAPI.Models.Constants;
using BlogCoreAPI.Models.DTOs.Tag;
using FluentValidation;

namespace BlogCoreAPI.Validators.Tag
{
    public class TagDtoValidator : AbstractValidator<ITagDto>
    {
        public TagDtoValidator()
        {
            RuleFor(p => p).NotNull().WithMessage(p => UserMessage.CannotBeNull(nameof(p)));
            RuleFor(p => p.Name).NotNull().NotEmpty().WithMessage(p => UserMessage.CannotBeNullOrEmpty(nameof(p.Name)));
            RuleFor(p => p.Name).Length(1, 50).WithMessage(p => UserMessage.CannotExceed(nameof(p.Name), 50));
        }
    }
}
