using StaffManagement.Common;
using StaffManagement.Common.Requests;
using StaffManagement.Common.Responses;
using StaffManagement.Data.UnitOfWork;
using StaffManagement.Service.Base;
using TaskEntity = StaffManagement.Data.Entities.Task;

namespace StaffManagement.Service.Services;

public sealed class TaskService : ITaskService
{
    private readonly UnitOfWork _unitOfWork;

    public TaskService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IBusinessResult> GetAllTask(string? term = null)
    {
        var tasks = new List<TaskEntity>();

        if (string.IsNullOrWhiteSpace(term))
        {
            tasks = _unitOfWork.TaskRepository.GetAll().ToList();
        }
        else
        {
            term = term.Trim();

            tasks = _unitOfWork.TaskRepository.FindByCondition(t =>
                t.Label.Contains(term) || t.Type.Contains(term) || t.Name.Contains(term)).ToList();
        }

        var taskResponses = tasks.Select(t => new TaskResponse
        {
            Id = t.Id,
            Idparent = t.Idparent,
            Label = t.Label,
            Type = t.Type,
            Name = t.Name,
            StartDate = t.StartDate,
            EndDate = t.EndDate,
            Duration = t.Duration,
            Progress = t.Progress,
            IsUnscheduled = t.IsUnscheduled
        }).ToList();

        return new BusinessResult(Const.SUCCESS_READ, Const.SUCCESS_READ_MSG, taskResponses);
    }

    public async Task<IBusinessResult> GetTaskId(int taskId)
    {
        var task = _unitOfWork.TaskRepository.GetById(taskId);

        if (task == null)
        {
            return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
        }

        var taskResponse = new TaskResponse
        {
            Id = task.Id,
            Idparent = task.Idparent,
            Label = task.Label,
            Type = task.Type,
            Name = task.Name,
            StartDate = task.StartDate,
            EndDate = task.EndDate,
            Duration = task.Duration,
            Progress = task.Progress,
            IsUnscheduled = task.IsUnscheduled
        };

        return new BusinessResult(Const.SUCCESS_READ, Const.SUCCESS_READ_MSG, taskResponse);
    }

    public async Task<IBusinessResult> CreateTask(CreateTaskRequest request)
    {
        var validationMessage = ValidateTaskRequest(request.Label, request.Type, request.Name,
            request.StartDate, request.EndDate, request.Duration, request.Progress);

        if (validationMessage != null)
        {
            return new BusinessResult(Const.FAIL_CREATE, validationMessage);
        }

        var parentId = request.Idparent == 0 ? null : request.Idparent;

        if (parentId.HasValue)
        {
            var parentTask = _unitOfWork.TaskRepository.GetById(parentId.Value);

            if (parentTask == null)
            {
                return new BusinessResult(Const.FAIL_CREATE,
                    "Không thể tạo task vì không tìm thấy task cha.");
            }
        }

        var task = new TaskEntity
        {
            Idparent = parentId,
            Label = request.Label,
            Type = request.Type,
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Duration = request.Duration,
            Progress = request.Progress,
            IsUnscheduled = request.IsUnscheduled
        };

        var result = await _unitOfWork.TaskRepository.CreateAsync(task);

        if (result > 0)
        {
            return new BusinessResult(Const.SUCCESS_CREATE, Const.SUCCESS_CREATE_MSG);
        }

        return new BusinessResult(Const.FAIL_CREATE, Const.FAIL_CREATE_MSG);
    }

