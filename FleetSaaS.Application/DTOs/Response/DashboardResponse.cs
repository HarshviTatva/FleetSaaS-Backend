using FleetSaaS.Domain.Enum;

namespace FleetSaaS.Application.DTOs.Response
{
    public class DashboardResponse
    {
        public int TotalVehicleCount {  get; set; }

        public int TotalActiveVehicleCount { get; set; }

        public int TotalPlannedTrips { get; set; }

        public int TotalDriversCount { get; set; }

        public int TotalCompletedTrips { get; set; }

        public int TotalTrips { get; set; }

        public int TotalOngoingTrips { get; set; }

        public List<ExpiryAlertDTO>? LicenseExpiryAlerts { get; set; }

    }
    public class ExpiryAlertDTO
    {
        public string Type { get; set; } = "";        // DriverLicense / VehicleInsurance
        public string Reference { get; set; } = "";   // LicenseNumber / Plate
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired { get; set; }
    }

    public class DispatcherDashboardResponse
    {
        public int ActiveTrips { get; set; }
        public int UpcomingTrips { get; set; }
        public DriverSummaryResponse Drivers { get; set; } = default!;
        public List<ActiveTripResponse> Trips { get; set; } = new();
        public List<UpcomingTripResponse> Upcoming { get; set; } = new();
    }
    public class ActiveTripResponse
    {
        public string TripNo { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string DriverName { get; set; } = default!;
        public string Vehicle { get; set; } = default!;
    }

    public class DriverSummaryResponse
    {
        public int Total { get; set; }
        public int Available { get; set; }
        public int OnTrip { get; set; }
        public int Unavailable { get; set; }
    }
    public class UpcomingTripResponse
    {
        public Guid Id { get; set; }
        public string TripNo { get; set; } 
        public string Time { get; set; } 
        public string Route { get; set; } 
    }

    public class DriverDashboardResponse
    {
        public int TotalTrips { get; set; }
        public int CompletedTripsCount { get; set; }
        public int AcceptedTripsCount { get; set; }
        public int CancelledTripsCount { get; set; }
        public List<DriverTodayTrip> TodayTrips { get; set; }
        public List<DriverCompletedTrips> CompletedTrips { get; set; }
    }

    public class DriverTodayTrip
    {
        public string TripCode { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public TripStatus Status { get; set; }
        public DateTime? ScheduledAt { get; set; }
    }

    public class DriverCompletedTrips
    {
        public Guid Id { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
