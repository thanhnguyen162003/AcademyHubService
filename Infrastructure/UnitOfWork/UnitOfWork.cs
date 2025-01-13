using Infrastructure.Persistence;
using Infrastructure.Repositories;
using StackExchange.Redis;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AcademyHubContext _context;

        public IAssignmentRepository _assignmentRepository = null!;
        public IGroupRepository _groupRepository = null!;
        public IPendingZoneInviteRepository _pendingZoneInviteRepository = null!;
        public ITestContentRepository _testContentRepository = null!;
        public ISubmissionRepository _submissionRepository = null!;
        public IZoneRepository _zoneRepository = null!;
        public IZoneBanRepository _zoneBanRepository = null!;
        public IZoneMembershipRepository _zoneMembershipRepository = null!;

        public UnitOfWork(AcademyHubContext context, IConnectionMultiplexer connectionMultiplexer)
        {
            _context = context;
        }

        public IAssignmentRepository AssignmentRepository => _assignmentRepository ?? new AssignmentRepository(_context);
        public IGroupRepository GroupRepository => _groupRepository ?? new GroupRepository(_context);
        public IPendingZoneInviteRepository PendingZoneInviteRepository => _pendingZoneInviteRepository ?? new PendingZoneInviteRepository(_context);
        public ITestContentRepository TestContentRepository => _testContentRepository ?? new TestContentRepository(_context);
        public ISubmissionRepository SubmissionRepository => _submissionRepository ?? new SubmissionRepository(_context);
        public IZoneRepository ZoneRepository => _zoneRepository ?? new ZoneRepository(_context);
        public IZoneBanRepository ZoneBanRepository => _zoneBanRepository ?? new ZoneBanRepository(_context);
        public IZoneMembershipRepository ZoneMembershipRepository => _zoneMembershipRepository ?? new ZoneMembershipRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public Task BeginTransaction()
        {
            _context.Database.BeginTransaction();

            return Task.CompletedTask;
        }

        public async Task<bool> CommitTransaction()
        {
            try
            {
                await _context.Database.CommitTransactionAsync();

                return true;
            } catch
            {
                return false;
            }

        }

    }
}
