using Microsoft.AspNetCore.Mvc;
using StaffManagement.Common.Requests;
using StaffManagement.Service.Services;

namespace StaffManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    [HttpGet]

    public async Task<IActionResult> GetAllStaff([FromQuery] string? term)
    {
        var result = await _staffService.GetAllStaff(term);
        return Ok(result);
    }

    [HttpGet("{staffId}")]
    public async Task<IActionResult> GetStaffId(int staffId)
    {
        var result = await _staffService.GetStaffId(staffId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStaff([FromBody] CreateStaffRequest request)
    {
        var result = await _staffService.CreateStaff(request);
        return Ok(result);
    }

    [HttpPut("{staffId}")]
    public async Task<IActionResult> UpdateStaff(int staffId, [FromBody] UpdateStaffRequest request)
    {
        var result = await _staffService.UpdateStaff(staffId, request);
        return Ok(result);
    }

    [HttpDelete("{staffId}")]
    public async Task<IActionResult> DeleteStaff(int staffId)
    {
        var result = await _staffService.DeleteStaff(staffId);
        return Ok(result);
    }

}
