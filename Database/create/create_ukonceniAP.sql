USE [LearDataAll]
GO

/****** Object:  Table [dbo].[UkonceniAP]    Script Date: 01.08.2021 11:44:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UkonceniAP](
	[UkonceniAPID] [int] IDENTITY(1,1) NOT NULL,
	[AkcniPlanID] [int] NOT NULL,
	[DatumUkonceni] [date] NOT NULL,
	[Poznamka] [nvarchar](250) NULL,
 CONSTRAINT [PK_DatumUkonceni] PRIMARY KEY CLUSTERED 
(
	[UkonceniAPID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
