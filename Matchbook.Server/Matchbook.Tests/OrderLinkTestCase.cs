using Matchbook.Db;
using Matchbook.Model;
using Matchbook.Tests.Persistence;
using Matchbook.WebHost.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Matchbook.Tests
{
    public class OrderLinkTestCase: IClassFixture<DbContextFixture>
    {
        //public static MatchbookDbContextWithData dbContextOptions { get; }
       // public static DbContextOptions<MatchbookDbContext> dbContextOptions { get; }

        private readonly DbContextFixture _fixture;
        //private MatchbookDbContext dbContext = _fixture.NewDbContext();

        public readonly MatchbookDbContext MatchbookDbContextWithData;
        //public static string connectionString = "Server=localhost\\SQLEXPRESS;Database=Matchbook;Trusted_Connection=True;";

        public OrderLinkTestCase(DbContextFixture fixture)
        {
            this._fixture = fixture;
            
            //dbContextOptions = new DbContextOptionsBuilder<MatchbookDbContext>()
            //  .UseInMemoryDatabase(databaseName: "Matchbook").Options;

            MatchbookDbContextWithData =  _fixture.NewDbContext();
        }


        #region Get By Id

        [Fact]
        public async void Task_GetLinkById_Return_NotFoundResult()
        {
            //Arrange
            var controller = new OrderLinkController(MatchbookDbContextWithData);
            var linkId = 1;

            //Act
            var data = controller.Get(1);

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_GetLinkById_Return_OKResult()
        {
            //Arrange
            var controller = new OrderLinkController(MatchbookDbContextWithData);
            var linkId = 1;

            var linkid = new OrderLink
            {
                LinkedOrders = new List<Order> { DataGenerator.NewOrder(), DataGenerator.NewOrder() },
                LinkName = "Test"
            };

            MatchbookDbContextWithData.OrderLink.Add(linkid);
            MatchbookDbContextWithData.SaveChanges();

            //Act
            var data =  controller.Get(linkId);

            //Assert
            Assert.IsType<OkObjectResult>(data);
            
        }


        #endregion

        #region Get All

        [Fact]
        public async void Task_GetAll_Return_OkResult()
        {
            //Arrange
            var controller = new OrderLinkController(MatchbookDbContextWithData);
            //var linkId = 1;

            //Act
            var data = controller.Get();

            //Assert
            Assert.IsType<Task<ActionResult<List<OrderLinkSummary>>>>(data);
        }

        #endregion

        #region Add new Link

        [Fact]
        public async void Task_AddNewLink_Return_OkResult_NoMatch()
        {
            //Arrange
           
            //var linkId = 1;
            var orderlinkrequest = new OrderLinkrequest
            {
                orderIds = "1,2",
                orderLinkname = "Test"
            };

                var order = DataGenerator.NewOrder();
                MatchbookDbContextWithData.Orders.Add(order);
                MatchbookDbContextWithData.SaveChanges();

            var order1 = DataGenerator.NewOrder();
            MatchbookDbContextWithData.Orders.Add(order1);
            MatchbookDbContextWithData.SaveChanges();

            var order2 = DataGenerator.NewOrder();
            MatchbookDbContextWithData.Orders.Add(order2);
            MatchbookDbContextWithData.SaveChanges();



            var controller = new OrderLinkController(MatchbookDbContextWithData);
            //Act
            var data = controller.Post(orderlinkrequest);

            //Assert
            Assert.IsType<HttpResponseMessage>(data);
            Assert.Equal(data.StatusCode, HttpStatusCode.OK);
            Assert.Equal(data.ReasonPhrase.TrimEnd(), "No Matching Item Found");
        }


        [Fact]
        public async void Task_AddNewLink_Return_OkResult_Match()
        {
            //Arrange

            //var linkId = 1;
            var orderlinkrequest = new OrderLinkrequest
            {
                orderIds = "1,3",
                orderLinkname = "Test"
            };

            var order = DataGenerator.NewOrder();
            MatchbookDbContextWithData.Orders.Add(order);
            MatchbookDbContextWithData.SaveChanges();

            var order1 = DataGenerator.NewOrder();
            MatchbookDbContextWithData.Orders.Add(order1);
            MatchbookDbContextWithData.SaveChanges();

            var order2 = DataGenerator.NewOrder();
            MatchbookDbContextWithData.Orders.Add(order2);
            MatchbookDbContextWithData.SaveChanges();

            order2.ProductSymbol = order.ProductSymbol;
            order2.SubAccountId = order.SubAccountId;
            MatchbookDbContextWithData.Orders.Update(order2);
            MatchbookDbContextWithData.SaveChanges();




            var controller = new OrderLinkController(MatchbookDbContextWithData);
            //Act
            var data = controller.Post(orderlinkrequest);

            //Assert
            Assert.IsType<HttpResponseMessage>(data);
            Assert.Equal(data.StatusCode, HttpStatusCode.Created);
            Assert.Equal(data.ReasonPhrase.TrimEnd(), "The Order Link has been created with Link Id"+1);
        }

        #endregion


    }
}
