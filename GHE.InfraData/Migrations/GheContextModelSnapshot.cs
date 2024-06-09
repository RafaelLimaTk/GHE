﻿// <auto-generated />
using System;
using GHE.InfraData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GHE.InfraData.Migrations
{
    [DbContext(typeof(GheContext))]
    partial class GheContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("GHE.Domain.Entities.Ghe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Dangerousness")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("GHE")
                        .HasColumnType("TEXT");

                    b.Property<string>("Matricule")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<bool>("NotApplicable")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Unhealthiness")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Ghe");
                });

            modelBuilder.Entity("GHE.Domain.Entities.Training", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ASO")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GheId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TrainingDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("TrainingDateFinal")
                        .HasColumnType("TEXT");

                    b.Property<string>("TrainingName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GheId");

                    b.ToTable("Training");
                });

            modelBuilder.Entity("GHE.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GHE.Domain.Entities.Training", b =>
                {
                    b.HasOne("GHE.Domain.Entities.Ghe", "Ghe")
                        .WithMany("Trainings")
                        .HasForeignKey("GheId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ghe");
                });

            modelBuilder.Entity("GHE.Domain.Entities.Ghe", b =>
                {
                    b.Navigation("Trainings");
                });
#pragma warning restore 612, 618
        }
    }
}