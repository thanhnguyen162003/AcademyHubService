using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ZoneBanRepository : SqlRepository<ZoneBan>, IZoneBanRepository
    {
        public ZoneBanRepository(DbContext context) : base(context)
        {
        }
    }
}
