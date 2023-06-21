USE [DrummersDatabase]
GO

/****** Object:  Table [dbo].[SubCategories]    Script Date: 6/21/2023 1:06:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SubCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubCategoryGuid] [uniqueidentifier] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[CategoryGuid] [uniqueidentifier] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Image] [varchar](100) NOT NULL,
	[Counter] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_SubCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SubCategories] ADD  CONSTRAINT [DF_SubCategories_SubCategoryGuid]  DEFAULT (newid()) FOR [SubCategoryGuid]
GO

ALTER TABLE [dbo].[SubCategories] ADD  CONSTRAINT [DF_SubCategories_Counter]  DEFAULT ((0)) FOR [Counter]
GO

ALTER TABLE [dbo].[SubCategories] ADD  CONSTRAINT [DF_SubCategories_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[SubCategories] ADD  CONSTRAINT [DF_SubCategories_Created]  DEFAULT (getdate()) FOR [Created]
GO


