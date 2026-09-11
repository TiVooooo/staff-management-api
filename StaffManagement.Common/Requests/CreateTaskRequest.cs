namespace StaffManagement.Common.Requests;

public class CreateTaskRequest
{
    public int? Idparent { get; set; }
    public string Label { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Duration { get; set; }
    public int Progress { get; set; }
    public bool IsUnscheduled { get; set; }
}
