USE [LearDataAll]
GO

/****** Object:  Table [dbo].[Akce]    Script Date: 01.08.2021 11:40:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Akce](
	[AkceID] [int] IDENTITY(1,1) NOT NULL,
	[BodAPID] [int] NOT NULL,
	[NapravnaOpatreni] [nvarchar](1000) NOT NULL,
	[OdpovednaOsoba1] [int] NOT NULL,
	[OdpovednaOsoba2] [int] NULL,
	[KontrolaEfektivnosti] [date] NULL,
	[OddeleniID] [int] NULL,
	[Priloha] [nvarchar](max) NULL,
	[Typ] [nchar](2) NOT NULL,
	[Storno] [bit] NOT NULL,
 CONSTRAINT [PK_Akce] PRIMARY KEY CLUSTERED 
(
	[AkceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Akce] ADD  CONSTRAINT [DF_storno]  DEFAULT ((0)) FOR [Storno]
GO

