using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PendingZoneInviteRepository : SqlRepository<PendingZoneInvite>, IPendingZoneInviteRepository
    {
        public PendingZoneInviteRepository(DbContext context) : base(context)
        {
        }
    }
}
