using System;
using System.Threading.Tasks;
using AutoMapper;
using Entities.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands;

namespace OrderService.Command.Api.Controllers
{
    [Route("orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IMediator _mediator;
        private IMapper _mapper;

        public OrderController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        
        [HttpPost] 
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderDto createOrderDto)
        {
            var command = _mapper.Map<CreateOrderCommand>(createOrderDto);
            await _mediator.Send(command);
            return new ObjectResult(command.OrderId) { StatusCode = 201 };
        }
        
        [HttpPut("{orderId}")] 
        public async Task<IActionResult> UpdateOrder([FromBody]UpdateOrderDto updateOrderDto, Guid orderId)
        {
            var command = _mapper.Map<UpdateOrderCommand>(updateOrderDto);
            command.OrderId = orderId;
            await _mediator.Send( command);
            return Ok(orderId);
        }
        
        
        [HttpDelete("{orderId}")] 
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            await _mediator.Send( new DeleteOrderCommand {Id = orderId});
            return Ok();
        }
        
        
    }
}