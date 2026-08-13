using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
