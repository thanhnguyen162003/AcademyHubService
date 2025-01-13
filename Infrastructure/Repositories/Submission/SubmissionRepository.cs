using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SubmissionRepository : SqlRepository<Submission>, ISubmissionRepository
    {
        public SubmissionRepository(DbContext context) : base(context)
        {
        }
    }
}
