using DrummersDatabaseAPI.Dtos;

namespace DrummersDatabaseAPI.Data
{
    public class DrummersDatabaseInMemDataStore
    {
        public List<CategoryDto> Categories { get; set; }

        public List<SubCategoryDto> SubCategories { get; set; }

        public List<EntryDto> Entries { get; set; }

        public DrummersDatabaseInMemDataStore()
        {
            Categories = new List<CategoryDto>
            {
                new CategoryDto()
                {
                    Id = 100,
                    Name = "Manufacturers",
                    CategoryGuid = Guid.NewGuid(),
                    Created = DateTime.Now,
                    Image = "x.jpg",
                    Active = true
                },
                new CategoryDto()
                {
                    Id = 101,
                    Name = "Drum Shops",
                    CategoryGuid = Guid.NewGuid(),
                    Created = DateTime.Now,
                    Image = "y.jpg",
                    Active = true
                }
            };

            SubCategories = new List<SubCategoryDto>()
            {
                new SubCategoryDto()
                {
                    Id = 200,
                    Name = "Drums",
                    SubCategoryGuid = Guid.NewGuid(),
                    CategoryId = 100,
                    CategoryGuid = new Guid("8793552E-23FD-4DD1-AB5D-78C3862C5BA9"),
                    Created = DateTime.Now,
                    Image = "x.jpg",
                    Active = true
                },
                new SubCategoryDto()
                {
                    Id = 201,
                    Name = "Cymbals",
                    SubCategoryGuid = Guid.NewGuid(),
                    CategoryId = 100,
                    CategoryGuid = new Guid("8793552E-23FD-4DD1-AB5D-78C3862C5BA9"),
                    Created = DateTime.Now,
                    Image = "x.jpg",
                    Active = true
                },
            };

            Entries = new List<EntryDto>()
            {
                new EntryDto()
                {
                    Id = 300,
                    Active = true,
                    Created = DateTime.Now,
                    Description = "Pearl Drums.",
                    EntryGuid = Guid.NewGuid(),
                    Image = "x.jpg",
                    Name = "Pearl",
                    SubCategoryId = 200,
                    SubCategoryGuid = Guid.NewGuid()
                }
            };
        }
    }
}