using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Queries;

namespace OrderService.Query.Api.Controllers
{
    [Route("orders/{orderId}/product")]
    [ApiController]
    //[ServiceFilter(typeof(ValidateCustomerExistAttribute))]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderProduct([FromRoute]Guid orderId)
        {
            var product = await _mediator.Send(new GetOrderProductQuery {OrderId = orderId});
            return Ok(product);
        }
    }
}