using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CommonLib.Caching;
using Entities.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Repository;

namespace OrderService.Application.Commands
{
    public class CreateOrderCommand : IRequest
    {
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Product Product { get; set; }
        public Guid OrderId { get; set; }
    }


    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
    {
        private readonly IRepository<Order> _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public CreateOrderCommandHandler(IRepository<Order> repository, IMapper mapper, IDistributedCache cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Order>(request);
            await _repository.CreateAsync(entity);
            request.OrderId = entity.Id;
            await _cache.SetRecordAsync($"order:{entity.Id}", entity);
            return Unit.Value;
        }
    }
}