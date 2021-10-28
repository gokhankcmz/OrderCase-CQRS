using AutoMapper;
using Entities.Models;
using Entities.RequestModels;
using Entities.ResponseModels;
using OrderService.Application.Commands;

namespace OrderService.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdateOrderDto, Order>();
            CreateMap<CreateOrderCommand, Order>();
            CreateMap<UpdateOrderCommand, Order>();
            CreateMap<CreateOrderDto, Order>();
            CreateMap<CreateOrderDto, CreateOrderCommand>();
            CreateMap<UpdateOrderDto, UpdateOrderCommand>();
            CreateMap<Order, OrderResponseDto>();
        }
    }
}