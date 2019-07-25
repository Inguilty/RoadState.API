using Microsoft.EntityFrameworkCore;
using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.DataAccessLayer
{
    public class RoadStateContext : DbContext
    { 
        public RoadStateContext(DbContextOptions<RoadStateContext> options) : base(options)
        {
            
        }
        public DbSet<BugReport> BugReports { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BugReportRate> BugReportRates { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<UserLikes> UserLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BugReportRate>().HasOne(x => x.BugReport)
                .WithMany(y => y.BugReportRates)
                .HasForeignKey(x => x.BugReportId);
            modelBuilder.Entity<BugReportRate>().HasOne(x => x.User)
                .WithMany(y => y.BugReportRates)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<Comment>().HasOne(x => x.Author)
                .WithMany(y => y.Comments)
                .HasForeignKey(x => x.AuthorId);
            modelBuilder.Entity<Comment>().HasOne(x => x.BugReport)
                .WithMany(y => y.Comments)
                .HasForeignKey(x => x.BugReportId);
            modelBuilder.Entity<UserLikes>().HasOne(x => x.User)
                .WithMany(y => y.UserLikes)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<UserLikes>().HasOne(x => x.Comment)
                .WithMany(y => y.UserLikes)
                .HasForeignKey(x => x.CommentId);
        }


    }
}
