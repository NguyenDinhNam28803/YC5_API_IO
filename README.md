# YC5_API_IO - Hệ thống Quản lý Công việc (Backend API)

YC5_API_IO là một giải pháp Web API mạnh mẽ được xây dựng trên nền tảng .NET 9, thiết kế để cung cấp các dịch vụ backend cho ứng dụng quản lý công việc (Task Management). Project được xây dựng với kiến trúc hướng đối tượng, hỗ trợ phân cấp công việc, tương tác người dùng và các tính năng theo dõi thời gian.

## 🚀 Công nghệ sử dụng

- **Runtime:** .NET 9.0 (ASP.NET Core Web API)
- **Cơ sở dữ liệu:** Microsoft SQL Server
- **ORM:** Entity Framework Core 9.0.12
- **Bảo mật & Xác thực:**
  - JWT Bearer Authentication (JSON Web Token)
  - BCrypt.Net-Next cho mã hóa mật khẩu
  - System.IdentityModel.Tokens.Jwt
- **Tiện ích:**
  - EPPlus (Xử lý Excel chuyên nghiệp)
  - OpenAPI/Swagger (Tài liệu hóa API)
- **DevOps:** Docker hỗ trợ môi trường container.

## 🏗️ Kiến trúc dữ liệu (Data Models)

Hệ thống bao gồm các thực thể chính với các thuộc tính chi tiết:

### 1. Người dùng (User)
- `UserId`: Khóa chính.
- `UserName`, `Email`, `PhoneNumber`: Thông tin định danh.
- `PasswordHasshed`: Mật khẩu đã được mã hóa.
- `CreatedAt`, `LastUpdatedAt`: Theo dõi thời gian tạo và cập nhật.
- **Quan hệ:** Một người dùng có thể có nhiều Danh mục, Công việc và Bộ đếm ngược.

### 2. Công việc (Task)
- `TaskId`: Khóa chính.
- `TaskName`, `TaskDescription`: Thông tin chi tiết công việc.
- `TaskStatus`: Trạng thái (`InProgress`, `Completed`).
- `Status` (Priority): Mức độ ưu tiên (`Low`, `Medium`, `High`).
- `DueDate`, `CompletedAt`: Quản lý thời hạn.
- **Tính năng đặc biệt:** Hỗ trợ `ParentTaskId` để tạo cấu trúc công việc con (Sub-tasks) không giới hạn cấp.
- **Quan hệ:** Gắn liền với Category, User, Tags, và Comments.

### 3. Danh mục (Category)
- `CategoryId`, `CategoryName`, `CategoryDescription`.
- `Color`: Mã màu để phân loại trực quan (Mặc định: "Gray").

### 4. Thành phần khác
- **Comment:** Hỗ trợ trao đổi trong từng công việc.
- **Tag:** Nhãn dán linh hoạt để lọc công việc.
- **Role:** Hệ thống phân quyền (Admin, User, v.v.).
- **CountDown:** Bộ đếm ngược cho các sự kiện quan trọng.

## ⚙️ Cấu hình hệ thống

Project sử dụng file `appsettings.json` để quản lý các tham số cấu hình:

- **ConnectionStrings:** Kết nối tới SQL Server (`YC5_THUCTAP_API`).
- **JwtSettings:** 
  - `SecretKey`: Khóa bí mật để ký token.
  - `Issuer`: NamNguyen.
  - `Audience`: TodoAppUsers.
  - `ExpiryMinutes`: 60 phút.
- **EmailSettings:** Cấu hình SMTP Gmail để gửi thông báo tự động.
- **EPPlus:** Giấy phép sử dụng Non-Commercial cho cá nhân.

## 📁 Cấu trúc thư mục chi tiết

```text
YC5_API_IO/
├── Controllers/       # Chứa các bộ điều khiển xử lý HTTP Request (Hiện tại: WeatherForecast)
├── Data/              # Quản lý Database Context (ApplicationDbContext)
├── Models/            # Định nghĩa các thực thể (Entities) của hệ thống
├── Interfaces/        # Định nghĩa các giao diện nghiệp vụ (ví dụ: IJwtInterfaces)
├── Services/          # Triển khai logic nghiệp vụ (ví dụ: JWTService)
├── Dto/               # Data Transfer Objects (Đang phát triển)
├── Properties/        # Cấu hình môi trường và launchSettings.json
├── YC5_API_IO.csproj  # File quản lý package và project
├── Dockerfile         # Cấu hình đóng gói ứng dụng
└── Program.cs         # Entry point, cấu hình Middleware và Dependency Injection
```

## 🛠️ Hướng dẫn cài đặt

### Tiền đề
- Cài đặt [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (LocalDB hoặc Express)

### Các bước thực hiện

1. **Clone repository:**
   ```bash
   git clone <url-repository>
   ```

2. **Cập nhật cấu hình:**
   Mở `YC5_API_IO/appsettings.json` và điều chỉnh `DefaultConnection` phù hợp với Server SQL của bạn.

3. **Khởi tạo Database:**
   Mở terminal tại thư mục project và chạy:
   ```bash
   dotnet ef database update
   ```

4. **Khởi chạy ứng dụng:**
   ```bash
   dotnet run --project YC5_API_IO
   ```

5. **Kiểm tra API:**
   Truy cập `https://localhost:7157/swagger` (cổng có thể thay đổi tùy cấu hình) để xem giao diện Swagger UI.

## 📝 Trạng thái dự án
Dự án hiện đã hoàn thành phần thiết kế Models và cấu hình Infrastructure (Authentication, DB Context). Các logic nghiệp vụ (Services) và các API Endpoints (Controllers) đang trong quá trình hoàn thiện.

---
**Phát triển bởi:** Nam Nguyễn (nguyendinhnam241209@gmail.com)
