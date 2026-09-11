using StaffManagement.Data.Base;
using StaffManagement.Data.Context;
using StaffManagement.Data.Entities;

namespace StaffManagement.Data.Repository;

public sealed class StaffRepository : GenericRepository<Staff>, IStaffRepository
{
    public StaffRepository(StaffManagementContext context) : base(context) { }
}
