using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class JamesthewContext : DbContext
{
    public JamesthewContext()
    {
    }

    public JamesthewContext(DbContextOptions<JamesthewContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Announcement> Announcements { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Contest> Contests { get; set; }
    public virtual DbSet<ContestSubmission> ContestSubmissions { get; set; }
    public virtual DbSet<Faq> Faqs { get; set; }
    public virtual DbSet<Feedback> Feedbacks { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Recipe> Recipes { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Saved> Saveds { get; set; }
    public virtual DbSet<Tip> Tips { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================
        // TIP
        // =========================
        modelBuilder.Entity<Tip>(entity =>
        {
            entity.HasKey(e => e.TipId);

            entity.Property(e => e.Title)
                  .HasMaxLength(200);

            entity.HasOne(d => d.UploadedByNavigation)
                  .WithMany(p => p.Tips)
                  .HasForeignKey(d => d.UploadedBy)
                  .HasConstraintName("FK_Tips_Users")
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // =========================
        // RECIPE
        // =========================
        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId);

            entity.Property(e => e.Title)
                  .HasMaxLength(200);

            entity.Property(e => e.UploadDate)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .HasColumnType("timestamp");

            entity.HasOne(d => d.UploadedByNavigation)
                  .WithMany(p => p.Recipes)
                  .HasForeignKey(d => d.UploadedBy)
                  .HasConstraintName("FK_Recipes_ToTable");
        });

        // =========================
        // ANNOUNCEMENT
        // =========================
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.AnnouncementId);

            entity.Property(e => e.AnnouncementId)
                  .HasColumnName("AnnouncementID");

            entity.Property(e => e.PostDate)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .HasColumnType("timestamp");

            entity.Property(e => e.Title)
                  .HasMaxLength(200);
        });

        // =========================
        // CATEGORY
        // =========================
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.Property(e => e.Name)
                  .HasMaxLength(100);
        });

        // =========================
        // CONTEST
        // =========================
        modelBuilder.Entity<Contest>(entity =>
        {
            entity.HasKey(e => e.ContestId);

            entity.Property(e => e.Title)
                  .HasMaxLength(200);
        });

        // =========================
        // CONTEST SUBMISSION
        // =========================
        modelBuilder.Entity<ContestSubmission>(entity =>
        {
            entity.HasKey(e => e.SubmissionId);

            entity.Property(e => e.Title)
                  .HasMaxLength(200);
        });

        // =========================
        // FAQ
        // =========================
        modelBuilder.Entity<Faq>(entity =>
        {
            entity.HasKey(e => e.Faqid);

            entity.ToTable("FAQ");

            entity.Property(e => e.Question)
                  .HasMaxLength(300);
        });

        // =========================
        // FEEDBACK
        // =========================
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId);

            entity.ToTable("Feedback");
        });

        // =========================
        // PAYMENT
        // =========================
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId);

            entity.Property(e => e.Amount)
                  .HasColumnType("decimal(10,2)");
        });

        // =========================
        // ROLE
        // =========================
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.Property(e => e.RoleName)
                  .HasMaxLength(50);
        });

        // =========================
        // SAVED
        // =========================
        modelBuilder.Entity<Saved>(entity =>
        {
            entity.ToTable("Saved");

            entity.HasKey(e => e.SavedId);

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .HasColumnType("timestamp");

            entity.HasOne(d => d.User)
                  .WithMany()
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK_Saved_User");

            entity.HasOne(d => d.Recipe)
                  .WithMany()
                  .HasForeignKey(d => d.RecipeId)
                  .HasConstraintName("FK_Saved_Recipe");
        });

        // =========================
        // USER
        // =========================
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserID);

            entity.HasIndex(e => e.Email)
                  .IsUnique();

            entity.Property(e => e.Email)
                  .HasMaxLength(100);

            entity.Property(e => e.Name)
                  .HasMaxLength(100);

            entity.Property(e => e.Password)
                  .HasMaxLength(100);

            entity.Property(e => e.RegistrationDate)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .HasColumnType("timestamp");

            entity.Property(e => e.Role)
                  .HasMaxLength(20);

            entity.Property(e => e.SubscriptionType)
                  .HasMaxLength(20);
        });

        // =========================
        // SUBSCRIPTION
        // =========================
        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId);

            entity.Property(e => e.PlanType)
                  .HasMaxLength(20)
                  .IsRequired();

            entity.Property(e => e.PaymentMethod)
                  .HasMaxLength(20)
                  .IsRequired();

            entity.Property(e => e.PhoneNumber)
                  .HasMaxLength(15)
                  .IsRequired();

            entity.Property(e => e.Status)
                  .HasMaxLength(20)
                  .HasDefaultValue("Pending");

            entity.Property(e => e.Amount)
                  .HasColumnType("decimal(10,2)");

            entity.Property(e => e.StartDate)
                  .HasColumnType("timestamp");

            entity.Property(e => e.EndDate)
                  .HasColumnType("timestamp");

            entity.Property(e => e.CreatedAt)
                  .HasColumnType("timestamp")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(s => s.User)
                  .WithMany()
                  .HasForeignKey(s => s.UserId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_Subscriptions_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}