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

    public virtual DbSet<System> Systems { get; set; }

    public virtual DbSet<User> Users { get; set; }

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
            entity.Property(e => e.Date).HasColumnType("DATETIME");

            entity.HasOne(d => d.AffectedSystemNavigation).WithMany(p => p.Incidents).HasForeignKey(d => d.AffectedSystem);
        });

        modelBuilder.Entity<System>(entity =>
        {
            entity.HasIndex(e => e.SystemId, "IX_Systems_SystemID").IsUnique();

            entity.Property(e => e.SystemId).HasColumnName("SystemID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Username, "IX_Users_Username").IsUnique();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("DATETIME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
