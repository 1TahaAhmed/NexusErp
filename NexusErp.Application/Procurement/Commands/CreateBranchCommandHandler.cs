using MediatR;
using Nexus.Erp.Domain.Entities.Organization;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Procurement.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Commands
{
    public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateBranchCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
        {
            var branchRepo = _unitOfWork.Repository<Branch>();

            var spec = new BranchByCodeSpecification(request.Code);
            var existingBranch = await branchRepo.GetEntityWithSpecAsync(spec);

            if (existingBranch != null) 
            {
                return Result.Failure<Guid>(new Error("Branch.DuplicateCode", "The code is already used"));
            }

            var Branch = new Branch
            {
                Code = request.Code,
                Name = request.Name,
                IsActive = true
            };

            await branchRepo.AddItem(Branch);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(Branch.Id);
        }
    }
}
