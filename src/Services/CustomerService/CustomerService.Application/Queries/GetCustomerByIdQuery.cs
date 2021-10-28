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

namespace CustomerService.Application.Queries
{
    public class GetCustomerByIdQuery : IRequest<CustomerResponseDto>
    {
        public Guid Id { get; set; }
    }
    
    public class GetCustomerByIdQueryHandler :  IRequestHandler<GetCustomerByIdQuery, CustomerResponseDto>
    {
        private readonly IRepository<Customer>  _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public GetCustomerByIdQueryHandler(IRepository<Customer>  repository, IMapper mapper, IDistributedCache cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<CustomerResponseDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _cache.GetRecordAsync<Customer>($"customer:{request.Id}");
            if (customer == null)
            {
                customer = await _repository.GetByIdAsync(request.Id);
                if (customer == null)
                {
                    throw new NotFound(nameof(Customer), request.Id.ToString());
                }
                await _cache.SetRecordAsync($"customer:{customer.Id}", customer);
            }

            return _mapper.Map<CustomerResponseDto>(customer);
        }
    }
}