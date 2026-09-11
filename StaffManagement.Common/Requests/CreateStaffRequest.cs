namespace StaffManagement.Common.Requests;

public class CreateStaffRequest
{
    public string FullName { get; set; } = null!;
    public string ShortName { get; set; } = null!;
}
