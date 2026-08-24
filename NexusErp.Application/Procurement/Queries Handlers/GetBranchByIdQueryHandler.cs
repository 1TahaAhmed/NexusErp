using MediatR;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using Nexus.Erp.Domain.Entities.Organization;

namespace NexusErp.Application.Procurement.Queries;

public record GetBranchByIdQuery(Guid Id) : IRequest<Result<BranchDto>>;

public class GetBranchByIdQueryHandler : IRequestHandler<GetBranchByIdQuery, Result<BranchDto>>
{
    private readonly IGenericRepository<Branch> _branchRepo;

    public GetBranchByIdQueryHandler(IGenericRepository<Branch> branchRepo)
    {
        _branchRepo = branchRepo;
    }

    public async Task<Result<BranchDto>> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
    {
        var branches = await _branchRepo.GetAllAsync();
        var branch = branches.FirstOrDefault(b => b.Id == request.Id);

        if (branch == null)
            return Result.Failure<BranchDto>(new Error("NotFound","Branch not found"));

        var dto = new BranchDto(branch.Id, branch.Name, branch.Code);
        return Result<BranchDto>.Success(dto);
    }
}