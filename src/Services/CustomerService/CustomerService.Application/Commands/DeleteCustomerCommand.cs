using System;
using System.Threading;
using System.Threading.Tasks;
using CommonLib.Models.ErrorModels;
using CommonLib.Rabbit.Producer;
using CustomerService.Application.Events.Sourcing;
using CustomerService.Application.ProducerExtensions;
using Entities.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Repository;

namespace CustomerService.Application.Commands
{
    public class DeleteCustomerCommand: IRequest
    {
        public Guid Id { get; set; }
    }


    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly IRepository<Customer> _repository;
        private readonly IDistributedCache _cache;
        private readonly EventProducer _producer;

        public DeleteCustomerCommandHandler(IRepository<Customer> repository, IDistributedCache cache, EventProducer producer)
        {
            _repository = repository;
            _cache = cache;
            _producer = producer;
        }

        public async Task<Unit> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
        {
            var customerEntity = await _repository.GetByIdAsync(command.Id);
            if (customerEntity == null)
            {
                throw new NotFound(nameof(Customer), command.Id.ToString());
            }
            _repository.Delete(customerEntity);
            await _cache.RemoveAsync($"customer:{customerEntity.Id}", cancellationToken);
            _producer.PublishCustomerDeleted(customerEntity.Id);
            return Unit.Value;
        }
    }
}