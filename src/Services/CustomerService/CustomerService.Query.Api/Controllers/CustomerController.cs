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
    [Route("customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private IMediator _mediator;
        private IMapper _mapper;

        public CustomerController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _mediator.Send(new GetAllCustomersQuery());
            return Ok(customers);
        }

        [HttpGet("{customerId}", Name = "CustomerById")]
        public async Task<IActionResult> GetCustomer(Guid customerId)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery {Id = customerId});
            return Ok(customer);
        }


        private void ValidateNonRestrictedRequest(Guid customerId)
        {
            var idFromToken = Request.Headers.GetClaimOrThrow("nameid");
            if (!idFromToken.Equals(customerId.ToString()))
            {
                throw new UnAuthorized(nameof(Customer), customerId.ToString());
            }
        }
    }
    

}