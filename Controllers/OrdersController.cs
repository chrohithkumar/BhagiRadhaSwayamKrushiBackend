using Microsoft.AspNetCore.Mvc;
using BhagiRadhaSwayamKrushi.Data;
using BhagiRadhaSwayamKrushi.DTO;
using BhagiRadhaSwayamKrushi.Models;

namespace BhagiRadhaSwayamKrushi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // POST api/orders
        [HttpPost("addOrder")]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            var order = new Order
            {
                Name = dto.Name,
                MobileNumber = dto.MobileNumber,
                Address = dto.Address,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                NormalQty = dto.NormalQty,
                CoolQty = dto.CoolQty,
                TotalAmount = dto.TotalAmount,
                 Status = OrderStatus.Pending,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Order placed successfully",
                orderId = order.Id
            });
        }

        // GET api/orders
  
        [HttpGet]
        public IActionResult GetOrders()
        {
            var ordersWithStatus = (from order in _context.Orders
                                    join user in _context.Users on order.MobileNumber equals user.Mobile into userJoin
                                    from subUser in userJoin.DefaultIfEmpty() // Left Join in case order exists but user doesn't
                                    select new OrderWithUserStatusDto
                                    {
                                        Id = order.Id,
                                        Name = order.Name,
                                        MobileNumber = order.MobileNumber,
                                        Address = order.Address,
                                        NormalQty = order.NormalQty,
                                        CoolQty = order.CoolQty,
                                        TotalAmount = order.TotalAmount,
                                        CreatedAt = order.CreatedAt,
                                        Status = order.Status.ToString(),
                                        UserActiveStatus = subUser != null ? subUser.ActiveStatus : "Unknown"
                                    })
                                    .OrderByDescending(o => o.CreatedAt)
                                    .ToList();

            return Ok(ordersWithStatus);
        }
        // PUT api/orders/update-status/5
        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] int status)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound("Order not found");

            order.Status = (OrderStatus)status;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Status updated successfully" });
        }


        // GET api/orders/user?mobile=1234567890
        [HttpGet("user")]
        public IActionResult GetOrdersByMobile(string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
                return BadRequest("Mobile number is required");

            var orders = (from order in _context.Orders
                          join user in _context.Users on order.MobileNumber equals user.Mobile
                          where order.MobileNumber == mobile
                          select new OrderWithUserStatusDto
                          {
                              Id = order.Id,
                              Name = order.Name,
                              MobileNumber = order.MobileNumber,
                              Address = order.Address,
                              NormalQty = order.NormalQty,
                              CoolQty = order.CoolQty,
                              TotalAmount = order.TotalAmount,
                              CreatedAt = order.CreatedAt,
                              Status = order.Status.ToString(),
                              UserActiveStatus = user.ActiveStatus // Returns "Active" or "Blocked"
                          })
                          .OrderByDescending(o => o.CreatedAt)
                          .ToList();

            return Ok(orders);
        }


    }
}
