using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsPortal.Models;

public class Shipment
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string TrackingId { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Origin { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Destination { get; set; } = null!;

    [Required]
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;

    [Required]
    public DateTime EstimatedDelivery { get; set; }

    [ForeignKey("Driver")]
    public int? DriverId { get; set; }

    public Driver? Driver { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;

    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
