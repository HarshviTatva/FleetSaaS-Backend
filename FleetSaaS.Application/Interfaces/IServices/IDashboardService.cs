using FleetSaaS.Application.DTOs.Response;

namespace FleetSaaS.Application.Interfaces.IServices
{
    public interface IDashboardService
    {
        Task<DashboardResponse> GetCompanyUserDashboardDetails(Guid companyId);
        Task<List<ExpiryAlertDTO>> GetExpiryAlerts(Guid companyId);
        Task<DispatcherDashboardResponse> GetDispatcherDashboardDetails(Guid companyId);
        Task<DriverDashboardResponse> GetDriverDashboardDetails(Guid companyId);
    }
}
