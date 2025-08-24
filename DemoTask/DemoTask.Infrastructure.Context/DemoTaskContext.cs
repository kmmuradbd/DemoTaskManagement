using DemoTask.Domain.DomainObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Infrastructure.Context
{
    public class DemoTaskContext:DbContext
    {
        public DemoTaskContext(DbContextOptions<DemoTaskContext> options)
           : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<MasterMenu> MasterMenus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
