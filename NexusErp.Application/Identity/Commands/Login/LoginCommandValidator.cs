using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Identity.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The Email is Required!")
                .EmailAddress().WithMessage("The email format is incorrect.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The Password is Required!")
                .MinimumLength(6).WithMessage("The password must be at least 6 characters long");
        }
    }
}
