using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Infrastructure.Common.Response;
using Microsoft.AspNetCore.Mvc;

namespace FleetSaaS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController(ICompanyService companyService) : ControllerBase
    {
        [HttpPost("register-company")]
        public async Task<IActionResult> CompanyUserRegister([FromBody] CompanyUserRegisterRequest companyUserRegisterRequest)
        {
            await companyService.CompanyRegisterAsync(companyUserRegisterRequest);
            return Ok(new SuccessApiResponse<object>(
                httpStatusCode: StatusCodes.Status201Created,
                message: new List<string> { MessageConstants.COMPANY_REGISTERED },
                data: null
            ));
        }
    }
}
