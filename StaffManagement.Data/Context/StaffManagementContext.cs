using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Data.Entities;
using TaskEntity = StaffManagement.Data.Entities.Task;

namespace StaffManagement.Data.Context;

public partial class StaffManagementContext : DbContext
{
    public StaffManagementContext(DbContextOptions<StaffManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<StaffInTask> StaffInTasks { get; set; }

    public virtual DbSet<TaskEntity> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Staff__3214EC07E193EC50");

            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.ShortName).HasMaxLength(50);
        });

        modelBuilder.Entity<StaffInTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StaffInT__3214EC0762CDEF99");

            entity.ToTable("StaffInTask");

            entity.Property(e => e.Idstaff).HasColumnName("IDStaff");
            entity.Property(e => e.Idtask).HasColumnName("IDTask");

            entity.HasOne(d => d.IdstaffNavigation).WithMany(p => p.StaffInTasks)
                .HasForeignKey(d => d.Idstaff)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StaffInTask_Staff");

            entity.HasOne(d => d.IdtaskNavigation).WithMany(p => p.StaffInTasks)
                .HasForeignKey(d => d.Idtask)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StaffInTask_Task");
        });

        modelBuilder.Entity<TaskEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Task__3214EC072A2741C5");

            entity.ToTable("Task");

            entity.Property(e => e.Duration).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Idparent).HasColumnName("IDParent");
            entity.Property(e => e.Label).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.IdparentNavigation).WithMany(p => p.InverseIdparentNavigation)
                .HasForeignKey(d => d.Idparent)
                .HasConstraintName("FK_Task_Parent");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
