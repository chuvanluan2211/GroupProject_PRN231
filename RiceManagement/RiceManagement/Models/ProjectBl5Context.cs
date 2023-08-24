using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RiceManagement.Models
{
    public partial class ProjectBl5Context : DbContext
    {
        public ProjectBl5Context()
        {
        }

        public ProjectBl5Context(DbContextOptions<ProjectBl5Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Export> Exports { get; set; } = null!;
        public virtual DbSet<ExportDetail> ExportDetails { get; set; } = null!;
        public virtual DbSet<Import> Imports { get; set; } = null!;
        public virtual DbSet<ImportRiceDetail> ImportRiceDetails { get; set; } = null!;
        public virtual DbSet<Rice> Rice { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=DESKTOP-I96LAE1;database=ProjectBl5;uid=sa;pwd=123;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Export>(entity =>
            {
                entity.HasKey(e => e.ExprotId);

                entity.ToTable("Export");

                entity.Property(e => e.ExportDate).HasColumnType("date");
            });

            modelBuilder.Entity<ExportDetail>(entity =>
            {
                entity.ToTable("ExportDetail");

                entity.Property(e => e.ExportDetailId).HasColumnName("ExportDetail_Id");

                entity.HasOne(d => d.Export)
                    .WithMany(p => p.ExportDetails)
                    .HasForeignKey(d => d.ExportId)
                    .HasConstraintName("FK_ExportDetail_Export");

                entity.HasOne(d => d.Import)
                    .WithMany(p => p.ExportDetails)
                    .HasForeignKey(d => d.ImportId)
                    .HasConstraintName("FK_ExportDetail_Import");

                entity.HasOne(d => d.Rice)
                    .WithMany(p => p.ExportDetails)
                    .HasForeignKey(d => d.RiceId)
                    .HasConstraintName("FK_ExportDetail_Rice");
            });

            modelBuilder.Entity<Import>(entity =>
            {
                entity.ToTable("Import");

                entity.Property(e => e.ImportDate).HasColumnType("date");
            });

            modelBuilder.Entity<ImportRiceDetail>(entity =>
            {
                entity.HasKey(e => e.ImportDetailId);

                entity.ToTable("ImportRiceDetail");

                entity.Property(e => e.ImportDetailId).HasColumnName("ImportDetail_Id");

                entity.HasOne(d => d.Import)
                    .WithMany(p => p.ImportRiceDetails)
                    .HasForeignKey(d => d.ImportId)
                    .HasConstraintName("FK_ImportRiceDetail_Import");

                entity.HasOne(d => d.Rice)
                    .WithMany(p => p.ImportRiceDetails)
                    .HasForeignKey(d => d.RiceId)
                    .HasConstraintName("FK_ImportRiceDetail_Rice");
            });

            modelBuilder.Entity<Rice>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Rice)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Rice_Category");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
