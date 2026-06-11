using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookReadingTracker.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookReading> BookReadings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=BRT;User ID=sa;Password=12345;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__books__490D1AE18DFCD338");

            entity.ToTable("books");

            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Author)
                .HasMaxLength(255)
                .HasColumnName("author");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(100)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.TotalPages).HasColumnName("total_pages");
        });

        modelBuilder.Entity<BookReading>(entity =>
        {
            entity.HasKey(e => e.BookReadingId).HasName("PK__book_rea__BC5E61793B6D445A");

            entity.ToTable("book_reading");

            entity.HasIndex(e => new { e.UserId, e.BookId }, "uq_book_reading").IsUnique();

            entity.Property(e => e.BookReadingId).HasColumnName("book_reading_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.CompletedDate).HasColumnName("completed_date");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_date");
            entity.Property(e => e.CurrentPage).HasColumnName("current_page");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(100)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");
            entity.Property(e => e.StartedDate).HasColumnName("started_date");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Not Started")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Book).WithMany(p => p.BookReadings)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_book_reading_book");

            entity.HasOne(d => d.User).WithMany(p => p.BookReadings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_book_reading_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370FE37B51A0");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E616407B138EB").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_date");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(100)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(500)
                .HasColumnName("password_hash");
            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .HasColumnName("role_name");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .HasColumnName("user_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
