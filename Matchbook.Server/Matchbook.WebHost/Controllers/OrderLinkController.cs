using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Matchbook.Db;
using Matchbook.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Matchbook.WebHost.Controllers
{

    public class OrderLinkSummary
    {
        public int Id { get; set; }
        public string LinkName { get; set; }
       
    }

    public class OrderLinkrequest
    {
        public string orderIds { get; set; }
        public string orderLinkname { get; set; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class OrderLinkController : ControllerBase
    {

        private readonly MatchbookDbContext dbContext;

        public OrderLinkController(MatchbookDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        // GET: api/OrderLink
        [HttpGet]
        public async Task<ActionResult<List<OrderLinkSummary>>> Get()
        {
            return await dbContext.OrderLink
               .Select(o => new OrderLinkSummary
               {
                   Id = o.Id,
                   LinkName = o.LinkName,
               })
               .ToListAsync();
        }

        // GET: api/OrderLink/5
        [HttpGet("{id}", Name = "Get")]
        public  IActionResult Get(int id)
        {
            var result =  dbContext.OrderLink
               .Select(o => new OrderLinkSummary
               {
                   Id = o.Id,
                   LinkName = o.LinkName,
               }).Where(x => x.Id == id).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }

        }

        // POST: api/OrderLink
        [HttpPost]
        [Route("~/orderlinking")]
        public HttpResponseMessage Post([FromBody] OrderLinkrequest linkrequest)
        {
            //var response = HttpContext.Response;
            var orderitemssplit = linkrequest.orderIds.Split(',');
            for(int i=0; i< orderitemssplit.Length;i++) 
            {
                var item1 = dbContext.Orders.FirstOrDefault(x => x.Id == Convert.ToUInt32(orderitemssplit[i]));
                if(item1==null)
                {
                    return new HttpResponseMessage() { StatusCode = HttpStatusCode.NotFound, ReasonPhrase = "The Order Id not found " };

                }

                if (item1.LinkId==null)
                {
                    for (int j = 1; j < orderitemssplit.Length; j++)
                    {
                        var item2 = dbContext.Orders.FirstOrDefault(x => x.Id == Convert.ToUInt32(orderitemssplit[j]));
                        if(item2==null)
                        {
                            continue;
                        }
                        if (item2.LinkId == null)
                        {
                            if (item1.SubAccountId == item2.SubAccountId && item1.ProductSymbol == item2.ProductSymbol)
                            {
                                if(dbContext.OrderLink.Any(x=>x.LinkName== linkrequest.orderLinkname))
                                {
                                    return new HttpResponseMessage() { StatusCode = HttpStatusCode.Forbidden, ReasonPhrase = "The Link Name is already provided" };
                                }
                                else
                                {
                                    var orderlink = new OrderLink();
                                    orderlink.LinkName = linkrequest.orderLinkname;
                                    orderlink.LinkedOrders = new List<Order>() { item1, item2 };
                                    dbContext.OrderLink.Add(orderlink);
                                    dbContext.SaveChanges();

                                    item1.LinkId = dbContext.OrderLink.FirstOrDefault(x => x.LinkName == linkrequest.orderLinkname).Id;
                                    item2.LinkId = dbContext.OrderLink.FirstOrDefault(x => x.LinkName == linkrequest.orderLinkname).Id;
                                    dbContext.Orders.Update(item1);
                                    dbContext.Orders.Update(item2);
                                    dbContext.SaveChanges();

                                    return new HttpResponseMessage() { StatusCode = HttpStatusCode.Created, ReasonPhrase = "The Order Link has been created with Link Id" + dbContext.OrderLink.FirstOrDefault(x => x.LinkName == linkrequest.orderLinkname).Id };

                                }
                            }
                        }
                    }
                    return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "No Matching Item Found " };
                }
                else
                {
                    return new HttpResponseMessage() { StatusCode = HttpStatusCode.Forbidden,ReasonPhrase="The Order is already Linked" };
                }
            }

            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "No Matching Item Found " };

        }

      
    }
}
