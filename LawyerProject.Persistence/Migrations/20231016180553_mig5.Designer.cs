﻿// <auto-generated />
using System;
using LawyerProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LawyerProject.Persistence.Migrations
{
    [DbContext(typeof(LawyerProjectContext))]
    [Migration("20231016180553_mig5")]
    partial class mig5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CaseCasePdfFile", b =>
                {
                    b.Property<int>("CasePdfFilesObjectId")
                        .HasColumnType("int");

                    b.Property<int>("CasesObjectId")
                        .HasColumnType("int");

                    b.HasKey("CasePdfFilesObjectId", "CasesObjectId");

                    b.HasIndex("CasesObjectId");

                    b.ToTable("CaseCasePdfFile");
                });

            modelBuilder.Entity("LawyerProject.Domain.Entities.Advert", b =>
                {
                    b.Property<int>("ObjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ObjectId"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<DateTime>("CaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CasePlace")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("CaseType")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DataState")
                        .HasColumnType("int");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ObjectId");

                    b.ToTable("Adverts");
                });

            modelBuilder.Entity("LawyerProject.Domain.Entities.Case", b =>
                {
                    b.Property<int>("ObjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ObjectId"), 1L, 1);

                    b.Property<DateTime?>("CaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CaseDescription")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CaseNot")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("CaseNumber")
                        .HasColumnType("int");

                    b.Property<int>("CaseType")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DataState")
                        .HasColumnType("int");

                    b.Property<string>("Files")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdUserFK")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ObjectId");

                    b.HasIndex("IdUserFK");

                    b.ToTable("Cases");
                });

            modelBuilder.Entity("LawyerProject.Domain.Entities.File", b =>
                {
                    b.Property<int>("ObjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ObjectId"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DataState")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Storage")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("ObjectId");

                    b.ToTable("Files");

                    b.HasDiscriminator<string>("Discriminator").HasValue("File");
                });

            modelBuilder.Entity("LawyerProject.Domain.Entities.User", b =>
                {
                    b.Property<int>("ObjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ObjectId"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DataState")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("ProfileImage")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ObjectId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LawyerProject.Domain.Entities.UserActivity", b =>
                {
                    b.Property<int>("ObjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ObjectId"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("DataState")
                        .HasColumnType("int");

                    b.Property<string>("IpAdresi")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("KullaniciId")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Tarih")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ObjectId");

                    b.ToTable("UserActivitys");
                });

            modelBuilder.Entity("LawyerProject.Domain.Entities.CasePdfFile", b =>
                {
                    b.HasBaseType("LawyerProject.Domain.Entities.File");

                    b.HasDiscriminator().HasValue("CasePdfFile");
                });

            modelBuilder.Entity("LawyerProject.Domain.Entities.UserImageFile", b =>
                {
                    b.HasBaseType("LawyerProject.Domain.Entities.File");

                    b.HasDiscriminator().HasValue("UserImageFile");
                });

            modelBuilder.Entity("CaseCasePdfFile", b =>
                {
                    b.HasOne("LawyerProject.Domain.Entities.CasePdfFile", null)
                        .WithMany()
                        .HasForeignKey("CasePdfFilesObjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("LawyerProject.Domain.Entities.Case", null)
                        .WithMany()
                        .HasForeignKey("CasesObjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("LawyerProject.Domain.Entities.Case", b =>
                {
                    b.HasOne("LawyerProject.Domain.Entities.User", "User")
                        .WithMany("Cases")
                        .HasForeignKey("IdUserFK")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LawyerProject.Domain.Entities.User", b =>
                {
                    b.Navigation("Cases");
                });
#pragma warning restore 612, 618
        }
    }
}
