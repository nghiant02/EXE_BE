
using EXE201.DAL.Models;
using Microsoft.EntityFrameworkCore;

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

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseSqlServer("Server=(local);uid=sa;pwd=Abcd1234;database=EXE201;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {

            entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2A1BF7B1ED9B");

            entity.ToTable("Address");

            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Street)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Address__UserID__5629CD9C");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD79719B3F71D");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Cart__ProductID__571DF1D5");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Cart__UserID__5812160E");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2B379E3808");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryDescription).HasColumnType("text");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Deposit>(entity =>
        {
            entity.HasKey(e => e.DepositId).HasName("PK__Deposit__AB60DF519D2E3953");

            entity.ToTable("Deposit");

            entity.Property(e => e.DepositId).HasColumnName("DepositID");
            entity.Property(e => e.DateDeposited).HasColumnType("datetime");
            entity.Property(e => e.DepositAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DepositStatus)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Order).WithMany(p => p.Deposits)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Deposit__OrderID__59063A47");

            entity.HasOne(d => d.User).WithMany(p => p.Deposits)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Deposit__UserID__59FA5E80");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {

            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDF69FDA649B");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.DateGiven).HasColumnType("datetime");
            entity.Property(e => e.FeedbackComment).HasColumnType("text");
            entity.Property(e => e.FeedbackImage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FeedbackStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Product).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Feedback__Produc__5AEE82B9");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Feedback__UserID__5BE2A6F2");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {

            entity.HasKey(e => e.InventoryId).HasName("PK__Inventor__F5FDE6D348F220C9");

            entity.ToTable("Inventory");

            entity.Property(e => e.InventoryId).HasColumnName("InventoryID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Inventory__Produ__5CD6CB2B");
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__Membersh__92A785994CE72E59");

            entity.ToTable("Membership");

            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.MembershipStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MembershipTypeId).HasColumnName("MembershipTypeID");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.MembershipType).WithMany(p => p.Memberships)
                .HasForeignKey(d => d.MembershipTypeId)
                .HasConstraintName("FK__Membershi__Membe__5DCAEF64");

            entity.HasOne(d => d.User).WithMany(p => p.Memberships)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Membershi__UserI__5EBF139D");
        });

        modelBuilder.Entity<MembershipType>(entity =>
        {

            entity.HasKey(e => e.MembershipTypeId).HasName("PK__Membersh__F35A3E59823ED399");

            entity.ToTable("MembershipType");

            entity.Property(e => e.MembershipTypeId).HasColumnName("MembershipTypeID");
            entity.Property(e => e.MembershipBenefits).HasColumnType("text");
            entity.Property(e => e.MembershipDescription).HasColumnType("text");
            entity.Property(e => e.MembershipTypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Notification>(entity =>
        {

            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32002451C9");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.DateSent).HasColumnType("datetime");
            entity.Property(e => e.NotificationMessage).HasColumnType("text");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notificat__UserI__5FB337D6");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A5809C034B0");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Payment__OrderID__60A75C0F");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Payment__UserID__619B8048");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6ED420650F5");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ProductDescription).HasColumnType("text");
            entity.Property(e => e.ProductImage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProductStatus)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__Categor__628FA481");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Rating__FCCDF85C6A96E80A");

            entity.ToTable("Rating");

            entity.Property(e => e.RatingId).HasColumnName("RatingID");
            entity.Property(e => e.DateGiven).HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Product).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Rating__ProductI__6383C8BA");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Rating__UserID__6477ECF3");
        });

        modelBuilder.Entity<RentalOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__RentalOr__C3905BAFC3FA06A0");

            entity.ToTable("RentalOrder");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.DatePlaced).HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.OrderTotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.RentalOrders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__RentalOrd__UserI__656C112C");
        });

        modelBuilder.Entity<RentalOrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailsId).HasName("PK__RentalOr__9DD74D9D109BF719");

            entity.Property(e => e.OrderDetailsId).HasColumnName("OrderDetailsID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.RentalEnd).HasColumnType("datetime");
            entity.Property(e => e.RentalStart).HasColumnType("datetime");
            entity.HasOne(d => d.Order).WithMany(p => p.RentalOrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__RentalOrd__Order__66603565");

            entity.HasOne(d => d.Product).WithMany(p => p.RentalOrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__RentalOrd__Produ__6754599E");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE3A830BC3ED");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC54CA819A");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.AccountStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ProfileImage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
                
            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRole__RoleID__68487DD7"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRole__UserID__693CA210"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF27604FCD7E2BD8");
                        j.ToTable("UserRole");
                    });
        });

        modelBuilder.Entity<VerifyCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VerifyCo__3214EC07F60D94EB");

            entity.ToTable("VerifyCode");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)")
                .HasComment("");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(NULL)")
                .HasComment("");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)")
                .HasComment("");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
