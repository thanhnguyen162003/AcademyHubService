using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ZoneRepository : SqlRepository<Zone>, IZoneRepository
    {
        public ZoneRepository(DbContext context) : base(context)
        {
        }
    }
}
