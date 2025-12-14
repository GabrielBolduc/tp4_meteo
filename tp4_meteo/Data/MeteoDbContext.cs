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
            // Récupère le chemin du dossier utilisateur (ex: C:\Users\TonNom)
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            // On nomme la BD "meteo_app.db"
            var dbPath = Path.Combine(folder, "meteo_app.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration de base
            base.OnModelCreating(modelBuilder);

            // SEEDING : On ajoute la ville par défaut si la BD est vide
            // J'utilise Shawinigan comme exemple (tiré de ton énoncé)
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