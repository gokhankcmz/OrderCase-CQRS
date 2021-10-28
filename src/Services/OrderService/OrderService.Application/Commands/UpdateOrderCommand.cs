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

namespace OrderService.Application.Commands
{
    public class UpdateOrderCommand: IRequest
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Product Product { get; set; }
        public Guid OrderId { get; set; }
    }


    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IRepository<Order> _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public UpdateOrderCommandHandler(IRepository<Order> repository, IMapper mapper, IDistributedCache cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Unit> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderEntity = await _repository.GetByIdAsync(command.OrderId);
            if (orderEntity == null)
            {
                throw new NotFound(nameof(Order), command.OrderId.ToString());
            }
            orderEntity.UpdatedAt = DateTime.Now; 
            _mapper.Map(command, orderEntity);
            await _repository.ReplaceAsync(orderEntity);
            await _cache.SetRecordAsync($"customer:{orderEntity.Id}", orderEntity);
            return Unit.Value;
        }
    }
}