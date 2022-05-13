using Microsoft.EntityFrameworkCore;
using Tracking.Data.General;

namespace Tracking
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Users> Users{ get;set; }
        public DbSet<Transactions> Transactions{ get;set; }
    }
}
