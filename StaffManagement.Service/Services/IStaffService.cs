using StaffManagement.Service.Base;
using StaffManagement.Common.Requests;

namespace StaffManagement.Service.Services;

public interface IStaffService 
{
    Task<IBusinessResult> GetAllStaff(string? term = null);
    Task<IBusinessResult> GetStaffId(int staffId);
    Task<IBusinessResult> CreateStaff(CreateStaffRequest request);
    Task<IBusinessResult> UpdateStaff(int staffId, UpdateStaffRequest request);
    Task<IBusinessResult> DeleteStaff(int staffId);
}
