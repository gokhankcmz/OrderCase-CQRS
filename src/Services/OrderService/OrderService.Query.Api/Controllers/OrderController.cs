using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Queries;

namespace OrderService.Query.Api.Controllers
{
    [Route("orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrderController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _mediator.Send(new GetAllOrdersQuery());
            return Ok(orders);
        }
        
        [HttpGet("{orderId}", Name = "OrderById")] 
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery {Id = orderId});
            return Ok(order);
        }

        
        
    }
}