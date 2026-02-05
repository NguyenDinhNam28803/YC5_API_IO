using Microsoft.EntityFrameworkCore;
using YC5_API_IO.Models;

namespace YC5_API_IO.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define your DbSets (tables) here
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CountDown> CountDowns { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your entity relationships and constraints here

            // Configure Role-User (One-to-Many)
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a Role if Users are associated

            // Configure User-Category (One-to-Many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Categories)
                .WithOne()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Delete Categories when User is deleted

            // Configure User-CountDown (One-to-Many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.CountDowns)
                .WithOne()
                .HasForeignKey(cd => cd.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Delete CountDowns when User is deleted

            // Configure User-Task (One-to-Many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Tasks)
                .WithOne()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Delete Tasks when User is deleted

            // Configure Category-Task (One-to-Many)
            modelBuilder.Entity<Category>()
                .HasMany<YC5_API_IO.Models.Task>() // Specify the entity type explicitly
                .WithOne()
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a Category if Tasks are associated

            // Configure Task-Comment (One-to-Many)
            modelBuilder.Entity<YC5_API_IO.Models.Task>()
                .HasMany(t => t.Comments)
                .WithOne()
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade); // Delete Comments when Task is deleted

            // Configure Task-Tag (One-to-Many)
            modelBuilder.Entity<YC5_API_IO.Models.Task>()
                .HasMany(t => t.Tags)
                .WithOne()
                .HasForeignKey(tg => tg.TaskId)
                .OnDelete(DeleteBehavior.Cascade); // Delete Tags when Task is deleted

            // Configure Task-SubTask (Self-Referencing, One-to-Many)
            modelBuilder.Entity<YC5_API_IO.Models.Task>()
                .HasMany(t => t.SubTasks)
                .WithOne()
                .HasForeignKey(st => st.ParentTaskId)
                .IsRequired(false) // ParentTaskId is nullable
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a parent Task if SubTasks exist

            // Seed Role data
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = Guid.NewGuid().ToString(), RoleName = "Admin", RoleDescription = "Administrator role with full access" },
                new Role { RoleId = Guid.NewGuid().ToString(), RoleName = "Manager", RoleDescription = "Manager role with elevated privileges" },
                new Role { RoleId = Guid.NewGuid().ToString(), RoleName = "User", RoleDescription = "Standard user role" }
            );

            // Configure User-Notification (One-to-Many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Delete Notifications when User is deleted

            // Configure User-Attachment (One-to-Many - for uploaded attachments)
            modelBuilder.Entity<User>()
                .HasMany(u => u.UploadedAttachments)
                .WithOne(a => a.UploadedByUser)
                .HasForeignKey(a => a.UploadedByUserId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict deleting a User if they have uploaded attachments

            // Configure Task-Attachment (One-to-Many)
            modelBuilder.Entity<YC5_API_IO.Models.Task>()
                .HasMany(t => t.Attachments)
                .WithOne(a => a.Task)
                .HasForeignKey(a => a.TaskId)
                .IsRequired(false) // TaskId is nullable in Attachment
                .OnDelete(DeleteBehavior.Cascade); // Delete Attachments when Task is deleted

            // Configure Comment-Attachment (One-to-Many)
            modelBuilder.Entity<Comment>()
                .HasMany(c => c.Attachments)
                .WithOne(a => a.Comment)
                .HasForeignKey(a => a.CommentId)
                .IsRequired(false) // CommentId is nullable in Attachment
                .OnDelete(DeleteBehavior.Cascade); // Delete Attachments when Comment is deleted
        }
    }
}
