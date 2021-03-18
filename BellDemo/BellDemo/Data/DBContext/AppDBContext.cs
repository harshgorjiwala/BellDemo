using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BellDemo.Data.DBContext
{
    public class AppDBContext: IdentityDbContext<User>
    {
        private readonly DbContextOptions _options;

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            _options = options;
        }

        public DbSet<WorkFlow> WorkFlows { get; set; }

        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<ServiceCategoryType> serviceCategoryTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkFlow>(x =>
            {
                x.HasOne<ServiceCategoryType>(w => w.ServiceCategoryType).WithMany(s => s.WorkFlows);
                x.HasOne<ServiceCategory>(w => w.ServiceCategory).WithMany(s => s.WorkFlows);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
