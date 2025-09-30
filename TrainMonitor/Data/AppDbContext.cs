using Microsoft.EntityFrameworkCore;
using TrainMonitor.Models;

namespace TrainMonitor.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Train> Trains { get; set; }
        public DbSet<Incident> Incidents { get; set; }
    }
}
