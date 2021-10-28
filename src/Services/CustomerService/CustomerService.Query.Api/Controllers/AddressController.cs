using System;
using System.Threading.Tasks;
using AutoMapper;
using CommonLib.Jwt;
using CommonLib.Models.ErrorModels;
using CustomerService.Application.Queries;
using Entities.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Query.Api.Controllers
{
    [Route("customers/{customerId}/address")]
    [ApiController]
    //[ServiceFilter(typeof(ValidateCustomerExistAttribute))]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _mediator;
        private IMapper _mapper;

        public AddressController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerAddress([FromRoute]Guid customerId)
        {
            var idFromToken = Request.Headers.GetClaimOrThrow("nameid");
            if (!idFromToken.Equals(customerId.ToString()))
            {
                throw new UnAuthorized(nameof(Customer), customerId.ToString());
            }
            var address = await _mediator.Send(new GetCustomerAddressesQuery {Id = customerId});
            return Ok(address);
        }
    }
}