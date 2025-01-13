using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class PendingZoneInvite
{
    public int Id { get; set; }

    public Guid? ZoneId { get; set; }

    public string Email { get; set; } = null!;

    public string Type { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Zone? Zone { get; set; }
}
