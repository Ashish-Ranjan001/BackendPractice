using Microsoft.EntityFrameworkCore;
using BackendPractice.AuthModle;
using BackendPractice.ProductModel;


namespace BackendPractice.DataAccessLayer
{
    public class AllDbContext : DbContext
    {
        public AllDbContext(DbContextOptions<AllDbContext> options) : base(options)
        {
        }
        public DbSet<ProductModel.ProductDBModel> Product { get; set; }
        public DbSet<AuthModle.RegisterDBModel> User { get; set; }
    }
}
