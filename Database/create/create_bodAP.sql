USE [LearDataAll]
GO

/****** Object:  Table [dbo].[BodAP]    Script Date: 01.08.2021 11:43:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BodAP](
	[BodAPID] [int] IDENTITY(1,1) NOT NULL,
	[AkcniPlanID] [int] NOT NULL,
	[AudityOstatni] [bit] NOT NULL,
	[CisloBoduAP] [int] NOT NULL,
	[DatumZalozeni] [date] NOT NULL,
	[OdkazNaNormu] [nvarchar](20) NULL,
	[HodnoceniNeshody] [nvarchar](20) NULL,
	[PopisProblemu] [nvarchar](1000) NOT NULL,
	[SkutecnaPricina] [nvarchar](1000) NOT NULL,
	[Storno] [bit] NOT NULL,
 CONSTRAINT [PK_BodyAPAudityOstatni] PRIMARY KEY CLUSTERED 
(
	[BodAPID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BodAP] ADD  CONSTRAINT [DF_BodyAPAudityOstatni_audityOstatni]  DEFAULT ((0)) FOR [AudityOstatni]
GO

ALTER TABLE [dbo].[BodAP] ADD  CONSTRAINT [DF_BodyAPAudityOstatni_storno]  DEFAULT ((0)) FOR [Storno]
GO

