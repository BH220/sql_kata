using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using MySqlConnector;
using sql_kata.Models;
using System;
using System.Collections.Generic;
using Task = sql_kata.Models.Task;

namespace sql_kata.Data;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {

    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PRIMARY");

            entity.ToTable("team");

            entity.Property(e => e.TeamId)
                .HasColumnType("int(11)")
                .HasColumnName("team_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.TeamId, "FK_user_team");

            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.Age)
                .HasColumnType("int(11)")
                .HasColumnName("age");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.TeamId)
                .HasColumnType("int(11)")
                .HasColumnName("team_id");

            entity.HasOne(d => d.Team).WithMany(p => p.Users)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_team");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PRIMARY");
            entity.ToTable("project");

            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PRIMARY");
            entity.ToTable("task");

            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId);

            entity.HasOne(d => d.User)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PRIMARY");
            entity.ToTable("comment");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");

            entity.HasOne(d => d.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(d => d.TaskId);

            entity.HasOne(d => d.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        MySqlConnectionStringBuilder mySqlConnBuilder = new MySqlConnectionStringBuilder();
        mySqlConnBuilder.Server = Convert.ToString(ConfigurationManager.AppSettings["Server"]);
        mySqlConnBuilder.Port = Convert.ToUInt32(ConfigurationManager.AppSettings["Port"]);
        mySqlConnBuilder.UserID = Convert.ToString(ConfigurationManager.AppSettings["UserID"]);
        mySqlConnBuilder.Password = Convert.ToString(ConfigurationManager.AppSettings["Password"]);
        mySqlConnBuilder.Database = Convert.ToString(ConfigurationManager.AppSettings["Database"]);
        mySqlConnBuilder.ConnectionTimeout = Convert.ToUInt32(ConfigurationManager.AppSettings["ConnectionTimeout"]);
        mySqlConnBuilder.AllowUserVariables = Convert.ToBoolean(ConfigurationManager.AppSettings["AllowUserVariables"]);
        optionsBuilder.UseMySql(mySqlConnBuilder.ConnectionString,
            ServerVersion.Parse("11.3.0-mariadb"));
    }
}
