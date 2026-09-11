using StaffManagement.Data.Base;
using TaskEntity = StaffManagement.Data.Entities.Task;

namespace StaffManagement.Data.Repository;

public interface ITaskRepository : IGenericRepository<TaskEntity> { }
