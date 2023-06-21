using AutoMapper;
using DrummersDatabaseAPI.Data.Entities;
using DrummersDatabaseAPI.Dtos;

namespace DrummersDatabaseAPI.Web.AutoMapperProfiles
{
    public class EntryProfile : Profile
    {
        public EntryProfile()
        {
            // source, destination
            CreateMap<Entry?, EntryDto>();
            CreateMap<Entry, EntryUpdateDto>();
            CreateMap<EntryCreateDto?, Entry>();
            CreateMap<EntryUpdateDto?, Entry>();
            CreateMap<EntryUpdateDto?, EntryDto>();
            CreateMap<EntryDto?, EntryUpdateDto>();
        }
    }
}
