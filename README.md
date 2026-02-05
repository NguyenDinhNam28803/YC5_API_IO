# YC5_API_IO

YC5_API_IO là một Web API được xây dựng bằng .NET 9, cung cấp các dịch vụ cho ứng dụng quản lý công việc (Task Management). Project bao gồm các tính năng xác thực người dùng, quản lý công việc, danh mục, nhãn và nhiều tính năng hỗ trợ khác.

## Công nghệ sử dụng

- **Framework:** .NET 9.0 (ASP.NET Core API)
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Authentication:** JWT (JSON Web Token)
- **Hashing:** BCrypt.Net-Next
- **Excel Library:** EPPlus
- **Documentation:** OpenAPI (Swagger)
- **Containerization:** Docker

## Tính năng chính

- **Quản lý người dùng:** Đăng ký, đăng nhập và xác thực qua JWT.
- **Quản lý công việc (Tasks):** Tạo, cập nhật, xóa và theo dõi trạng thái công việc (InProgress, Completed) cùng mức độ ưu tiên (Low, Medium, High).
- **Phân loại & Nhãn:** Tổ chức công việc theo danh mục (Categories) và nhãn (Tags).
- **Tương tác:** Cho phép thêm bình luận (Comments) vào các công việc.
- **Công việc con (Sub-tasks):** Hỗ trợ cấu trúc công việc phân cấp.
- **Đếm ngược (Countdowns):** Tính năng theo dõi thời gian.
- **Thông báo Email:** Tích hợp gửi email qua SMTP.
- **Xuất nhập dữ liệu:** Hỗ trợ xử lý file Excel thông qua EPPlus.

## Cấu trúc thư mục

```
YC5_API_IO/
├── Controllers/    # Các API endpoints
├── Data/           # DbContext và cấu hình Database
├── Interfaces/     # Các Interface (ví dụ: IJwtInterface)
├── Models/         # Các Entity models (User, Task, Category, v.v.)
├── Services/       # Logic xử lý nghiệp vụ (ví dụ: JWTService)
├── Properties/     # Cấu hình khởi chạy
└── Program.cs      # File cấu hình chính của ứng dụng
```

## Cài đặt và Chạy project

### Yêu cầu hệ thống

- .NET 9.0 SDK
- SQL Server
- Visual Studio 2022 hoặc VS Code

### Các bước thực hiện

1. **Clone project:**
   ```bash
   git clone <repository_url>
   cd <project_folder>
   ```

2. **Cấu hình Database:**
   Cập nhật chuỗi kết nối trong file `YC5_API_IO/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=<Your_Server>;Database=YC5_THUCTAP_API;Trusted_Connection=True;..."
   }
   ```

3. **Cấu hình JWT & Email:**
   Thay đổi các thông tin trong `JwtSettings` và `EmailSettings` trong file `appsettings.json` cho phù hợp với môi trường của bạn.

4. **Chạy Migration:**
   ```bash
   dotnet ef database update
   ```

5. **Chạy ứng dụng:**
   ```bash
   dotnet run --project YC5_API_IO
   ```
   Ứng dụng sẽ mặc định chạy tại `https://localhost:7157` (hoặc cổng được cấu hình trong `launchSettings.json`). Truy cập `/swagger` hoặc dùng công cụ hỗ trợ OpenAPI để xem tài liệu API.

## Tác giả
- Nam Nguyễn (nguyendinhnam241209@gmail.com)

---
*Dự án đang trong quá trình phát triển.*
---
*Dự án đang trong quá trình phát triển.*
