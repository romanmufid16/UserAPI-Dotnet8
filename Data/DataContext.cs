using Microsoft.EntityFrameworkCore;
using UserAPI_Dotnet8.Entities;

namespace UserAPI_Dotnet8.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
