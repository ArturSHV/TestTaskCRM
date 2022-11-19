using Microsoft.EntityFrameworkCore;

namespace WebSite.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Provider> Provider { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    Id = 1,
                    Name = "Пенал",
                    OrderId = 1,
                    Quantity = 1,
                    Unit = "шт"
                },
                new OrderItem
                {
                    Id = 2,
                    Name = "Ручка",
                    OrderId = 2,
                    Quantity = 3,
                    Unit = "шт"
                },
                new OrderItem
                {
                    Id = 3,
                    Name = "Тетрадь",
                    OrderId = 3,
                    Quantity = 2,
                    Unit = "шт"
                },
                new OrderItem
                {
                    Id = 4,
                    Name = "Груша",
                    OrderId = 3,
                    Quantity = 1,
                    Unit = "кг"
                },
                new OrderItem
                {
                    Id = 5,
                    Name = "Кабель",
                    OrderId = 4,
                    Quantity = 3,
                    Unit = "м"
                },
                new OrderItem
                {
                    Id = 6,
                    Name = "Яблоко",
                    OrderId = 1,
                    Quantity = 1,
                    Unit = "кг"
                }
                );

            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    Date = DateTime.Now,
                    Number = "121/a-2022",
                    ProviderId = 1
                },
                new Order
                {
                    Id = 2,
                    Date = DateTime.Now,
                    Number = "122/b-2022",
                    ProviderId = 2
                },
                new Order
                {
                    Id = 3,
                    Date = DateTime.Now,
                    Number = "123/a-2022",
                    ProviderId = 3
                },
                new Order
                {
                    Id = 4,
                    Date = DateTime.Now,
                    Number = "124/b-2022",
                    ProviderId = 3
                }
                );

            modelBuilder.Entity<Provider>().HasData(
                new Provider
                {
                    Id = 1,
                    Name = "Поставщик N1"
                },
                new Provider
                {
                    Id = 2,
                    Name = "Поставщик N2"
                },
                new Provider
                {
                    Id = 3,
                    Name = "Поставщик N3"
                }
                );
        }
    }
}
