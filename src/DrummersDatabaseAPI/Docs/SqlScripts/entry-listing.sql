SELECT [C].Id AS CatId, [C].[Name], [SC].[Id] AS SbCatId, [SC].[Name], [E].[Name] , [E].[Url], [E].[Active]
FROM [dbo].[Entries] AS E
	INNER JOIN [SubCategories] AS SC on [E].SubCategoryId = [SC].[Id]
	INNER JOIN [Categories] AS C on [SC].[CategoryId] = [C].[Id]
ORDER BY [C].[Name], [SC].[Name], [E].[Name]