﻿// <auto-generated />
using HLTV.Api.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HLTV.Api.Migrations
{
    [DbContext(typeof(HltvContext))]
    [Migration("20240714114057_mig_1")]
    partial class mig_1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("HLTV.Api.Models.Ranking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Points")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.Property<string>("TeamLogo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Rankings");
                });
#pragma warning restore 612, 618
        }
    }
}
