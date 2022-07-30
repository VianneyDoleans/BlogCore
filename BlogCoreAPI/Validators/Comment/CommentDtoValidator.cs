using BlogCoreAPI.Constants;
using BlogCoreAPI.Models.DTOs.Comment;
using FluentValidation;

namespace BlogCoreAPI.Validators.Comment
{
    public class CommentDtoValidator : AbstractValidator<ICommentDto>
    {
        public CommentDtoValidator()
        {
            RuleFor(p => p).NotNull().WithMessage(p => UserMessage.CannotBeNull(nameof(p)));
            RuleFor(p => p.Content).NotNull().NotEmpty().WithMessage(p => UserMessage.CannotBeNullOrEmpty(p.Content));
        }
    }
}
