using Nexus.Erp.Domain.Entities.Catalog;
using NexusErp.Application.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Specifications
{
    public class ProductByBarcodeSpecification : BaseSpecification<Product>
    {
        public ProductByBarcodeSpecification(string barcode)
            : base(p => p.Barcode == barcode)
        {
            
        }
    }
}
