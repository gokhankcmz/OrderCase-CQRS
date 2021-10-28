using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CommonLib.Caching;
using Entities.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Repository;

namespace CustomerService.Application.Commands
{
    public class CreateCustomerCommand :  IRequest
    {
        public string Name { get; set; }
        
        public Address Address { get; set; }
        public string Email { get; set; }
        public Guid CustomerId { get; set; }
        

    }


    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand>
    {
        private readonly IRepository<Customer> _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public CreateCustomerCommandHandler(IRepository<Customer> repository, IMapper mapper, IDistributedCache cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Unit> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Customer>(request);
            await _repository.CreateAsync(entity);
            request.CustomerId = entity.Id;
            await _cache.SetRecordAsync($"customer:{entity.Id}", entity);
            return Unit.Value;
        }
    }
}