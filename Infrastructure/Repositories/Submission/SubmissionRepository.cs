using Domain.Entity;
using Domain.Models.Common;
using Infrastructure.Extensions;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories
{
    public class SubmissionRepository : SqlRepository<Submission>, ISubmissionRepository
    {
        public SubmissionRepository(DbContext context) : base(context)
        {
        }
        public async Task<PagedList<Submission>> GetSubmission(Guid assignmentId, int memberId, int page, int pageSize, bool isAscending)
        {
            var query = _dbSet.AsQueryable()
                .Where(x => x.AssignmentId.Equals(assignmentId) && x.MemberId.Equals(memberId));

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
    }
}
