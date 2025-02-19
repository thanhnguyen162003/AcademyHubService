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

        public async Task<bool> CreateTestContent(List<TestContent> test)
        {
            try
            {
                await _dbSet.AddRangeAsync(test);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
