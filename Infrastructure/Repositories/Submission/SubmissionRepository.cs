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
        public async Task<Submission> GetSubmission(Guid assignmentId, int memberId)
        {
            var query = await _dbSet.AsQueryable()
                .FirstOrDefaultAsync(x => x.AssignmentId.Equals(assignmentId) && x.MemberId.Equals(memberId));
            return query;
        }
    }
}
