using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AssignmentRepository : SqlRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(DbContext context) : base(context)
        {
        }
    }
}
