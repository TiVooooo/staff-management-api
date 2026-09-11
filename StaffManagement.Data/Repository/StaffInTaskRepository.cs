using StaffManagement.Data.Base;
using StaffManagement.Data.Context;
using StaffManagement.Data.Entities;

namespace StaffManagement.Data.Repository;

public sealed class StaffInTaskRepository : GenericRepository<StaffInTask>
{
    public StaffInTaskRepository(StaffManagementContext context) : base(context) { }
}
