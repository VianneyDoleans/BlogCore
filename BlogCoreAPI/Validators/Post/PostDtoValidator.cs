using System;
using BlogCoreAPI.Models.Constants;
using BlogCoreAPI.Models.DTOs.Post;
using FluentValidation;

namespace BlogCoreAPI.Validators.Post
{
    public class PostDtoValidator : AbstractValidator<IPostDto>
    {
        private static bool IsUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var hypotheticalUrl) &&
                   (hypotheticalUrl.Scheme == Uri.UriSchemeHttp ||
                    hypotheticalUrl.Scheme == Uri.UriSchemeHttps);
        }

        public PostDtoValidator()
        {
            RuleFor(p => p).NotNull().WithMessage(p => UserMessage.CannotBeNull(nameof(p)));
            RuleFor(p => p.Content).NotNull().NotEmpty().WithMessage(p => UserMessage.CannotBeNullOrEmpty(nameof(p.Content)));
            RuleFor(p => p.Name).NotNull().NotEmpty().WithMessage(p => UserMessage.CannotBeNullOrEmpty(nameof(p.Name)));
            RuleFor(p => p.Name).Length(1, 250).WithMessage(p => UserMessage.CannotExceed(nameof(p.Name), 250));
            RuleFor(p => p.ThumbnailUrl).Must(IsUrl).When(p => !string.IsNullOrEmpty(p.ThumbnailUrl)).WithMessage("The given thumbnail's url isn't valid.");
        }
    }
}
