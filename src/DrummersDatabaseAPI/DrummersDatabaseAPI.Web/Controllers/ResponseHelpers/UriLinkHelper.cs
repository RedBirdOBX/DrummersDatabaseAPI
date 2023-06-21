using DrummersDatabaseAPI.Dtos;
using DrummersDatabaseAPI.Web.Controllers.RequestHelpers;

namespace DrummersDatabaseAPI.Web.Controllers.ResponseHelpers
{
    #pragma warning disable CS1591

    public static class UriLinkHelper
    {
        public static CategoryDto CreateLinksForCategory(HttpRequest request, CategoryDto category)
        {
            string protocol = (request.IsHttps) ? "https" : "http";
            try
            {
                category.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories/{category.Id}", "self", "GET"));
                category.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories", "category-collection", "GET"));

                // you may or may not want to expose this.
                // you could also wrap this in some custom logic...perhaps only show if authenticated...or policy is included in token
                // link to create
                category.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories", "category-create", "POST"));
                category.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories/{category.Id}", "category-put", "PUT"));
                category.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories/{category.Id}", "category-patch", "PATCH"));
            }
            catch
            {
                throw;
            }
            return category;
        }

        public static LinkDto CreateLinkForCategoryWithinCollection(HttpRequest request, CategoryDto category)
        {
            string protocol = (request.IsHttps) ? "https" : "http";
            try
            {
                LinkDto link = new LinkDto($"{protocol}://{request.Host}/api/categories/{category.Id}", "category", "GET");
                return link;
            }
            catch
            {
                throw;
            }
        }

        public static SubCategoryDto CreateLinksForSubCategory(HttpRequest request, SubCategoryDto subCategory)
        {
            string protocol = (request.IsHttps) ? "https" : "http";
            try
            {
                subCategory.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories/{subCategory.CategoryId}/subcategories/{subCategory.Id}", "self", "GET"));
                subCategory.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories/{subCategory.CategoryId}/subcategories", "subcategory-collection", "GET"));

                // you may or may not want to expose this.
                // you could also wrap this in some custom logic...perhaps only show if authenticated...or policy is included in token
                // link to create
                subCategory.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories/{subCategory.CategoryId}/subcategories", "subcategory-create", "POST"));
                subCategory.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories/{subCategory.CategoryId}/subcategories/{subCategory.CategoryId}", "subcategory-put", "PUT"));
                subCategory.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories/{subCategory.CategoryId}/subcategories/{subCategory.CategoryId}", "subcategory-patch", "PATCH"));
            }
            catch
            {
                throw;
            }
            return subCategory;
        }

        public static LinkDto CreateLinkForSubCategoryWithinCollection(HttpRequest request, SubCategoryDto subcategory)
        {
            string protocol = (request.IsHttps) ? "https" : "http";
            try
            {
                LinkDto link = new LinkDto($"{protocol}://{request.Host}/api/categories/{subcategory.CategoryId}/subcategories/{subcategory.Id}", "subcategory", "GET");
                return link;
            }
            catch
            {
                throw;
            }
        }

        public static EntryDto CreateLinksForEntry(HttpRequest request, EntryDto entry, int categoryId)
        {
            string protocol = (request.IsHttps) ? "https" : "http";
            try
            {
                entry.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories/{categoryId}/subcategories/{entry.SubCategoryId}/entries/{entry.Id}", "self", "GET"));
                entry.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories/{categoryId}/subcategories/{entry.SubCategoryId}/entries?pageNumber={DefaultRequestParameters.DefaultPageNumber}&pageSize={DefaultRequestParameters.DefaultPageSize}", "entry-collection", "GET"));

                // you may or may not want to expose this.
                // you could also wrap this in some custom logic...perhaps only show if authenticated...or policy is included in token
                // link to create
                entry.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories/{categoryId}/subcategories/{entry.SubCategoryId}/entries", "entry-create", "POST"));
                entry.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories/{categoryId}/subcategories/{entry.SubCategoryId}/entries/{entry.Id}", "entry-put", "PUT"));
                entry.Links.Add(new LinkDto($"{protocol}://{request.Host}/api/categories/{categoryId}/subcategories/{entry.SubCategoryId}/entries/{entry.Id}", "entry-patch", "PATCH"));
            }
            catch
            {
                throw;
            }
            return entry;
        }

        public static LinkDto CreateLinkForEntriesWithinCollection(HttpRequest request, EntryDto entry, int categoryId)
        {
            string protocol = (request.IsHttps) ? "https" : "http";
            try
            {
                LinkDto link = new LinkDto($"{protocol}://{request.Host}/api/categories/{categoryId}/subcategories/{entry.SubCategoryId}/entries/{entry.Id}", "entry", "GET");
                return link;
            }
            catch
            {
                throw;
            }
        }
    }

    #pragma warning restore CS1591
}
