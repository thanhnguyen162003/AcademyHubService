using System;
using System.Collections.Generic;

namespace Domain.Entity;

public partial class Submission
{
    public Guid Id { get; set; }

    public int? MemberId { get; set; }

    public Guid? AssignmentId { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? SavedAt { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public double? Score { get; set; }

    public virtual Assignment? Assignment { get; set; }

    public virtual ZoneMembership? Member { get; set; }
}
