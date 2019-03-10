using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace invoicing.server.Data
{
    public class InvoicingDbContext : IdentityDbContext<Models.User, IdentityRole<Guid>, Guid> {
        public DbSet<Models.Organization> Organizations {get;set;}

        public InvoicingDbContext(DbContextOptions<InvoicingDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}