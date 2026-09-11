using StaffManagement.Common.Requests;
using StaffManagement.Service.Base;

namespace StaffManagement.Service.Services;

public interface ITaskService
{
    Task<IBusinessResult> GetAllTask(string? term = null);
    Task<IBusinessResult> GetTaskId(int taskId);
    Task<IBusinessResult> CreateTask(CreateTaskRequest request);
    Task<IBusinessResult> UpdateTask(int taskId, UpdateTaskRequest request);
    Task<IBusinessResult> DeleteTask(int taskId);
}
