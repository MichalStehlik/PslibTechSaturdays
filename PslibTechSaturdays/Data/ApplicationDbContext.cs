using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Reports.GetGroupArchivedPrintJobsWithGroupIdWithStartDateTimeWithEndDateTime;
using Microsoft.VisualBasic;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        private ILogger<ApplicationDbContext> _logger;
        public DbSet<Models.Action> Actions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Models.File> Files { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasMany(u => u.Roles).WithMany(r => r.Users).UsingEntity<IdentityUserRole<Guid>>();
                entity.HasMany(u => u.Certificates).WithOne(c => c.User).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(u => u.Enrollments).WithOne(e => e.User).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(u => u.CreatedEnrollments).WithOne(e => e.CreatedBy).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(u => u.CancelledEnrollments).WithOne(e => e.CancelledBy).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(u => u.CancelledEnrollments).WithOne(e => e.CancelledBy).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(u => u.CreatedGroups).WithOne(e => e.CreatedBy).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(u => u.Lectoring).WithMany(g => g.Lectors).UsingEntity<LectorAssignment>(
                    la => la.HasOne(la => la.Group).WithMany().HasForeignKey("GroupId").OnDelete(DeleteBehavior.ClientCascade),
                    la => la.HasOne(la => la.User).WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientCascade)
                ).ToTable("LectorAssignments")
                .HasKey(x => new { x.GroupId, x.UserId });
            });
            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationRole>(entity =>
            {

                entity.HasData(new ApplicationRole
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111110000"),
                    Name = Constants.Security.ADMIN_ROLE,
                    NormalizedName = Constants.Security.ADMIN_ROLE.ToUpper()
                });
                entity.HasData(new ApplicationRole
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222220000"),
                    Name = Constants.Security.LECTOR_ROLE,
                    NormalizedName = Constants.Security.LECTOR_ROLE.ToUpper()
                });
            });
            modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.HasData(new IdentityRoleClaim<Guid>
                {
                    Id = 1,
                    RoleId = Guid.Parse("11111111-1111-1111-1111-111111110000"),
                    ClaimType = Constants.Security.ADMIN_CLAIM,
                    ClaimValue = "1"
                });
                entity.HasData(new IdentityRoleClaim<Guid>
                {
                    Id = 2,
                    RoleId = Guid.Parse("22222222-2222-2222-2222-222222220000"),
                    ClaimType = Constants.Security.LECTOR_CLAIM,
                    ClaimValue = "1"
                });
            });
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasData(new ApplicationUser
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Email = "soboty@pslib.cz",
                    NormalizedEmail = "SOBOTY@PSLIB.CZ",
                    UserName = "soboty@pslib.cz",
                    PasswordHash = hasher.HashPassword(new ApplicationUser(), "Admin_1234"),
                    SecurityStamp = "G56SBMMYFYXDNGIMOS5RMZUDSTQ4BQHI",
                    NormalizedUserName = "SOBOTY@PSLIB.CZ",
                    EmailConfirmed = true,
                    FirstName = "Soboty",
                    LastName = "s Technikou",
                });
            });
            modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.HasKey(x => new { x.RoleId, x.UserId });
                entity.HasData(new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    RoleId = Guid.Parse("11111111-1111-1111-1111-111111110000")
                });
            });
            modelBuilder.Entity<Models.Action>(entity =>
            {
                entity.HasData(new Models.Action
                {
                    ActionId = 1,
                    Name = "Testovací akce",
                    Description = "Tato akce slouží k testovacím účelům.",
                    Year = 2023,
                    Active = true,
                    Published = true,
                    Created = DateTime.Now,
                    CreatedById = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Start = new DateTime(2024, 10, 10, 10, 10, 0),
                    End = new DateTime(2024, 10, 10, 10, 30, 0)
                });
            });
            modelBuilder.Entity<Tag>(entity => 
            {
                entity.HasData(new Tag
                {
                    TagId = 1,
                    Text = "IT",
                    BackgroundColor = "#e34242",
                    ForegroundColor = "#ffffff"
                });
                entity.HasData(new Tag
                {
                    TagId = 2,
                    Text = "Strojírenství",
                    BackgroundColor = "#429fe3",
                    ForegroundColor = "#ffffff"
                });
                entity.HasData(new Tag
                {
                    TagId = 3,
                    Text = "Elektrotechnika",
                    BackgroundColor = "#3cab68",
                    ForegroundColor = "#ffffff"
                });
                entity.HasData(new Tag
                {
                    TagId = 4,
                    Text = "Lyceum",
                    BackgroundColor = "#e3a342",
                    ForegroundColor = "#ffffff"
                });
                entity.HasData(new Tag
                {
                    TagId = 5,
                    Text = "Oděvnictví",
                    BackgroundColor = "#9c42e3",
                    ForegroundColor = "#ffffff"
                });
                entity.HasData(new Tag
                {
                    TagId = 6,
                    Text = "Textilnictví",
                    BackgroundColor = "#e3428f",
                    ForegroundColor = "#ffffff"
                });
                entity.HasData(new Tag
                {
                    TagId = 7,
                    Text = "VOŠ",
                    BackgroundColor = "#436a68",
                    ForegroundColor = "#ffffff"
                });
            });
            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasMany(g => g.Tags)
                    .WithMany(t => t.Groups)
                    .UsingEntity("GroupTags",
                    l => l.HasOne(typeof(Tag)).WithMany().HasForeignKey("TagId"),
                    r => r.HasOne(typeof(Group)).WithMany().HasForeignKey("GroupId"));
                entity.HasData(new Group
                {
                    GroupId = 1,
                    Name = "První skupina",
                    Description = "Skupina pro drobné pokusy.",
                    ActionId = 1,
                    Capacity = 5,
                    MinGrade = SchoolGrade.None,
                    Note = "Poznámka",
                    LectorsNote = "",
                    PlannedOpening = new DateTime(2023,9,9,9,9,9),
                    Created = DateTime.Now,
                    CreatedById = Guid.Parse("11111111-1111-1111-1111-111111111111")
                });
                entity.HasData(new Group
                {
                    GroupId = 2,
                    Name = "Druhá skupina",
                    Description = "Skupina pro další drobné pokusy.",
                    ActionId = 1,
                    Capacity = 10,
                    MinGrade = SchoolGrade.Ninth,
                    Note = "Poznámka",
                    LectorsNote = "Lektoři jsou velmi dobří.",
                    PlannedOpening = new DateTime(2023, 9, 9, 9, 9, 9),
                    Created = DateTime.Now,
                    CreatedById = Guid.Parse("11111111-1111-1111-1111-111111111111")
                });
            });
        }
    }
}