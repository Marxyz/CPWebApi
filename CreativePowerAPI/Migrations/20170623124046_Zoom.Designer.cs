using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CreativePowerAPI.Models;

namespace CreativePowerAPI.Migrations
{
    [DbContext(typeof(DBC))]
    [Migration("20170623124046_Zoom")]
    partial class Zoom
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
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

            modelBuilder.Entity("CreativePowerAPI.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateDateTime");

                    b.Property<string>("RegisterAccountId");

                    b.HasKey("Id");

                    b.HasIndex("RegisterAccountId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Polyline", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<int>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Polyline");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.PriceList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("InvestorId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("InvestorId");

                    b.ToTable("PriceLists");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.PriceListElement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CostPer");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<float>("Price");

                    b.Property<int?>("PriceListId");

                    b.Property<float>("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("PriceListId");

                    b.ToTable("PriceListElement");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDateTime");

                    b.Property<int>("InvestorId");

                    b.Property<string>("Name");

                    b.Property<int?>("PriceListId");

                    b.HasKey("Id");

                    b.HasIndex("InvestorId");

                    b.HasIndex("PriceListId");

                    b.ToTable("ProjectSet");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.ProjectTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<DateTime>("DeadlineDate");

                    b.Property<string>("Description");

                    b.Property<string>("NISnumber");

                    b.Property<string>("Name");

                    b.Property<int>("ProjectId");

                    b.Property<string>("RegisterAccountId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("RegisterAccountId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.RegisterAccount", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("CompanyAddress");

                    b.Property<string>("CompanyName");

                    b.Property<string>("CompanyPhoneNumber");

                    b.Property<string>("CompanyPostalCode");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<int?>("CredentialsId");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NIP");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("Role");

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

            modelBuilder.Entity("CreativePowerAPI.Models.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("NISnumber");

                    b.Property<int?>("PriceListId");

                    b.Property<int>("ProjectTaskId");

                    b.Property<string>("ProjectTaskName");

                    b.Property<string>("ReportCsvUrl");

                    b.Property<int>("State");

                    b.Property<string>("SwitcherNumber");

                    b.HasKey("Id");

                    b.HasIndex("PriceListId");

                    b.HasIndex("ProjectTaskId")
                        .IsUnique();

                    b.ToTable("ReportSet");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.ReportFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ReportId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("ReportId");

                    b.ToTable("ReportFile");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.TaskPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Label");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<int?>("PolylineId");

                    b.Property<int?>("ProjectTaskId");

                    b.Property<int>("Zoom");

                    b.HasKey("Id");

                    b.HasIndex("PolylineId");

                    b.HasIndex("ProjectTaskId");

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

            modelBuilder.Entity("CreativePowerAPI.Models.Investor", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.ContactPerson", "ContactPerson")
                        .WithMany()
                        .HasForeignKey("ContactPersonId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Notification", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.RegisterAccount")
                        .WithMany("Notifications")
                        .HasForeignKey("RegisterAccountId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Polyline", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.Project")
                        .WithMany("Polylines")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CreativePowerAPI.Models.PriceList", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.Investor")
                        .WithMany("PriceLists")
                        .HasForeignKey("InvestorId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.PriceListElement", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.PriceList")
                        .WithMany("ListofPriceListElements")
                        .HasForeignKey("PriceListId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Project", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.Investor")
                        .WithMany("Projects")
                        .HasForeignKey("InvestorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CreativePowerAPI.Models.PriceList", "PriceList")
                        .WithMany()
                        .HasForeignKey("PriceListId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.ProjectTask", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CreativePowerAPI.Models.RegisterAccount")
                        .WithMany("TaskList")
                        .HasForeignKey("RegisterAccountId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.RegisterAccount", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.LoginUser", "Credentials")
                        .WithMany()
                        .HasForeignKey("CredentialsId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.Report", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.PriceList", "PriceList")
                        .WithMany()
                        .HasForeignKey("PriceListId");

                    b.HasOne("CreativePowerAPI.Models.ProjectTask")
                        .WithOne("Report")
                        .HasForeignKey("CreativePowerAPI.Models.Report", "ProjectTaskId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CreativePowerAPI.Models.ReportFile", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.Report")
                        .WithMany("Files")
                        .HasForeignKey("ReportId");
                });

            modelBuilder.Entity("CreativePowerAPI.Models.TaskPosition", b =>
                {
                    b.HasOne("CreativePowerAPI.Models.Polyline")
                        .WithMany("ListOfPositions")
                        .HasForeignKey("PolylineId");

                    b.HasOne("CreativePowerAPI.Models.ProjectTask")
                        .WithMany("ListOfTaskPoints")
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
