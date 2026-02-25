using LogisticsPortal.Models;
using LogisticsPortal.Services;

namespace LogisticsPortal.State;

/// <summary>
/// Scoped state container for managing shipment-related state across the application.
/// This helps keep the shipment counts and filters synchronized across different components.
/// </summary>
public class ShipmentState
{
    private readonly ShipmentService _shipmentService;
    private Dictionary<ShipmentStatus, int> _statusCounts = new();
    private List<Shipment> _shipments = new();
    private string _filterText = string.Empty;
    private ShipmentStatus? _statusFilter = null;
    private string _destinationFilter = string.Empty;

    public event Action? OnStateChanged;

    public ShipmentState(ShipmentService shipmentService)
    {
        _shipmentService = shipmentService;
    }

    // Properties
    public IReadOnlyDictionary<ShipmentStatus, int> StatusCounts => _statusCounts;
    public IReadOnlyList<Shipment> Shipments => _shipments;
    public string FilterText => _filterText;
    public ShipmentStatus? StatusFilter => _statusFilter;
    public string DestinationFilter => _destinationFilter;

    // Initialize the state
    public async Task InitializeAsync()
    {
        await RefreshShipmentsAsync();
        await RefreshStatusCountsAsync();
    }

    // Refresh shipments from the database
    public async Task RefreshShipmentsAsync()
    {
        _shipments = await _shipmentService.GetAllShipmentsAsync();
        NotifyStateChanged();
    }

    // Refresh status counts
    public async Task RefreshStatusCountsAsync()
    {
        _statusCounts = await _shipmentService.GetShipmentCountByStatusAsync();
        NotifyStateChanged();
    }

    // Set filter text (for tracking ID or origin search)
    public void SetFilterText(string text)
    {
        _filterText = text;
        NotifyStateChanged();
    }

    // Set status filter
    public void SetStatusFilter(ShipmentStatus? status)
    {
        _statusFilter = status;
        NotifyStateChanged();
    }

    // Set destination filter
    public void SetDestinationFilter(string destination)
    {
        _destinationFilter = destination;
        NotifyStateChanged();
    }

    // Clear all filters
    public void ClearFilters()
    {
        _filterText = string.Empty;
        _statusFilter = null;
        _destinationFilter = string.Empty;
        NotifyStateChanged();
    }

    // Get filtered shipments
    public List<Shipment> GetFilteredShipments()
    {
        var filtered = _shipments.Where(s => !s.IsDeleted).ToList();

        // Apply text filter
        if (!string.IsNullOrWhiteSpace(_filterText))
        {
            var searchTerm = _filterText.ToLower();
            filtered = filtered.Where(s =>
                s.TrackingId.ToLower().Contains(searchTerm) ||
                s.Origin.ToLower().Contains(searchTerm)
            ).ToList();
        }

        // Apply status filter
        if (_statusFilter.HasValue)
        {
            filtered = filtered.Where(s => s.Status == _statusFilter.Value).ToList();
        }

        // Apply destination filter
        if (!string.IsNullOrWhiteSpace(_destinationFilter))
        {
            var searchTerm = _destinationFilter.ToLower();
            filtered = filtered.Where(s =>
                s.Destination.ToLower().Contains(searchTerm)
            ).ToList();
        }

        return filtered;
    }

    // Notify all subscribers of state change
    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }

    // Get count for a specific status
    public int GetStatusCount(ShipmentStatus status)
    {
        return _statusCounts.TryGetValue(status, out var count) ? count : 0;
    }
}
