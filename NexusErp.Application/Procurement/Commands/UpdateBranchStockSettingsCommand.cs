using MediatR;
using NexusErp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Commands
{
    public record UpdateBranchStockSettingsCommand(
        Guid BranchId,
        Guid ProductId,
        decimal ReorderLevel
        ) : IRequest<Result<Guid>>;
}
