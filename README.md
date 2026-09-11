# StaffManagement

Solution độc lập gồm API, Common, Data và Service, theo cấu trúc SeniorEssentials.

## Database-first

Mở solution bằng Visual Studio, đặt project `StaffManagement` làm Startup Project, rồi chạy trong **Package Manager Console**:

```powershell
Scaffold-DbContext "<connection-string-database>" Microsoft.EntityFrameworkCore.SqlServer -Project StaffManagement.Data -StartupProject StaffManagement -Context StaffManagementContext -ContextDir Context -OutputDir Entities -NoOnConfiguring -Force
```

Lệnh sẽ sinh entity `Staff`, `Task` và context từ database. Khung endpoint nằm ở hai controller và cố ý trả `501 Not Implemented` vì chưa triển khai CRUD.
