using AutoMapper;
using DrummersDatabaseAPI.Data.Entities;
using DrummersDatabaseAPI.Data.Repositories;
using DrummersDatabaseAPI.Dtos;
using DrummersDatabaseAPI.Service;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace DrummersDatabaseAPI.Test;

// https://www.youtube.com/watch?v=DwbYxP-etMY
// https://www.youtube.com/watch?v=9ZvDBSQa_so  9:30


[TestClass]
public class CategoryProcessorTests
{
    private CategoryProcessor _testProcessor;
    private readonly Mock<IDrummersDatabaseRepository> _repo = new Mock<IDrummersDatabaseRepository>();
    private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
    private readonly Mock<ILogger<CategoryProcessor>> _logger = new Mock<ILogger<CategoryProcessor>>();

    public CategoryProcessorTests()
    {
        _testProcessor = new CategoryProcessor(_repo.Object, _mapper.Object, _logger.Object);
    }

    [TestMethod]
    public async Task GetCategoryAsync()
    {
        // arrange
        int categoryId = 2;
        var category = new Category()
        {
            Id = categoryId,
            CategoryGuid = Guid.NewGuid(),
            Name = "Manufacturers",
            Image = "img.jpg",
            Counter = 1,
            Created = DateTime.Now,
            Active = true,
            SubCategories = new List<SubCategory>()
        };

        // setting up the method of the repo we want to mock.
        // Mock the REPO method - prevents making a true db call.
        // Instructs the moq to return what we just created
        _repo.Setup(x => x.GetCategoryAsync(categoryId, true)).ReturnsAsync(category);

        // https://www.youtube.com/watch?v=9ZvDBSQa_so - stuck at 12:00

        // act
        var categoryDto = await _testProcessor.GetCategoryAsync(categoryId, true);

        // assert
        Assert.AreEqual(categoryId, categoryDto.Id);
    }
}