using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TestContentRepository : SqlRepository<TestContent>, ITestContentRepository
    {
        public TestContentRepository(DbContext context) : base(context)
        {
        }
    }
}
