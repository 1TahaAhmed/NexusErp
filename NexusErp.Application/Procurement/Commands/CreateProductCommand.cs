using MediatR;
using NexusErp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Commands
{
    public record CreateProductCommand(
        Guid CategoryId,
        string Name,
        string Barcode,
        decimal DefaultUnitCost,
        decimal SellingPrice
        ) : IRequest<Result<Guid>>;
}
