using LogisticsPortal.Models;

namespace LogisticsPortal.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(LogisticsContext context)
    {
        // Don't seed if data already exists
        if (context.Drivers.Any())
            return;

        var drivers = new[]
        {
            new Driver
            {
                Name = "John Smith",
                License = "DL123456",
                PhoneNumber = "+1-555-0101",
                Email = "john.smith@example.com",
                IsAvailable = true
            },
            new Driver
            {
                Name = "Sarah Johnson",
                License = "DL123457",
                PhoneNumber = "+1-555-0102",
                Email = "sarah.johnson@example.com",
                IsAvailable = true
            },
            new Driver
            {
                Name = "Mike Davis",
                License = "DL123458",
                PhoneNumber = "+1-555-0103",
                Email = "mike.davis@example.com",
                IsAvailable = false
            },
            new Driver
            {
                Name = "Emma Wilson",
                License = "DL123459",
                PhoneNumber = "+1-555-0104",
                Email = "emma.wilson@example.com",
                IsAvailable = true
            },
            new Driver
            {
                Name = "Robert Brown",
                License = "DL123460",
                PhoneNumber = "+1-555-0105",
                Email = "robert.brown@example.com",
                IsAvailable = true
            }
        };

        context.Drivers.AddRange(drivers);
        await context.SaveChangesAsync();

        var shipments = new[]
        {
            new Shipment
            {
                TrackingId = "SHIP001",
                Origin = "New York, NY",
                Destination = "Los Angeles, CA",
                Status = ShipmentStatus.InTransit,
                EstimatedDelivery = DateTime.UtcNow.AddDays(3),
                DriverId = drivers[0].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Shipment
            {
                TrackingId = "SHIP002",
                Origin = "Chicago, IL",
                Destination = "Houston, TX",
                Status = ShipmentStatus.Delivered,
                EstimatedDelivery = DateTime.UtcNow.AddDays(-1),
                DriverId = drivers[1].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Shipment
            {
                TrackingId = "SHIP003",
                Origin = "Phoenix, AZ",
                Destination = "Philadelphia, PA",
                Status = ShipmentStatus.Pending,
                EstimatedDelivery = DateTime.UtcNow.AddDays(5),
                CreatedAt = DateTime.UtcNow
            },
            new Shipment
            {
                TrackingId = "SHIP004",
                Origin = "San Antonio, TX",
                Destination = "San Diego, CA",
                Status = ShipmentStatus.InTransit,
                EstimatedDelivery = DateTime.UtcNow.AddDays(2),
                DriverId = drivers[2].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Shipment
            {
                TrackingId = "SHIP005",
                Origin = "Dallas, TX",
                Destination = "San Jose, CA",
                Status = ShipmentStatus.Pending,
                EstimatedDelivery = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            },
            new Shipment
            {
                TrackingId = "SHIP006",
                Origin = "Austin, TX",
                Destination = "Jacksonville, FL",
                Status = ShipmentStatus.Delivered,
                EstimatedDelivery = DateTime.UtcNow.AddDays(-2),
                DriverId = drivers[3].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-8),
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Shipment
            {
                TrackingId = "SHIP007",
                Origin = "Fort Worth, TX",
                Destination = "Austin, TX",
                Status = ShipmentStatus.InTransit,
                EstimatedDelivery = DateTime.UtcNow.AddDays(1),
                DriverId = drivers[4].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Shipment
            {
                TrackingId = "SHIP008",
                Origin = "Columbus, OH",
                Destination = "Charlotte, NC",
                Status = ShipmentStatus.Pending,
                EstimatedDelivery = DateTime.UtcNow.AddDays(6),
                CreatedAt = DateTime.UtcNow
            },
            new Shipment
            {
                TrackingId = "SHIP009",
                Origin = "Indianapolis, IN",
                Destination = "Detroit, MI",
                Status = ShipmentStatus.Delivered,
                EstimatedDelivery = DateTime.UtcNow.AddDays(-3),
                DriverId = drivers[0].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new Shipment
            {
                TrackingId = "SHIP010",
                Origin = "Memphis, TN",
                Destination = "Boston, MA",
                Status = ShipmentStatus.InTransit,
                EstimatedDelivery = DateTime.UtcNow.AddDays(4),
                DriverId = drivers[1].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Shipment
            {
                TrackingId = "SHIP011",
                Origin = "Boston, MA",
                Destination = "Seattle, WA",
                Status = ShipmentStatus.Pending,
                EstimatedDelivery = DateTime.UtcNow.AddDays(8),
                CreatedAt = DateTime.UtcNow
            },
            new Shipment
            {
                TrackingId = "SHIP012",
                Origin = "Denver, CO",
                Destination = "Portland, OR",
                Status = ShipmentStatus.Delivered,
                EstimatedDelivery = DateTime.UtcNow.AddDays(-1),
                DriverId = drivers[3].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Shipment
            {
                TrackingId = "SHIP013",
                Origin = "Nashville, TN",
                Destination = "Las Vegas, NV",
                Status = ShipmentStatus.InTransit,
                EstimatedDelivery = DateTime.UtcNow.AddDays(3),
                DriverId = drivers[4].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Shipment
            {
                TrackingId = "SHIP014",
                Origin = "Baltimore, MD",
                Destination = "Miami, FL",
                Status = ShipmentStatus.Pending,
                EstimatedDelivery = DateTime.UtcNow.AddDays(4),
                CreatedAt = DateTime.UtcNow
            },
            new Shipment
            {
                TrackingId = "SHIP015",
                Origin = "Louisville, KY",
                Destination = "Atlanta, GA",
                Status = ShipmentStatus.Delivered,
                EstimatedDelivery = DateTime.UtcNow.AddDays(-4),
                DriverId = drivers[0].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-12),
                UpdatedAt = DateTime.UtcNow.AddDays(-4)
            },
            new Shipment
            {
                TrackingId = "SHIP016",
                Origin = "Portland, OR",
                Destination = "Denver, CO",
                Status = ShipmentStatus.InTransit,
                EstimatedDelivery = DateTime.UtcNow.AddDays(2),
                DriverId = drivers[1].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Shipment
            {
                TrackingId = "SHIP017",
                Origin = "Las Vegas, NV",
                Destination = "Phoenix, AZ",
                Status = ShipmentStatus.Pending,
                EstimatedDelivery = DateTime.UtcNow.AddDays(3),
                CreatedAt = DateTime.UtcNow
            },
            new Shipment
            {
                TrackingId = "SHIP018",
                Origin = "Atlanta, GA",
                Destination = "Nashville, TN",
                Status = ShipmentStatus.Delivered,
                EstimatedDelivery = DateTime.UtcNow.AddDays(-2),
                DriverId = drivers[2].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-6),
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Shipment
            {
                TrackingId = "SHIP019",
                Origin = "Miami, FL",
                Destination = "New York, NY",
                Status = ShipmentStatus.InTransit,
                EstimatedDelivery = DateTime.UtcNow.AddDays(5),
                DriverId = drivers[3].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new Shipment
            {
                TrackingId = "SHIP020",
                Origin = "Seattle, WA",
                Destination = "San Francisco, CA",
                Status = ShipmentStatus.Pending,
                EstimatedDelivery = DateTime.UtcNow.AddDays(2),
                CreatedAt = DateTime.UtcNow
            },
            new Shipment
            {
                TrackingId = "SHIP021",
                Origin = "San Francisco, CA",
                Destination = "Chicago, IL",
                Status = ShipmentStatus.Cancelled,
                EstimatedDelivery = DateTime.UtcNow.AddDays(10),
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            }
        };

        context.Shipments.AddRange(shipments);
        await context.SaveChangesAsync();

        // Add audit logs for some shipments
        await AddAuditLogsAsync(context, shipments);
    }

    private static async Task AddAuditLogsAsync(LogisticsContext context, Shipment[] shipments)
    {
        var auditLogs = new List<AuditLog>();

        foreach (var shipment in shipments)
        {
            auditLogs.Add(new AuditLog
            {
                ShipmentId = shipment.Id,
                Action = "Shipment created",
                Timestamp = shipment.CreatedAt
            });

            if (shipment.Status == ShipmentStatus.InTransit)
            {
                auditLogs.Add(new AuditLog
                {
                    ShipmentId = shipment.Id,
                    Action = "Status changed to In-Transit",
                    Timestamp = shipment.CreatedAt.AddHours(12)
                });
            }
            else if (shipment.Status == ShipmentStatus.Delivered)
            {
                auditLogs.Add(new AuditLog
                {
                    ShipmentId = shipment.Id,
                    Action = "Status changed to In-Transit",
                    Timestamp = shipment.CreatedAt.AddHours(12)
                });

                auditLogs.Add(new AuditLog
                {
                    ShipmentId = shipment.Id,
                    Action = "Status changed to Delivered",
                    Timestamp = shipment.UpdatedAt ?? shipment.CreatedAt.AddDays(4)
                });
            }
            else if (shipment.Status == ShipmentStatus.Cancelled)
            {
                auditLogs.Add(new AuditLog
                {
                    ShipmentId = shipment.Id,
                    Action = "Status changed to Cancelled",
                    Timestamp = shipment.CreatedAt.AddHours(6)
                });
            }
        }

        context.AuditLogs.AddRange(auditLogs);
        await context.SaveChangesAsync();
    }
}
