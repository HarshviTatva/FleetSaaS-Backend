using FleetSaaS.Application.DTOs.Request;

namespace FleetSaaS.Application.Interfaces.IServices
{
    public interface ICompanyService
    {
        Task<string> CompanyRegisterAsync(CompanyUserRegisterRequest companyUserRegisterRequest);
        Task<bool> IsCompanyRegistered(Guid companyId);
    }
}
