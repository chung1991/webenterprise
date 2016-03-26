USE [CMRDB]
GO

ALTER TABLE [dbo].[Comment] DROP CONSTRAINT [FK__Comment__monitor__2B3F6F97]
GO

ALTER TABLE [dbo].[Comment] DROP CONSTRAINT [FK__Comment__account__2A4B4B5E]
GO

/****** Object:  Table [dbo].[Comment]    Script Date: 3/23/2016 7:07:33 AM ******/
DROP TABLE [dbo].[Comment]
GO

/****** Object:  Table [dbo].[Comment]    Script Date: 3/23/2016 7:07:33 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Comment](
	[commentId] [int] IDENTITY(1,1) NOT NULL,
	[content] [varchar](1000) NULL,
	[accountId] [int] NULL,
	[monitoringReportId] [int] NULL,
	[time] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[commentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Comment]  WITH CHECK ADD FOREIGN KEY([accountId])
REFERENCES [dbo].[Account] ([accountId])
GO

ALTER TABLE [dbo].[Comment]  WITH CHECK ADD FOREIGN KEY([monitoringReportId])
REFERENCES [dbo].[CourseMonitoringReport] ([CourseMonitoringReportId])
GO

