 using System;
using System.Collections.Generic;
using ForestProtectionForce.Models;
using Microsoft.EntityFrameworkCore;

namespace ForestProtectionForce.Data;

public partial class ForestProtectionForceContext : DbContext
{
    public ForestProtectionForceContext(DbContextOptions<ForestProtectionForceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Test> Tests { get; set; }
    public virtual DbSet<UserType>  UserType { get; set; }
    public virtual DbSet<Province> Province { get; set; }
    public virtual DbSet<Circle> Circle { get; set; }
    public virtual DbSet<District> District { get; set; }
    public virtual DbSet<Division> Division { get; set; }
    public virtual DbSet<Compartment> Compartment { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("test");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Address).HasMaxLength(45);
            entity.Property(e => e.City).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
