using MediatR;
using NexusErp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Commands
{
    public record CreateSupplierCommand(
        string CompanyName,
        string Phone,
        string TaxNumber
        ) : IRequest<Result<Guid>>;
}
