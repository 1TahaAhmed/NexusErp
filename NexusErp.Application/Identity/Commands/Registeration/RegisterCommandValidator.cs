using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NexusErp.Application.Identity.Commands.Registeration
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("The Email is Required!")
                .EmailAddress()
                .WithMessage("The email format is incorrect.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The Password is Required!")
                .MinimumLength(6).WithMessage("The password must be at least 6 characters long");

            RuleFor(x => x.UserName)
                            .NotEmpty().WithMessage("The Username is Required!")
                            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores, without spaces.");
        }
    }
}
