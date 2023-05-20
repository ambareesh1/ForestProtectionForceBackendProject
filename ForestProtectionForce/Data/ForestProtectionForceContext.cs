 using System;
using System.Collections.Generic;
using ForestProtectionForce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ForestProtectionForce.Data;

public partial class ForestProtectionForceContext : DbContext
{
    public ForestProtectionForceContext(DbContextOptions<ForestProtectionForceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserTypes>  UserTypes { get; set; }
    public virtual DbSet<Province> Province { get; set; }
    public virtual DbSet<Circle> Circle { get; set; }
    public virtual DbSet<District> District { get; set; }
    public virtual DbSet<Division> Division { get; set; }
    public virtual DbSet<Compartment> Compartment { get; set; }
    public virtual DbSet<Offender>? Offender { get; set; }
    public virtual DbSet<Baseline>? Baseline { get; set; }
    public virtual DbSet<UserDetails>? UserDetails { get; set; }
    public virtual DbSet<Superadmin>? Superadmin { get; set; }
    public virtual DbSet<Images>? Images { get; set; }
    public virtual DbSet<Seizures_Form_A>? Seizures_Form_A { get; set; }
    public virtual DbSet<Gamma_unit_form_b>? gamma_unit_form_b { get; set; }

    public virtual DbSet<Seizure_CasesMonth_Form_C>? status_of_cases_form_c { get; set; }
    public virtual DbSet<seizure_man_animal_conflict>? seizure_man_animal_conflict { get; set; }
    //public virtual DbSet<ApplicationRole>? Roles { get; set; }
    //public virtual DbSet<IdentityUserRole<string>>? UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
           .UseCollation("utf8mb4_0900_ai_ci")
        .HasCharSet("utf8mb4");

      

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

   
}
