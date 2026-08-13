using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Commands
{
    public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
    {
        public CreateSupplierCommandValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .WithMessage("Company name is required")
                .MaximumLength(150)
                .WithMessage("Company name maximum length is 150");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Supplier phone number is required")
                .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Phone number isn't correct");

            RuleFor(x => x.TaxNumber)
                .NotEmpty().WithMessage("Tax number is required")
                .MaximumLength(50).WithMessage("Tax number can't pass 50 character");
        }
    }
}
