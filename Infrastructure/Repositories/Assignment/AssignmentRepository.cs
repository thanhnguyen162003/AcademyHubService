using Domain.Entity;
using Domain.Models.Common;
using Infrastructure.Extensions;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories
{
    public class AssignmentRepository : SqlRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(DbContext context) : base(context)
        {

        }
        public async Task<PagedList<Assignment>> GetAssignment(int page, int pageSize, string? search, bool isAscending)
        {
            var query = _dbSet.AsQueryable();
            if (!search.IsNullOrEmpty())
            {
                query = _dbSet.Where(x => x.Title.Contains(search) || x.Type.Contains(search) || x.Noticed.Contains(search));
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

        public async Task<Assignment> GetAssignmentDetail(Guid assignmentId)
        {
            var query = _dbSet
            .AsNoTracking()
            .Where(x =>
                x.Id.Equals(assignmentId)
            )
            .Include(x => x.Submissions)
            .Include(x => x.Questions);
            var result = await query.FirstOrDefaultAsync();
            return result;
        }
    }
}
