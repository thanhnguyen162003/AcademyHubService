using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ZoneMembershipRepository : SqlRepository<ZoneMembership>, IZoneMembershipRepository
    {
        public ZoneMembershipRepository(DbContext context) : base(context)
        {
        }
    }
}
