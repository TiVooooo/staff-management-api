using StaffManagement.Data.Repository;

namespace StaffManagement.Data.UnitOfWork;

public interface IUnitOfWork
{
    IStaffRepository StaffRepository { get; }
    ITaskRepository TaskRepository { get; }
    StaffInTaskRepository StaffInTaskRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChangesWithTransaction();
    Task<int> SaveChangesWithTransactionAsync();
}
