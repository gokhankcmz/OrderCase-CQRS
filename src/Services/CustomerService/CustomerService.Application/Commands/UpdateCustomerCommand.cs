using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CommonLib.Caching;
using CommonLib.Models.ErrorModels;
using Entities.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Repository;

namespace CustomerService.Application.Commands
{
    public class UpdateCustomerCommand: IRequest
    {
        public string Name { get; set; }
        
        public Address Address { get; set; }
        public string Email { get; set; }
        public Guid CustomerId { get; set; }
    }


    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
    {
        private readonly IRepository<Customer> _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public UpdateCustomerCommandHandler(IRepository<Customer> repository, IMapper mapper, IDistributedCache cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Unit> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customerEntity = await _repository.GetByIdAsync(command.CustomerId);
            if (customerEntity == null)
            {
                throw new NotFound(nameof(Customer), command.CustomerId.ToString());
            }
            customerEntity.UpdatedAt = DateTime.Now; 
            _mapper.Map(command, customerEntity);
            await _repository.ReplaceAsync(customerEntity);
            await _cache.SetRecordAsync($"customer:{customerEntity.Id}", customerEntity);
            return Unit.Value;
        }
    }
}