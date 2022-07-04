using Microsoft.EntityFrameworkCore;
using TestTaskDomain.Models;

namespace TestTaskDomain.Concrete
{
    public class MyContext : DbContext
    {
        public virtual DbSet<CompanySubdivision> CompanySubdivisions { get; set; }

        public MyContext(DbContextOptions<MyContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestDB3;Trusted_Connection=True;");
        }
    }
     
}
