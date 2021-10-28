using System;
using System.Threading.Tasks;
using Entities.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    
    [Route("orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly Service _orderServiceCommand;
        private readonly Service _orderServiceQuery;

        public OrdersController()
        {
            _orderServiceCommand = new Service("orderservicecommand", 80);
            _orderServiceQuery = new Service("orderservicequery", 80);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderDto createOrderDto)
        {
            var (response, _) = await _orderServiceCommand.Route(HttpContext);
            return Ok(response);
        }


        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var (response, _) = await _orderServiceQuery.Route(HttpContext);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var (response, _) = await _orderServiceQuery.Route(HttpContext);
            return Ok(response);
        }

        [HttpPut("{orderId:guid}")]
        public async Task<IActionResult> UpdateOrder(Guid orderId, [FromBody] UpdateOrderDto updateOrderDto)
        {
            var (response, _) = await _orderServiceCommand.Route(HttpContext, bodyArg:updateOrderDto);
            return Ok(response);
        }


        [HttpDelete("{orderId:guid}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var (response, _) = await _orderServiceCommand.Route(HttpContext);
            return Ok(response);
        }
    }
}