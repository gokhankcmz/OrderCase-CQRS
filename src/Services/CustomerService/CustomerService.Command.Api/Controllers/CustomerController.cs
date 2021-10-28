using System;
using System.Threading.Tasks;
using AutoMapper;
using CommonLib.Jwt;
using CommonLib.Models.ErrorModels;
using CustomerService.Application.Commands;
using Entities.Models;
using Entities.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Command.Api.Controllers
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

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto createCustomerDto)
        {
            var command = _mapper.Map<CreateCustomerCommand>(createCustomerDto);
            await _mediator.Send(command);
            return new ObjectResult(command.CustomerId) { StatusCode = 201 };
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerDto updateCustomerDto, Guid customerId)
        {
            //ValidateNonRestrictedRequest(customerId);
            var command = _mapper.Map<UpdateCustomerCommand>(updateCustomerDto);
            command.CustomerId = customerId;
            await _mediator.Send(command);
            return Ok(customerId);
        }


        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            //ValidateNonRestrictedRequest(customerId);
            await _mediator.Send(new DeleteCustomerCommand {Id = customerId});
            return Ok();
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