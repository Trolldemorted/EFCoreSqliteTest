
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseManager
{
    public class FooDBContext : DbContext
    {
        static FooDBContext()
        {
            using (var ctx = new FooDBContext())
            {
                if (ctx.Database.GetPendingMigrations().Count() > 0)
                {
                    ctx.Database.Migrate();
                }
                if (ctx.Customers.Count() == 0)
                {
                    ctx.Customers.Add(new Customer()
                    {
                        Name = "kevin",
                        OrdersCount = 0
                    });
                    ctx.SaveChanges();
                }
            }
        }
        private static Semaphore GlobalSemaphore;
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Filename=..\LocalCache\Foo.db", x => x.SuppressForeignKeyEnforcement());
        }
        public static void Lock()
        {
            GlobalSemaphore = new Semaphore(1, 1, "ourfancyexamplesemaphore", out bool b);
            GlobalSemaphore.WaitOne();
        }

        public static void Unlock()
        {
            GlobalSemaphore.Release();
        }

        public static void PlaceOrder()
        {
            Lock();
            try
            {
                using (var ctx = new FooDBContext())
                {
                    Customer customer = ctx.Customers
                        .Include(c => c.Orders)
                        .First();
                    customer.Orders.Add(new Order()
                    {
                        Whatever = "cookies"
                    });
                    customer.OrdersCount += 1;
                    ctx.SaveChanges();
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("{0}: {1}", e.Message, e.StackTrace);
            }
            finally
            {
                Unlock();
            }
        }
    }

    public class Customer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long OrdersCount { get; set; }
        public List<Order> Orders { get; set; }
    }
    public class Order
    {
        public long Id { get; set; }
        public string Whatever { get; set; }
    }
}
