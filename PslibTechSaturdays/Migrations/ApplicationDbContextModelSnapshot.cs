﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PslibTechSaturdays.Data;

#nullable disable

namespace PslibTechSaturdays.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GroupTags", b =>
                {
                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("GroupId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("GroupTags");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClaimType = "admin",
                            ClaimValue = "1",
                            RoleId = new Guid("11111111-1111-1111-1111-111111110000")
                        },
                        new
                        {
                            Id = 2,
                            ClaimType = "lector",
                            ClaimValue = "1",
                            RoleId = new Guid("22222222-2222-2222-2222-222222220000")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            RoleId = new Guid("11111111-1111-1111-1111-111111110000"),
                            UserId = new Guid("11111111-1111-1111-1111-111111111111")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.Action", b =>
                {
                    b.Property<int>("ActionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ActionId"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("End")
                        .HasColumnType("datetime2");

                    b.Property<bool>("ExclusiveEnrollment")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Published")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Start")
                        .HasColumnType("datetime2");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("ActionId");

                    b.HasIndex("CreatedById");

                    b.ToTable("Actions");

                    b.HasData(
                        new
                        {
                            ActionId = 1,
                            Active = true,
                            Created = new DateTime(2023, 11, 24, 19, 39, 8, 816, DateTimeKind.Local).AddTicks(8499),
                            CreatedById = new Guid("11111111-1111-1111-1111-111111111111"),
                            Description = "Tato akce slouží k testovacím účelům.",
                            End = new DateTime(2024, 10, 10, 10, 30, 0, 0, DateTimeKind.Unspecified),
                            ExclusiveEnrollment = true,
                            Name = "Testovací akce",
                            Published = true,
                            Start = new DateTime(2024, 10, 10, 10, 10, 0, 0, DateTimeKind.Unspecified),
                            Year = 2023
                        });
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("11111111-1111-1111-1111-111111110000"),
                            Name = "Administrátor",
                            NormalizedName = "ADMINISTRÁTOR"
                        },
                        new
                        {
                            Id = new Guid("22222222-2222-2222-2222-222222220000"),
                            Name = "Lektor",
                            NormalizedName = "LEKTOR"
                        });
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<bool>("Aspirant")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("MailList")
                        .HasColumnType("bit");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SchoolName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("11111111-1111-1111-1111-111111111111"),
                            AccessFailedCount = 0,
                            Active = true,
                            Aspirant = false,
                            ConcurrencyStamp = "5113675a-feed-487e-a724-f28f039bc1d3",
                            Created = new DateTime(2023, 11, 24, 19, 39, 8, 776, DateTimeKind.Local).AddTicks(7088),
                            Email = "soboty@pslib.cz",
                            EmailConfirmed = true,
                            FirstName = "Soboty",
                            Grade = 0,
                            LastName = "s Technikou",
                            LockoutEnabled = false,
                            MailList = false,
                            NormalizedEmail = "SOBOTY@PSLIB.CZ",
                            NormalizedUserName = "SOBOTY@PSLIB.CZ",
                            PasswordHash = "AQAAAAIAAYagAAAAEFOHwj63T7mw1ijVtUdVgmy5IML1KsgoRf/m6MyGelthtn7v9oXsIMnFSjTqWMMGCw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "G56SBMMYFYXDNGIMOS5RMZUDSTQ4BQHI",
                            TwoFactorEnabled = false,
                            Updated = new DateTime(2023, 11, 24, 19, 39, 8, 776, DateTimeKind.Local).AddTicks(7135),
                            UserName = "soboty@pslib.cz"
                        });
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.Certificate", b =>
                {
                    b.Property<Guid>("CertificateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Issued")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CertificateId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UserId");

                    b.ToTable("Certificates");
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.Enrollment", b =>
                {
                    b.Property<int>("EnrollmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EnrollmentId"));

                    b.Property<Guid>("ApplicationUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Cancelled")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CancelledById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CertificateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("Present")
                        .HasColumnType("int");

                    b.HasKey("EnrollmentId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("CancelledById");

                    b.HasIndex("CertificateId")
                        .IsUnique()
                        .HasFilter("[CertificateId] IS NOT NULL");

                    b.HasIndex("CreatedById");

                    b.HasIndex("GroupId");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.File", b =>
                {
                    b.Property<Guid>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OriginalName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UploaderId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FileId");

                    b.HasIndex("UploaderId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupId"));

                    b.Property<int>("ActionId")
                        .HasColumnType("int");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ClosedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EnrollmentsCountVisible")
                        .HasColumnType("bit");

                    b.Property<string>("LectorsNote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MinGrade")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("OpenedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("PlannedOpening")
                        .HasColumnType("datetime2");

                    b.HasKey("GroupId");

                    b.HasIndex("ActionId");

                    b.HasIndex("CreatedById");

                    b.ToTable("Groups");

                    b.HasData(
                        new
                        {
                            GroupId = 1,
                            ActionId = 1,
                            Capacity = 5,
                            Created = new DateTime(2023, 11, 24, 19, 39, 8, 817, DateTimeKind.Local).AddTicks(8035),
                            CreatedById = new Guid("11111111-1111-1111-1111-111111111111"),
                            Description = "Skupina pro drobné pokusy.",
                            EnrollmentsCountVisible = false,
                            LectorsNote = "",
                            MinGrade = 0,
                            Name = "První skupina",
                            Note = "Poznámka",
                            PlannedOpening = new DateTime(2023, 9, 9, 9, 9, 9, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            GroupId = 2,
                            ActionId = 1,
                            Capacity = 10,
                            Created = new DateTime(2023, 11, 24, 19, 39, 8, 817, DateTimeKind.Local).AddTicks(8052),
                            CreatedById = new Guid("11111111-1111-1111-1111-111111111111"),
                            Description = "Skupina pro další drobné pokusy.",
                            EnrollmentsCountVisible = false,
                            LectorsNote = "Lektoři jsou velmi dobří.",
                            MinGrade = 9,
                            Name = "Druhá skupina",
                            Note = "Poznámka",
                            PlannedOpening = new DateTime(2023, 9, 9, 9, 9, 9, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.LectorAssignment", b =>
                {
                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("LectorAssignments", (string)null);
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TagId"));

                    b.Property<string>("BackgroundColor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ForegroundColor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagId");

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            TagId = 1,
                            BackgroundColor = "#e34242",
                            ForegroundColor = "#ffffff",
                            Text = "IT"
                        },
                        new
                        {
                            TagId = 2,
                            BackgroundColor = "#429fe3",
                            ForegroundColor = "#ffffff",
                            Text = "Strojírenství"
                        },
                        new
                        {
                            TagId = 3,
                            BackgroundColor = "#3cab68",
                            ForegroundColor = "#ffffff",
                            Text = "Elektrotechnika"
                        },
                        new
                        {
                            TagId = 4,
                            BackgroundColor = "#e3a342",
                            ForegroundColor = "#ffffff",
                            Text = "Lyceum"
                        },
                        new
                        {
                            TagId = 5,
                            BackgroundColor = "#9c42e3",
                            ForegroundColor = "#ffffff",
                            Text = "Oděvnictví"
                        },
                        new
                        {
                            TagId = 6,
                            BackgroundColor = "#e3428f",
                            ForegroundColor = "#ffffff",
                            Text = "Textilnictví"
                        },
                        new
                        {
                            TagId = 7,
                            BackgroundColor = "#436a68",
                            ForegroundColor = "#ffffff",
                            Text = "VOŠ"
                        });
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.Text", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Texts");
                });

            modelBuilder.Entity("GroupTags", b =>
                {
                    b.HasOne("PslibTechSaturdays.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PslibTechSaturdays.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("PslibTechSaturdays.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("PslibTechSaturdays.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("PslibTechSaturdays.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("PslibTechSaturdays.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PslibTechSaturdays.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("PslibTechSaturdays.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.Action", b =>
                {
                    b.HasOne("PslibTechSaturdays.Models.ApplicationUser", "CreatedBy")
                        .WithMany("CreatedActions")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.Certificate", b =>
                {
                    b.HasOne("PslibTechSaturdays.Models.ApplicationUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PslibTechSaturdays.Models.ApplicationUser", "User")
                        .WithMany("Certificates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.Enrollment", b =>
                {
                    b.HasOne("PslibTechSaturdays.Models.ApplicationUser", "User")
                        .WithMany("Enrollments")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PslibTechSaturdays.Models.ApplicationUser", "CancelledBy")
                        .WithMany("CancelledEnrollments")
                        .HasForeignKey("CancelledById")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PslibTechSaturdays.Models.Certificate", "Certificate")
                        .WithOne("Enrollment")
                        .HasForeignKey("PslibTechSaturdays.Models.Enrollment", "CertificateId");

                    b.HasOne("PslibTechSaturdays.Models.ApplicationUser", "CreatedBy")
                        .WithMany("CreatedEnrollments")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PslibTechSaturdays.Models.Group", "Group")
                        .WithMany("Enrollments")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CancelledBy");

                    b.Navigation("Certificate");

                    b.Navigation("CreatedBy");

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.File", b =>
                {
                    b.HasOne("PslibTechSaturdays.Models.ApplicationUser", "Uploader")
                        .WithMany("UploadedFiles")
                        .HasForeignKey("UploaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Uploader");
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.Group", b =>
                {
                    b.HasOne("PslibTechSaturdays.Models.Action", "Action")
                        .WithMany("Groups")
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PslibTechSaturdays.Models.ApplicationUser", "CreatedBy")
                        .WithMany("CreatedGroups")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Action");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.LectorAssignment", b =>
                {
                    b.HasOne("PslibTechSaturdays.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("PslibTechSaturdays.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.Action", b =>
                {
                    b.Navigation("Groups");
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.ApplicationUser", b =>
                {
                    b.Navigation("CancelledEnrollments");

                    b.Navigation("Certificates");

                    b.Navigation("CreatedActions");

                    b.Navigation("CreatedEnrollments");

                    b.Navigation("CreatedGroups");

                    b.Navigation("Enrollments");

                    b.Navigation("UploadedFiles");
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.Certificate", b =>
                {
                    b.Navigation("Enrollment");
                });

            modelBuilder.Entity("PslibTechSaturdays.Models.Group", b =>
                {
                    b.Navigation("Enrollments");
                });
#pragma warning restore 612, 618
        }
    }
}
