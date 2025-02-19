using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;

namespace Infrastructure.Repositories
{
    public interface ITestContentRepository : ISqlRepository<TestContent>
    {
        Task<bool> CreateTestContent(List<TestContent> test);
    }
}
