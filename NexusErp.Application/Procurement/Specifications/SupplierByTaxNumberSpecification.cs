using Nexus.Erp.Domain.Entities.Procurement;
using NexusErp.Application.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Specifications
{
    public class SupplierByTaxNumberSpecification : BaseSpecification<Supplier>
    {
        public SupplierByTaxNumberSpecification(string taxNumber)
            : base(s => s.TaxNumber == taxNumber)
        {
            
        }
    }
}
