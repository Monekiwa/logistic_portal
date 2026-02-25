using LogisticsPortal.Data;
using LogisticsPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPortal.Services;

public class ShipmentService
{
    private readonly LogisticsContext _context;

    public ShipmentService(LogisticsContext context)
    {
        _context = context;
    }

    // Get all non-deleted shipments with driver info
    public async Task<List<Shipment>> GetAllShipmentsAsync()
    {
        return await _context.Shipments
            .Where(s => !s.IsDeleted)
            .Include(s => s.Driver)
            .Include(s => s.AuditLogs)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    // Get a specific shipment by ID
    public async Task<Shipment?> GetShipmentByIdAsync(int id)
    {
        return await _context.Shipments
            .Include(s => s.Driver)
            .Include(s => s.AuditLogs.OrderByDescending(a => a.Timestamp))
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
    }

    // Get shipment by tracking ID
    public async Task<Shipment?> GetShipmentByTrackingIdAsync(string trackingId)
    {
        return await _context.Shipments
            .Include(s => s.Driver)
            .FirstOrDefaultAsync(s => s.TrackingId == trackingId && !s.IsDeleted);
    }

    // Create a new shipment
    public async Task<Shipment> CreateShipmentAsync(Shipment shipment)
    {
        shipment.CreatedAt = DateTime.UtcNow;
        shipment.IsDeleted = false;
        
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();

        // Log the creation
        var auditLog = new AuditLog
        {
            ShipmentId = shipment.Id,
            Action = "Shipment created",
            Timestamp = DateTime.UtcNow
        };
        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();

        return shipment;
    }

    // Update shipment status
    public async Task<Shipment?> UpdateShipmentStatusAsync(int id, ShipmentStatus newStatus)
    {
        var shipment = await _context.Shipments
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

        if (shipment == null)
            return null;

        var oldStatus = shipment.Status;
        shipment.Status = newStatus;
        shipment.UpdatedAt = DateTime.UtcNow;

        // Log the status change
        var auditLog = new AuditLog
        {
            ShipmentId = shipment.Id,
            Action = $"Status changed from {oldStatus} to {newStatus}",
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();

        return shipment;
    }

    // Assign a driver to a shipment
    public async Task<Shipment?> AssignDriverAsync(int shipmentId, int driverId)
    {
        var shipment = await _context.Shipments
            .FirstOrDefaultAsync(s => s.Id == shipmentId && !s.IsDeleted);

        if (shipment == null)
            return null;

        var driver = await _context.Drivers.FindAsync(driverId);
        if (driver == null)
            return null;

        var oldDriver = shipment.Driver?.Name ?? "None";
        shipment.DriverId = driverId;
        shipment.UpdatedAt = DateTime.UtcNow;

        var auditLog = new AuditLog
        {
            ShipmentId = shipment.Id,
            Action = $"Driver assigned: {driver.Name} (was {oldDriver})",
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();

        await _context.Entry(shipment).Reference(s => s.Driver).LoadAsync();
        return shipment;
    }

    // Soft delete a shipment
    public async Task<bool> DeleteShipmentAsync(int id)
    {
        var shipment = await _context.Shipments
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

        if (shipment == null)
            return false;

        shipment.IsDeleted = true;
        shipment.UpdatedAt = DateTime.UtcNow;

        var auditLog = new AuditLog
        {
            ShipmentId = shipment.Id,
            Action = "Shipment deleted",
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();

        return true;
    }

    // Get shipments by status
    public async Task<List<Shipment>> GetShipmentsByStatusAsync(ShipmentStatus status)
    {
        return await _context.Shipments
            .Where(s => s.Status == status && !s.IsDeleted)
            .Include(s => s.Driver)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    // Get shipments by destination
    public async Task<List<Shipment>> GetShipmentsByDestinationAsync(string destination)
    {
        return await _context.Shipments
            .Where(s => s.Destination.Contains(destination) && !s.IsDeleted)
            .Include(s => s.Driver)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    // Get shipment count by status
    public async Task<Dictionary<ShipmentStatus, int>> GetShipmentCountByStatusAsync()
    {
        var statuses = Enum.GetValues<ShipmentStatus>();
        var counts = new Dictionary<ShipmentStatus, int>();

        foreach (var status in statuses)
        {
            counts[status] = await _context.Shipments
                .Where(s => s.Status == status && !s.IsDeleted)
                .CountAsync();
        }

        return counts;
    }

    // Get audit logs for a shipment
    public async Task<List<AuditLog>> GetAuditLogsAsync(int shipmentId)
    {
        return await _context.AuditLogs
            .Where(a => a.ShipmentId == shipmentId)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }

    // Update shipment (general update)
    public async Task<Shipment?> UpdateShipmentAsync(Shipment shipment)
    {
        var existingShipment = await _context.Shipments
            .FirstOrDefaultAsync(s => s.Id == shipment.Id && !s.IsDeleted);

        if (existingShipment == null)
            return null;

        existingShipment.Origin = shipment.Origin;
        existingShipment.Destination = shipment.Destination;
        existingShipment.EstimatedDelivery = shipment.EstimatedDelivery;
        existingShipment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingShipment;
    }
}
