using Microsoft.EntityFrameworkCore;

namespace MyWebApiApp.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }
        #region Dbset
        public DbSet<Product> Products { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<User> Users { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(e =>
            {
                e.ToTable("Order");
                e.HasKey(o => o.OrderID);
                e.Property(o => o.DateOrder).HasDefaultValueSql("getutcdate()");
                e.Property(o => o.NameGuess).IsRequired().HasMaxLength(100);

            });
            modelBuilder.Entity<OrderDetail>(e =>
            {
                e.ToTable("Order_Detail");
                e.HasKey(o => new { o.OrderID, o.ProductId });

                //order
                e.HasOne(o => o.Order).WithMany(o => o.OrderDetails)
                 .HasForeignKey(o => o.OrderID)
                 .HasConstraintName("FK_OrderDetail_Order");

                //product
                e.HasOne(p => p.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(p => p.OrderID)
                .HasConstraintName("FK_OrderDetail_Product");
            });
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();
                e.HasIndex(e => e.UserName ).IsUnique();
                e.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                e.Property(e => e.Password).IsRequired().HasMaxLength(100);
                e.Property(e => e.Name).IsRequired().HasMaxLength(150);
                e.Property(e => e.Email).IsRequired().HasMaxLength(150);
            });
        }
    }
}
