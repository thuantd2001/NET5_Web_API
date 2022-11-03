using Microsoft.EntityFrameworkCore;

namespace MyWebApiApp.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }
        #region Dbset
        public DbSet<Product> Products { get; set; }
        public DbSet<Type> Types { get; set; }
        #endregion
    }
}
