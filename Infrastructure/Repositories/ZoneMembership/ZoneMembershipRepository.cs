using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Domain.Enums.ZoneEnums;

namespace Infrastructure.Repositories
{
    public class ZoneMembershipRepository : SqlRepository<ZoneMembership>, IZoneMembershipRepository
    {
        public ZoneMembershipRepository(DbContext context) : base(context)
        {
        }

        public async Task<bool> IsTeacherInZone(Guid userId, Guid zoneId)
        {
            return await _dbSet.AnyAsync(x => x.UserId.Equals(userId) && x.ZoneId.Equals(zoneId) && x.Type.Equals(ZoneMembershipType.Teacher.ToString()));
        }

        public async Task<ZoneMembership?> GetMembership(Guid userId, Guid zoneId, bool includeDeleted = false)
        {
            return await _dbSet.FirstOrDefaultAsync(x =>
                x.UserId.Equals(userId) &&
                x.ZoneId.Equals(zoneId) &&
                (includeDeleted || x.DeletedAt == null));
        }

        public async Task<bool> IsMembership(string email, Guid zoneId)
        {
            return await _dbSet.AnyAsync(x => x.ZoneId.Equals(zoneId) && x.Email!.Equals(email) && (x.DeletedAt == null));
        }

        public async Task<IEnumerable<string>> CheckMemberInZone(IEnumerable<string> emails, Guid zoneId)
        {
            return await _dbSet
                .Where(x => x.ZoneId.Equals(zoneId) && emails.Contains(x.Email!) && x.DeletedAt == null)
                .Select(x => x.Email!)
                .ToListAsync();
        }

    }
}
