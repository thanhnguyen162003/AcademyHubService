using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using static Domain.Enums.ZoneEnums;

namespace Infrastructure.Repositories
{
    public class ZoneMembershipRepository : SqlRepository<ZoneMembership>, IZoneMembershipRepository
    {
        public ZoneMembershipRepository(DbContext context) : base(context)
        {
        }

        public async Task<bool> IsAdminZone(Guid userId, Guid zoneId)
        {
            return await _dbSet.AnyAsync(x => x.UserId.Equals(userId) && x.ZoneId.Equals(zoneId) && x.Type.Equals(ZoneMembershipType.Admin.ToString()));
        }

    }
}
