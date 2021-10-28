using System.Threading.Tasks;
using AutoMapper;
using CustomerService.Application.Commands;
using Entities.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CustomerService.Command.Api.Controllers
{
    [Route("token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TokenController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] AuthDto authDto)
        {
            var command = _mapper.Map<CreateTokenCommand>(authDto);
            await _mediator.Send(command);
            return Ok(command.Token.ToJson());
        }
    }
}