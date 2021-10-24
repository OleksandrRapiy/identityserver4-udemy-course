using Microsoft.EntityFrameworkCore;

namespace IdentityServerCourse.API.Models
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) 
            : base(options)
        { }

        public DbSet<Customer> Customers { get; set; }
    }
}
