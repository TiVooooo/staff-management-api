using System;
using System.Collections.Generic;

namespace StaffManagement.Data.Entities;

public partial class Staff
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public virtual ICollection<StaffInTask> StaffInTasks { get; set; } = new List<StaffInTask>();
}
