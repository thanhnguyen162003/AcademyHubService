using Domain.Entity;
using Domain.Models.Common;
using Infrastructure.Repositories.GenericRepository;

namespace Infrastructure.Repositories
{
    public interface ISubmissionRepository : ISqlRepository<Submission>
    {
        Task<PagedList<Submission>> GetSubmission(Guid assignmentId, int memberId, int page, int pageSize, bool isAscending);
    }
}