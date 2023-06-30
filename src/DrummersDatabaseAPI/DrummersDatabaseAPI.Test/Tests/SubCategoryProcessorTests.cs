using AutoMapper;
using DrummersDatabaseAPI.Data.Entities;
using DrummersDatabaseAPI.Data.Repositories;
using DrummersDatabaseAPI.Dtos;
using DrummersDatabaseAPI.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace DrummersDatabaseAPI.Test;

[TestClass]
public class SubCategoryProcessorTests
{
    private SubCategoryProcessor _testSubCategoryProcessor;
    private readonly Mock<IDrummersDatabaseRepository> _repo = new Mock<IDrummersDatabaseRepository>();
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<SubCategoryProcessor>> _logger = new Mock<ILogger<SubCategoryProcessor>>();
    private readonly int _categoryId = 100;
    private readonly int _subCategoryId = 200;

    public SubCategoryProcessorTests()
    {
        _mapper = SetUpAutoMapper.SetUp();
        _testSubCategoryProcessor = new SubCategoryProcessor(_repo.Object, _mapper, _logger.Object);
    }

    [TestMethod]
    [TestCategory($"{nameof(SubCategoryProcessor)} Tests")]
    [Description("Ensure processor returns a collection of SubCategoryDtos")]
    public async Task GetSubCategoriesAsyncReturnsCategoryDtos()
    {
        var subCategories = new List<SubCategory>
        {
            new SubCategory()
            {
                Id = _subCategoryId,
                SubCategoryGuid = Guid.NewGuid(),
                Name = "Drums",
                CategoryId = _categoryId,
                CategoryGuid = Guid.NewGuid(),
                Created = DateTime.Now,
                Image = "x.jpg",
                Active = true
            },
            new SubCategory()
            {
                Id = _subCategoryId + 1,
                SubCategoryGuid = Guid.NewGuid(),
                Name = "Cymbals",
                CategoryId = _categoryId,
                CategoryGuid = Guid.NewGuid(),
                Created = DateTime.Now,
                Image = "y.jpg",
                Active = true
            }
        };

        _repo.Setup(x => x.GetSubCategoriesAsync(_categoryId, true)).ReturnsAsync(subCategories);

        var subCategoryDtos = await _testSubCategoryProcessor.GetSubCategoriesAsync(_categoryId, true);
        Assert.IsTrue(subCategoryDtos.Count() > 0, "GetSubCategories did not return any SubCategories.");
    }

    [TestMethod]
    [TestCategory($"{nameof(SubCategoryProcessor)} Tests")]
    [Description("Ensure processor returns a SubCategoryDto")]
    public async Task GetSubCategoryAsyncReturnsCategoryDto()
    {
        var subCategory = new SubCategory()
        {
            Id = _subCategoryId,
            SubCategoryGuid = Guid.NewGuid(),
            CategoryId = _categoryId,
            CategoryGuid = Guid.NewGuid(),
            Name = "Drums",
            Image = "img.jpg",
            Counter = 1,
            Created = DateTime.Now,
            Active = true,
        };

        _repo.Setup(x => x.GetSubCategoryAsync(_subCategoryId, true)).ReturnsAsync(subCategory);

        var subCategoryDto = await _testSubCategoryProcessor.GetSubCategoryAsync(_subCategoryId, true) ?? new SubCategoryDto();
        Assert.AreEqual(_subCategoryId, subCategoryDto.Id, "GetSubCategory did not return a SubCategory.");
    }

    [TestMethod]
    [TestCategory($"{nameof(SubCategoryProcessor)} Tests")]
    [Description("Ensures creating a new sub category returns a SubCategoryDto")]
    public async Task CreateSubCategoryAsyncReturnsSubCategoryDto()
    {
        var categoryDto = new CategoryDto
        {
            Active = true,
            CategoryGuid = new Guid(),
            Id = _categoryId,
            Image = "new.jpg",
            Name = "New SubCategory"
        };

        var subCategoryCreateDto = new SubCategoryCreateDto()
        {
            SubCategoryGuid = new Guid(),
            CategoryId = categoryDto.Id,
            CategoryGuid = new Guid(),
            Name = "New SubCategory",
            Image = "new.jpg",
            Active = true
        };

        var input = new SubCategory
        {
            SubCategoryGuid = new Guid(),
            CategoryId = categoryDto.Id,
            CategoryGuid = categoryDto.CategoryGuid,
            Active = subCategoryCreateDto.Active,
            Image = subCategoryCreateDto.Image,
            Name = subCategoryCreateDto.Name,
        };

        _repo.Setup(x => x.CreateSubCategoryAsync(input));
        _repo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        var results = await _testSubCategoryProcessor.CreateSubCategoryAsync(subCategoryCreateDto, categoryDto);

        Assert.IsInstanceOfType(results, typeof(SubCategoryDto), "'results' was not a SubCategoryDto.");
    }

    [TestMethod]
    [TestCategory($"{nameof(SubCategoryProcessor)} Tests")]
    [Description("Ensures updating an existing sub category returns a SubCategoryDto")]
    public async Task UpdateSubCategoryAsyncReturnsSubCategoryDto()
    {
        var input = new SubCategoryUpdateDto()
        {
            CategoryId = _categoryId,
            CategoryGuid = new Guid(),
            Name = "Updated Sub Category",
            Image = "updated.jpg",
            Active = true
        };

        var subCategory = new SubCategory()
        {
            Id = _subCategoryId,
            CategoryGuid = Guid.NewGuid(),
            Name = "Drums",
            Image = "img.jpg",
            Counter = 1,
            Created = DateTime.Now,
            Active = true,
            Entries = new List<Entry>()
        };
        _repo.Setup(x => x.GetSubCategoryAsync(_subCategoryId, false)).ReturnsAsync(subCategory);

        _repo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        var results = await _testSubCategoryProcessor.UpdateSubCategoryAsync(_subCategoryId, input);

        Assert.IsInstanceOfType(results, typeof(SubCategoryDto), "'results' was not a SubCategoryDto.");
    }

    [TestMethod]
    [TestCategory($"{nameof(SubCategoryProcessor)} Tests")]
    [Description("Ensures that patching a sub category returns a successful response")]
    public async Task PatchSubCategoryAsyncReturnsReturnsSuccessfulResponse()
    {
        var input = new SubCategoryUpdateDto()
        {
            Name = "Updated SubCategory",
            Image = "updated.jpg",
            CategoryId = _categoryId,
            CategoryGuid = new Guid(),
            Active = true
        };

        var subCategory = new SubCategory()
        {
            Id = _subCategoryId,
            CategoryGuid = Guid.NewGuid(),
            Name = "Drums",
            Image = "img.jpg",
            Counter = 1,
            Created = DateTime.Now,
            Active = true,
            Entries = new List<Entry>()
        };
        _repo.Setup(x => x.GetSubCategoryAsync(_subCategoryId, false)).ReturnsAsync(subCategory);
        _repo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        bool success = await _testSubCategoryProcessor.PatchSubCategoryAsync(input, _subCategoryId);

        Assert.IsTrue(success, "patch test was not successful");
    }
}