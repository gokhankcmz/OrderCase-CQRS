using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using Entities.ResponseModels;
using MediatR;
using Repository;

namespace CustomerService.Application.Queries
{
    public class GetAllCustomersQuery : IRequest<List<CustomerCollectionDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
    
    public class GetAllCustomersQueryHandler :  IRequestHandler<GetAllCustomersQuery, List<CustomerCollectionDto>>
    {
        private readonly IRepository<Customer>  _repository;
        private readonly IMapper _mapper;

        public GetAllCustomersQueryHandler(IRepository<Customer>  repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CustomerCollectionDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _repository.GetAllAsync();
            return _mapper.Map<List<CustomerCollectionDto>>(customers);
        }
    }
}