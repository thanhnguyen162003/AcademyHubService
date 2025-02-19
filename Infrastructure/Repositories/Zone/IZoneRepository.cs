using Domain.Entity;
using Domain.Models.Common;
using Infrastructure.Repositories.GenericRepository;

namespace Infrastructure.Repositories
{
    public interface IZoneRepository : ISqlRepository<Zone>
    {
        Task<PagedList<Zone>> GetZoneForStudent(int page, int pageSize, string? search);
        Task<Zone> GetZoneDetail(Guid zoneId);
    }
}