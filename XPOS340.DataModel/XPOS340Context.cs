using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace XPOS340.DataModel
{
    public partial class XPOS340Context : DbContext
    {
        public XPOS340Context()
        {
        }

        public XPOS340Context(DbContextOptions<XPOS340Context> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCoba> TblCobas { get; set; } = null!;
        public virtual DbSet<TblMCategory> TblMCategories { get; set; } = null!;
        public virtual DbSet<TblMCustomer> TblMCustomers { get; set; } = null!;
        public virtual DbSet<TblMProduct> TblMProducts { get; set; } = null!;
        public virtual DbSet<TblMVariant> TblMVariants { get; set; } = null!;
        public virtual DbSet<TblTOrderDetail> TblTOrderDetails { get; set; } = null!;
        public virtual DbSet<TblTOrderHeader> TblTOrderHeaders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=XPOS340;user id=sa;Password=P@ssw0rd");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblCoba>(entity =>
            {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<TblMCategory>(entity =>
            {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblMCustomer>(entity =>
            {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblMProduct>(entity =>
            {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Price).HasDefaultValueSql("((0))");

                entity.Property(e => e.Stock).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblMVariant>(entity =>
            {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblTOrderDetail>(entity =>
            {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblTOrderHeader>(entity =>
            {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
