using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;

namespace Board.Application.Jobs.PostJob
{
    public class PostJobCommand : IRequest<int>
    {
        public string CompanyName { get; set; }

        public byte[] Logo { get; set; }

        public string Website { get; set; }


        public string Position { get; set; }

        public string Description { get; set; }

        public bool Remote { get; set; }

        public string Location { get; set; }

        public string ApplyUrl { get; set; }

        public List<string> Tags { get; set; } = new List<string>();


        public string Email { get; set; }
    }

    public class PostJobCommandValidator : AbstractValidator<PostJobCommand>
    {
        public PostJobCommandValidator()
        {
            RuleFor(cmd => cmd.CompanyName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(cmd => cmd.Logo)
                .IsPngImage(50_000);

            RuleFor(cmd => cmd.Website)
                .NotEmpty()
                .MaximumLength(100)
                .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _))
                .WithMessage($"Is not a valid url");

            RuleFor(cmd => cmd.Position)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(cmd => cmd.Description)
                .NotEmpty()
                .MaximumLength(5000);

            RuleFor(cmd => cmd.Location)
                .NotEmpty()
                .When(x => !x.Remote);

            RuleFor(cmd => cmd.ApplyUrl)
                .NotEmpty()
                .MaximumLength(200)
                .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _))
                .WithMessage($"Is not a valid url");

            RuleFor(cmd => cmd.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
