using Microsoft.EntityFrameworkCore;
using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.DataAccessLayer
{
    public class RoadStateContext : DbContext
    {
        public RoadStateContext(DbContextOptions<RoadStateContext> options) : base(options) { }
        public DbSet<BugReport> BugReports { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
