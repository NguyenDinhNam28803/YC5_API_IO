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

## ✨ Tính năng chính

Hệ thống cung cấp một bộ tính năng toàn diện để quản lý công việc và năng suất:

1.  **Quản lý Công việc Toàn diện:**
    *   Tạo, cập nhật, xóa và theo dõi trạng thái công việc.
    *   **Cấu trúc phân cấp (Sub-tasks):** Hỗ trợ tạo các công việc con không giới hạn cấp độ, giúp chia nhỏ các dự án lớn.
    *   **Phân loại & Gắn thẻ:** Sử dụng Categories và Tags (N-N) để tổ chức công việc khoa học.
    *   **Mức độ ưu tiên:** Thiết lập độ ưu tiên (Low, Medium, High) để tập trung vào việc quan trọng.

2.  **Tương tác & Cộng tác:**
    *   **Comments:** Gửi phản hồi và thảo luận trực tiếp trên từng đầu việc.
    *   **Attachments:** Đính kèm tài liệu, hình ảnh liên quan đến công việc hoặc bình luận.

3.  **Theo dõi Thời gian & Nhắc nhở:**
    *   **Reminders:** Hệ thống nhắc nhở linh hoạt cho từng công việc.
    *   **Notifications:** Thông báo thời gian thực về các thay đổi và nhắc nhở.
    *   **CountDown:** Bộ đếm ngược cho các sự kiện hoặc deadline quan trọng.

4.  **Phân tích & Báo cáo:**
    *   **Analysis:** Tự động tổng hợp thống kê hiệu suất làm việc của người dùng (tổng số việc, số việc hoàn thành, hoạt động gần nhất).
    *   **Xuất dữ liệu Excel:** Hỗ trợ xuất báo cáo thống kê chi tiết ra file Excel chuyên nghiệp.
    *   **Nhập dữ liệu Excel:** Cho phép import hàng loạt công việc từ file Excel giúp tiết kiệm thời gian.

5.  **Bảo mật & Phân quyền:**
    *   Hệ thống xác thực JWT mạnh mẽ.
    *   Phân quyền người dùng (Role-based access control) giúp quản lý hệ thống an toàn.

## 🏗️ Kiến trúc dữ liệu (Data Models)

Hệ thống được thiết kế với các thực thể có mối quan hệ chặt chẽ:

### 1. Người dùng (User)
- Trung tâm của hệ thống, sở hữu các tài nguyên cá nhân.
- **Quan hệ:** 1-N với Task, Category, CountDown, Notification, Comment, và Analysis.

### 2. Công việc (Task)
- Thực thể quan trọng nhất, hỗ trợ đệ quy.
- **Quan hệ:**
    - Thuộc về 1 User và 1 Category.
    - 1-N với Sub-tasks (Self-referencing).
    - 1-N với Comments và Reminders.
    - N-N với Tags (thông qua bảng trung gian TaskTags).
    - 1-N với Attachments.

### 3. Danh mục (Category)
- Nhóm các công việc cùng chủ đề.
- **Quan hệ:** 1-N với Task.

### 4. Thành phần hỗ trợ
- **Tag:** Nhãn dán linh hoạt, có thể gắn cho nhiều Task khác nhau.
- **Comment:** Lưu vết trao đổi, hỗ trợ đính kèm file.
- **Reminder & Notification:** Hỗ trợ nhắc lịch và thông báo hệ thống.
- **CountDown:** Theo dõi các mốc thời gian đặc biệt.
- **Analysis:** Lưu trữ dữ liệu thống kê định kỳ cho từng người dùng.
- **Role:** Xác định quyền hạn của người dùng trong hệ thống.

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

## 📡 Tài liệu API (API Reference)

Dưới đây là tóm tắt các nhóm API chính. Chi tiết xem tại Swagger UI (`/swagger`).

### 🔐 Xác thực & Người dùng
- `POST /api/Auths/Register`: Đăng ký tài khoản mới.
- `POST /api/Auths/Login`: Đăng nhập lấy Access Token & Refresh Token.
- `POST /api/Auths/Refresh`: Làm mới Access Token.
- `GET /api/Users/{userId}`: Lấy thông tin chi tiết người dùng.
- `POST /api/Users/Update`: Cập nhật thông tin cá nhân.

### ✅ Công việc (Tasks)
- `GET /api/Tasks`: Lấy danh sách công việc của người dùng hiện tại.
- `POST /api/Tasks`: Tạo công việc mới.
- `GET /api/Tasks/{taskId}`: Xem chi tiết một công việc.
- `PUT /api/Tasks/{taskId}`: Cập nhật công việc.
- `DELETE /api/Tasks/{taskId}`: Xóa công việc.
- `GET /api/Tasks/ByCategory/{categoryId}`: Lọc công việc theo danh mục.

### 📁 Danh mục & Thẻ (Categories & Tags)
- `GET /api/Categories`: Quản lý danh mục công việc.
- `GET /api/Tags`: Quản lý các nhãn dán.

### 💬 Tương tác (Comments & Notifications)
- `GET /api/Comments?taskId={id}`: Lấy danh sách bình luận của công việc.
- `POST /api/Comments`: Thêm bình luận mới.
- `GET /api/Notifications`: Lấy danh sách thông báo.
- `PUT /api/Notifications/{id}/mark-as-read`: Đánh dấu đã đọc.

### ⏰ Nhắc nhở & Đếm ngược
- `GET /api/Reminders`: Quản lý các nhắc nhở công việc.
- `GET /api/Countdowns`: Quản lý các bộ đếm ngược sự kiện.

### 📊 Phân tích & Excel
- `POST /api/Analysis/generate-statistics`: Tổng hợp dữ liệu thống kê mới nhất.
- `GET /api/Analysis/export-excel`: Xuất báo cáo thống kê toàn hệ thống (Excel).
- `POST /api/Excel/import/tasks`: Import danh sách công việc từ file Excel.

## 📁 Cấu trúc thư mục chi tiết

```text
YC5_API_IO/
├── Controllers/       # Chứa các bộ điều khiển xử lý HTTP Request (Task, User, Category, v.v.)
├── Data/              # Quản lý Database Context (ApplicationDbContext) và Migrations
├── Models/            # Định nghĩa các thực thể (Entities) của hệ thống (POCO classes)
├── Interfaces/        # Định nghĩa các giao diện nghiệp vụ (Abstractions)
├── Services/          # Triển khai logic nghiệp vụ chi tiết
├── Dto/               # Data Transfer Objects - Chuyển đổi dữ liệu giữa API và Service
├── Migrations/        # Các file migration của Entity Framework Core
├── Properties/        # Cấu hình môi trường và launchSettings.json
├── YC5_API_IO.csproj  # File quản lý package và project .NET
├── Dockerfile         # Cấu hình đóng gói ứng dụng (Containerization)
└── Program.cs         # Entry point, cấu hình Middleware, Dependency Injection, và Swagger
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
Dự án đã hoàn thiện khung kiến trúc cơ bản (Clean Architecture), tích hợp đầy đủ các tính năng nghiệp vụ cốt lõi từ quản lý công việc, phân quyền, đến phân tích dữ liệu và báo cáo Excel. Hệ thống đã sẵn sàng để tích hợp với các ứng dụng frontend.

---
**Phát triển bởi:** Nam Nguyễn (nguyendinhnam241209@gmail.com)
