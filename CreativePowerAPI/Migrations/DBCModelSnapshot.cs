using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CreativePowerAPI.Models;

namespace CreativePowerAPI.Migrations
{
    [DbContext(typeof(DBC))]
    partial class DBCModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CreativePowerAPI.Models.ContactPerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Affiliation");

                    b.Property<string>("CompanyName");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Nip");

                    b.Property<string>("RegisterAccountId");

                    b.HasKey("Id");

                    b.HasIndex("RegisterAccountId");

                    b.ToTable("ContactPerson");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("RaportId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("RaportId");

                    b.ToTable("File");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Investor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ContactPersonId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ContactPersonId");

                    b.ToTable("Investors");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.LoginUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Password");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("LoginUser");
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

                    b.Property<DateTime>("CreateDateTime");

                    b.Property<int?>("InvestorId");

                    b.Property<string>("Name");

                    b.Property<int?>("PriceListId");

                    b.HasKey("Id");

                    b.HasIndex("InvestorId");

                    b.HasIndex("PriceListId");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.ProjectTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<DateTime>("DeadlineDate");

                    b.Property<string>("Description");

                    b.Property<int?>("PositionId");

                    b.Property<int?>("ProjectId");

                    b.Property<int?>("RaportId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("RaportId");

                    b.ToTable("ProjectTask");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Raport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BoxNumber");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateDate");

                    b.Property<int?>("PriceListId");

                    b.Property<int>("SwitcherNumber");

                    b.HasKey("Id");

                    b.HasIndex("PriceListId");

                    b.ToTable("Raport");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.RegisterAccount", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("CompanyName");

                    b.Property<string>("CompayNip");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<int?>("CredentialsId");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CredentialsId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.TaskComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<int?>("ProjectTaskId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectTaskId");

                    b.ToTable("TaskComment");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.TaskPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.HasKey("Id");

                    b.ToTable("TaskPosition");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.ContactPerson", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.RegisterAccount")
                        .WithMany("Contacts")
                        .HasForeignKey("RegisterAccountId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.File", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.Raport")
                        .WithMany("Files")
                        .HasForeignKey("RaportId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Investor", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.ContactPerson", "ContactPerson")
                        .WithMany()
                        .HasForeignKey("ContactPersonId");
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

                    b.HasOne("CreativePowerAPI.Models.PriceList", "PriceList")
                        .WithMany()
                        .HasForeignKey("PriceListId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.ProjectTask", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.TaskPosition", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId");

                    b.HasOne("CreativePowerAPI.Models.Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId");

                    b.HasOne("CreativePowerAPI.Models.Raport", "Raport")
                        .WithMany()
                        .HasForeignKey("RaportId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Raport", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.PriceList", "PriceList")
                        .WithMany()
                        .HasForeignKey("PriceListId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.RegisterAccount", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.LoginUser", "Credentials")
                        .WithMany()
                        .HasForeignKey("CredentialsId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.TaskComment", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.ProjectTask")
                        .WithMany("TaskComment")
                        .HasForeignKey("ProjectTaskId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.RegisterAccount")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.RegisterAccount")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CreativePowerAPI.Models.RegisterAccount")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
