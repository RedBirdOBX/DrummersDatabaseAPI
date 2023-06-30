using AutoMapper;
using DrummersDatabaseAPI.Data.Entities;
using DrummersDatabaseAPI.Data.Repositories;
using DrummersDatabaseAPI.Dtos;
using DrummersDatabaseAPI.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace DrummersDatabaseAPI.Test;


[TestClass]
public class CategoryProcessorTests
{
    private CategoryProcessor _testCategoryProcessor;
    private readonly Mock<IDrummersDatabaseRepository> _repo = new Mock<IDrummersDatabaseRepository>();
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<CategoryProcessor>> _logger = new Mock<ILogger<CategoryProcessor>>();

    public CategoryProcessorTests()
    {
        _mapper = SetUpAutoMapper.SetUp();
        _testCategoryProcessor = new CategoryProcessor(_repo.Object, _mapper, _logger.Object);
    }

    [TestMethod]
    [TestCategory($"{nameof(CategoryProcessor)} Tests")]
    [Description("Ensure processor returns a collection of CategoryDtos")]
    public async Task GetCategoriesAsyncReturnsCategoryDtos()
    {
        // build moq'd response
        var categories = new List<Category>
        {
            new Category()
            {
                Id = 100,
                Name = "Manufacturers",
                CategoryGuid = Guid.NewGuid(),
                Created = DateTime.Now,
                Image = "x.jpg",
                Active = true
            },
            new Category()
            {
                Id = 101,
                Name = "Drum Shops",
                CategoryGuid = Guid.NewGuid(),
                Created = DateTime.Now,
                Image = "y.jpg",
                Active = true
            }
        };

        // setting up the method of the repo we want to mock.
        // Mock the REPO method - prevents making a true db call.
        // Instructs the moq to return what we just created
        _repo.Setup(x => x.GetCategoriesAsync(true)).ReturnsAsync(categories);

        var categoryDtos = await _testCategoryProcessor.GetCategoriesAsync(true);
        Assert.IsTrue(categoryDtos.Count() > 0, "GetCategories did not return any Categories.");
    }

    [TestMethod]
    [TestCategory($"{nameof(CategoryProcessor)} Tests")]
    [Description("Ensure processor returns a CategoryDto")]
    public async Task GetCategoryAsyncReturnsCategoryDto()
    {
        // build moq'd response
        int categoryId = 1;
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

        var categoryDto = await _testCategoryProcessor.GetCategoryAsync(categoryId, true) ?? new CategoryDto();
        Assert.AreEqual(categoryId, categoryDto.Id, "GetCategory did not return a Category.");
    }

    [TestMethod]
    [TestCategory($"{nameof(CategoryProcessor)} Tests")]
    [Description("Ensures creating a new category returns a CategoryWithoutSubCategoriesDto")]
    public async Task CreateCategoryAsyncReturnsCategoryWithoutSubCategoriesDto()
    {
        // 1) CategoryCreateDto comes in from controller as 'input', goes into service.
        var newCategory = new CategoryCreateDto()
        {
            CategoryGuid = new Guid(),
            Name = "New Category",
            Image = "new.jpg",
            Active = true
        };

        // 2) service maps 'input' to a new entity type and calls repo to add to memory & and save
        Category input = new Category
        {
            Active = newCategory.Active,
            CategoryGuid = newCategory.CategoryGuid,
            Id = 0,
            Image = newCategory.Image,
            Name = newCategory.Name
        };

        // 3) repo is told to save and if save is successful, service maps `input` (entity) to CategoryWithoutSubCategoriesDto.
        //    this what repo does - we need to mock them.
        _repo.Setup(x => x.CreateCategoryAsync(input));
        _repo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        // 4) service returns CategoryWithoutSubCategoriesDto
        var results = await _testCategoryProcessor.CreateCategoryAsync(newCategory);

        Assert.IsInstanceOfType(results, typeof(CategoryWithoutSubCategoriesDto), "'results' was not a CategoryWithoutSubCategoriesDto.");
    }

    [TestMethod]
    [TestCategory($"{nameof(CategoryProcessor)} Tests")]
    [Description("Ensures updating an existing category returns a CategoryWithoutSubCategoriesDto")]
    public async Task UpdateCategoryAsyncReturnsCategoryWithoutSubCategoriesDto()
    {
        // 1) CategoryUpdateDto comes in from controller as 'input' along with categoryId and goes into service.
        var input = new CategoryUpdateDto()
        {
            Name = "Updated Category",
            Image = "updated.jpg",
            Active = true
        };

        // 2) service fetches entity from repo, maps to dto and saves. Need to mock the repo methods.
        int updatedCategoryId = 1;
        var category = new Category()
        {
            Id = updatedCategoryId,
            CategoryGuid = Guid.NewGuid(),
            Name = "Manufacturers",
            Image = "img.jpg",
            Counter = 1,
            Created = DateTime.Now,
            Active = true,
            SubCategories = new List<SubCategory>()
        };
        _repo.Setup(x => x.GetCategoryAsync(updatedCategoryId, false)).ReturnsAsync(category);

        // service all calls SaveChanges of repo. Also need to mock.
        _repo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        // call service with moq'd repo methods
        var results = await _testCategoryProcessor.UpdateCategoryAsync(updatedCategoryId, input);

        Assert.IsInstanceOfType(results, typeof(CategoryWithoutSubCategoriesDto), "'results' was not a CategoryWithoutSubCategoriesDto.");
    }

    [TestMethod]
    [TestCategory($"{nameof(CategoryProcessor)} Tests")]
    [Description("Ensures that patching a category returns a successful response")]
    public async Task PatchCategoryAsyncReturnsReturnsSuccessfulResponse()
    {
        // 1) CategoryUpdateDto comes in from controller as 'input' along with categoryId and goes into service.
        var input = new CategoryUpdateDto()
        {
            Name = "Updated Category",
            Image = "updated.jpg",
            Active = true
        };

        // 2) service fetches entity from repo, maps to dto and saves. Need to mock the repo methods.
        int patchedCategoryId = 1;
        var category = new Category()
        {
            Id = patchedCategoryId,
            CategoryGuid = Guid.NewGuid(),
            Name = "Manufacturers",
            Image = "img.jpg",
            Counter = 1,
            Created = DateTime.Now,
            Active = true,
            SubCategories = new List<SubCategory>()
        };
        _repo.Setup(x => x.GetCategoryAsync(patchedCategoryId, false)).ReturnsAsync(category);

        // service all calls SaveChanges of repo. Also need to mock.
        _repo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        // call service with moq'd repo methods
        bool success = await _testCategoryProcessor.PatchCategoryAsync(patchedCategoryId, input);

        Assert.IsTrue(success, "patch test was not successful");
    }
}