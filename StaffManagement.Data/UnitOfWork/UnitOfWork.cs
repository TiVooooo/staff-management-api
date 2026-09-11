using StaffManagement.Data.Context;
using StaffManagement.Data.Repository;

namespace StaffManagement.Data.UnitOfWork;

/// <summary>Tập trung repository và transaction, theo pattern SeniorEssentials.</summary>
public sealed class UnitOfWork : IUnitOfWork
{
    public StaffManagementContext _unitOfWorkContext;
    private StaffRepository? _staffRepository;
    private TaskRepository? _taskRepository;
    private StaffInTaskRepository? _staffInTaskRepository;

    public UnitOfWork(StaffManagementContext unitOfWorkContext) =>
        _unitOfWorkContext = unitOfWorkContext ?? throw new ArgumentNullException(nameof(unitOfWorkContext));

    public IStaffRepository StaffRepository => _staffRepository ??= new StaffRepository(_unitOfWorkContext);
    public ITaskRepository TaskRepository => _taskRepository ??= new TaskRepository(_unitOfWorkContext);
    public StaffInTaskRepository StaffInTaskRepository => _staffInTaskRepository ??= new StaffInTaskRepository(_unitOfWorkContext);
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _unitOfWorkContext.SaveChangesAsync(cancellationToken);

    public int SaveChangesWithTransaction()
    {
        using var transaction = _unitOfWorkContext.Database.BeginTransaction();
        try
        {
            var result = _unitOfWorkContext.SaveChanges();
            transaction.Commit();
            return result;
        }
        catch
        {
            transaction.Rollback();
            return -1;
        }
    }

    public async Task<int> SaveChangesWithTransactionAsync()
    {
        await using var transaction = await _unitOfWorkContext.Database.BeginTransactionAsync();
        try
        {
            var result = await _unitOfWorkContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            return -1;
        }
    }
}
