using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Infrastructure.Context
{
    public class ApplicationContext: IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {        
        }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Class>()
              .Property(c => c.IsActive)
              .HasDefaultValue(true);

            modelBuilder.Entity<Class>().HasQueryFilter(i => i.IsActive);

            base.OnModelCreating(modelBuilder);
        }
    }
}
