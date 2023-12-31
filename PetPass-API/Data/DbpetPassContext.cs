﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PetPass_API.Models;

namespace PetPass_API.Data;

public partial class DbPetPassContext : DbContext
{
    public DbPetPassContext()
    {
    }

    public DbPetPassContext(DbContextOptions<DbPetPassContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Campaign> Campaigns { get; set; }

    public virtual DbSet<ConfigPet> ConfigPets { get; set; }

    public virtual DbSet<ConfigUser> ConfigUsers { get; set; }

    public virtual DbSet<Patrol> Patrols { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonRegister> PersonRegisters { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<PetRegister> PetRegisters { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Zone> Zones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Campaign>(entity =>
        {
            entity.Property(e => e.State)
                .HasComputedColumnSql("(case when [EndDate]<getdate() then CONVERT([bit],(0)) else CONVERT([bit],(1)) end)", false)
                .HasComment("0 : Inactive\r\n1: Active");
        });

        modelBuilder.Entity<ConfigPet>(entity =>
        {
            entity.HasOne(d => d.Pet).WithMany(p => p.ConfigPets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Config_Pet_Pet1");
        });

        modelBuilder.Entity<ConfigUser>(entity =>
        {
            entity.HasKey(e => e.UserImageId).HasName("PK_Config_User_1");

            entity.HasOne(d => d.Person).WithMany(p => p.ConfigUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Config_User_User");
        });

        modelBuilder.Entity<Patrol>(entity =>
        {
            entity.Property(e => e.PatrolDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Campaign).WithMany(p => p.Patrols)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patrol_Campaign");

            entity.HasOne(d => d.Person).WithMany(p => p.Patrols)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patrol_User");

            entity.HasOne(d => d.Zone).WithMany(p => p.Patrols)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patrol_Zone");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.Property(e => e.Gender)
                .IsFixedLength()
                .HasComment("F : Female\r\nM : Male");
            entity.Property(e => e.State)
                .HasDefaultValueSql("((1))")
                .HasComment("0 : Inactive\r\n1: Active");
        });

        modelBuilder.Entity<PersonRegister>(entity =>
        {
            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Person).WithMany(p => p.PersonRegisters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Person_Register_Person");

            entity.HasOne(d => d.UserPerson).WithMany(p => p.PersonRegisters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Person_Register_User");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.Property(e => e.Gender)
                .IsFixedLength()
                .HasComment("F : Female\r\nM : Male");
            entity.Property(e => e.State)
                .HasDefaultValueSql("((1))")
                .HasComment("0 : Inactive\r\n1: Active");

            entity.HasOne(d => d.Person).WithMany(p => p.Pets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pet_Person");
        });

        modelBuilder.Entity<PetRegister>(entity =>
        {
            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Pet).WithMany(p => p.PetRegisters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pet_Register_Pet");

            entity.HasOne(d => d.UserPerson).WithMany(p => p.PetRegisters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pet_Register_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.PersonId).ValueGeneratedNever();
            entity.Property(e => e.FirstSessionLogin)
                .HasDefaultValueSql("((1))")
                .IsFixedLength()
                .HasComment("1: TRUE\r\n0: FALSE");
            entity.Property(e => e.Rol)
                .IsFixedLength()
                .HasComment("A: Admin\r\nB: Brigadier\r\nO: Owner");

            entity.HasOne(d => d.Person).WithOne(p => p.User)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Person");
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.Property(e => e.ZoneId).ValueGeneratedOnAdd();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
