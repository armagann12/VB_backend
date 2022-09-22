﻿// <auto-generated />
using System;
using InvoiceApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InvoiceApi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("InvoiceApi.Models.CreditCardModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<long>("Balance")
                        .HasColumnType("bigint");

                    b.Property<string>("BankName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CVC")
                        .HasColumnType("int");

                    b.Property<long>("Number")
                        .HasColumnType("bigint");

                    b.Property<int>("UserModelId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ValidDate")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.HasIndex("UserModelId");

                    b.ToTable("CreditCardModels");
                });

            modelBuilder.Entity("InvoiceApi.Models.InstitutionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Detail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("Mail")
                        .IsUnique();

                    b.ToTable("InstitutionModels");
                });

            modelBuilder.Entity("InvoiceApi.Models.InvoiceModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Detail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("InstitutionModelId")
                        .HasColumnType("int");

                    b.Property<int>("InvoiceNumber")
                        .HasColumnType("int");

                    b.Property<string>("Month")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("UserModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("InstitutionModelId");

                    b.HasIndex("InvoiceNumber")
                        .IsUnique();

                    b.HasIndex("UserModelId");

                    b.ToTable("InvoiceModels");
                });

            modelBuilder.Entity("InvoiceApi.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<long>("TC")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Mail")
                        .IsUnique();

                    b.HasIndex("TC")
                        .IsUnique();

                    b.ToTable("UserModels");
                });

            modelBuilder.Entity("InvoiceApi.Models.CreditCardModel", b =>
                {
                    b.HasOne("InvoiceApi.Models.UserModel", "UserModel")
                        .WithMany()
                        .HasForeignKey("UserModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserModel");
                });

            modelBuilder.Entity("InvoiceApi.Models.InvoiceModel", b =>
                {
                    b.HasOne("InvoiceApi.Models.InstitutionModel", "InstitutionModel")
                        .WithMany()
                        .HasForeignKey("InstitutionModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InvoiceApi.Models.UserModel", "UserModel")
                        .WithMany()
                        .HasForeignKey("UserModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InstitutionModel");

                    b.Navigation("UserModel");
                });
#pragma warning restore 612, 618
        }
    }
}
