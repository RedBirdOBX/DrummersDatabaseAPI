USE [DrummersDatabase]
GO

/****** Object:  Table [dbo].[Entries]    Script Date: 6/21/2023 1:06:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Entries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EntryGuid] [uniqueidentifier] NOT NULL,
	[SubCategoryId] [int] NOT NULL,
	[SubCategoryGuid] [uniqueidentifier] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[Url] [varchar](100) NULL,
	[Image] [varchar](100) NULL,
	[Counter] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_Entries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Entries] ADD  CONSTRAINT [DF_Entries_EntryGuid]  DEFAULT (newid()) FOR [EntryGuid]
GO

ALTER TABLE [dbo].[Entries] ADD  CONSTRAINT [DF_Entries_Counter]  DEFAULT ((0)) FOR [Counter]
GO

ALTER TABLE [dbo].[Entries] ADD  CONSTRAINT [DF_Entries_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[Entries] ADD  CONSTRAINT [DF_Entries_Created]  DEFAULT (getdate()) FOR [Created]
GO


