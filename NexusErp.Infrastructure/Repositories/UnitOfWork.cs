using Microsoft.EntityFrameworkCore.Storage;
using Nexus.Erp.Domain.Common;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly Dictionary<string, object> _repositories = new();
        private IDbContextTransaction? _currentTransaction;

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;

            if (!_repositories.TryGetValue(type, out var repository))
            {
                var repositoryType = typeof(GenericRepository<>);

                repository = Activator.CreateInstance(
                    repositoryType.MakeGenericType(typeof(TEntity)),
                    _context
                )!;

                _repositories.Add(type, repository);

            }

            return (IGenericRepository<TEntity>)repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _currentTransaction ??= await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_currentTransaction != null)
                    await _currentTransaction.RollbackAsync();
            }
            finally
            {
                if( _currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction= null;
                }
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
