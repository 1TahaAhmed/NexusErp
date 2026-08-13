using Nexus.Erp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Common.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task AddItem(TEntity item);
        void UpdateItem(TEntity item);
        void DeleteItem(TEntity item);
        Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity> spec);
        Task<TEntity?> GetEntityWithSpecAsync(ISpecification<TEntity> spec);
        Task<int> CountAsync(ISpecification<TEntity> spec);
    }
}
