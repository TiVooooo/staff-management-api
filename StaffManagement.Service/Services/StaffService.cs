using StaffManagement.Common;
using StaffManagement.Common.Requests;
using StaffManagement.Common.Responses;
using StaffManagement.Data.Entities;
using StaffManagement.Data.UnitOfWork;
using StaffManagement.Service.Base;

namespace StaffManagement.Service.Services;

public sealed class StaffService : IStaffService
{
    private readonly UnitOfWork _unitOfWork;

    public StaffService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IBusinessResult> GetAllStaff(string? term = null)
    {
        var users = new List<Data.Entities.Staff>();

        if (string.IsNullOrWhiteSpace(term))
        {
            users = _unitOfWork.StaffRepository.GetAll().ToList();
        }
        else
        {
            term = term.Trim();

            users = _unitOfWork.StaffRepository.FindByCondition(s =>
                s.FullName.Contains(term) || s.ShortName.Contains(term)).ToList();
        }

        var staffResponses = users.Select(s => new StaffResponse
        {
            Id = s.Id,
            FullName = s.FullName,
            ShortName = s.ShortName
        }).ToList();

        return new BusinessResult(Const.SUCCESS_READ, Const.SUCCESS_READ_MSG, staffResponses);
    }
    public async Task<IBusinessResult> GetStaffId(int staffId)
    {
        var staff = _unitOfWork.StaffRepository.GetById(staffId);

        if (staff == null)
        {
            return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
        }

        var staffResponse = new StaffResponse
        {
            Id = staff.Id,
            FullName = staff.FullName,
            ShortName = staff.ShortName
        };

        return new BusinessResult(Const.SUCCESS_READ, Const.SUCCESS_READ_MSG, staffResponse);
    }

    public async Task<IBusinessResult> CreateStaff(CreateStaffRequest request)
    {
        var validationMessage = ValidateStaffRequest(request.FullName, request.ShortName);

        if (validationMessage != null)
        {
            return new BusinessResult(Const.FAIL_CREATE, validationMessage);
        }

        var staff = new Staff
        {
            FullName = request.FullName,
            ShortName = request.ShortName
        };

        var result = await _unitOfWork.StaffRepository.CreateAsync(staff);

        if (result > 0)
        {
            return new BusinessResult(Const.SUCCESS_CREATE, Const.SUCCESS_CREATE_MSG, staff);
        }

        return new BusinessResult(Const.FAIL_CREATE, Const.FAIL_CREATE_MSG);
    }

    public async Task<IBusinessResult> UpdateStaff(int staffId, UpdateStaffRequest request)
    {
        var staff = _unitOfWork.StaffRepository.GetById(staffId);

        if (staff == null)
        {
            return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
        }

        var validationMessage = ValidateStaffRequest(request.FullName, request.ShortName);

        if (validationMessage != null)
        {
            return new BusinessResult(Const.FAIL_UPDATE, validationMessage);
        }

        staff.FullName = request.FullName;
        staff.ShortName = request.ShortName;

        var result = await _unitOfWork.StaffRepository.UpdateAsync(staff);

        if (result > 0)
        {
            return new BusinessResult(Const.SUCCESS_UPDATE, Const.SUCCESS_UPDATE_MSG, staff);
        }

        return new BusinessResult(Const.FAIL_UPDATE, Const.FAIL_UPDATE_MSG);
    }

    public async Task<IBusinessResult> DeleteStaff(int staffId)
    {
        var staff = _unitOfWork.StaffRepository.GetById(staffId);

        if (staff == null)
        {
            return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
        }

        var staffInTasks = _unitOfWork.StaffInTaskRepository
            .FindByCondition(s => s.Idstaff == staffId).ToList();

        if (staffInTasks.Any())
        {
            return new BusinessResult(Const.FAIL_DELETE,
                "Không thể xóa nhân viên vì nhân viên đang được phân công cho task.");
        }

        var result = await _unitOfWork.StaffRepository.RemoveAsync(staff);

        if (result)
        {
            return new BusinessResult(Const.SUCCESS_DELETE, Const.SUCCESS_DELETE_MSG);
        }

        return new BusinessResult(Const.FAIL_DELETE, Const.FAIL_DELETE_MSG);
    }

    private string? ValidateStaffRequest(string? fullName, string? shortName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return "Họ và tên không được để trống.";
        }

        if (fullName.Trim().Length > 100)
        {
            return "Họ và tên không được vượt quá 100 ký tự.";
        }

        if (string.IsNullOrWhiteSpace(shortName))
        {
            return "Tên ngắn không được để trống.";
        }

        if (shortName.Trim().Length > 50)
        {
            return "Tên ngắn không được vượt quá 50 ký tự.";
        }

        return null;
    }
}
