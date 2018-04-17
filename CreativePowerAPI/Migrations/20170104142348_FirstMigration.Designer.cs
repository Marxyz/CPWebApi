using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CreativePowerAPI.Models;

namespace CreativePowerAPI.Migrations
{
    [DbContext(typeof(DBC))]
    [Migration("20170104142348_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CreativePowerAPI.Models.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ProjectTaskId");

                    b.Property<int?>("RaportId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("ProjectTaskId");

                    b.HasIndex("RaportId");

                    b.ToTable("File");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Investor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Investors");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.PriceList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("InvestorId");

                    b.HasKey("Id");

                    b.HasIndex("InvestorId");

                    b.ToTable("PriceList");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int?>("InvestorId");

                    b.Property<string>("Name");

                    b.Property<string>("State");

                    b.HasKey("Id");

                    b.HasIndex("InvestorId");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.ProjectTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<DateTime>("DeadlineDate");

                    b.Property<string>("Description");

                    b.Property<int?>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectTask");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Raport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateDate");

                    b.Property<int?>("ProjectTaskId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectTaskId");

                    b.ToTable("Raport");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.File", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.ProjectTask")
                        .WithMany("Files")
                        .HasForeignKey("ProjectTaskId");

                    b.HasOne("CreativePowerAPI.Models.Raport")
                        .WithMany("Files")
                        .HasForeignKey("RaportId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.PriceList", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.Investor")
                        .WithMany("PriceLists")
                        .HasForeignKey("InvestorId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Project", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.Investor")
                        .WithMany("Projects")
                        .HasForeignKey("InvestorId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.ProjectTask", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Raport", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.ProjectTask")
                        .WithMany("Raports")
                        .HasForeignKey("ProjectTaskId");
                });
        }
    }
}
