﻿// <auto-generated />
using System;
using ForestProtectionForce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ForestProtectionForce.Migrations
{
    [DbContext(typeof(ForestProtectionForceContext))]
    [Migration("20230319053608_added-offenders-update-01")]
    partial class addedoffendersupdate01
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

            modelBuilder.Entity("ForestProtectionForce.Models.Circle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("ProvinceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProvinceId");

                    b.ToTable("Circle");
                });

            modelBuilder.Entity("ForestProtectionForce.Models.Compartment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DivisionId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DivisionId");

                    b.ToTable("Compartment");
                });

            modelBuilder.Entity("ForestProtectionForce.Models.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CircleId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CircleId");

                    b.ToTable("District");
                });

            modelBuilder.Entity("ForestProtectionForce.Models.Division", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DistrictId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.ToTable("Division");
                });

            modelBuilder.Entity("ForestProtectionForce.Models.Offender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AadhaarNo")
                        .HasColumnType("longtext")
                        .HasColumnName("Aadhaar_No");

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("BankAccountNo")
                        .HasColumnType("longtext")
                        .HasColumnName("Bank_Account_No");

                    b.Property<string>("CaseId")
                        .HasColumnType("longtext");

                    b.Property<string>("Caste")
                        .HasColumnType("longtext");

                    b.Property<string>("Citizenship")
                        .HasColumnType("longtext");

                    b.Property<string>("City")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("Date_of_Birth");

                    b.Property<DateTime?>("DateOfPhotography")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("Date_of_Photography");

                    b.Property<int?>("DistrictId")
                        .HasColumnType("int")
                        .HasColumnName("districtId");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FatherHusbandNameAlias")
                        .HasColumnType("longtext")
                        .HasColumnName("Father_Husband_Name_Alias");

                    b.Property<string>("HouseNo")
                        .HasColumnType("longtext")
                        .HasColumnName("House_No");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("isActive");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("PassportNo")
                        .HasColumnType("longtext")
                        .HasColumnName("Passport_No");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("longblob");

                    b.Property<string>("PinCode")
                        .HasColumnType("longtext")
                        .HasColumnName("PinCode");

                    b.Property<string>("PoliceStation")
                        .HasColumnType("longtext")
                        .HasColumnName("policestation");

                    b.Property<string>("Sex")
                        .HasColumnType("longtext");

                    b.Property<string>("Street")
                        .HasColumnType("longtext");

                    b.Property<string>("SurnameAlias")
                        .HasColumnType("longtext")
                        .HasColumnName("Surname_Alias");

                    b.Property<string>("TelephoneMobileNo")
                        .HasColumnType("longtext")
                        .HasColumnName("Telephone_Mobile_No");

                    b.Property<string>("TradeProfession")
                        .HasColumnType("longtext")
                        .HasColumnName("Trade_Profession");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext")
                        .HasColumnName("updatedBy");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("updatedOn");

                    b.Property<string>("Village")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Offender");
                });

            modelBuilder.Entity("ForestProtectionForce.Models.Province", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Province");
                });

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

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("UserType");
                });

            modelBuilder.Entity("ForestProtectionForce.Models.Circle", b =>
                {
                    b.HasOne("ForestProtectionForce.Models.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Province");
                });

            modelBuilder.Entity("ForestProtectionForce.Models.Compartment", b =>
                {
                    b.HasOne("ForestProtectionForce.Models.Division", "Division")
                        .WithMany()
                        .HasForeignKey("DivisionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Division");
                });

            modelBuilder.Entity("ForestProtectionForce.Models.District", b =>
                {
                    b.HasOne("ForestProtectionForce.Models.Circle", "Circle")
                        .WithMany()
                        .HasForeignKey("CircleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Circle");
                });

            modelBuilder.Entity("ForestProtectionForce.Models.Division", b =>
                {
                    b.HasOne("ForestProtectionForce.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("District");
                });
#pragma warning restore 612, 618
        }
    }
}
