# YC5_API_IO

<<<<<<< HEAD
## Tổng quan dự án

YC5_API_IO là một API RESTful mạnh mẽ và có khả năng mở rộng được xây dựng với ASP.NET Core, được thiết kế để quản lý các công việc (task), người dùng (user), danh mục (category) và các thực thể liên quan khác. Dự án nổi bật với kiến trúc có cấu trúc, xác thực dựa trên JWT với hỗ trợ refresh token và một lược đồ cơ sở dữ liệu linh hoạt sử dụng Entity Framework Core. API này đóng vai trò là nền tảng backend cho các ứng dụng khác nhau yêu cầu quản lý người dùng, tổ chức công việc và lưu trữ dữ liệu.

## Các tính năng chính

*   **Quản lý người dùng:** Đăng ký, xác thực và quản lý tài khoản người dùng.
*   **Phân quyền dựa trên vai trò:** Hỗ trợ các vai trò riêng biệt (Admin, Manager, User) để kiểm soát quyền truy cập.
*   **Quản lý công việc:** Tạo, tổ chức và theo dõi các công việc với danh mục, thẻ (tag), bình luận và công việc con.
*   **Quản lý danh mục:** Tổ chức các công việc thành các danh mục hợp lý.
*   **Theo dõi đếm ngược:** Quản lý các bộ đếm ngược liên quan đến người dùng.
*   **Hệ thống thông báo:** Hệ thống cơ bản để gửi thông báo đến người dùng.
*   **Quản lý tệp/tệp đính kèm:** Chức năng liên kết tệp/tệp đính kèm với công việc và bình luận.
*   **Xác thực JWT:** Truy cập API an toàn bằng cách sử dụng Access Token và Refresh Token.
*   **Entity Framework Core:** ORM để tương tác cơ sở dữ liệu với SQL Server.
*   **Bố cục dự án có cấu trúc:** Phân tách rõ ràng các mối quan tâm với các thư mục riêng biệt cho Models, DTOs, Interfaces, Services và Controllers.

## Công nghệ sử dụng

*   **Backend:** ASP.NET Core (.NET 9.0)
*   **Ngôn ngữ:** C#
*   **Cơ sở dữ liệu:** SQL Server (thông qua Entity Framework Core)
*   **Xác thực:** JWT (JSON Web Tokens)
*   **Công cụ:** NuGet Package Manager

## Bắt đầu

Làm theo các bước sau để thiết lập và chạy dự án cục bộ.

### Điều kiện tiên quyết

