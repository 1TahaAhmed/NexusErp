using MediatR;
using Nexus.Erp.Domain.Entities.Organization;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Queries
{
    public record BranchDto(
        Guid Id,
        string Name,
        string Code
        );

    public record GetBarnchesListQuery() : IRequest<Result<IReadOnlyList<BranchDto>>>;
    public class GetBranchesListQueryHandler : IRequestHandler<GetBarnchesListQuery, Result<IReadOnlyList<BranchDto>>>
    {
        private readonly IGenericRepository<Branch> _branchRepo;

        public GetBranchesListQueryHandler(IGenericRepository<Branch> branchRepo)
        {
            _branchRepo = branchRepo;
        }
        public async Task<Result<IReadOnlyList<BranchDto>>> Handle(GetBarnchesListQuery request, CancellationToken cancellationToken)
        {
            var branches = await _branchRepo.GetAllAsync();

            IReadOnlyList<BranchDto> dtos = branches.Select(b => new BranchDto(
                b.Id,
                b.Name,
                b.Code
                )).ToList();

            return Result<IReadOnlyList<BranchDto>>.Success(dtos);
        }
    }
}