    public async Task<IBusinessResult> UpdateTask(int taskId, UpdateTaskRequest request)
    {
        var task = _unitOfWork.TaskRepository.GetById(taskId);

        if (task == null)
        {
            return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
        }

        var validationMessage = ValidateTaskRequest(request.Label, request.Type, request.Name,
            request.StartDate, request.EndDate, request.Duration, request.Progress);

        if (validationMessage != null)
        {
            return new BusinessResult(Const.FAIL_UPDATE, validationMessage);
        }

        var parentId = request.Idparent == 0 ? null : request.Idparent;

        if (parentId == taskId)
        {
            return new BusinessResult(Const.FAIL_UPDATE,
                "Không thể cập nhật task vì task không thể là task cha của chính nó.");
        }

        if (parentId.HasValue)
        {
            var parentTask = _unitOfWork.TaskRepository.GetById(parentId.Value);

            if (parentTask == null)
            {
                return new BusinessResult(Const.FAIL_UPDATE,
                    "Không thể cập nhật task vì không tìm thấy task cha.");
            }

            while (parentTask != null)
            {
                if (parentTask.Id == taskId)
                {
                    return new BusinessResult(Const.FAIL_UPDATE,
                        "Không thể cập nhật task vì quan hệ task cha tạo thành vòng lặp.");
                }

                if (!parentTask.Idparent.HasValue)
                {
                    break;
                }

                parentTask = _unitOfWork.TaskRepository.GetById(parentTask.Idparent.Value);
            }
        }

        task.Idparent = parentId;
        task.Label = request.Label;
        task.Type = request.Type;
        task.Name = request.Name;
        task.StartDate = request.StartDate;
        task.EndDate = request.EndDate;
        task.Duration = request.Duration;
        task.Progress = request.Progress;
        task.IsUnscheduled = request.IsUnscheduled;

        var result = await _unitOfWork.TaskRepository.UpdateAsync(task);

        if (result > 0)
        {
            return new BusinessResult(Const.SUCCESS_UPDATE, Const.SUCCESS_UPDATE_MSG);
        }

        return new BusinessResult(Const.FAIL_UPDATE, Const.FAIL_UPDATE_MSG);
    }

    public async Task<IBusinessResult> DeleteTask(int taskId)
    {
        var task = _unitOfWork.TaskRepository.GetById(taskId);

        if (task == null)
        {
            return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
        }

        var staffInTasks = _unitOfWork.StaffInTaskRepository
            .FindByCondition(s => s.Idtask == taskId).ToList();

        if (staffInTasks.Any())
        {
            return new BusinessResult(Const.FAIL_DELETE,
                "Không thể xóa task vì task đang được phân công cho nhân viên.");
        }

        var childTasks = _unitOfWork.TaskRepository
            .FindByCondition(t => t.Idparent == taskId).ToList();

        if (childTasks.Any())
        {
            return new BusinessResult(Const.FAIL_DELETE,
                "Không thể xóa task vì task đang có task con.");
        }

        var result = await _unitOfWork.TaskRepository.RemoveAsync(task);

        if (result)
        {
            return new BusinessResult(Const.SUCCESS_DELETE, Const.SUCCESS_DELETE_MSG);
        }

        return new BusinessResult(Const.FAIL_DELETE, Const.FAIL_DELETE_MSG);
    }

    private string? ValidateTaskRequest(string? label, string? type, string? name,
        DateTime? startDate, DateTime? endDate, decimal? duration, int progress)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            return "Label không được để trống.";
        }

        if (label.Trim().Length > 200)
        {
            return "Label không được vượt quá 200 ký tự.";
        }

        if (string.IsNullOrWhiteSpace(type))
        {
            return "Type không được để trống.";
        }

        if (type.Trim().Length > 50)
        {
            return "Type không được vượt quá 50 ký tự.";
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return "Tên task không được để trống.";
        }

        if (name.Trim().Length > 200)
        {
            return "Tên task không được vượt quá 200 ký tự.";
        }

        if (startDate.HasValue && endDate.HasValue && endDate < startDate)
        {
            return "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.";
        }

        if (duration.HasValue && (duration < 0 || duration > 99999999.99m || decimal.Round(duration.Value, 2) != duration.Value))
        {
            return "Duration phải từ 0 đến 99999999.99 và có tối đa 2 chữ số thập phân.";
        }

        if (progress < 0 || progress > 100)
        {
            return "Progress phải nằm trong khoảng từ 0 đến 100.";
        }

        return null;
    }
}
