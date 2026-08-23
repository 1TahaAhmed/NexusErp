using FluentValidation;
using NexusErp.Application.Procurement.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Identity.Commands.SalesInvoiceRequestValidator
{
    public class CreateSalesInvoiceRequestValidator : AbstractValidator<CreateSalesInvoiceRequest>
    {
        public CreateSalesInvoiceRequestValidator()
        {
            RuleFor(x => x.BranchId).NotEmpty();
            RuleFor(x => x.Items).NotEmpty().WithMessage("add one product at least!");
            RuleFor(x => x.DiscountAmount).GreaterThanOrEqualTo(0);
        }
    }
}
