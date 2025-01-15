using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;

namespace Infrastructure.Repositories
{
    public interface IZoneMembershipRepository : ISqlRepository<ZoneMembership>
    {
        Task<bool> IsAdminZone(Guid userId, Guid zoneId);
    }
}