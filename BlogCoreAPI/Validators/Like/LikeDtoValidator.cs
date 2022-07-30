using BlogCoreAPI.Constants;
using BlogCoreAPI.Models.DTOs.Like;
using DBAccess.Data;
using FluentValidation;

namespace BlogCoreAPI.Validators.Like
{
    public class LikeDtoValidator : AbstractValidator<ILikeDto>
    {
        public LikeDtoValidator()
        {
            RuleFor(p => p).NotNull().WithMessage(p => UserMessage.CannotBeNull(nameof(p)));
            RuleFor(p => new {p.Post, p.Comment}).Must(like => !(like.Comment != null && like.Post != null))
                .WithMessage(p => "A " + nameof(p) + " can't be assigned to a " + nameof(p.Comment) + " and a " + nameof(p.Post) + " at the same time.");
            RuleFor(p => p.Comment).NotNull().When(like => like.LikeableType == LikeableType.Comment)
                .WithMessage(p => UserMessage.CannotBeNull(nameof(p.Comment)));
            RuleFor(p => p.Post).NotNull().When(like => like.LikeableType == LikeableType.Post)
                .WithMessage(p => UserMessage.CannotBeNull(nameof(p.Post)));
        }
    }
}