*   [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
*   [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (hoặc một phiên bản SQL Server Express/LocalDB tương thích)
*   Một trình soạn thảo mã như [Visual Studio](https://visualstudio.microsoft.com/) hoặc [VS Code](https://code.visualstudio.com/)

### Cài đặt

1.  **Clone kho lưu trữ:**
    ```bash
    git clone https://github.com/your-username/YC5_API_IO.git
    cd YC5_API_IO
    ```
2.  **Điều hướng đến thư mục dự án:**
    ```bash
    cd YC5_API_IO\YC5_API_IO
    ```
3.  **Khôi phục các gói NuGet:**
    ```bash
    dotnet restore
    ```

### Thiết lập cơ sở dữ liệu

1.  **Cấu hình chuỗi kết nối:**
    Mở `appsettings.json` (và `appsettings.Development.json`) và cập nhật chuỗi `DefaultConnection` trong mục `ConnectionStrings` để trỏ đến phiên bản SQL Server của bạn.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=YC5_THUCTAP_API;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
    },
    ```
    Thay thế `YOUR_SERVER_NAME` bằng tên phiên bản SQL Server thực tế của bạn.

2.  **Chạy Migrations và Cập nhật cơ sở dữ liệu:**
    Từ thư mục `YC5_API_IO\YC5_API_IO`, thực thi các lệnh sau để áp dụng các migration cơ sở dữ liệu và tạo lược đồ, bao gồm cả việc gieo dữ liệu `Role` ban đầu:
    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```
    *Lưu ý: Nếu bạn gặp sự cố, hãy đảm bảo bạn đã cài đặt các công cụ `dotnet ef` (`dotnet tool install --global dotnet-ef`).*

### Chạy ứng dụng

Từ thư mục `YC5_API_IO\YC5_API_IO`:

```bash
dotnet run
```

API thường sẽ chạy trên `https://localhost:7081` (hoặc một cổng tương tự). Bạn có thể kiểm tra đầu ra của console để biết URL chính xác.

## Tổng quan các Endpoint API

API cung cấp các endpoint cho:

*   **Xác thực:** Đăng ký người dùng, đăng nhập, làm mới token.
*   **Quản lý người dùng:** Các thao tác CRUD cho người dùng (được bảo vệ).
*   **Quản lý vai trò:** (Được xử lý ngầm bởi các vai trò đã gieo).
*   **Quản lý danh mục, công việc, bình luận, thẻ, đếm ngược:** Các thao tác CRUD cho các thực thể này.
*   **Thông báo:** Quản lý thông báo người dùng.
*   **Tệp đính kèm:** Tải lên và quản lý các tệp liên quan đến công việc/bình luận.

Tài liệu Swagger/OpenAPI sẽ có sẵn tại `/swagger` (ví dụ: `https://localhost:7081/swagger`) khi ứng dụng đang chạy trong môi trường Development.
( Hiện tại chưa có bản swagger, sẽ được cập nhật trong các phiên bản sau)

## Xác thực

API này sử dụng JWT (JSON Web Tokens) để xác thực.

*   **Access Token:** Token có thời hạn ngắn được sử dụng để xác thực các yêu cầu đến các endpoint được bảo vệ.
*   **Refresh Token:** Token có thời hạn dài được sử dụng để lấy Access Token mới khi token hiện tại hết hạn, mà không yêu cầu xác thực lại.

Để truy cập các endpoint được bảo vệ, bạn phải bao gồm Access Token của mình trong tiêu đề `Authorization` dưới dạng Bearer token: `Authorization: Bearer <YourAccessToken>`.

## Tổng quan lược đồ cơ sở dữ liệu

Lược đồ cơ sở dữ liệu bao gồm các thực thể chính sau và mối quan hệ của chúng:

*   **`User` (Người dùng)**: Thông tin cốt lõi của người dùng.
    *   Liên kết với `Role` (Nhiều-một).
    *   Có nhiều `Categories`, `Tasks`, `CountDowns`, `Notifications`.
    *   Tải lên nhiều `Attachments`.
*   **`Role` (Vai trò)**: Định nghĩa các vai trò người dùng (ví dụ: Admin, Manager, User).
    *   Có nhiều `Users` (Một-nhiều).
*   **`Category` (Danh mục)**: Tổ chức các công việc.
    *   Liên kết với `User` (Nhiều-một).
    *   Có nhiều `Tasks` (Một-nhiều).
*   **`Task` (Công việc)**: Đại diện cho các công việc riêng lẻ.
    *   Liên kết với `User` (Nhiều-một).
    *   Liên kết với `Category` (Nhiều-một).
    *   Có thể có `ParentTask` (Tự tham chiếu, Nhiều-một).
    *   Có nhiều `Tags`, `Comments`, `SubTasks`, `Attachments`.
*   **`Comment` (Bình luận)**: Bình luận của người dùng về các công việc.
    *   Liên kết với `Task` (Nhiều-một).
    *   Có nhiều `Attachments`.
*   **`Tag` (Thẻ)**: Nhãn cho các công việc.
    *   Liên kết với `Task` (Nhiều-một).
*   **`CountDown` (Đếm ngược)**: Các bộ đếm ngược dành riêng cho người dùng.
    *   Liên kết với `User` (Nhiều-một).
*   **`Notification` (Thông báo)**: Thông báo hệ thống cho người dùng.
    *   Liên kết với `User` (Nhiều-một).
*   **`Attachment` (Tệp đính kèm)**: Các tệp liên quan đến công việc hoặc bình luận.
    *   Được tải lên bởi một `User` (Nhiều-một).
    *   Có thể được liên kết với một `Task` hoặc `Comment`.

( Dự án hiện tại đã được thiết kế để dễ dàng mở rộng lược đồ cơ sở dữ liệu trong tương lai khi cần thiết.)

## Cấu trúc dự án

```
YC5_API_IO/
├───.git/
├───.github/
├───.vs/
├───YC5_API_IO.slnx
└───YC5_API_IO/
    ├───appsettings.json           # Cấu hình ứng dụng
    ├───Program.cs                 # Điểm khởi chạy ứng dụng và cấu hình dịch vụ
    ├───Controllers/               # Định nghĩa các Endpoint API
    ├───Data/                      # DbContext và logic liên quan đến cơ sở dữ liệu
    │   └───ApplicationDbContext.cs
    ├───Dto/                       # Đối tượng truyền dữ liệu (Data Transfer Objects)
    │   └───JwtTokenDto.cs
    ├───Interfaces/                # Định nghĩa Interface (ví dụ: IJwtInterface)
    │   └───IJwtInterface.cs
    ├───Models/                    # Các mô hình/thực thể cơ sở dữ liệu
    │   ├───User.cs
    │   ├───Role.cs
    │   ├───Category.cs
    │   ├───Comment.cs
    │   ├───CountDown.cs
    │   ├───Tag.cs
    │   ├───Task.cs
    │   ├───Notification.cs
    │   └───Attachment.cs
    ├───Properties/
    ├───Services/                  # Logic nghiệp vụ và triển khai dịch vụ (ví dụ: JWTService)
    │   └───JWTService.cs
    └───YC5_API_IO.csproj          # Tệp dự án
```

( Cấu trúc dự án có thể được mở rộng thêm khi các tính năng mới được thêm vào.)

## Giấy phép

Dự án này được cấp phép theo Giấy phép MIT - xem tệp LICENSE.md để biết chi tiết.
=======
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
>>>>>>> 33b5d558c1d790514eb381b8526b1720017f97d6
