using System;
using DA_TOTNGHIEP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DA_TOTNGHIEP.Data
{
    public partial class DA_TOTNGHIEP2Context : IdentityDbContext<ApplicationUser>
    {
        public DA_TOTNGHIEP2Context()
        {
        }

        public DA_TOTNGHIEP2Context(DbContextOptions<DA_TOTNGHIEP2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<Carts> Carts { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<ImageProduct> ImageProduct { get; set; }
        public virtual DbSet<ImportWarehouse> ImportWarehouse { get; set; }
        public virtual DbSet<InvoiceDetails> InvoiceDetails { get; set; }
        public virtual DbSet<Invoices> Invoices { get; set; }
        public virtual DbSet<ProductTypes> ProductTypes { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Warehouse> Warehouse { get; set; }
        /*public virtual DbSet<WarehouseDetails> WarehouseDetails { get; set; }*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=MSI\\SQLEXPRESS;Initial Catalog=DA_TOTNGHIEP2;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Username).IsRequired();
            });

            modelBuilder.Entity<Carts>(entity =>
            {
                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProductId);
            });

            modelBuilder.Entity<Comment>(entity =>
            {

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifyAt)
                    .HasColumnName("Modify_at")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Accounts");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Products");
            });

            modelBuilder.Entity<ImageProduct>(entity =>
            {
                entity.Property(e => e.ProductsId).HasColumnName("ProductsID");

                entity.HasOne(d => d.Products)
                    .WithMany(p => p.ImageProduct)
                    .HasForeignKey(d => d.ProductsId)
                    .HasConstraintName("FK_ImageProduct_Products");
            });

            modelBuilder.Entity<ImportWarehouse>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_at")
                    .HasColumnType("datetime");

                /*entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductName).IsRequired();

                entity.Property(e => e.ShipmentId).HasColumnName("ShipmentID");

                entity.Property(e => e.Supplier).IsRequired();

                entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ImportWarehouse)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ImportWarehouse_Products");

                entity.HasOne(d => d.Shipments)
                    .WithMany(p => p.ImportWarehouse)
                    .HasForeignKey(d => d.ShipmentId)
                    .HasConstraintName("FK_ImportWarehouse_WarehouseDetails");*/
                entity.Property(e => e.ProductName).IsRequired();

                entity.Property(e => e.Supplier).IsRequired();

                entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");
            });

            modelBuilder.Entity<InvoiceDetails>(entity =>
            {

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.InvoiceDetails)
                    .HasForeignKey(d => d.ProductId);
            });

            modelBuilder.Entity<Invoices>(entity =>
            {
                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.ProductItems)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.ProductItemsId);
            });

            modelBuilder.Entity<ProductTypes>(entity =>
            {
                entity.Property(e => e.Color)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_at")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.Property(e => e.ShipmentId).HasColumnName("ShipmentID");

                entity.Property(e => e.Sku).HasColumnName("SKU");

                entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");

                entity.HasOne(d => d.ImageProductss)
                    .WithMany(p => p.Productss)
                    .HasForeignKey(d => d.ImageProductId)
                    .HasConstraintName("FK_Products_ImageProduct");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.Cascade);

                /*entity.HasOne(d => d.Shipments)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ShipmentId)
                    .HasConstraintName("FK_Products_WarehouseDetails");*/

                entity.HasOne(d => d.Warehouse)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.WarehouseId)
                    .HasConstraintName("FK_Products_Warehouse");
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                /*entity.Property(e => e.ImportWarehouseId).HasColumnName("ImportWarehouseID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.WarehouseDetailsId).HasColumnName("WarehouseDetailsID");

                entity.HasOne(d => d.ImportWarehouse)
                    .WithMany(p => p.Warehouse)
                    .HasForeignKey(d => d.ImportWarehouseId)
                    .HasConstraintName("FK_Warehouse_ImportWarehouse");*/

                entity.HasIndex(e => e.ImportWarehouseId);

                entity.Property(e => e.ImportWarehouseId).HasColumnName("ImportWarehouseID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.ImportWarehouse)
                    .WithMany(p => p.Warehouse)
                    .HasForeignKey(d => d.ImportWarehouseId)
                    .HasConstraintName("FK_Warehouse_ImportWarehouse");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.WarehouseNavigation)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Warehouse_Products");

               /* entity.HasOne(d => d.WarehouseDetails)
                    .WithMany(p => p.Warehouse)
                    .HasForeignKey(d => d.WarehouseDetailsId)
                    .HasConstraintName("FK_Warehouse_WarehouseDetails");*/
            });

            /*modelBuilder.Entity<WarehouseDetails>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.Shipment)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();
            });*/

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
