using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using Entities.ResponseModels;
using MediatR;
using Repository;

namespace OrderService.Application.Queries
{
    public class GetAllOrdersQuery : IRequest<List<OrderResponseDto>>
    {
    }
    
    public class GetAllOrdersQueryHandler :  IRequestHandler<GetAllOrdersQuery, List<OrderResponseDto>>
    {
        private readonly IRepository<Order>  _repository;
        private readonly IMapper _mapper;

        public GetAllOrdersQueryHandler(IRepository<Order>  repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OrderResponseDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repository.GetAllAsync();
            return _mapper.Map<List<OrderResponseDto>>(orders);
        }
    }
}