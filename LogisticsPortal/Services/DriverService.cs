using LogisticsPortal.Data;
using LogisticsPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPortal.Services;

public class DriverService
{
    private readonly LogisticsContext _context;

    public DriverService(LogisticsContext context)
    {
        _context = context;
    }

    // Get all drivers
    public async Task<List<Driver>> GetAllDriversAsync()
    {
        return await _context.Drivers
            .Include(d => d.Shipments)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    // Get a specific driver by ID
    public async Task<Driver?> GetDriverByIdAsync(int id)
    {
        return await _context.Drivers
            .Include(d => d.Shipments)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    // Get available drivers
    public async Task<List<Driver>> GetAvailableDriversAsync()
    {
        return await _context.Drivers
            .Where(d => d.IsAvailable)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    // Create a new driver
    public async Task<Driver> CreateDriverAsync(Driver driver)
    {
        driver.CreatedAt = DateTime.UtcNow;
        driver.IsAvailable = true;

        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();

        return driver;
    }

    // Update driver
    public async Task<Driver?> UpdateDriverAsync(Driver driver)
    {
        var existingDriver = await _context.Drivers.FindAsync(driver.Id);
        if (existingDriver == null)
            return null;

        existingDriver.Name = driver.Name;
        existingDriver.License = driver.License;
        existingDriver.PhoneNumber = driver.PhoneNumber;
        existingDriver.Email = driver.Email;
        existingDriver.IsAvailable = driver.IsAvailable;

        await _context.SaveChangesAsync();
        return existingDriver;
    }

    // Delete driver
    public async Task<bool> DeleteDriverAsync(int id)
    {
        var driver = await _context.Drivers.FindAsync(id);
        if (driver == null)
            return false;

        // Unassign this driver from all shipments
        var shipments = await _context.Shipments
            .Where(s => s.DriverId == id && !s.IsDeleted)
            .ToListAsync();

        foreach (var shipment in shipments)
        {
            shipment.DriverId = null;
        }

        _context.Drivers.Remove(driver);
        await _context.SaveChangesAsync();

        return true;
    }

    // Get drivers by shipment count
    public async Task<List<(Driver Driver, int ShipmentCount)>> GetDriversWithShipmentCountAsync()
    {
        var drivers = await _context.Drivers
            .Include(d => d.Shipments.Where(s => !s.IsDeleted))
            .ToListAsync();

        return drivers
            .Select(d => (d, d.Shipments.Count))
            .OrderByDescending(x => x.Item2)
            .ToList();
    }

    // Validate driver license uniqueness
    public async Task<bool> IsLicenseUniqueAsync(string license, int? excludeDriverId = null)
    {
        return !await _context.Drivers
            .Where(d => d.License == license && (!excludeDriverId.HasValue || d.Id != excludeDriverId))
            .AnyAsync();
    }
}
