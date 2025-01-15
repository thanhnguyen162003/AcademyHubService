using Domain.CustomEntities;

namespace Domain.Entity;

public partial class Assignment : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public Guid ZoneId { get; set; }

    public string Type { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int? TotalQuestion { get; set; }

    public int? TotalTime { get; set; }

    public DateTime? AvailableAt { get; set; }

    public DateTime? DueAt { get; set; }

    public DateTime? LockedAt { get; set; }

    public bool? Published { get; set; }

    public virtual ICollection<TestContent> Questions { get; set; } = new List<TestContent>();

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

    public virtual Zone Zone { get; set; } = null!;
}
