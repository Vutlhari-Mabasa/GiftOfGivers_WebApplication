using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GiftOfGivers_WebApplication.Models;

namespace GiftOfGivers_WebApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Donor> Donors { get; set; }
        public DbSet<IncidentReport> IncidentReports { get; set; }
        public DbSet<ReliefProject> ReliefProjects { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<ResourceTracking> ResourceTracking { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<VolunteerTask> VolunteerTasks { get; set; }
        public DbSet<VolunteerAssignment> VolunteerAssignments { get; set; }
    }
}
