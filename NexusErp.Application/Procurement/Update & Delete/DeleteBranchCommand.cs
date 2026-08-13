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
    public record DeleteBranchCommand(Guid Id) : IRequest<Result<bool>>;

    public class DeleteBranchCommandHandler : IRequestHandler<DeleteBranchCommand, Result<bool>>
    {
        private readonly IGenericRepository<Branch> _branchRepo;

        public DeleteBranchCommandHandler(IGenericRepository<Branch> branchRepo)
        {
            _branchRepo = branchRepo;
        }

        public async Task<Result<bool>> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
        {
            var branches = await _branchRepo.GetAllAsync();
            var branch = branches.FirstOrDefault(b => b.Id == request.Id);

            if (branch == null)
            {
                return Result.Failure<bool>(new Error("NotFound", "Branch not found"));
            }

            _branchRepo.DeleteItem(branch);

            return Result<bool>.Success(true);
        }
    }
}