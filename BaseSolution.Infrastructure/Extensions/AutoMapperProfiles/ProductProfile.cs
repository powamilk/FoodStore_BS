using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Product.Request;
using BaseSolution.Application.DataTransferObjects.Product;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Extensions.AutoMapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductCreateRequest, Product>();
            CreateMap<ProductUpdateRequest, Product>();
        }
    }
}
