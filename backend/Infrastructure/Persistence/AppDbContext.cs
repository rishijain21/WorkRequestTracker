using Microsoft.EntityFrameworkCore;
using WorkRequestTracker.Domain.Entities;
using WorkRequestTracker.Domain.Enums;

namespace WorkRequestTracker.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<WorkRequest> WorkRequests => Set<WorkRequest>();
    public DbSet<Note> Notes => Set<Note>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // WorkRequest configuration
        modelBuilder.Entity<WorkRequest>(entity =>
        {
            entity.HasKey(w => w.Id);

            // Guid → string for SQLite compatibility
            entity.Property(w => w.Id)
                .HasConversion<string>();

            entity.Property(w => w.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(w => w.ClientName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(w => w.Description)
                .IsRequired()
                .HasMaxLength(2000);

            // Enums → stored as int in DB
            entity.Property(w => w.Priority)
                .HasConversion<int>()
                .IsRequired();

            entity.Property(w => w.Status)
                .HasConversion<int>()
                .IsRequired();

            entity.Property(w => w.DueDate).IsRequired();
            entity.Property(w => w.CreatedAt).IsRequired();
            entity.Property(w => w.UpdatedAt).IsRequired();

            // Indexes for common query patterns
            entity.HasIndex(w => w.Status)
                .HasDatabaseName("IX_WorkRequests_Status");

            entity.HasIndex(w => w.Title)
                .HasDatabaseName("IX_WorkRequests_Title");

            // One-to-many: WorkRequest → Notes, cascade delete
            entity.HasMany(w => w.Notes)
                .WithOne(n => n.WorkRequest)
                .HasForeignKey(n => n.WorkRequestId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Note configuration
        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(n => n.Id);

            // Guid → string for SQLite compatibility
            entity.Property(n => n.Id)
                .HasConversion<string>();

            entity.Property(n => n.WorkRequestId)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(n => n.Content).IsRequired();
            entity.Property(n => n.CreatedAt).IsRequired();

            // Index on FK — every detail load joins on this
            entity.HasIndex(n => n.WorkRequestId)
                .HasDatabaseName("IX_Notes_WorkRequestId");
        });

        // Seed data
        // HasData bypasses SaveChanges, so timestamps must be set explicitly.
        // Fixed GUIDs keep the migration deterministic.

        var wr1 = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000001");
        var wr2 = Guid.Parse("a1b2c3d4-0002-0000-0000-000000000002");
        var wr3 = Guid.Parse("a1b2c3d4-0003-0000-0000-000000000003");
        var wr4 = Guid.Parse("a1b2c3d4-0004-0000-0000-000000000004");
        var wr5 = Guid.Parse("a1b2c3d4-0005-0000-0000-000000000005");

        var seedDate = new DateTimeOffset(2026, 5, 1, 0, 0, 0, TimeSpan.Zero);
        var futureDate1 = new DateTimeOffset(2026, 8, 15, 0, 0, 0, TimeSpan.Zero);
        var futureDate2 = new DateTimeOffset(2026, 9, 1, 0, 0, 0, TimeSpan.Zero);
        var futureDate3 = new DateTimeOffset(2026, 7, 20, 0, 0, 0, TimeSpan.Zero);
        var futureDate4 = new DateTimeOffset(2026, 10, 10, 0, 0, 0, TimeSpan.Zero);
        var futureDate5 = new DateTimeOffset(2026, 6, 30, 0, 0, 0, TimeSpan.Zero);

        modelBuilder.Entity<WorkRequest>().HasData(
            new WorkRequest
            {
                Id = wr1,
                Title = "Redesign Landing Page",
                ClientName = "Acme Corp",
                Description = "Complete overhaul of the main landing page with new branding guidelines and responsive layout.",
                Priority = Priority.High,
                Status = WorkRequestStatus.InProgress,
                DueDate = futureDate1,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new WorkRequest
            {
                Id = wr2,
                Title = "Fix Payment Gateway Timeout",
                ClientName = "GlobalTech Solutions",
                Description = "Payment processing times out after 30 seconds on high-traffic days. Investigate and fix.",
                Priority = Priority.High,
                Status = WorkRequestStatus.Blocked,
                DueDate = futureDate2,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new WorkRequest
            {
                Id = wr3,
                Title = "Add Export to CSV Feature",
                ClientName = "Acme Corp",
                Description = "Users need the ability to export their order history to CSV format from the dashboard.",
                Priority = Priority.Medium,
                Status = WorkRequestStatus.New,
                DueDate = futureDate3,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new WorkRequest
            {
                Id = wr4,
                Title = "Update Privacy Policy Page",
                ClientName = "Northwind Traders",
                Description = "Legal team provided updated privacy policy text. Replace existing content and update footer links.",
                Priority = Priority.Low,
                Status = WorkRequestStatus.Completed,
                DueDate = futureDate4,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new WorkRequest
            {
                Id = wr5,
                Title = "Mobile App Push Notifications",
                ClientName = "Summit Financial",
                Description = "Implement push notification support for iOS and Android. Include transaction alerts and marketing opt-in.",
                Priority = Priority.Medium,
                Status = WorkRequestStatus.New,
                DueDate = futureDate5,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            }
        );

        // Notes — attached to wr1 (Acme redesign) and wr2 (payment gateway)
        modelBuilder.Entity<Note>().HasData(
            new
            {
                Id = Guid.Parse("b1b2c3d4-0001-0000-0000-000000000001"),
                WorkRequestId = wr1,
                Content = "Initial mockups approved by the client. Moving to development phase.",
                CreatedAt = seedDate
            },
            new
            {
                Id = Guid.Parse("b1b2c3d4-0002-0000-0000-000000000002"),
                WorkRequestId = wr1,
                Content = "Header and hero section completed. Footer still in progress.",
                CreatedAt = seedDate.AddDays(1)
            },
            new
            {
                Id = Guid.Parse("b1b2c3d4-0003-0000-0000-000000000003"),
                WorkRequestId = wr2,
                Content = "Confirmed the timeout is on the payment provider's side. Awaiting their API fix.",
                CreatedAt = seedDate
            },
            new
            {
                Id = Guid.Parse("b1b2c3d4-0004-0000-0000-000000000004"),
                WorkRequestId = wr2,
                Content = "Provider says fix will be deployed next week. Keeping status as Blocked.",
                CreatedAt = seedDate.AddDays(2)
            }
        );
    }

    public override int SaveChanges()
    {
        SetTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetTimestamps()
    {
        var now = DateTimeOffset.UtcNow;

        foreach (var entry in ChangeTracker.Entries<WorkRequest>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.UpdatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }

        foreach (var entry in ChangeTracker.Entries<Note>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
            }
        }
    }
}
