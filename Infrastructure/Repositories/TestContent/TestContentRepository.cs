using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TestContentRepository : SqlRepository<TestContent>, ITestContentRepository
    {
        private readonly DbContext _context;
        public TestContentRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task CreateTestContent(List<TestContent> test)
        {
            await _dbSet.AddRangeAsync(test);
        }
        public async Task Delete(IEnumerable<TestContent> testContents)
        {
            _dbSet.RemoveRange(testContents);

            await Task.CompletedTask;
        }
    }
}
