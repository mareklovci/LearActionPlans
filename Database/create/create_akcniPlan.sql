USE [LearDataAll]
GO

/****** Object:  Table [dbo].[AkcniPlan]    Script Date: 01.08.2021 11:42:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AkcniPlan](
	[AkcniPlanID] [int] IDENTITY(1,1) NOT NULL,
	[CisloAP] [int] NOT NULL,
	[Zadavatel1ID] [int] NOT NULL,
	[Zadavatel2ID] [int] NULL,
	[Tema] [nvarchar](100) NOT NULL,
	[ProjektID] [int] NULL,
	[DatumZalozeni] [date] NOT NULL,
	[ZakaznikID] [int] NOT NULL,
	[AudityOstatni] [bit] NOT NULL,
 CONSTRAINT [PK_AkcniPlany] PRIMARY KEY CLUSTERED 
(
	[AkcniPlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AkcniPlan] ADD  CONSTRAINT [DF_AkcniPlany_audityOstatni]  DEFAULT ((0)) FOR [AudityOstatni]
GO

