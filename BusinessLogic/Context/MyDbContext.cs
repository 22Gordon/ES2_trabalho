using System;
using System.Collections.Generic;
using BusinessLogic.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Freelancer> Freelancers { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTask> UserTasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:15432;Database=es2;Username=es2;Password=es2");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("postgis")
            .HasPostgresExtension("uuid-ossp")
            .HasPostgresExtension("topology", "postgis_topology");

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("client_pkey");

            entity.ToTable("client");

            entity.Property(e => e.Userid)
                .ValueGeneratedNever()
                .HasColumnName("userid");

            entity.HasOne(d => d.User).WithOne(p => p.Client)
                .HasForeignKey<Client>(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("client_userid_fkey");
        });

        modelBuilder.Entity<Freelancer>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("freelancer_pkey");

            entity.ToTable("freelancer");

            entity.Property(e => e.Userid)
                .ValueGeneratedNever()
                .HasColumnName("userid");
            entity.Property(e => e.Dailyavghours).HasColumnName("dailyavghours");

            entity.HasOne(d => d.User).WithOne(p => p.Freelancer)
                .HasForeignKey<Freelancer>(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("freelancer_userid_fkey");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Projectid).HasName("project_pkey");

            entity.ToTable("project");

            entity.Property(e => e.Projectid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("projectid");
            entity.Property(e => e.Clientid).HasColumnName("clientid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Pricehour).HasColumnName("pricehour");
            entity.Property(e => e.Projectleaderid).HasColumnName("projectleaderid");

            entity.HasOne(d => d.Client).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Clientid)
                .HasConstraintName("project_clientid_fkey");

            entity.HasOne(d => d.Projectleader).WithMany(p => p.ProjectsNavigation)
                .HasForeignKey(d => d.Projectleaderid)
                .HasConstraintName("project_projectleaderid_fkey");

            entity.HasMany(d => d.Freelancers).WithMany(p => p.Projects)
                .UsingEntity<Dictionary<string, object>>(
                    "Invite",
                    r => r.HasOne<Freelancer>().WithMany()
                        .HasForeignKey("Freelancerid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("invite_freelancerid_fkey"),
                    l => l.HasOne<Project>().WithMany()
                        .HasForeignKey("Projectid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("invite_projectid_fkey"),
                    j =>
                    {
                        j.HasKey("Projectid", "Freelancerid").HasName("invite_pkey");
                        j.ToTable("invite");
                        j.IndexerProperty<Guid>("Projectid").HasColumnName("projectid");
                        j.IndexerProperty<Guid>("Freelancerid").HasColumnName("freelancerid");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Userid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("userid");
            entity.Property(e => e.Displayname)
                .HasMaxLength(255)
                .HasColumnName("displayname");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserTask>(entity =>
        {
            entity.HasKey(e => e.Taskid).HasName("user_task_pkey");

            entity.ToTable("user_task");

            entity.Property(e => e.Taskid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("taskid");
            entity.Property(e => e.Enddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("enddate");
            entity.Property(e => e.Freelancerid).HasColumnName("freelancerid");
            entity.Property(e => e.Pricehour).HasColumnName("pricehour");
            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Startdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("startdate");

            entity.HasOne(d => d.Freelancer).WithMany(p => p.UserTasks)
                .HasForeignKey(d => d.Freelancerid)
                .HasConstraintName("user_task_freelancerid_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.UserTasks)
                .HasForeignKey(d => d.Projectid)
                .HasConstraintName("user_task_projectid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
