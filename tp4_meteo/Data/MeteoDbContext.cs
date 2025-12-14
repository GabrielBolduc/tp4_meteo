using MeteoWPF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;

namespace MeteoWPF.Data
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

            // seed
            modelBuilder.Entity<Region>().HasData(
                new Region
                {
                    Id = 1,
                    Nom = "Shawinigan",
                    Latitude = 46.56984172477484,
                    Longitude = -72.73811285651442
                }
            );
        }
    }
}