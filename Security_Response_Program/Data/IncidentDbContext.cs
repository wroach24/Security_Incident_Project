using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Security_Response_Program.Models;

public partial class IncidentDbContext : DbContext
{
    public IncidentDbContext()
    {
    }

    public IncidentDbContext(DbContextOptions<IncidentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Incident> Incidents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var dbPath = Path.Combine(baseDirectory, "IncidentDatabase.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasIndex(e => e.IncidentId, "IX_Incidents_IncidentID").IsUnique();

            entity.Property(e => e.IncidentId).HasColumnName("IncidentID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
