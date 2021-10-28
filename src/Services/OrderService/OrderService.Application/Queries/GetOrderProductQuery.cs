using System;
using System.Threading;
using System.Threading.Tasks;
using CommonLib.Caching;
using CommonLib.Models.ErrorModels;
using Entities.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Repository;

namespace OrderService.Application.Queries
{
    public class GetOrderProductQuery : IRequest<Product>
    {
        public Guid OrderId { get; set; }
    }

    public class GetOrderProductQueryHandler : IRequestHandler<GetOrderProductQuery, Product>
    {
        private readonly IRepository<Order> _repository;
        private readonly IDistributedCache _cache;

        public GetOrderProductQueryHandler(IRepository<Order> repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }
        public async Task<Product> Handle(GetOrderProductQuery request, CancellationToken cancellationToken)
        {
            
            var order = await _cache.GetRecordAsync<Order>($"order:{request.OrderId}");
            if (order == null)
            {
                order = await _repository.GetByIdAsync(request.OrderId);
                if (order == null)
                {
                    throw new NotFound(nameof(Order), request.OrderId.ToString());
                }
                await _cache.SetRecordAsync($"order:{order.Id}", order);
            }
            return (await _repository.GetByIdAsync(request.OrderId)).Product;
        }
    }
}