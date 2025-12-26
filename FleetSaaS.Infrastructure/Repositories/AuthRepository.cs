using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Infrastructure.Data;

namespace FleetSaaS.Infrastructure.Repositories
{
    public class AuthRepository(ApplicationDbContext dbContext) : IAuthRepository
    {
    }
}
