using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
                    ClaimType = "admin",
                    ClaimValue = "1"
                });
                entity.HasData(new IdentityRoleClaim<Guid>
                {
                    Id = 2,
                    RoleId = Guid.Parse("22222222-2222-2222-2222-222222220000"),
                    ClaimType = "lektor",
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
        }
    }
}