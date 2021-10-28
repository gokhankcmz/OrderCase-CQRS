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
    public class DeleteOrderCommand: IRequest
    {
        public Guid Id { get; set; }
    }


    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IRepository<Order> _repository;
        private readonly IDistributedCache _cache;

        public DeleteOrderCommandHandler(IRepository<Order> repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<Unit> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var orderEntity = await _repository.GetByIdAsync(command.Id);
            if (orderEntity == null)
            {
                throw new NotFound(nameof(Order), command.Id.ToString());
            }
            _repository.Delete(orderEntity);
            await _cache.RemoveAsync($"order:{orderEntity.Id}", cancellationToken);
            return Unit.Value;
        }
    }
}