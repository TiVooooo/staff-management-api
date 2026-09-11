using StaffManagement.Data.Base;
using StaffManagement.Data.Context;
using TaskEntity = StaffManagement.Data.Entities.Task;

namespace StaffManagement.Data.Repository;

public sealed class TaskRepository : GenericRepository<TaskEntity>, ITaskRepository
{
    public TaskRepository(StaffManagementContext context) : base(context) { }
}
