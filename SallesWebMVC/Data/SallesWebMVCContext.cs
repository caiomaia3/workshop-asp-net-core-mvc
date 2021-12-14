using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SallesWebMVC.Models;

namespace SallesWebMVC.Data
{
    public class SallesWebMVCContext : DbContext
    {
        public SallesWebMVCContext (DbContextOptions<SallesWebMVCContext> options)
            : base(options)
        {
        }

        public DbSet<SallesWebMVC.Models.Department> Department { get; set; }
    }
}
