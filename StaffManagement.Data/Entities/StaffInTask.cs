using System;
using System.Collections.Generic;

namespace StaffManagement.Data.Entities;

public partial class StaffInTask
{
    public int Id { get; set; }

    public int Idstaff { get; set; }

    public int Idtask { get; set; }

    public virtual Staff IdstaffNavigation { get; set; } = null!;

    public virtual Task IdtaskNavigation { get; set; } = null!;
}
