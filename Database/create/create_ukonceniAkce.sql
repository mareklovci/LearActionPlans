USE [LearDataAll]
GO

/****** Object:  Table [dbo].[UkonceniAkce]    Script Date: 01.08.2021 11:44:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UkonceniAkce](
	[UkonceniAkceID] [int] IDENTITY(1,1) NOT NULL,
	[AkceID] [int] NOT NULL,
	[DatumUkonceni] [date] NOT NULL,
	[Poznamka] [nvarchar](250) NULL,
 CONSTRAINT [PK_UkonceniBoduAP] PRIMARY KEY CLUSTERED 
(
	[UkonceniAkceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

