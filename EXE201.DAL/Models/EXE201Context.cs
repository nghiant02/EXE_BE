﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EXE201.DAL.Models;

public partial class EXE201Context : DbContext
{
    public EXE201Context()
    {
    }

    public EXE201Context(DbContextOptions<EXE201Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Deposit> Deposits { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<MembershipType> MembershipTypes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<RentalOrder> RentalOrders { get; set; }

    public virtual DbSet<RentalOrderDetail> RentalOrderDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VerifyCode> VerifyCodes { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=LEO\\SQLEXPRESS;Initial Catalog=EXE201;User ID=sa;Password=Abcd1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2A1BCA6BAFFA");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses).HasConstraintName("FK__Address__UserID__7B5B524B");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD79700873C5A");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts).HasConstraintName("FK__Cart__ProductID__787EE5A0");

            entity.HasOne(d => d.User).WithMany(p => p.Carts).HasConstraintName("FK__Cart__UserID__778AC167");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2B4BE98E6E");
        });

        modelBuilder.Entity<Deposit>(entity =>
        {
            entity.HasKey(e => e.DepositId).HasName("PK__Deposit__AB60DF514B50B0B6");

            entity.HasOne(d => d.Order).WithMany(p => p.Deposits).HasConstraintName("FK__Deposit__OrderID__7D439ABD");

            entity.HasOne(d => d.User).WithMany(p => p.Deposits).HasConstraintName("FK__Deposit__UserID__7E37BEF6");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDF67F8194B0");

            entity.HasOne(d => d.Product).WithMany(p => p.Feedbacks).HasConstraintName("FK__Feedback__Produc__73BA3083");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks).HasConstraintName("FK__Feedback__UserID__72C60C4A");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__Inventor__F5FDE6D33AAEAE1E");

            entity.HasOne(d => d.Product).WithMany(p => p.Inventories).HasConstraintName("FK__Inventory__Produ__6FE99F9F");
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__Membersh__92A78599AA4C1A32");

            entity.HasOne(d => d.MembershipType).WithMany(p => p.Memberships).HasConstraintName("FK__Membershi__Membe__7A672E12");

            entity.HasOne(d => d.User).WithMany(p => p.Memberships).HasConstraintName("FK__Membershi__UserI__797309D9");
        });

        modelBuilder.Entity<MembershipType>(entity =>
        {
            entity.HasKey(e => e.MembershipTypeId).HasName("PK__Membersh__F35A3E59A98BFD47");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32392FC554");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications).HasConstraintName("FK__Notificat__UserI__71D1E811");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A58E6EA9BBA");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments).HasConstraintName("FK__Payment__OrderID__70DDC3D8");

            entity.HasOne(d => d.User).WithMany(p => p.Payments).HasConstraintName("FK__Payment__UserID__7C4F7684");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6ED3348DE92");

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK__Product__Categor__6C190EBB");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Rating__FCCDF85CF429E4E0");

            entity.HasOne(d => d.Feedback).WithMany(p => p.Ratings).HasConstraintName("FK__Rating__Feedback__76969D2E");

            entity.HasOne(d => d.Product).WithMany(p => p.Ratings).HasConstraintName("FK__Rating__ProductI__75A278F5");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings).HasConstraintName("FK__Rating__UserID__74AE54BC");
        });

        modelBuilder.Entity<RentalOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__RentalOr__C3905BAF45172F88");

            entity.HasOne(d => d.User).WithMany(p => p.RentalOrders).HasConstraintName("FK__RentalOrd__UserI__6D0D32F4");
        });

        modelBuilder.Entity<RentalOrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailsId).HasName("PK__RentalOr__9DD74D9DACD341BD");

            entity.HasOne(d => d.Order).WithMany(p => p.RentalOrderDetails).HasConstraintName("FK__RentalOrd__Order__6E01572D");

            entity.HasOne(d => d.Product).WithMany(p => p.RentalOrderDetails).HasConstraintName("FK__RentalOrd__Produ__6EF57B66");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE3A21EFF44D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACBDA61EED");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRole__RoleID__6B24EA82"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRole__UserID__6A30C649"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF27604F7DFAC1B0");
                        j.ToTable("UserRole");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                        j.IndexerProperty<int>("RoleId").HasColumnName("RoleID");
                    });
        });

        modelBuilder.Entity<VerifyCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VerifyCo__3214EC07382F4563");

            entity.HasOne(d => d.User).WithMany(p => p.VerifyCodes).HasConstraintName("FK__VerifyCod__UserI__7F2BE32F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}