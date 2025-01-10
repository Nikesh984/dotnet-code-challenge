using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Enable lazy loading proxies to display direct reportees in the response
            // Not using lazyLoading as it is not a best practice

            //options.UseLazyLoadingProxies();
        }


        public DbSet<Employee> Employees { get; set; }

        public DbSet<Compensation> Compensations { get; set; }
    }
}
