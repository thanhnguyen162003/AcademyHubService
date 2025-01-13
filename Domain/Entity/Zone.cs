using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class Zone
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? LogoUrl { get; set; }

    public string? BannerUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<PendingZoneInvite> PendingZoneInvites { get; set; } = new List<PendingZoneInvite>();

    public virtual ICollection<ZoneBan> ZoneBans { get; set; } = new List<ZoneBan>();

    public virtual ICollection<ZoneMembership> ZoneMemberships { get; set; } = new List<ZoneMembership>();
}
