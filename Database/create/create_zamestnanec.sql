USE [LearDataAll]
GO

/****** Object:  Table [dbo].[Zamestnanec]    Script Date: 01.08.2021 11:45:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Zamestnanec](
	[ZamestnanecID] [int] IDENTITY(1,1) NOT NULL,
	[Jmeno] [nvarchar](30) NOT NULL,
	[Prijmeni] [nvarchar](30) NOT NULL,
	[PersonalniCislo] [nvarchar](30) NOT NULL,
	[OddeleniID] [int] NOT NULL,
	[Email] [nvarchar](100) NULL,
	[JeZamestnanec] [bit] NOT NULL,
	[Storno] [bit] NOT NULL,
 CONSTRAINT [PK_Zamestnanci] PRIMARY KEY CLUSTERED 
(
	[ZamestnanecID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Zamestnanec] ADD  CONSTRAINT [DF_Zamestnanci_jeZamestnanec]  DEFAULT ((0)) FOR [JeZamestnanec]
GO

ALTER TABLE [dbo].[Zamestnanec] ADD  CONSTRAINT [DF_Zamestnanci_storno]  DEFAULT ((0)) FOR [Storno]
GO

