using System.ComponentModel.DataAnnotations;

namespace LogisticsPortal.Models;

public class Driver
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string License { get; set; } = null!;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public bool IsAvailable { get; set; } = true;

    public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
