using AutoMapper;
using DrummersDatabaseAPI.Data.Entities;
using DrummersDatabaseAPI.Dtos;

namespace DrummersDatabaseAPI.Web.AutoMapperProfiles
{
    public class SubCategoryProfile : Profile
    {
        public SubCategoryProfile()
        {
            // source, destination
            CreateMap<SubCategory, SubCategoryDto>();
            CreateMap<SubCategory, SubCategoryWithoutEntriesDto>();
            CreateMap<SubCategory, SubCategoryUpdateDto>();
            CreateMap<SubCategoryDto, SubCategoryUpdateDto>();
            CreateMap<SubCategoryCreateDto, SubCategory>();
            CreateMap<SubCategoryUpdateDto, SubCategory>();
            CreateMap<SubCategoryUpdateDto, SubCategory>();
        }
    }
}
