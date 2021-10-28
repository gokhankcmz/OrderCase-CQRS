using System;
using System.Threading;
using System.Threading.Tasks;
using CommonLib.Caching;
using CommonLib.Models.ErrorModels;
using Entities.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Repository;

namespace CustomerService.Application.Queries
{
    public class GetCustomerAddressesQuery : IRequest<Address>
    {
        public Guid Id { get; set; }
    }
    
    public class GetCustomerAddressesQueryHandler :  IRequestHandler<GetCustomerAddressesQuery, Address>
    {
        private readonly IRepository<Customer>  _repository;
        private readonly IDistributedCache _cache;

        public GetCustomerAddressesQueryHandler(IRepository<Customer>  repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<Address> Handle(GetCustomerAddressesQuery request, CancellationToken cancellationToken)
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
            return customer.Address;
        }
    }
}