using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommonLib.Jwt;
using CommonLib.Models.ErrorModels;
using Entities.Models;
using MediatR;
using Repository;

namespace CustomerService.Application.Commands
{
    public class CreateTokenCommand : IRequest
    {
        public Guid Id { get; set; }
        public string email { get; set; }
        public string Token { get; set; }
    }
    
    public class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand>
    {
        private readonly IRepository<Customer> _repository;
        private readonly AuthenticationManager<Customer> _authManager;

        public CreateTokenCommandHandler(IRepository<Customer> repository, AuthenticationManager<Customer> authManager)
        {
            _repository = repository;
            _authManager = authManager;
        }

        public async Task<Unit> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            var customer = (await _repository.GetByConditionAsync(x => x.Id.Equals(request.Id) && x.Email.Equals(request.email))).FirstOrDefault();
            if (customer == null)
            {
                throw new Conflict();
            }

            request.Token = _authManager.Authenticate(customer);
            return Unit.Value;
        }
    }
}