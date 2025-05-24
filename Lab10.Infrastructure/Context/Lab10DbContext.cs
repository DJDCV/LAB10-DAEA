using Lab10.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lab10.Infrastructure.Context;

public partial class Lab10DbContext : DbContext
{
    public Lab10DbContext(DbContextOptions<Lab10DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<response> responses { get; set; }

    public virtual DbSet<role> roles { get; set; }

    public virtual DbSet<ticket> tickets { get; set; }

    public virtual DbSet<user> users { get; set; }

    public virtual DbSet<user_role> user_roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<response>(entity =>
        {
            entity.HasKey(e => e.response_id).HasName("PRIMARY");

            entity.HasIndex(e => e.responder_id, "fk_responses_responder");

            entity.HasIndex(e => e.ticket_id, "fk_responses_ticket");

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.message).HasColumnType("text");

            entity.HasOne(d => d.responder).WithMany(p => p.responses)
                .HasForeignKey(d => d.responder_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_responses_responder");

            entity.HasOne(d => d.ticket).WithMany(p => p.responses)
                .HasForeignKey(d => d.ticket_id)
                .HasConstraintName("fk_responses_ticket");
        });

        modelBuilder.Entity<role>(entity =>
        {
            entity.HasKey(e => e.role_id).HasName("PRIMARY");

            entity.HasIndex(e => e.role_name, "role_name").IsUnique();

            entity.Property(e => e.role_name).HasMaxLength(50);
        });

        modelBuilder.Entity<ticket>(entity =>
        {
            entity.HasKey(e => e.ticket_id).HasName("PRIMARY");

            entity.HasIndex(e => e.user_id, "fk_tickets_user");

            entity.Property(e => e.closed_at).HasColumnType("timestamp");
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.description).HasColumnType("text");
            entity.Property(e => e.status).HasColumnType("enum('abierto','en_proceso','cerrado')");
            entity.Property(e => e.title).HasMaxLength(255);

            entity.HasOne(d => d.user).WithMany(p => p.tickets)
                .HasForeignKey(d => d.user_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tickets_user");
        });

        modelBuilder.Entity<user>(entity =>
        {
            entity.HasKey(e => e.user_id).HasName("PRIMARY");

            entity.HasIndex(e => e.email, "email").IsUnique();

            entity.HasIndex(e => e.username, "username").IsUnique();

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.email).HasMaxLength(150);
            entity.Property(e => e.password_hash).HasMaxLength(255);
            entity.Property(e => e.username).HasMaxLength(100);
        });

        modelBuilder.Entity<user_role>(entity =>
        {
            entity.HasKey(e => new { e.user_id, e.role_id })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.HasIndex(e => e.role_id, "fk_userroles_role");

            entity.Property(e => e.assigned_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.role).WithMany(p => p.user_roles)
                .HasForeignKey(d => d.role_id)
                .HasConstraintName("fk_userroles_role");

            entity.HasOne(d => d.user).WithMany(p => p.user_roles)
                .HasForeignKey(d => d.user_id)
                .HasConstraintName("fk_userroles_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
