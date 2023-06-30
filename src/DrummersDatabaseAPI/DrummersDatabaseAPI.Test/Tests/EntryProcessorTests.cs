using AutoMapper;
using DrummersDatabaseAPI.Data.Entities;
using DrummersDatabaseAPI.Data.Repositories;
using DrummersDatabaseAPI.Dtos;
using DrummersDatabaseAPI.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace DrummersDatabaseAPI.Test;

[TestClass]
public class EntryProcessorTests
{
    private EntryProcessor _testEntryProcessor;
    private readonly Mock<IDrummersDatabaseRepository> _repo = new Mock<IDrummersDatabaseRepository>();
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<EntryProcessor>> _logger = new Mock<ILogger<EntryProcessor>>();
    private readonly int _categoryId = 100;
    private readonly int _subCategoryId = 200;
    private readonly int _entryId = 300;

    public EntryProcessorTests()
    {
        _mapper = SetUpAutoMapper.SetUp();
        _testEntryProcessor = new EntryProcessor(_repo.Object, _mapper, _logger.Object);
    }

    [TestMethod]
    [TestCategory($"{nameof(EntryProcessor)} Tests")]
    [Description("Ensure processor returns a collection of EntryDtos")]
    public async Task GetEntriesAsyncReturnsEntryDtos()
    {
        var entries = new List<Entry>
        {
            new Entry()
            {
                Id = _entryId,
                EntryGuid = Guid.NewGuid(),
                Name = "Pearl Drums",
                SubCategoryId = _subCategoryId,
                SubCategoryGuid = Guid.NewGuid(),
                Created = DateTime.Now,
                Url = string.Empty,
                Active = true,
                Description = string.Empty
            },
            new Entry()
            {
                Id = _entryId + 1,
                EntryGuid = Guid.NewGuid(),
                Name = "Ludwig Drums",
                SubCategoryId = _subCategoryId,
                SubCategoryGuid = Guid.NewGuid(),
                Created = DateTime.Now,
                Url = string.Empty,
                Active = true,
                Description = string.Empty
            }
        };

        _repo.Setup(x => x.GetEntriesAsync(_subCategoryId, null, null, 1, 100, true)).ReturnsAsync(entries);

        var entryDtos = await _testEntryProcessor.GetEntriesAsync(_subCategoryId, null, null, 1, 100, true);
        Assert.IsTrue(entryDtos.Count() > 0, "GetEntries did not return any EntryDtos.");
    }

    [TestMethod]
    [TestCategory($"{nameof(EntryProcessor)} Tests")]
    [Description("Ensure processor returns a SubCategoryDto")]
    public async Task GetEntryAsyncReturnsEntryDto()
    {
        var entry = new Entry()
        {
            Id = _entryId,
            EntryGuid = Guid.NewGuid(),
            Name = "Pearl Drums",
            SubCategoryId = _subCategoryId,
            SubCategoryGuid = Guid.NewGuid(),
            Created = DateTime.Now,
            Url = string.Empty,
            Active = true,
            Description = string.Empty
        };

        _repo.Setup(x => x.GetEntryAsync(_entryId)).ReturnsAsync(entry);

        var entryDto = await _testEntryProcessor.GetEntryAsync(_entryId) ?? new EntryDto();
        Assert.AreEqual(_entryId, entryDto.Id, "GetEntry did not return a EntryDto.");
    }

    [TestMethod]
    [TestCategory($"{nameof(EntryProcessor)} Tests")]
    [Description("Ensures creating a new entry returns a EntryDto")]
    public async Task CreateEntryAsyncReturnsEntryDto()
    {
        var subCategoryDto = new SubCategoryDto
        {
            Active = true,
            CategoryId = _subCategoryId,
            SubCategoryGuid = new Guid(),
            Id = _categoryId,
            Image = "x.jpg",
            Name = "Drums"
        };

        var newEntryDto = new EntryCreateDto()
        {
            SubCategoryGuid = subCategoryDto.SubCategoryGuid,
            SubCategoryId = subCategoryDto.Id,
            EntryGuid = new Guid(),
            Name = "New Entry",
            Description = string.Empty,
            Image = "new.jpg",
            Active = true
        };

        var input = new Entry
        {
            SubCategoryGuid = subCategoryDto.SubCategoryGuid,
            SubCategoryId = subCategoryDto.Id,
            EntryGuid = new Guid(),
            Active = true,
            Name = "Pearl Drums",
            Description = string.Empty,
            Url = string.Empty
        };

        _repo.Setup(x => x.CreateEntryAsync(input));
        _repo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        var results = await _testEntryProcessor.CreateEntryAsync(newEntryDto, subCategoryDto);

        Assert.IsInstanceOfType(results, typeof(EntryDto), "'results' was not a EntryDto.");
    }

    [TestMethod]
    [TestCategory($"{nameof(EntryProcessor)} Tests")]
    [Description("Ensures updating an existing entry returns a EntryDto")]
    public async Task UpdateEntryAsyncReturnsEntryDto()
    {
        var input = new EntryUpdateDto()
        {
            SubCategoryId = _subCategoryId,
            SubCategoryGuid = new Guid(),
            Name = "Updated Entry",
            Image = "updated.jpg",
            Active = true
        };

        var entry = new Entry()
        {
            Id = _entryId,
            EntryGuid = Guid.NewGuid(),
            Name = "Pearl Drums",
            Url = string.Empty,
            Description = string.Empty,
            Counter = 1,
            Created = DateTime.Now,
            Active = true
        };
        _repo.Setup(x => x.GetEntryAsync(_entryId)).ReturnsAsync(entry);

        _repo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        var results = await _testEntryProcessor.UpdateEntryAsync(_entryId, input);

        Assert.IsInstanceOfType(results, typeof(EntryDto), "'results' was not a EntryDto.");
    }

    [TestMethod]
    [TestCategory($"{nameof(EntryProcessor)} Tests")]
    [Description("Ensures that patching a entry returns a successful response")]
    public async Task PatchEntryAsyncReturnsReturnsSuccessfulResponse()
    {
        var input = new EntryUpdateDto()
        {
            SubCategoryId = _subCategoryId,
            SubCategoryGuid = new Guid(),
            Name = "Updated Entry",
            Image = "updated.jpg",
            Active = true
        };

        var entry = new Entry()
        {
            Id = _entryId,
            SubCategoryGuid = Guid.NewGuid(),
            Name = "Pearl Drums",
            Description = string.Empty,
            Url = string.Empty,
            Counter = 1,
            Created = DateTime.Now,
            Active = true,
        };
        _repo.Setup(x => x.GetEntryAsync(_entryId)).ReturnsAsync(entry);
        _repo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        bool success = await _testEntryProcessor.PatchEntryAsync(_entryId, input);

        Assert.IsTrue(success, "patch test was not successful");
    }
}