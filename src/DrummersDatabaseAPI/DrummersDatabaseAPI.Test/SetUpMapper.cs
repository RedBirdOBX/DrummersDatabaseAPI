using AutoMapper;
using DrummersDatabaseAPI.Web.AutoMapperProfiles;

namespace DrummersDatabaseAPI.Test
{
    public static class SetUpAutoMapper
    {
        public static IMapper SetUp()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new CategoryProfile());
                opts.AddProfile(new SubCategoryProfile());
                opts.AddProfile(new EntryProfile());
            });

            return config.CreateMapper();
        }
    }
}
