using Matchbook.Db;
using Matchbook.Tests.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matchbook.Tests
{
    public class DummyDataDBInitializer
    {
        public DummyDataDBInitializer()
        {
        }

        public void Seed(MatchbookDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Orders.AddRange(
                DataGenerator.NewOrders()
            );

            //context.Post.AddRange(
            //    new Post() { Title = "Test Title 1", Description = "Test Description 1", CategoryId = 2, CreatedDate = DateTime.Now },
            //    new Post() { Title = "Test Title 2", Description = "Test Description 2", CategoryId = 3, CreatedDate = DateTime.Now }
            //);
            context.SaveChanges();
        }
    }
}
