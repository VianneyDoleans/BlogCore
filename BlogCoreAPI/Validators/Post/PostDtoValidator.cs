using BlogCoreAPI.Constants;
using BlogCoreAPI.DTOs.Post;
using FluentValidation;

namespace BlogCoreAPI.Validators.Post
{
    public class PostDtoValidator : AbstractValidator<IPostDto>
    {
        public PostDtoValidator()
        {
            RuleFor(p => p).NotNull().WithMessage(p => UserMessage.CannotBeNull(nameof(p)));
            RuleFor(p => p.Content).NotNull().NotEmpty().WithMessage(p => UserMessage.CannotBeNullOrEmpty(nameof(p.Content)));
            RuleFor(p => p.Name).NotNull().NotEmpty().WithMessage(p => UserMessage.CannotBeNullOrEmpty(nameof(p.Name)));
            RuleFor(p => p.Name).Length(1, 250).WithMessage(p => UserMessage.CannotExceed(nameof(p.Name), 250));
        }
    }
}
