﻿// <auto-generated />
using System;
using Api_fsc_Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api_fsc.Migrations
{
    [DbContext(typeof(FscContext))]
    partial class FscContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Api_fsc_Entities.Models.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateTime")
                        .HasMaxLength(256)
                        .HasColumnType("DATETIME");

                    b.Property<string>("Disk")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("NVARCHAR");

                    b.Property<float?>("FreeSpace")
                        .HasMaxLength(256)
                        .HasColumnType("FLOAT");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("NVARCHAR");

                    b.Property<float?>("TotalSpace")
                        .HasMaxLength(256)
                        .HasColumnType("FLOAT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id")
                        .HasName("PK_Device");

                    b.ToTable("Device", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}