using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Services;
using OfficeOpenXml; // Add this using directive

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<YC5_API_IO.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register JWT Service
builder.Services.AddScoped<IJwtInterface, JWTService>();
builder.Services.AddScoped<AuthInterface, AuthService>();
builder.Services.AddScoped<UserInterface, UserService>();
builder.Services.AddScoped<ICategoryInterface, CategoryService>();
builder.Services.AddScoped<ITagInterface, TagService>();
builder.Services.AddScoped<ITaskInterface, TaskService>();
builder.Services.AddScoped<ICountdownInterface, CountdownService>();
builder.Services.AddScoped<IReminderInterface, ReminderService>();
builder.Services.AddScoped<INotificationInterface, NotificationService>();
builder.Services.AddScoped<ICommentInterface, CommentService>();
builder.Services.AddScoped<IAnalysisInterface, AnalysisService>(); // Register Analysis Service

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    // Thông tin cơ bản về API
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Productivity API",
        Description = "API for TODO, Habit Tracker, and Pomodoro System",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Nam Nguyễn",
            Email = "nguyendinhnam241209@gmail.com",
            Url = new Uri("https://example.com/contact")
        }
    });

    // Thêm XML comments để hiển thị documentation
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);

    // Thêm JWT Bearer Authentication
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            Array.Empty<string>()
        }
    });

    // Enable Annotations (nếu đã cài package)
    options.EnableAnnotations();
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]) ??
                throw new InvalidOperationException("JWT Secret Key is missing!"))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalDev", policy =>
    {
        policy.AllowAnyOrigin() // <-- Allow requests from any origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("AllowLocalDev");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Productivity API V1");
        options.RoutePrefix = string.Empty; // Swagger UI tại root (https://localhost:5000/)

        // Tùy chỉnh giao diện
        options.DocumentTitle = "Productivity API Documentation";
        options.DefaultModelsExpandDepth(-1); // Ẩn schemas mặc định
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); // Collapse tất cả endpoints
        options.DisplayRequestDuration(); // Hiển thị thời gian request
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
