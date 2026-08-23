using FluentValidation;
using NexusErp.Application.Sales.Commands;

namespace NexusErp.Application.Sales.Validators;

public class CreateSalesInvoiceCommandValidator : AbstractValidator<CreateSalesInvoiceCommand>
{
    public CreateSalesInvoiceCommandValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.");

        RuleFor(x => x.CreateByUserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Invoice cannot be created without items.");

        RuleFor(x => x.Payments)
            .NotEmpty().WithMessage("At least one payment method must be provided.");

        RuleFor(x => x.DiscountAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Discount amount cannot be negative.");
    }
}