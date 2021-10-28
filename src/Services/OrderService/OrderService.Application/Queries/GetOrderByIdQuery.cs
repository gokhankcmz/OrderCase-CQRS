using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CommonLib.Caching;
using CommonLib.Models.ErrorModels;
using Entities.Models;
using Entities.ResponseModels;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Repository;

namespace OrderService.Application.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderResponseDto>
    {
        public Guid Id { get; set; }
    }
    
    public class GetOrderByIdQueryHandler :  IRequestHandler<GetOrderByIdQuery, OrderResponseDto>
    {
        private readonly IRepository<Order>  _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public GetOrderByIdQueryHandler(IRepository<Order>  repository, IMapper mapper, IDistributedCache cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<OrderResponseDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var orderFromCache = await _cache.GetRecordAsync<Order>($"order:{request.Id}");
            if (orderFromCache == null)
            {
                var orderFromDb = await _repository.GetByIdAsync(request.Id);
                if (orderFromDb == null)
                {
                    throw new NotFound(nameof(Order), request.Id.ToString());
                }

                await _cache.SetRecordAsync($"order:{orderFromDb.Id}", orderFromDb);
                return _mapper.Map<OrderResponseDto>(orderFromDb);
            }
            
            return _mapper.Map<OrderResponseDto>(orderFromCache);
        }
    }
}