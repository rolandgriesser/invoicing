using Microsoft.EntityFrameworkCore;

namespace invoicing.server.Data
{
    public class InvoicingDbContext : DbContext {
        public DbSet<Models.Organization> Organizations {get;set;}

        public InvoicingDbContext(DbContextOptions<InvoicingDbContext> options) : base(options)
        {
            
        }
    }
}