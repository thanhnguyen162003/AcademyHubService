using Domain.Entity;
using Domain.Models.Common;
using Infrastructure.Extensions;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories
{
    public class ZoneRepository : SqlRepository<Zone>, IZoneRepository
    {
        public ZoneRepository(DbContext context) : base(context)
        { 
        }

        public async Task<PagedList<Zone>> GetZoneForStudent(int page, int pageSize, string? search, bool isAscending)
        {
            var query = _dbSet.AsQueryable();
            if (!search.IsNullOrEmpty())
            {
                query = _dbSet.Where(x => x.Name.Contains(search) || x.Description.Contains(search));
            }
            if (isAscending)
            {
                query = query.OrderBy(x => x.CreatedAt); 
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedAt); 
            }
            return await query.ToPagedListAsync(page, pageSize);
        }

        public async Task<Zone> GetZoneDetail(Guid zoneId)
        {
            var query = _dbSet
            .AsNoTracking()
            .Where(x =>
                x.Id.Equals(zoneId) &&
                x.DeletedAt == null
            )
            .Include(x => x.Assignments)
            .Include(x => x.ZoneBans)
            .Include(x => x.PendingZoneInvites)
            .Include(x => x.ZoneMemberships);
            var result = await query.FirstOrDefaultAsync();
            return result;
        }

    }
}

