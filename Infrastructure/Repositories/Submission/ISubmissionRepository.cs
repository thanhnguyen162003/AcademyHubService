using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;

namespace Infrastructure.Repositories
{
    public interface ISubmissionRepository : ISqlRepository<Submission>
    {
    }
}