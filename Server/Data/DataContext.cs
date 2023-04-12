using Microsoft.EntityFrameworkCore;
using RafaStore.Server.Data.Map;
using RafaStore.Shared.Model;

namespace RafaStore.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<UserModel> User { get; set; }
        public DbSet<CustomerModel> Customer { get; set; }
        public DbSet<NoteFileModel> Note { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfiguration(new UserMap());
           modelBuilder.ApplyConfiguration(new CustomerMap());
           modelBuilder.ApplyConfiguration(new FileMap());
        }
    }
}
