using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Entities.Catalog;
using Nexus.Erp.Domain.Entities.Identity;
using Nexus.Erp.Domain.Entities.Inventory;
using Nexus.Erp.Domain.Entities.Organization;
using Nexus.Erp.Domain.Entities.Procurement;
using Nexus.Erp.Domain.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Branch> Branches => Set<Branch>();


        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<BranchStock> BranchStocks => Set<BranchStock>();
        public DbSet<ProductBatch> ProductBatches => Set<ProductBatch>();


        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
        public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();
        public DbSet<GoodsReceiptNote> GoodsReceiptNotes => Set<GoodsReceiptNote>();
        public DbSet<GRNItem> GRNItems => Set<GRNItem>();


        public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();
        public DbSet<SalesInvoiceItem> SalesInvoiceItems => Set<SalesInvoiceItem>();
        public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
        public DbSet<SalesReturn> SalesReturns => Set<SalesReturn>();
        public DbSet<SalesReturnItem> SalesReturnItems => Set<SalesReturnItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties()
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?))))
            {
                property.SetColumnType("decimal(18,2)");
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Category>(b =>
            {
                b.Property(n => n.Name)
                .HasMaxLength(100)
                .IsRequired();

                b.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Product>(b =>
            {
                b.Property(p => p.CategoryId)
                .IsRequired();

                b.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

                b.HasIndex(p => p.Barcode)
                .IsUnique();

                b.Property(p => p.Barcode)
                .IsRequired()
                .HasMaxLength(50);

                b.Property(p => p.DefaultUnitCost)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

                b.Property(p => p.SellingPrice)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

                b.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                b.Property(r => r.Description).HasMaxLength(250);
            });

            modelBuilder.Entity<ApplicationUser>( b =>
            {
                b.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
                b.Property(u => u.LastName).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Role>(b =>
            {
                b.Property(r => r.Name)
                .HasMaxLength(100)
                .IsRequired();

                b.HasIndex(r => r.Name)
                .IsUnique();

                b.HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(b =>
            {
                b.Property(q => q.BranchId)
                .IsRequired();

                b.Property(u => u.RoleId)
                .IsRequired();

                b.Property(u => u.UserName)
                .HasMaxLength(100)
                .IsRequired();

                b.HasIndex(u => u.UserName)
                .IsUnique();

                b.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

                b.Property(u => u.FullName)
                .HasMaxLength(100)
                .IsRequired();

                b.Property(u => u.IsActive)
                .HasDefaultValue(true);

                b.HasOne(u => u.Role)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(u => u.Branch)
                .WithMany(br => br.Users)
                .HasForeignKey(u => u.BranchId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BranchStock>(b =>
            {
                b.Property(bs => bs.BranchId)
                .IsRequired();

                b.Property(bs => bs.ProductId)
                .IsRequired();

                b.HasIndex(bs => new { bs.BranchId, bs.ProductId })
                .IsUnique();

                b.Property(bs => bs.QuantityOnHand)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                b.Property(bs => bs.QuantityReserved)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0)
                .IsRequired();

                b.Property(bs => bs.ReorderLevel)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0)
                .IsRequired();

                b.HasOne(bs => bs.Branch)
                .WithMany(br => br.BranchStocks)
                .HasForeignKey(bs => bs.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(bs => bs.Product)
                .WithMany(p => p.BranchStocks)
                .HasForeignKey(bs => bs.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProductBatch>(b =>
            {
                b.Property(bi => bi.BranchId)
                .IsRequired();

                b.Property(bi => bi.ProductId)
                .IsRequired();

                b.Property(bi => bi.GRNItemId)
                .IsRequired();

                b.Property(bi => bi.BatchNumber)
                .HasMaxLength(100)
                .IsRequired();

                b.HasIndex(bi => new { bi.BranchId, bi.ProductId, bi.BatchNumber })
                .IsUnique();

                b.Property(pb => pb.ExpiryDate)
                .IsRequired();

                b.Property(bi => bi.QuantityAvailable)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0)
                .IsRequired();

                b.Property(bi => bi.UnitCost)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                b.Property(bi => bi.InitialQuantity)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                b.HasOne(bi => bi.Branch)
                .WithMany(br => br.ProductBatches)
                .HasForeignKey(bi => bi.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(bi => bi.Product)
                .WithMany(p => p.ProductBatches)
                .HasForeignKey(bi => bi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(bi => bi.GRNItem)
                .WithMany(grni => grni.productBatches)
                .HasForeignKey(bi => bi.GRNItemId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Branch>(b =>
            {
                b.Property(br => br.Code)
                .HasMaxLength(100)
                .IsRequired();

                b.HasIndex(br => br.Code)
                .IsUnique();

                b.Property(br => br.Name)
                .IsRequired()
                .HasMaxLength(100);

                b.Property(br => br.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

                b.HasMany(br => br.Users)
                .WithOne(u => u.Branch)
                .HasForeignKey(u => u.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(br => br.BranchStocks)
                .WithOne(bs => bs.Branch)
                .HasForeignKey(bs => bs.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(br => br.ProductBatches)
                .WithOne(pb => pb.Branch)
                .HasForeignKey(pb => pb.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(br => br.PurchaseOrders)
                .WithOne(po => po.Branch)
                .HasForeignKey(po => po.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(br => br.SalesInvoices)
                .WithOne(si => si.Branch)
                .HasForeignKey(si => si.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(br => br.GoodsReceiptNotes)
                .WithOne(si => si.Branch)
                .HasForeignKey(si => si.BranchId)
                .OnDelete(DeleteBehavior.Restrict); 
            });

            modelBuilder.Entity<GoodsReceiptNote>(b =>
            {
                b.Property(i => i.PurchaseOrderId)
                .IsRequired();

                b.Property(i => i.SupplierId)
                .IsRequired();

                b.Property(i => i.BranchId)
                .IsRequired();
                
                 b.Property(i => i.ReceivedDate)
                .IsRequired();

                b.Property(i => i.InvoiceNumber)
                .HasMaxLength(100)
                .IsRequired();

                b.HasOne(i => i.PurchaseOrder)
                .WithMany(po => po.GoodsReceiptNotes)
                .HasForeignKey(i => i.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(i => i.Supplier)
                .WithMany(s => s.GoodsReceiptNotes)
                .HasForeignKey(i => i.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(i => i.Branch)
                .WithMany(b => b.GoodsReceiptNotes)
                .HasForeignKey(i => i.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(i => i.GRNItems)
                .WithOne(gi => gi.GRN)
                .HasForeignKey(gi => gi.GRNId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<GRNItem>(b =>
            {
                b.Property(i => i.GRNId)
                .IsRequired();

                b.Property(i => i.ProductId)
                .IsRequired();

                b.Property(i => i.BatchNumber)
                .IsRequired()
                .HasMaxLength(100);

                b.Property(i => i.ExpiryDate)
                .IsRequired();

                b.Property(i => i.QuantityReceived)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                b.Property(i => i.QuantityRejected)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0)
                .IsRequired();

                b.Property(i => i.UnitCost)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                b.HasOne(i => i.GRN)
                .WithMany(grn => grn.GRNItems)
                .HasForeignKey(i => i.GRNId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(i => i.Product)
                .WithMany(p => p.GRNItems)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(i => i.productBatches)
                .WithOne(pb => pb.GRNItem)
                .HasForeignKey(pb => pb.GRNItemId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PurchaseOrder>(b =>
            {
                b.Property(po => po.BranchId)
                .IsRequired();

                b.Property(po => po.SupplierId)
                .IsRequired();

                b.Property(po => po.OrderDate)
                .IsRequired();

                b.Property(po => po.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

                b.HasOne(po => po.Branch)
                .WithMany(b => b.PurchaseOrders)
                .HasForeignKey(po => po.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(po => po.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(po => po.Items)
                .WithOne(poi => poi.PurchaseOrder)
                .HasForeignKey(poi => poi.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);
            
                b.HasMany(po => po.GoodsReceiptNotes)
                .WithOne(grn => grn.PurchaseOrder)
                .HasForeignKey(grn => grn.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PurchaseOrderItem>(b =>
            {
                b.Property(poi => poi.PurchaseOrderId)
                .IsRequired();

                b.Property(poi => poi.ProductId)
                .IsRequired();

                b.HasIndex(poi => new { poi.PurchaseOrderId, poi.ProductId })
                .IsUnique();

                b.Property(poi => poi.QuantityOrdered)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                b.Property(poi => poi.UnitCost)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                b.HasOne(poi => poi.PurchaseOrder)
                .WithMany(po => po.Items)
                .HasForeignKey(poi => poi.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(poi => poi.Product)
                .WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(poi => poi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Supplier>(b =>
            {
                b.Property(s => s.CompanyName)
                .HasMaxLength(100)
                .IsRequired();

                b.HasIndex(s => s.CompanyName)
                .IsUnique();

                b.Property(s => s.Phone)
                .HasMaxLength(20);

                b.Property(t => t.TaxNumber)
                .IsRequired()
                .HasMaxLength(50);

                b.HasMany(s => s.PurchaseOrders)
                .WithOne(s => s.Supplier)
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(s => s.GoodsReceiptNotes)
                .WithOne(s => s.Supplier)
                .HasForeignKey(grn => grn.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PaymentTransaction>(b =>
            {
                b.Property(pt => pt.SalesInvoiceId)
                .IsRequired();

                b.Property(pt => pt.GatewayProvider)
                .HasMaxLength(50)
                .IsRequired();

                b.Property(pt => pt.TransactionReference)
                .HasMaxLength(150)
                .IsRequired();

                b.Property(pt => pt.TransactionDate)
                .IsRequired();

                b.Property(r => r.RawResponse)
                .IsRequired();

                b.Property(pt => pt.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                b.Property(pt => pt.PaymentMethod)
                .HasMaxLength(50)
                .HasConversion<string>() 
                .IsRequired();

                b.HasOne(pt => pt.SalesInvoice)
                .WithMany(si => si.PaymentTransactions)
                .HasForeignKey(pt => pt.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SalesInvoice>(b =>
            {
                b.Property(si => si.BranchId)
                .IsRequired();

                b.Property(si => si.CreatedByUserId)
                .IsRequired();

                b.Property(si => si.InvoiceNumber)
                .HasMaxLength(100)
                .IsRequired();

                b.Property(si => si.InvoiceDate)
                .IsRequired();

                b.Property(si => si.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                b.Property(dis => dis.DiscountAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                b.Property(P => P.PaymentStatus)
                .HasMaxLength(50)
                .HasConversion<string>()
                .IsRequired();

                b.Property(P => P.PaymentMethod)
                .HasMaxLength(50)
                .HasConversion<string>()
                .IsRequired();

                b.HasOne(si => si.Branch)
                .WithMany(b => b.SalesInvoices)
                .HasForeignKey(si => si.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(u => u.User)
                .WithMany(u => u.SalesInvoices)
                .HasForeignKey(si => si.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SalesInvoiceItem>(b =>
            {
                b.Property(sii => sii.SalesInvoiceId)
                .IsRequired();

                b.Property(sii => sii.ProductId)
                .IsRequired();

                b.Property(sii => sii.ProductBatchId)
                .IsRequired();

                b.HasIndex(sii => new { sii.SalesInvoiceId, sii.ProductId, sii.ProductBatchId })
                .IsUnique();

                b.Property(sii => sii.Quantity)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                b.Property(sii => sii.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                b.HasOne(sii => sii.SalesInvoice)
                .WithMany(si => si.SalesInvoiceItems)
                .HasForeignKey(sii => sii.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(sii => sii.Product)
                .WithMany(p => p.SalesInvoiceItems)
                .HasForeignKey(sii => sii.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(sii => sii.ProductBatch)
                .WithMany()
                .HasForeignKey(sii => sii.ProductBatchId)
                .OnDelete(DeleteBehavior.Restrict);
            }); 
        }
    }
}
