using System;
using System.Collections.Generic;

namespace StaffManagement.Data.Entities;

public partial class Task
{
    public int Id { get; set; }

    public int? Idparent { get; set; }

    public string Label { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? Duration { get; set; }

    public int Progress { get; set; }

    public bool IsUnscheduled { get; set; }

    public virtual Task? IdparentNavigation { get; set; }

    public virtual ICollection<Task> InverseIdparentNavigation { get; set; } = new List<Task>();

    public virtual ICollection<StaffInTask> StaffInTasks { get; set; } = new List<StaffInTask>();
}
