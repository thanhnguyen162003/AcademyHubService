using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GroupRepository : SqlRepository<Group>, IGroupRepository
    {
        public GroupRepository(DbContext context) : base(context)
        {
        }
    }
}
