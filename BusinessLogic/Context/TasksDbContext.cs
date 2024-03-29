﻿using System;
using System.Collections.Generic;
using BusinessLogic.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Context;

public partial class TasksDbContext : DbContext
{
    public TasksDbContext()
    {
    }

    public TasksDbContext(DbContextOptions<TasksDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Freelancer> Freelancers { get; set; }

    public virtual DbSet<Invite> Invites { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Taskproject> Taskprojects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTask> UserTasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:15432;Database=es2;Username=es2;Password=es2;");

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

        modelBuilder.Entity<Invite>(entity =>
        {
            entity.HasKey(e => new { e.Projectid, e.Freelancerid }).HasName("invite_pkey");

            entity.ToTable("invite");

            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Freelancerid).HasColumnName("freelancerid");
            entity.Property(e => e.Isaccepted).HasColumnName("isaccepted");

            entity.HasOne(d => d.Freelancer).WithMany(p => p.Invites)
                .HasForeignKey(d => d.Freelancerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invite_freelancerid_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.Invites)
                .HasForeignKey(d => d.Projectid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invite_projectid_fkey");
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

            entity.HasOne(d => d.Projectleader).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Projectleaderid)
                .HasConstraintName("project_projectleaderid_fkey");
        });

        modelBuilder.Entity<Taskproject>(entity =>
        {
            entity.HasKey(e => new { e.Taskid, e.Projectid }).HasName("taskproject_pkey");

            entity.ToTable("taskproject");

            entity.Property(e => e.Taskid).HasColumnName("taskid");
            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.AuxColumn).HasColumnName("aux_column");

            entity.HasOne(d => d.Project).WithMany(p => p.Taskprojects)
                .HasForeignKey(d => d.Projectid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("taskproject_projectid_fkey");

            entity.HasOne(d => d.Task).WithMany(p => p.Taskprojects)
                .HasForeignKey(d => d.Taskid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("taskproject_taskid_fkey");
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
            entity.Property(e => e.Clientid).HasColumnName("clientid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Enddate).HasColumnName("enddate");
            entity.Property(e => e.Freelancerid).HasColumnName("freelancerid");
            entity.Property(e => e.Pricehour).HasColumnName("pricehour");
            entity.Property(e => e.Startdate).HasColumnName("startdate");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Client).WithMany(p => p.UserTasks)
                .HasForeignKey(d => d.Clientid)
                .HasConstraintName("user_task_clientid_fkey");

            entity.HasOne(d => d.Freelancer).WithMany(p => p.UserTasks)
                .HasForeignKey(d => d.Freelancerid)
                .HasConstraintName("user_task_freelancerid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
