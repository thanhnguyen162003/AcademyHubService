using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AssignmentRepository : SqlRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(DbContext context) : base(context)
        {
        }
    }
}
