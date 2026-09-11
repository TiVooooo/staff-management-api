using Microsoft.AspNetCore.Mvc;
using StaffManagement.Common.Requests;
using StaffManagement.Service.Services;

namespace StaffManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTask([FromQuery] string? term)
    {
        var result = await _taskService.GetAllTask(term);
        return Ok(result);
    }

    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetTaskId(int taskId)
    {
        var result = await _taskService.GetTaskId(taskId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
    {
        var result = await _taskService.CreateTask(request);
        return Ok(result);
    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(int taskId, [FromBody] UpdateTaskRequest request)
    {
        var result = await _taskService.UpdateTask(taskId, request);
        return Ok(result);
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        var result = await _taskService.DeleteTask(taskId);
        return Ok(result);
    }
}
