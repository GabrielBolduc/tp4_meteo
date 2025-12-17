using Microsoft.EntityFrameworkCore;
using tp4_meteo.Models;
using System;
using System.IO;

namespace tp4_meteo.Data
{
    public class MeteoDbContext : DbContext
    {
        public DbSet<Region> Regions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var dbPath = Path.Combine(folder, "meteo_app.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Region>().HasData(
                new Region { Id = 1, Nom = "Shawinigan", Latitude = 46.5698, Longitude = -72.7381 },
                new Region { Id = 2, Nom = "Montréal", Latitude = 45.5017, Longitude = -73.5673 },
                new Region { Id = 3, Nom = "Québec", Latitude = 46.8139, Longitude = -71.2080 }
            );
        }
    }
}