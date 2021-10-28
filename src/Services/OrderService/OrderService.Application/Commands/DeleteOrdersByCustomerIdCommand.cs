using System;
using System.Threading;
using System.Threading.Tasks;
using CommonLib.Models.ErrorModels;
using Entities.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Repository;

namespace OrderService.Application.Commands
{
    public class DeleteOrdersByCustomerIdCommand: IRequest
    {
        public Guid CustomerId { get; set; }
    }


    public class DeleteOrdersByCustomerIdCommandHandler : IRequestHandler<DeleteOrdersByCustomerIdCommand>
    {
        private readonly IRepository<Order> _repository;
        private readonly IDistributedCache _cache;

        public DeleteOrdersByCustomerIdCommandHandler(IRepository<Order> repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<Unit> Handle(DeleteOrdersByCustomerIdCommand command, CancellationToken cancellationToken)
        {
            var orders = await  _repository.GetByConditionAsync(x => x.CustomerId.Equals(command.CustomerId));
            foreach (var order in orders)
            {
                _repository.Delete(order);
                await _cache.RemoveAsync($"order:{order.Id}", cancellationToken);
            }
            return Unit.Value;
        }
    }
}