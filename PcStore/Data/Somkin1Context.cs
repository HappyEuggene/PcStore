using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PcStore.Models;

namespace PcStore.Data;

public partial class Somkin1Context : DbContext
{
    public Somkin1Context()
    {
    }

    public Somkin1Context(DbContextOptions<Somkin1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemSpec> ItemSpecs { get; set; }

    public virtual DbSet<ItemType> ItemTypes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item");

            entity.HasIndex(e => e.ItemSpecId, "IX_Item_ItemSpec");

            entity.HasIndex(e => e.ItemTypeId, "IX_Item_ItemType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.ItemSpec).WithMany(p => p.Items).HasForeignKey(d => d.ItemSpecId);

            entity.HasOne(d => d.ItemType).WithMany(p => p.Items).HasForeignKey(d => d.ItemTypeId);

            entity.HasMany(d => d.Orders).WithMany(p => p.Items)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderItem",
                    r => r.HasOne<Order>().WithMany()
                        .HasForeignKey("OrdersId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_OrderItem_Orders"),
                    l => l.HasOne<Item>().WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_OrderItem_Item"),
                    j =>
                    {
                        j.HasKey("ItemId", "OrdersId");
                        j.ToTable("OrderItem");
                        j.HasIndex(new[] { "OrdersId" }, "IX_OrderItem_Orders");
                    });
        });

        modelBuilder.Entity<ItemSpec>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ItemSpec_1");

            entity.ToTable("ItemSpec");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cores)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Frequency)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MemCapacity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MemType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Power)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ItemType>(entity =>
        {
            entity.ToTable("ItemType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Manufacture)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Model)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Orders_Users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Delivery)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
