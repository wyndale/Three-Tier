using AutoMapper;
using server.DTOs;
using server.Models;

namespace server.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductsDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ReverseMap()
            .ForMember(dest => dest.Category, opt => opt.Ignore()); // Avoid circular reference

        CreateMap<Product, UpdateProductDTO>().ReverseMap();

        CreateMap<Category, CategoryDTO>().ReverseMap();
    }
}
