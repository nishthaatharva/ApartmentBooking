﻿// <auto-generated />
using System;
using ApartmentBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApartmentBooking.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ApartmentBooking.Domain.Entities.Amenities", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Amenities");

                    b.HasData(
                        new
                        {
                            Id = new Guid("d0f230c1-a0bd-4085-7239-08dc02176302"),
                            CreatedOn = new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4263),
                            DeletedOn = new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4264),
                            IsDeleted = false,
                            ModifiedOn = new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4264),
                            Name = "Gym"
                        },
                        new
                        {
                            Id = new Guid("7984dce3-39a8-4da3-bfac-08dc103b5c3f"),
                            CreatedOn = new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4275),
                            DeletedOn = new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4275),
                            IsDeleted = false,
                            ModifiedOn = new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4275),
                            Name = "Pool"
                        },
                        new
                        {
                            Id = new Guid("70edb255-7bf7-4597-d02c-08dc10d6610e"),
                            CreatedOn = new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4276),
                            DeletedOn = new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4277),
                            IsDeleted = false,
                            ModifiedOn = new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4277),
                            Name = "Garden"
                        });
                });

            modelBuilder.Entity("ApartmentBooking.Domain.Entities.Apartment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rooms")
                        .HasColumnType("int");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Apartments");
                });

            modelBuilder.Entity("ApartmentBooking.Domain.Entities.ApartmentAmenitiesAssociation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AmenitiesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AmenitiesId");

                    b.HasIndex("ApartmentId");

                    b.ToTable("ApartmentAmenitiesAssociations");
                });

            modelBuilder.Entity("ApartmentBooking.Domain.Entities.ApartmentAmenitiesAssociation", b =>
                {
                    b.HasOne("ApartmentBooking.Domain.Entities.Amenities", "Amenities")
                        .WithMany()
                        .HasForeignKey("AmenitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApartmentBooking.Domain.Entities.Apartment", "Apartment")
                        .WithMany("ApartmentAmenitiesAssociations")
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Amenities");

                    b.Navigation("Apartment");
                });

            modelBuilder.Entity("ApartmentBooking.Domain.Entities.Apartment", b =>
                {
                    b.Navigation("ApartmentAmenitiesAssociations");
                });
#pragma warning restore 612, 618
        }
    }
}
