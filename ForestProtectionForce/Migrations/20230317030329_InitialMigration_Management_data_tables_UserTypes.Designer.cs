﻿// <auto-generated />
using ForestProtectionForce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ForestProtectionForce.Migrations
{
    [DbContext(typeof(ForestProtectionForceContext))]
    [Migration("20230317030329_InitialMigration_Management_data_tables_UserTypes")]
    partial class InitialMigration_Management_data_tables_UserTypes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("ForestProtectionForce.Models.Test", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("City")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("Name")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("test", (string)null);
                });

            modelBuilder.Entity("ForestProtectionForce.Models.UserType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("userType")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("UserTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
