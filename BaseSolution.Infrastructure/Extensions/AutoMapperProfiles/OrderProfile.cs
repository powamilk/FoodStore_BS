using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Order.OrderRequest;
using BaseSolution.Application.DataTransferObjects.Order;
using BaseSolution.Application.DataTransferObjects.OrderItem.OrderItemRequest;
using BaseSolution.Application.DataTransferObjects.OrderItem;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Extensions.AutoMapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount)) 
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems)); 

            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore()); 

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name)) 
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price)); 

            CreateMap<OrderItemDto, OrderItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));

            CreateMap<CreateOrderRequest, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items)); 

            CreateMap<OrderItemCreateRequest, OrderItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));

            CreateMap<UpdateOrderRequest, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items)); 

            CreateMap<OrderItemUpdateRequest, OrderItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));
        }
    }
}
