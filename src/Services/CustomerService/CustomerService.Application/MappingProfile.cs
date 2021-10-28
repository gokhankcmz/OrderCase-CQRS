using AutoMapper;
using CustomerService.Application.Commands;
using Entities.Models;
using Entities.RequestModels;
using Entities.ResponseModels;

namespace CustomerService.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdateCustomerDto, Customer>();
            CreateMap<CreateCustomerCommand, Customer>();
            CreateMap<UpdateCustomerCommand, Customer>();
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<CreateCustomerDto, CreateCustomerCommand>();
            CreateMap<UpdateCustomerDto, UpdateCustomerCommand>();
            CreateMap<Customer, CustomerResponseDto>();
            CreateMap<Customer, CustomerCollectionDto>();
            CreateMap<AuthDto, CreateTokenCommand>();
        }
    }
}