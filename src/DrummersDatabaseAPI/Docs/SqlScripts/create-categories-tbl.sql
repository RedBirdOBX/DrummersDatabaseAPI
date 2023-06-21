USE [DrummersDatabase]
GO

/****** Object:  Table [dbo].[Categories]    Script Date: 6/21/2023 1:05:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryGuid] [uniqueidentifier] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Image] [varchar](100) NULL,
	[Counter] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF_Categories_CategoryGuid]  DEFAULT (newid()) FOR [CategoryGuid]
GO

ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF_Categories_Counter]  DEFAULT ((0)) FOR [Counter]
GO

ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF_Categories_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF_Categories_Created]  DEFAULT (getdate()) FOR [Created]
GO


