using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsPortal.Models;

public class AuditLog
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Shipment")]
    public int ShipmentId { get; set; }

    [Required]
    [StringLength(500)]
    public string Action { get; set; } = null!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public Shipment? Shipment { get; set; }
}
