using LogisticsPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPortal.Data;

public class LogisticsContext : DbContext
{
    public LogisticsContext(DbContextOptions<LogisticsContext> options)
        : base(options)
    {
    }

    public DbSet<Shipment> Shipments { get; set; } = null!;
    public DbSet<Driver> Drivers { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Shipment entity
        modelBuilder.Entity<Shipment>()
            .HasOne(s => s.Driver)
            .WithMany(d => d.Shipments)
            .HasForeignKey(s => s.DriverId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Shipment>()
            .HasMany(s => s.AuditLogs)
            .WithOne(a => a.Shipment)
            .HasForeignKey(a => a.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Add indexes for performance
        modelBuilder.Entity<Shipment>()
            .HasIndex(s => s.TrackingId)
            .IsUnique();

        modelBuilder.Entity<Shipment>()
            .HasIndex(s => s.Status);

        modelBuilder.Entity<Shipment>()
            .HasIndex(s => s.Destination);

        modelBuilder.Entity<Driver>()
            .HasIndex(d => d.License)
            .IsUnique();
    }
}
