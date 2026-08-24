using MediatR;
using Nexus.Erp.Domain.Entities.Organization;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NexusErp.Application.Procurement.Commands
{
    public record UpdateBranchCommand(Guid Id, string Name, string Code) : IRequest<Result<bool>>;

    public class UpdateBranchCommandHandler : IRequestHandler<UpdateBranchCommand, Result<bool>>
    {
        private readonly IGenericRepository<Branch> _branchRepo;

        public UpdateBranchCommandHandler(IGenericRepository<Branch> branchRepo)
        {
            _branchRepo = branchRepo;
        }

        public async Task<Result<bool>> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
        {
            var branches = await _branchRepo.GetAllAsync();
            var branch = branches.FirstOrDefault(b => b.Id == request.Id);

            if (branch == null)
            {
                return Result.Failure<bool>(new Error("NotFound", "Branch not found"));
            }

            branch.Name = request.Name;
            branch.Code = request.Code;

            _branchRepo.UpdateItem(branch);

            return Result<bool>.Success(true);
        }
    }
}