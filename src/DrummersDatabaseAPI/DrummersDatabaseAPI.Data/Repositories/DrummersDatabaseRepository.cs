using DrummersDatabaseAPI.Data.DbContexts;
using DrummersDatabaseAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace DrummersDatabaseAPI.Data.Repositories
{
    public class DrummersDatabaseRepository : IDrummersDatabaseRepository
    {
        private DrummersDatabaseDbContext _dbContext;

        public DrummersDatabaseRepository(DrummersDatabaseDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(DrummersDatabaseDbContext));
        }


        // categories
        public async Task<IEnumerable<Category>> GetCategoriesAsync(bool showAll)
        {
            var results = new List<Category>();
            if (showAll)
            {
                results = await _dbContext.Categories.OrderBy(c => c.Name).ToListAsync();
            }
            else
            {
                results = await _dbContext.Categories.Where(c => c.Active == true).OrderBy(c => c.Name).ToListAsync();
            }
            return results;
        }

        public async Task<Category?> GetCategoryAsync(int categoryId, bool includeSubCategories = true)
        {

            if (includeSubCategories)
            {
                var category = await _dbContext.Categories.Include(c => c.SubCategories).Where(c => c.Id == categoryId).FirstOrDefaultAsync();
                return category;
            }
            else
            {
                var category = await _dbContext.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();
                return category;
            }
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
        }

        public async Task<bool> DoesCategoryExistAsync(int categoryId)
        {
            return await _dbContext.Categories.AnyAsync(c => c.Id == categoryId);
        }


        // subcategories
        public async Task<IEnumerable<SubCategory>> GetSubCategoriesAsync(int categoryId, bool showAll)
        {
            var results = new List<SubCategory>();
            if (showAll)
            {
                results = await _dbContext.SubCategories.OrderBy(s => s.Name).Where(s => s.CategoryId == categoryId).ToListAsync();
            }
            else
            {
                results = await _dbContext.SubCategories.OrderBy(s => s.Name).Where(s => s.CategoryId == categoryId && s.Active == true).ToListAsync();
            }
            return results;
        }

        public async Task<SubCategory?> GetSubCategoryAsync(int subCategoryId, bool includeEntries)
        {
            if (includeEntries)
            {
                var subCategory = await _dbContext.SubCategories.Include(s => s.Entries).Where(s => s.Id == subCategoryId).FirstOrDefaultAsync();
                return subCategory;
            }
            else
            {
                var subCategory = await _dbContext.SubCategories.Where(s => s.Id == subCategoryId).FirstOrDefaultAsync();
                return subCategory;
            }
        }

        public async Task CreateSubCategoryAsync(SubCategory subCategory)
        {
            await _dbContext.SubCategories.AddAsync(subCategory);
        }

        public async Task<bool> DoesSubCategoryExistAsync(int subCategoryId)
        {
            return await _dbContext.SubCategories.AnyAsync(s => s.Id == subCategoryId);
        }

        // entries
        public async Task<IEnumerable<Entry>> GetEntriesAsync(int subCategoryId, string? filter, string? search, int pageNum, int pageSize, bool showAll)
        {
            var entries = _dbContext.Entries as IQueryable<Entry>;

            // building the query //
            if (!string.IsNullOrEmpty(filter))
            {
                entries = entries.Where(e => e.Name == filter.Trim());
            }

            if (!string.IsNullOrEmpty(search))
            {
                entries = entries.Where(e => e.SubCategoryId == subCategoryId
                                    && (e.Name.Contains(search.Trim()) || e.Description.Contains(search.Trim())));
            }

            if (!showAll)
            {
                entries = entries.Where(e => e.Active == true);
            }

            var results = await entries.Where(e => e.SubCategoryId == subCategoryId)
                                        .OrderBy(e => e.Name)
                                        .Skip(pageSize * (pageNum - 1))
                                        .Take(pageSize)
                                        .ToListAsync();

            return results;
        }

        public async Task<Entry?> GetEntryAsync(int entryId)
        {
            // how can we include the parent obj?
            //var entry = await _dbContext.Entries.Include(e => e.SubCategory).Where(e => e.Id == entryId).FirstOrDefaultAsync();
            var entry = await _dbContext.Entries.Where(e => e.Id == entryId).FirstOrDefaultAsync();
            return entry;
        }

        public async Task<int> GetCountOfEntriesAsync()
        {
            var count = await _dbContext.Entries.CountAsync();
            return count;
        }

        public async Task CreateEntryAsync(Entry entry)
        {
            await _dbContext.Entries.AddAsync(entry);
        }

        public void DeleteEntry(Entry entry)
        {
            _dbContext.Entries.Remove(entry);
        }

        public async Task<bool> DoesEntryExistAsync(int entryId)
        {
            return await _dbContext.Entries.AnyAsync(e => e.Id == entryId);
        }

        // global
        public async Task<bool> SaveChangesAsync()
        {
            // returns count of entities which have been changed.
            return await _dbContext.SaveChangesAsync() >= 0;
        }
    }
}
