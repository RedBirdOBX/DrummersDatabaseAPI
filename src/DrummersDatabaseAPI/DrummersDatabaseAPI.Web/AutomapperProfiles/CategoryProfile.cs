using AutoMapper;
using DrummersDatabaseAPI.Data.Entities;
using DrummersDatabaseAPI.Dtos;

namespace DrummersDatabaseAPI.Web.AutoMapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            // source, destination
            CreateMap<Category, CategoryWithoutSubCategoriesDto>();
            CreateMap<Category?, CategoryDto>();
            CreateMap<Category?, CategoryUpdateDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<CategoryWithoutSubCategoriesDto, CategoryUpdateDto>();
        }
    }
}
