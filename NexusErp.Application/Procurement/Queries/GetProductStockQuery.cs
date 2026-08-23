using MediatR;
using NexusErp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Queries
{
    public record GetProductStockQuery(
        Guid BranchId,
        Guid ProductId
        ) : IRequest<Result<decimal>>;
}
