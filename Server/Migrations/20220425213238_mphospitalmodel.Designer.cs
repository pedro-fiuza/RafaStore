﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RafaStore.Server.Data;

#nullable disable

namespace RafaStore.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220425213238_mphospitalmodel")]
    partial class mphospitalmodel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HospitalPatient", b =>
                {
                    b.Property<int>("HospitalId")
                        .HasColumnType("integer");

                    b.Property<int>("PatientId")
                        .HasColumnType("integer");

                    b.HasKey("HospitalId", "PatientId");

                    b.HasIndex("PatientId");

                    b.ToTable("HospitalPatient");
                });

            modelBuilder.Entity("RafaStore.Shared.Model.EventModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Description");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("timestamp")
                        .HasColumnName("EventDate");

                    b.Property<int>("ResumeId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("varchar")
                        .HasColumnName("Title");

                    b.HasKey("Id");

                    b.HasIndex("ResumeId");

                    b.ToTable("Event", (string)null);
                });

            modelBuilder.Entity("RafaStore.Shared.Model.HospitalModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("Address");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("City");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("Name");

                    b.Property<string>("Uf")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("Uf");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "IX_Hospital_Name");

                    b.ToTable("Hospital", (string)null);
                });

            modelBuilder.Entity("RafaStore.Shared.Model.PatientModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("Age")
                        .IsRequired()
                        .HasColumnType("int4")
                        .HasColumnName("Age");

                    b.Property<DateTime?>("BirthDate")
                        .IsRequired()
                        .HasColumnType("timestamp")
                        .HasColumnName("BirthDate");

                    b.Property<string>("MotherName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar")
                        .HasColumnName("MotherName");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("Patient", (string)null);
                });

            modelBuilder.Entity("RafaStore.Shared.Model.ResumeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("AdmissionDate")
                        .IsRequired()
                        .HasColumnType("timestamp")
                        .HasColumnName("AdmissionDate");

                    b.Property<string>("Bed")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar")
                        .HasColumnName("Bed");

                    b.Property<string>("Complications")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Complications");

                    b.Property<string>("MainDiagnosis")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("MainDiagnosis");

                    b.Property<int>("PatientId")
                        .HasColumnType("integer");

                    b.Property<string>("ProposalOfTheDay")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ProposalOfTheDay");

                    b.Property<string>("Surgeries")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Surgeries");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("Resume", (string)null);
                });

            modelBuilder.Entity("RafaStore.Shared.Model.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp")
                        .HasColumnName("CreatedDate");

                    b.Property<string>("Crm")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar")
                        .HasColumnName("Crm");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar")
                        .HasColumnName("Email");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("PasswordHash");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("PasswordSalt");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("UserResume", b =>
                {
                    b.Property<int>("ResumeId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("ResumeId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserResume");
                });

            modelBuilder.Entity("HospitalPatient", b =>
                {
                    b.HasOne("RafaStore.Shared.Model.HospitalModel", null)
                        .WithMany()
                        .HasForeignKey("HospitalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_HospitalPatient_HospitalId");

                    b.HasOne("RafaStore.Shared.Model.PatientModel", null)
                        .WithMany()
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_HospitalPatient_PatientId");
                });

            modelBuilder.Entity("RafaStore.Shared.Model.EventModel", b =>
                {
                    b.HasOne("RafaStore.Shared.Model.ResumeModel", "Resume")
                        .WithMany("Events")
                        .HasForeignKey("ResumeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Event_Resume");

                    b.Navigation("Resume");
                });

            modelBuilder.Entity("RafaStore.Shared.Model.ResumeModel", b =>
                {
                    b.HasOne("RafaStore.Shared.Model.PatientModel", "Patient")
                        .WithMany("Resumes")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Resume_Patient");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("UserResume", b =>
                {
                    b.HasOne("RafaStore.Shared.Model.ResumeModel", null)
                        .WithMany()
                        .HasForeignKey("ResumeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_UserResume_ResumeId");

                    b.HasOne("RafaStore.Shared.Model.UserModel", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_UserResume_UserId");
                });

            modelBuilder.Entity("RafaStore.Shared.Model.PatientModel", b =>
                {
                    b.Navigation("Resumes");
                });

            modelBuilder.Entity("RafaStore.Shared.Model.ResumeModel", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}