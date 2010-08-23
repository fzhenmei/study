SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Creating [dbo].[Users]'
GO
CREATE TABLE [dbo].[Users]
(
[Pk] [int] NOT NULL,
[UserId] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Users_UserId] DEFAULT (''),
[Password] [nvarchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Users_Password] DEFAULT (''),
[UserName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Users_UserName] DEFAULT (''),
[UserCompany] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Users_UserCompany] DEFAULT (''),
[LastCustomer] [int] NOT NULL CONSTRAINT [DF_Users_LastCustomer] DEFAULT ((0)),
[LastProject] [int] NOT NULL CONSTRAINT [DF_Users_LastProject] DEFAULT ((0)),
[tversion] [timestamp] NOT NULL
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_tt_Users] on [dbo].[Users]'
GO
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [PK_tt_Users] PRIMARY KEY CLUSTERED ([Pk])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Entries]'
GO
CREATE TABLE [dbo].[Entries]
(
[Pk] [int] NOT NULL IDENTITY(1, 1),
[CustomerPk] [int] NOT NULL CONSTRAINT [DF_Entries_CustomerPk] DEFAULT ((0)),
[ProjectPk] [int] NOT NULL CONSTRAINT [DF_Entries_ProjectPk] DEFAULT ((0)),
[InvoicePk] [int] NOT NULL CONSTRAINT [DF_Entries_InvoicePk] DEFAULT ((0)),
[UserPk] [int] NOT NULL CONSTRAINT [DF_Entries_UserPk] DEFAULT ((0)),
[Title] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_tt_entries_Title] DEFAULT (''),
[Description] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_tt_entries_Description] DEFAULT (''),
[TimeIn] [datetime] NOT NULL CONSTRAINT [DF_tt_entries_TimeIn] DEFAULT (getdate()),
[TimeOut] [datetime] NOT NULL CONSTRAINT [DF_tt_entries_TimeOut] DEFAULT (getdate()),
[PunchedOut] [bit] NOT NULL CONSTRAINT [DF_tt_entries_PunchedOut] DEFAULT ((0)),
[Qty] [decimal] (18, 2) NOT NULL CONSTRAINT [DF_tt_entries_Qty] DEFAULT ((0)),
[Rate] [decimal] (18, 2) NOT NULL CONSTRAINT [DF_tt_entries_Rate] DEFAULT ((0.00)),
[TotalHours] [decimal] (18, 2) NOT NULL CONSTRAINT [DF_tt_Entries_TotalHours] DEFAULT ((0.00)),
[ItemTotal] [decimal] (18, 2) NOT NULL CONSTRAINT [DF_tt_entries_ItemTotal] DEFAULT ((0.00)),
[Taxable] [bit] NOT NULL CONSTRAINT [DF_tt_entries_Taxable] DEFAULT ((0)),
[Billed] [bit] NOT NULL CONSTRAINT [DF_tt_entries_Billed] DEFAULT ((0)),
[Imported] [bit] NOT NULL CONSTRAINT [DF_tt_entries_Imported] DEFAULT ((0)),
[Xml] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [DF_tt_entries_Xml] DEFAULT (''),
[tversion] [timestamp] NOT NULL
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_tt_Entries] on [dbo].[Entries]'
GO
ALTER TABLE [dbo].[Entries] ADD CONSTRAINT [PK_tt_Entries] PRIMARY KEY CLUSTERED ([Pk])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [CustomerPk] on [dbo].[Entries]'
GO
CREATE NONCLUSTERED INDEX [CustomerPk] ON [dbo].[Entries] ([CustomerPk])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [TimeIn] on [dbo].[Entries]'
GO
CREATE NONCLUSTERED INDEX [TimeIn] ON [dbo].[Entries] ([TimeIn])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Projects]'
GO
CREATE TABLE [dbo].[Projects]
(
[Pk] [int] NOT NULL IDENTITY(1, 1),
[CustomerPk] [int] NULL,
[ProjectName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Entered] [datetime] NULL CONSTRAINT [DF_tt_Projects_Entered] DEFAULT (getdate()),
[StartDate] [datetime] NULL CONSTRAINT [DF_tt_Projects_StartDate] DEFAULT (getdate()),
[EndDate] [datetime] NULL CONSTRAINT [DF_tt_Projects_EndDate] DEFAULT (getdate()),
[Status] [int] NULL CONSTRAINT [DF_tt_Projects_Status] DEFAULT ((0)),
[tversion] [timestamp] NULL
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_tt_Projects] on [dbo].[Projects]'
GO
ALTER TABLE [dbo].[Projects] ADD CONSTRAINT [PK_tt_Projects] PRIMARY KEY CLUSTERED ([Pk])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Lookups]'
GO
CREATE TABLE [dbo].[Lookups]
(
[pk] [int] NOT NULL IDENTITY(1, 1),
[type] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [DF_wws_lookups_type] DEFAULT (''),
[cdata] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [DF_wws_lookups_cdata] DEFAULT (''),
[cdata1] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [DF_wws_lookups_cdata1] DEFAULT (''),
[cdata2] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [DF_wws_lookups_cdata2] DEFAULT (''),
[idata] [int] NULL CONSTRAINT [DF_wws_lookups_idata] DEFAULT ((0)),
[tversion] [timestamp] NULL
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_wws_lookups] on [dbo].[Lookups]'
GO
ALTER TABLE [dbo].[Lookups] ADD CONSTRAINT [PK_wws_lookups] PRIMARY KEY NONCLUSTERED ([pk])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Customers]'
GO
CREATE TABLE [dbo].[Customers]
(
[Pk] [int] NOT NULL IDENTITY(1, 1),
[LastName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_tt_customers_LastName] DEFAULT (''),
[FirstName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_tt_customers_FirstName] DEFAULT (''),
[Company] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_tt_customers_Company] DEFAULT (''),
[Address] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_tt_customers_Address] DEFAULT (''),
[City] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_tt_customers_City] DEFAULT (''),
[State] [nchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_tt_customers_State] DEFAULT (''),
[Zip] [nchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_tt_customers_Zip] DEFAULT (''),
[Country] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_tt_customers_Country] DEFAULT (''),
[CountryId] [nchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_tt_customers_CountryId] DEFAULT (''),
[Phone] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_tt_customers_Phone] DEFAULT (''),
[Email] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_tt_customers_Email] DEFAULT (''),
[Fax] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_tt_customers_Fax] DEFAULT (''),
[Notes] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Entered] [smalldatetime] NOT NULL CONSTRAINT [DF_Customers_Entered] DEFAULT (((1)/(1))/(1900)),
[Updated] [smalldatetime] NOT NULL CONSTRAINT [DF_Customers_Updated] DEFAULT (((1)/(1))/(1900)),
[LastOrder] [smalldatetime] NOT NULL CONSTRAINT [DF_Customers_LastOrder] DEFAULT (((1)/(1))/(1900)),
[BillingRate] [decimal] (18, 2) NULL CONSTRAINT [DF_Customers_BillingRate] DEFAULT ((0)),
[Xml] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[tversion] [timestamp] NOT NULL
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_tt_customers] on [dbo].[Customers]'
GO
ALTER TABLE [dbo].[Customers] ADD CONSTRAINT [PK_tt_customers] PRIMARY KEY CLUSTERED ([Pk])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Invoices]'
GO
CREATE TABLE [dbo].[Invoices]
(
[Pk] [int] NOT NULL IDENTITY(1, 1),
[CustomerPk] [int] NOT NULL,
[InvoiceNo] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InvoiceDate] [datetime] NULL,
[SubTotal] [decimal] (18, 2) NULL,
[Tax] [decimal] (18, 2) NULL,
[Paid] [bit] NULL,
[tversion] [timestamp] NULL
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Invoices] on [dbo].[Invoices]'
GO
ALTER TABLE [dbo].[Invoices] ADD CONSTRAINT [PK_Invoices] PRIMARY KEY CLUSTERED ([Pk])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[TT_EntryView]'
GO
CREATE VIEW dbo.TT_EntryView
AS
SELECT     dbo.tt_Entries.*, dbo.tt_customers.lastname AS LastName, dbo.tt_customers.firstname AS FirstName, dbo.tt_customers.company AS Company
FROM         dbo.tt_customers INNER JOIN
                      dbo.tt_Entries ON dbo.tt_customers.pk = dbo.tt_Entries.CustomerPk

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Invoices]'
GO
ALTER TABLE [dbo].[Invoices] WITH NOCHECK ADD
CONSTRAINT [FK_Invoices_Customers] FOREIGN KEY ([CustomerPk]) REFERENCES [dbo].[Customers] ([Pk])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Entries]'
GO
ALTER TABLE [dbo].[Entries] WITH NOCHECK ADD
CONSTRAINT [FK_tt_entries_tt_Projects] FOREIGN KEY ([ProjectPk]) REFERENCES [dbo].[Projects] ([Pk]) ON DELETE CASCADE,
CONSTRAINT [FK_Entries_Invoices] FOREIGN KEY ([InvoicePk]) REFERENCES [dbo].[Invoices] ([Pk]),
CONSTRAINT [FK_tt_entries_tt_users] FOREIGN KEY ([UserPk]) REFERENCES [dbo].[Users] ([Pk])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Projects]'
GO
ALTER TABLE [dbo].[Projects] ADD
CONSTRAINT [FK_Projects_Customers] FOREIGN KEY ([CustomerPk]) REFERENCES [dbo].[Customers] ([Pk])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Disabling constraints on [dbo].[Invoices]'
GO
ALTER TABLE [dbo].[Invoices] NOCHECK CONSTRAINT
[FK_Invoices_Customers]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Disabling constraints on [dbo].[Entries]'
GO
ALTER TABLE [dbo].[Entries] NOCHECK CONSTRAINT
[FK_tt_entries_tt_Projects],
[FK_Entries_Invoices],
[FK_tt_entries_tt_users]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Determines whether this entry has been invoiced', 'SCHEMA', N'dbo', 'TABLE', N'Entries', 'COLUMN', N'Billed'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Detailed Description of the entry', 'SCHEMA', N'dbo', 'TABLE', N'Entries', 'COLUMN', N'Description'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Determines whether this entry has been imported', 'SCHEMA', N'dbo', 'TABLE', N'Entries', 'COLUMN', N'Imported'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Item total for this entry', 'SCHEMA', N'dbo', 'TABLE', N'Entries', 'COLUMN', N'ItemTotal'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Flag that indicates whether this entry is punched out', 'SCHEMA', N'dbo', 'TABLE', N'Entries', 'COLUMN', N'PunchedOut'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Quantity for non-time entries', 'SCHEMA', N'dbo', 'TABLE', N'Entries', 'COLUMN', N'Qty'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'The billing rate of the entry', 'SCHEMA', N'dbo', 'TABLE', N'Entries', 'COLUMN', N'Rate'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Determines whether this entry is taxable', 'SCHEMA', N'dbo', 'TABLE', N'Entries', 'COLUMN', N'Taxable'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Timestamp when this entry was punched in', 'SCHEMA', N'dbo', 'TABLE', N'Entries', 'COLUMN', N'TimeIn'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Timestamp when this entry was punched out', 'SCHEMA', N'dbo', 'TABLE', N'Entries', 'COLUMN', N'TimeOut'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Summary of the description', 'SCHEMA', N'dbo', 'TABLE', N'Entries', 'COLUMN', N'Title'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'The total hours spent on this entry', 'SCHEMA', N'dbo', 'TABLE', N'Entries', 'COLUMN', N'TotalHours'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Optional XML data to additional data', 'SCHEMA', N'dbo', 'TABLE', N'Entries', 'COLUMN', N'Xml'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Determines whether the invoice was paid', 'SCHEMA', N'dbo', 'TABLE', N'Invoices', 'COLUMN', N'Paid'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Generic overloaded lookup table. Table currently holds the list of countries/countrycode and the store''s categories. Overloading is done on the TYPE field.', 'SCHEMA', N'dbo', 'TABLE', N'Lookups', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Primary Character data (15 chars)', 'SCHEMA', N'dbo', 'TABLE', N'Lookups', 'COLUMN', N'cdata'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Secondary Character data (40 chars)', 'SCHEMA', N'dbo', 'TABLE', N'Lookups', 'COLUMN', N'cdata1'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Memo style character  data ', 'SCHEMA', N'dbo', 'TABLE', N'Lookups', 'COLUMN', N'cdata2'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Int data', 'SCHEMA', N'dbo', 'TABLE', N'Lookups', 'COLUMN', N'idata'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Item Type (CATEGORY, COUNTRY, STATE etc.)', 'SCHEMA', N'dbo', 'TABLE', N'Lookups', 'COLUMN', N'type'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'The estimated end date for the project.', 'SCHEMA', N'dbo', 'TABLE', N'Projects', 'COLUMN', N'EndDate'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'The date when the project was entered', 'SCHEMA', N'dbo', 'TABLE', N'Projects', 'COLUMN', N'Entered'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'The date when the project was actually started', 'SCHEMA', N'dbo', 'TABLE', N'Projects', 'COLUMN', N'StartDate'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_Description', N'Determines project status: 0 - Entered  1 - Started 2 - Completed', 'SCHEMA', N'dbo', 'TABLE', N'Projects', 'COLUMN', N'Status'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
GO
SET DATEFORMAT YMD
GO
-- Pointer used for text / image updates. This might not be needed, but is declared here just in case
DECLARE @pv binary(16)

BEGIN TRANSACTION

-- Drop constraints from [dbo].[Projects]
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_Projects_Customers]

-- Drop constraints from [dbo].[Entries]
ALTER TABLE [dbo].[Entries] DROP CONSTRAINT [FK_Entries_Invoices]
ALTER TABLE [dbo].[Entries] DROP CONSTRAINT [FK_tt_entries_tt_Projects]
ALTER TABLE [dbo].[Entries] DROP CONSTRAINT [FK_tt_entries_tt_users]

-- Add 10 rows to [dbo].[Customers]
SET IDENTITY_INSERT [dbo].[Customers] ON
INSERT INTO [dbo].[Customers] ([Pk], [LastName], [FirstName], [Company], [Address], [City], [State], [Zip], [Country], [CountryId], [Phone], [Email], [Fax], [Notes], [Entered], [Updated], [LastOrder], [BillingRate], [Xml]) VALUES (1, N'Strahl', N'Rick', N'West Wind Technologies', N'', N'', N'          ', N'            ', N'Afghanistan', N'AF        ', N'', N'', N'', N'', '2007-08-22 00:00:00.000', '2007-10-12 00:00:00.000', '1900-01-11 00:00:00.000', 150.00, NULL)
INSERT INTO [dbo].[Customers] ([Pk], [LastName], [FirstName], [Company], [Address], [City], [State], [Zip], [Country], [CountryId], [Phone], [Email], [Fax], [Notes], [Entered], [Updated], [LastOrder], [BillingRate], [Xml]) VALUES (2, N'Egger', N'Markus', N'EPS Software', N'', N'', N'          ', N'            ', N'USA', N'US        ', N'', N'', N'', N'', '2007-08-01 10:00:00.000', '2007-09-01 16:31:00.000', '1900-01-01 20:00:00.000', 100.00, NULL)
INSERT INTO [dbo].[Customers] ([Pk], [LastName], [FirstName], [Company], [Address], [City], [State], [Zip], [Country], [CountryId], [Phone], [Email], [Fax], [Notes], [Entered], [Updated], [LastOrder], [BillingRate], [Xml]) VALUES (3, N'McNeish', N'Kevin', N'Oak Leaf Enterprises', N'', N'', N'          ', N'            ', N'', N'          ', N'', N'', N'', N'', '2007-08-20 00:00:00.000', '2007-08-31 20:31:00.000', '1900-01-01 00:00:00.000', 120.00, NULL)
INSERT INTO [dbo].[Customers] ([Pk], [LastName], [FirstName], [Company], [Address], [City], [State], [Zip], [Country], [CountryId], [Phone], [Email], [Fax], [Notes], [Entered], [Updated], [LastOrder], [BillingRate], [Xml]) VALUES (4, N'Plander', N'Bill', N'SummaLP', N'', N'', N'          ', N'            ', N'', N'          ', N'', N'', N'', N'', '1900-01-01 10:00:00.000', '2007-09-01 06:31:00.000', '1900-01-01 10:00:00.000', 125.00, NULL)
INSERT INTO [dbo].[Customers] ([Pk], [LastName], [FirstName], [Company], [Address], [City], [State], [Zip], [Country], [CountryId], [Phone], [Email], [Fax], [Notes], [Entered], [Updated], [LastOrder], [BillingRate], [Xml]) VALUES (15, N'Lavier', N'Paul', N'Lavier Consulting', N'', N'', N'          ', N'            ', N'', N'          ', N'', N'', N'', N'', '2007-08-01 00:00:00.000', '2007-08-01 00:00:00.000', '1900-01-01 00:00:00.000', 150.00, ' ')
INSERT INTO [dbo].[Customers] ([Pk], [LastName], [FirstName], [Company], [Address], [City], [State], [Zip], [Country], [CountryId], [Phone], [Email], [Fax], [Notes], [Entered], [Updated], [LastOrder], [BillingRate], [Xml]) VALUES (16, N'Siegel', N'Benjamin', N'New Generation Computing', N'', N'', N'          ', N'            ', N'', N'          ', N'', N'', N'', N'', '1900-01-01 00:00:00.000', '1900-01-01 00:00:00.000', '1900-01-01 00:00:00.000', 125.00, NULL)
INSERT INTO [dbo].[Customers] ([Pk], [LastName], [FirstName], [Company], [Address], [City], [State], [Zip], [Country], [CountryId], [Phone], [Email], [Fax], [Notes], [Entered], [Updated], [LastOrder], [BillingRate], [Xml]) VALUES (23, N'Brown', N'Mark', N'Sonora Engineering', N'', N'', N'          ', N'            ', N'', N'          ', N'', N'', N'', N'', '2007-08-05 00:00:00.000', '2007-08-05 00:00:00.000', '1900-01-01 00:00:00.000', 150.00, NULL)
INSERT INTO [dbo].[Customers] ([Pk], [LastName], [FirstName], [Company], [Address], [City], [State], [Zip], [Country], [CountryId], [Phone], [Email], [Fax], [Notes], [Entered], [Updated], [LastOrder], [BillingRate], [Xml]) VALUES (24, N'Scott', N'Johnston', N'Scott Johnston', N'', N'', N'          ', N'            ', N' ', N'          ', N'', N'', N' ', N'', '2007-12-06 00:00:00.000', '2007-12-06 00:00:00.000', '1900-01-01 00:00:00.000', 150.00, NULL)
INSERT INTO [dbo].[Customers] ([Pk], [LastName], [FirstName], [Company], [Address], [City], [State], [Zip], [Country], [CountryId], [Phone], [Email], [Fax], [Notes], [Entered], [Updated], [LastOrder], [BillingRate], [Xml]) VALUES (25, N'Tucker', N'Craig', N'Centiv', N'', N'', N'          ', N'            ', N'Afghanistan', N'AF        ', N'', N'', N'', N'', '2008-01-01 00:00:00.000', '2008-01-01 00:00:00.000', '1900-01-01 00:00:00.000', 150.00, NULL)
INSERT INTO [dbo].[Customers] ([Pk], [LastName], [FirstName], [Company], [Address], [City], [State], [Zip], [Country], [CountryId], [Phone], [Email], [Fax], [Notes], [Entered], [Updated], [LastOrder], [BillingRate], [Xml]) VALUES (26, N'vanWerkhoven', N'Marcel', N'Marcel vanWerkhoven', N'', N'', N'          ', N'            ', N'', N'          ', N'', N'', N'', NULL, '1900-01-01 00:00:00.000', '1900-01-01 00:00:00.000', '1900-01-01 00:00:00.000', 150.00, NULL)
SET IDENTITY_INSERT [dbo].[Customers] OFF

-- Add 55 rows to [dbo].[Entries]
SET IDENTITY_INSERT [dbo].[Entries] ON
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (194, 24, 49, 0, 1, N'Intial Review of Materials', N'Took a brief look at the provided documentation and review Fox code. Also looked at WSDL and possible fixups for wwSOAP.

Decide: Not worth the effort. WS-Security extensions require a bit of extra work and the complex type requirements in the service are somewhat extensive. Recommend to use .NET with WCF instead.', '2007-12-07 01:25:00.000', '2007-12-07 02:15:00.000', 1, 0.00, 150.00, 0.83, 124.99, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (195, 24, 49, 0, 1, N'Build Interop Wrappers for .NET and FoxPro', N'Got solution to initial problems with WCF communications and can now call service reliably.

Created generic .NET service instantiation code so the service can read all settings in one place from the configuration.

Test Ping Service and examine the Enrolment Service and NewEnrolment method code from SOAP examples provided. Run tests through .NET code.

Create FoxPro Interop wrapper for both services using wwDotNetBridge. Test and verify the service results.

Start writing up documentation. Write extended message for Scott to review progress.', '2007-12-10 21:35:00.000', '2007-12-11 01:25:00.000', 1, 0.00, 150.00, 3.83, 574.99, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (196, 24, 49, 0, 1, N'First Attempts at WCF Calling Web Service', N'Created Client Service Contracts for WCF to test accessing the Web Service. Ran into several problems with protocol errors.

Tried to resolve errors various ways without much luck.

Finally resolved to write a blog entry to solicit outside input on this specific communication layer problem.', '2007-12-09 20:10:00.000', '2007-12-09 21:45:00.000', 1, 0.00, 150.00, 1.58, 237.49, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (197, 24, 49, 0, 1, N'Create documentation for original Examples', N'Update initial documentation with steps for instantiating types and how types are referenced in Visual FoxPro. 

Check out the sample code sent by Scott to ensure the service works - it does. Add several small enhancements to the .NET and FoxPro service base classes. Exception trapping on both, and a common base class for the Fox services.', '2007-12-12 19:00:00.000', '2007-12-12 20:00:00.000', 1, 0.00, 150.00, 1.00, 150.00, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (198, 24, 49, 0, 1, N'Continue Documentation and Testing', N'Work on documentation for the FoxPro and .NET code as well as update the configuration and requirements docs. Almost complete but will have to rework the .NET docs for several refactoring changes made.', '2007-12-13 09:45:00.000', '2007-12-13 11:15:00.000', 1, 0.00, 150.00, 1.50, 225.00, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (199, 24, 49, 0, 1, N'Refactor .NET Service Implementation Code', N'Refactored the .NET wrapper class implementation so that there''s less code to create for each service class. Using Generics to remove repetitive initialization code for each Web Service.

Experiment with direct Web Service access from VFP, but due to serialization issues in the imported service direct COM access is not possible - wrapper classes required.', '2007-12-14 00:05:00.000', '2007-12-14 01:55:00.000', 1, 0.00, 150.00, 1.83, 274.99, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (200, 24, 49, 0, 1, N'Walk through with Scott', N'Walked through the application interfaces and explained operation, nuances and a few small debugging issues.', '2007-12-16 19:30:00.000', '2007-12-16 22:30:00.000', 1, 0.00, 150.00, 3.00, 450.00, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (201, 24, 49, 0, 1, N'Miscellaneous Updates', N'Fix Project Instantion code and error handling so connection level errors ripple back to the FoxPro client.

Add code to pass all parameters from VFP to the .NET object. Change default code on Initialize() to require no parameters and initialize .NET object from FoxPro properties. This allows complete configuration through FoxPro code.

Miscellaneous minor code fixes and update the docs', '2007-12-16 23:10:00.000', '2007-12-17 00:20:00.000', 1, 0.00, 150.00, 1.16, 175.00, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (202, 24, 49, 0, 1, N'Create wwDotNetBridge Array Routines (partial)', N'Create various array and collection handling routines that will be needed for some of the WCF input message types. Added methods for CreateArrayOnInstance, AddArrayItem, RemoveArrayItem, GetArrayItem to manipulate arrays indirectly.

Only picked up a initial design time here - actual time was quite longer.', '2007-12-17 19:05:00.000', '2007-12-17 20:25:00.000', 1, 0.00, 150.00, 1.33, 199.99, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (204, 15, 18, 0, 1, N'Review exiting Omni System for Changes', N'Review existing system and configure to ensure that it works sending messages to the server. Confirm operation, but unable to get clear indication whether the remote server is poroperly accpeting messages. 

Tried to update to more recent WSDL files (just in case there were changes that affect behavior), but failed to import -these files require changes that were made to the original WSDL to make them work in .NET environment.', '2008-01-14 23:25:00.000', '2008-01-15 00:45:00.000', 1, 0.00, 150.00, 1.33, 199.99, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (205, 15, 18, 0, 1, N'Overview Review Phone Call with Paul', N'Review existing work that had been done and walk through base operations. Outline a few improvements to be made and discuss setting up another example using InstructAgent message format.

To do list:
* Check WCF message Structure (better future proofing)
* Hook up wwDotNetBridge instead of COM Interop creation (CreateObjectDotNet())
* Hook up Enum function to simplify Enum access (Enum())
* Check received server messages and parsing', '2008-01-15 09:00:00.000', '2008-01-15 10:00:00.000', 1, 0.00, 150.00, 1.00, 150.00, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (206, 15, 18, 0, 1, N'Create FoxPro Helper Routines', N'Create helper methods that use wwDotNetBridge for .NET object access instead of COM instantiation. Create quick access helpers for Enum(), CreateInstance(), CreateOmniBusinessInstance(), AddArrayItem() and a few other small helper methods that use wwDotNetBridge to call into .NET code.

Also checkout Message retrieval (from the West Wind Server) to check for valid operation. Run through full cycle of posting message to the Omni server, and receiving the returned message from the Omni server from the West Wind server and echo back content.', '2008-01-16 14:45:00.000', '2008-01-16 16:15:00.000', 1, 0.00, 150.00, 1.50, 225.00, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (207, 15, 18, 0, 1, N'Create InstructAgent Code', N'Start creating the Instruct Agent Code. Update various additional helper functions. Notes to Mike Russell and Paul.', '2008-01-16 23:25:00.000', '2008-01-17 00:45:00.000', 1, 0.00, 150.00, 1.33, 199.99, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (208, 15, 18, 0, 1, N'Test GetMessages Code', N'Test the InstructAgent code against our own server. Post messages with SendInstructAgent, and then retrieve and parse the inbound messages for test purpose. Create additional helpers include GetEnumString which turns an enum value into its field string value (ie. CountryCode numeric ID into the actual country code).

Continue miscellaneous testing scenarios.

Message to Mike Russell requesting more info on routing information for InstructAgent messages so they route back to our server. Status update for Paul.

To do list:
* Documentation updates for new functions
* Document Array behavior for Paul - Array behavior on retrieval is less than optimal
* What do we need for ''GetMessages()''
* Install procedures and requirements
', '2008-01-17 22:10:00.000', '2008-01-17 23:25:00.000', 1, 0.00, 150.00, 1.25, 187.50, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (209, 15, 18, 0, 1, N'Continue work on InstructAgent Code', N'Continue working through the InstructAgent code. Continue abstracting helper functions for common repetitive ''object'' operations.', '2008-01-17 14:05:00.000', '2008-01-17 15:45:00.000', 1, 0.00, 150.00, 1.66, 250.00, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (210, 15, 18, 0, 1, N'Fix InstructAgent Validation Errors', N'Fix several small validation errors in the InstructAgent request per Mike Russells comments. Ended up having to fix the WSDL to get a around a few small problems. Documented WSDL changes to data WSDL_FIXES.TXT in the business project. Note to Mike Russell.', '2008-01-18 23:10:00.000', '2008-01-18 23:35:00.000', 1, 0.00, 150.00, 0.41, 62.50, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (211, 25, 50, 0, 1, N'Initial Orientation', N'Initial review of system and talk with Craig.', '2008-01-24 12:25:00.000', '2008-01-24 12:50:00.000', 1, 0.00, 150.00, 0.41, 62.50, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (212, 25, 50, 0, 1, N'Create Sample Service', N'Create the sample service and try running the sample with the simulated link information.
Unable to test due to the portal failing to work on TradeOne site.', '2008-01-24 20:45:00.000', '2008-01-24 20:50:00.000', 1, 0.00, 150.00, 0.08, 12.49, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (213, 15, 18, 0, 1, N'Update documentation for Omni Service and FoxPro helpers', N'Update documentation to the latest set of changes. Includes updated FoxPro class documenation for helper classes as well as updated documentation for procedures which have been simplified somewhat from the previous version. Also updated the ''.NET Object Access'' topic to walk through several of the issues you need to deal with when accessing these message objects.

Cleaned up the code samples a bit, adding additional comments so they''re easier to read and see what''s going on. Tested several additional messages, and passed a few messages back and forth with Mike Russell.', '2008-01-25 11:10:00.000', '2008-01-25 14:55:00.000', 1, 0.00, 150.00, 3.75, 562.50, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (214, 15, 18, 0, 1, N'Few Maintenance Fixes', N'Make several small changes to the provider loading code and examples locally. Also retest Web Service operation locally.

Fix provider configuration lookup for the service URLs so web app uses web.config and desktop app uses omnicell.config. Add Configuration object to main provider object (loProvider.Configuration) for easy access to the configuration settings from Fox code.

Add error logging to SQL server to the Receiver service so any errors that occur in the message parsing code at least will result in a log entry. Note this only affects method code, not invalid messages which will be rejected at the protocol level and will reject with SOAP protocol errors to the client.', '2008-01-28 18:00:00.000', '2008-01-28 18:50:00.000', 1, 0.00, 150.00, 0.83, 124.99, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (215, 15, 18, 0, 1, N'Help Paul Set up System', N'Install system on Paul''s machine and walk him through basic configuration and startup steps.', '2008-01-30 18:00:00.000', '2008-01-30 19:10:00.000', 1, 0.00, 150.00, 1.16, 175.00, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (216, 26, 52, 0, 1, N'Initial Research into Xml Document Signing', N'Check out SignedXml class and try to duplicate the exact XML signature of the sample documents using the existing WSE example as a baseline. Code is based on new security classes in .NET 3.0.

Create XmlDoc signing code routine that takes input XML doc, adds a signature for a Body element and embeds it into the Soap header.', '2008-01-30 22:10:00.000', '2008-01-31 01:05:00.000', 1, 0.00, 150.00, 2.91, 437.50, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (217, 26, 52, 0, 1, N'Continue working on Signature Code', N'Quick phone discussion with Marcel to review outstanding issues. Wait for test scenario with live server calls.

Test with actual certificate from RouteOne. Refactor code a bit and comment. Update Unit Test for base scenario.', '2008-01-31 15:25:00.000', '2008-01-31 17:05:00.000', 1, 0.00, 150.00, 1.66, 250.00, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (218, 25, 50, 0, 1, N'Check out TradeOne Sample', N'Work on TradeOne sample code provided and try to make it work. Code doesn''t work out of box so spent some time debugging and fixing up references.

Walked through the sign up process with Karl. Apparently there''s a cookie related bug on the site that causes the site to crash.

Stuck again - Web Service doesn''t work and doesn''t return a valid result.', '2008-02-01 15:15:00.000', '2008-02-01 16:45:00.000', 1, 0.00, 150.00, 1.50, 225.00, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (221, 25, 50, 0, 1, N'Run Karl''s "updated" Code', N'Received updated code from Karl. Checked out code and again the code doesn''t compile and requires fixing. Fixed and re-ran sample with transfer tokens, but the Web Service still fails to work and returns server side error without any further information.', '2008-02-04 21:15:00.000', '2008-02-04 21:35:00.000', 1, 0.00, 150.00, 0.33, 49.99, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (222, 25, 50, 0, 1, N'Try Karl''s Updated Code', N'Update to Karl''s latest code. Again, fixed code to more standard ASP.NET layout. Ran into problems with the Portal AGAIN. Couldn''t log in with IE and FireFox failed to fill session data from Session Link ID. Made it work after all and finally completed test against the portal.

Tested valid key scenarios and invalid keys and timeouts. Expected behavior.

Sent Karl basic fixed site layout so hopefully they''ll use that for future sample code.', '2008-02-07 00:45:00.000', '2008-02-07 01:25:00.000', 1, 0.00, 150.00, 0.66, 100.00, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (223, 25, 50, 0, 1, N'Create Centiv Version of TradeOne Logon', N'Break out Karl''s sample code into self contained class and configuration so we can completely control the operation with configuration switches.', '2008-02-07 13:10:00.000', '2008-02-07 13:55:00.000', 1, 0.00, 150.00, 0.75, 112.50, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (224, 25, 50, 0, 1, N'Continue work on TradeOne Interface', N'Continue building out the Session Transfer operation. Complete classwrapper installation to reduce actual code to literally two lines of code. Set up class to be purely configurable through configuration settings.

Create SQL Server table and test insertion code for picking up session variables. Add functionality to tag Sessions as picked up. Add support for error handling in the table so Craigs code has an easy way to check and read error information.

Test scenario end to end. Write up open issues for Craig to review.', '2008-02-07 23:05:00.000', '2008-02-08 02:10:00.000', 1, 0.00, 150.00, 3.08, 462.49, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (225, 25, 50, 0, 1, N'Continue work on TradeOne Interface', N'Switch to Properties field that contains all the service variables, rather than one field per key into the database. This should facilitate writing out the data as well as reading it and allows addition of new keys later.

Also added code to deal with duplicate session keys.

Clean up sample code, and test various configurations. Code cleanup and add additional code commenting.', '2008-02-08 12:35:00.000', '2008-02-08 14:15:00.000', 1, 0.00, 150.00, 1.66, 250.00, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (226, 25, 50, 0, 1, N'Create FoxPro Classes and Documentation', N'Refactor final .NET classes. Create matching FoxPro classes to provide Craig''s fox interface for accessing the SQL data from the TradeOne Session.

Update the samples - clean up pages and create test page that simplifies testing without having to reload tokens from TradeOne for every hit.

Create full Html Help Documentation for class reference, configuration and process flow and developer information.', '2008-02-08 22:15:00.000', '2008-02-09 02:20:00.000', 1, 0.00, 150.00, 4.08, 612.49, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (227, 25, 50, 0, 1, N'Package TradeOne Integration Application', N'Retest samples one more time to make sure nothing has changed before wrapping up.

Added additional documentation for Fox classes and updated the deployment topics in the help documentation.

Packaged and sent to Craig with basic installation notes.', '2008-02-13 00:15:00.000', '2008-02-13 01:25:00.000', 1, 0.00, 150.00, 1.16, 175.00, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (228, 25, 50, 0, 1, N'Walk through with Craig', N'Walk through .NET and FoxPro code with Craig through GotoMeeting. 

Fix a few related code issues in the FoxPro end of Craig''s framework. Review planned integration of TradeOne code.', '2008-02-15 09:00:00.000', '2008-02-15 11:20:00.000', 1, 0.00, 150.00, 2.33, 349.99, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (229, 26, 52, 0, 1, N'Check out samples and XML structure', N'Rework the encoding code and add code to validate the XML Signature to ensure it works correctly. Sure enough the signing code was not correct and it took some experimentation to make sure the signature works correctly both ways.', '2008-02-18 22:05:00.000', '2008-02-19 00:35:00.000', 1, 0.00, 150.00, 2.50, 375.00, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (230, 26, 52, 0, 1, N'Test Marcel''s Key', N'Install Marcel''s Tidewater key and test application with the ''offical key''. Deal with various registration issues to get the key properly installed. 

Write up key installation procedure.

Set up IIS test scenario with ForwardToRouteOne.aspx page that handles the actual operation of message forwarding along with two test pages SimulateXmlPost.aspx which picks up a test file from disk and posts it to ForwardToRouteOne.aspx and another PostRecipient.aspx that acts as the endpoint that receives the the file. The PostRecipient page simply echos back the data received.

Tested with local VS.NET server and this works fine. Unfortunately ran into another issue with IIS security - IIS is unable to access the private key encryption.
', '2008-02-22 10:50:00.000', '2008-02-22 14:30:00.000', 1, 0.00, 150.00, 3.66, 550.00, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (231, 25, 50, 0, 1, N'Help Craig move sample to live', N'Help Craig configure Web Server which exhibited problems with Authentication. Turns out web.config on the root forced authentication against all ASPX pages. 

Attempted to test on live server, but unfortunately TradeOne Portal app crashed again so we couldn''t walk through the full test scenario. We were able to test though with a manually retrieved key and validate the application working on the live Web Server.

Outline problem issues with TradeOne server to Karl via Email. We need better token creation flexibility or else we''ll be stuck in production without an effective way to debug.', '2008-02-20 13:20:00.000', '2008-02-20 15:05:00.000', 1, 0.00, 150.00, 1.75, 262.50, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (232, 26, 52, 0, 1, N'Work out Key Encryption for IIS Security', N'Troubleshoot key encryption errors in IIS. Turns out the key was not installed properly in the Local_Machine store. Simply moving is not enough to get the key properly installed for security.

It''s required to explicitly import the key from the .PFX file into the Configuration store then apply security via WinHttpCertCfg.exe.

Re-document key installation with updated information.', '2008-02-23 12:35:00.000', '2008-02-23 13:55:00.000', 1, 0.00, 150.00, 1.33, 199.99, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (233, 26, 52, 0, 1, N'Refactor and Cleanup Code, Update docs, package', N'Refactored the code by renaming various classes and consolidating code. Took the IIS Receiver page and simplified the processing of inbound requests based on Marcel''s original code and stored with existing ''business'' class.

Cleaned up comments in document, updated help file for configuration and new names.

Outlined progress to Marcel and sent message to Chris at RouteOne for more information on formal interface testing across the network.', '2008-02-24 17:35:00.000', '2008-02-24 20:15:00.000', 1, 0.00, 150.00, 2.66, 400.00, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (234, 26, 52, 0, 1, N'Research Signature Validation Problems', N'Send back several messages to Chris at Route One to see if we can get them to validate. No luck originally.

After some experimentation with XmlDocument options found PreserveWhiteSpace requirement to allow at least parsing of route one messages on our end. Send test messages to Chris for checking', '2008-03-01 08:55:00.000', '2008-03-01 09:45:00.000', 1, 0.00, 150.00, 0.83, 124.99, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (235, 26, 52, 0, 1, N'Miscellaneous Adjustments to Samples', N'Update code to use PreserveWhitespace and re-run through the sample scenarios.

Talk to Marcel for setting up the sample server to test live messages against. Review of message flow with Marcel.', '2008-03-04 09:55:00.000', '2008-03-04 10:45:00.000', 1, 0.00, 150.00, 0.83, 124.99, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (237, 26, 52, 0, 1, N'Install Application on live Server', N'Move RouteOne interface code to the Web Server. Install certificate on the server in local machine store. Configure certificate and run through various test scenarios. Inbound requests from RouteOne work, but requests using certificates fail due to permissions. Attempt to set permissions for the certificate, but unable to get the certificate to work under NETWORK SERVICE. Switched application pool to use SYSTEM which does work.', '2008-03-07 13:10:00.000', '2008-03-07 15:00:00.000', 1, 0.00, 150.00, 1.83, 274.99, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (238, 26, 52, 0, 1, N'POST Problems with RouteOne', N'Check on issues related to POST formatting to the live server. Add RouteOne headers to the POST data sent to RouteOne.

Also update the RouteOne_Receive.aspx page to return Ok on success and a SOAP fault on failure as specified in the specs. Update live server at Tidewater. 

Run several tests locally and agains the remote Tidewater site and check responses.', '2008-03-10 11:20:00.000', '2008-03-10 13:30:00.000', 1, 0.00, 150.00, 2.16, 325.00, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (239, 26, 52, 0, 1, N'Header Bug Fixes', N'Fixed several small header issues and updated code on the live server. Few tests with the updated data.', '2008-03-11 18:25:00.000', '2008-03-11 18:50:00.000', 1, 0.00, 150.00, 0.41, 62.50, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (240, 25, 50, 0, 1, N'Troubleshooting of application', N'Verify operation of the token request form for Craig. Test with new portal links and fail session dump. Double check with provided long term test token and that token works. Apparently there''s a failure in the test token generation of the session dump.', '2008-03-14 11:35:00.000', '2008-03-14 12:15:00.000', 1, 0.00, 150.00, 0.66, 100.00, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (244, 26, 52, 0, 1, N'Fix Header Parsing for SendDecision', N'Fixed the header parsing for the MessageIdentifier header to parse out the formatting from the sub entries in the XML document.

Refactor the POST code for SendDecision. Update on live server and forward updated code to Marcel.', '2008-03-15 16:05:00.000', '2008-03-15 16:30:00.000', 1, 0.00, 150.00, 0.41, 62.50, 0, 1, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (245, 25, 50, 0, 1, N'Fix Duplicate Session Keys', N'Interactive bug fixing with Craig. Trouble shoot problem with duplicate session keys returned from TradeOne. Fixed code to account for duplicates copied into our internal dictionary. Walk Craig through debugging application interactively.', '2008-03-24 10:05:00.000', '2008-03-24 11:15:00.000', 1, 0.00, 150.00, 1.16, 175.00, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (246, 4, 53, 0, 1, N'Start VFP -> ASP.NET Data Bridge Service', N'Design XML Service Message interface for passing messages from server to client. Deal with SQL Commands and parameter passing semantics.

Implement VFP Client component that generates the necessary messages. Start with infrastructure implementation and Execute() method operation that returns cursors.

Implement server handler interface that can handle inbound message requests. Implement Xml Message parser and output generator for returning DataSet data and error messages consistently. Implement server side Execute functionality for returning data set based result sets.


', '2008-03-27 18:00:00.000', '2008-03-27 22:55:00.000', 1, 0.00, 125.00, 4.91, 614.58, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (247, 4, 53, 0, 1, N'Continue building Data Service', N'Continue building out the ASP.NET Data Service interface. Handle ExecuteNonQuery operation and Stored Procedure forwarding including parameter names. Handle encoding issues in FoxPro - add support for UTF-8 decoding to allow access to Unicode SQL data (ie. nChar, nVarChar and nText support).

Deal with type conversion issues in FoxPro, .NET and Sql. Refactor several times, end up with using SQL types to explicitly declare types for parameters. Parameters guessing is used to map types automatically. Work around number issues by using Float as the default SQL type (same as FoxPro Sql Passthrough).

Test various scenarios', '2008-03-28 13:45:00.000', '2008-03-28 21:35:00.000', 1, 0.00, 125.00, 7.83, 979.16, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (248, 4, 53, 0, 1, N'Write up Documentation, Add Additional Testing', N'Created Help File for documentation of the FoxPro component, installation and examples to use the remote data provider.

Add basic authentication support to the service so we can at least block out open access.

Few bug fixes during additional testing', '2008-03-29 11:35:00.000', '2008-03-29 13:45:00.000', 1, 0.00, 125.00, 2.16, 270.83, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (250, 25, 50, 0, 1, N'Check out Order Web Service with wwSoap', N'Create wwSOAP::CreateObjectFromWsdl method that allows creating a Fox object from the WSDL schema. This object can then be passed as a parameter to the service. This greatly simplifies calling services with complex types.

Start looking at the order service and attempt calling service with the above tools. Call is missing parameter values and requires some cleanup but it appears the structure is sound and will likely work with a complete set of data. Awaiting more data from Craig.', '2008-03-31 12:40:00.000', '2008-03-31 14:00:00.000', 1, 0.00, 150.00, 1.33, 199.99, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (252, 1, 38, 0, 1, N'Get ready for LINQ to SQL Talk', N'Review samples to ensure they work. Zip up all samples and latest slides and upload to site. Revisit LINQ to SQL queries to be hand coded during demo.', '2008-04-22 10:15:00.000', '2008-04-22 11:35:00.000', 1, 0.00, 150.00, 1.33, 199.99, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (253, 26, 38, 0, 1, N'DevConnection Session', N'', '2008-04-22 17:30:00.000', '1900-01-01 00:00:00.000', 0, 0.00, 0.00, 0.00, 0.00, 0, 0, 0, NULL)
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (256, 0, 0, 0, 1, N'New Entry', N'New Time Entry', '2009-05-18 02:36:16.873', '1900-01-01 00:00:00.000', 0, 0.00, 0.00, 0.00, 0.00, 0, 0, 0, N'')
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (257, 0, 0, 0, 1, N'New Entry', N'New Time Entry', '2009-05-18 02:37:30.100', '1900-01-01 00:00:00.000', 0, 0.00, 0.00, 0.00, 0.00, 0, 0, 0, N'')
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (258, 0, 0, 0, 1, N'New Entry', N'New Time Entry', '2009-05-18 02:42:22.253', '1900-01-01 00:00:00.000', 0, 0.00, 0.00, 0.00, 0.00, 0, 0, 0, N'')
INSERT INTO [dbo].[Entries] ([Pk], [CustomerPk], [ProjectPk], [InvoicePk], [UserPk], [Title], [Description], [TimeIn], [TimeOut], [PunchedOut], [Qty], [Rate], [TotalHours], [ItemTotal], [Taxable], [Billed], [Imported], [Xml]) VALUES (259, 0, 0, 0, 1, N'New Entry', N'New Time Entry', '2009-05-18 02:43:21.893', '1900-01-01 00:00:00.000', 0, 0.00, 0.00, 0.00, 0.00, 0, 0, 0, N'')
SET IDENTITY_INSERT [dbo].[Entries] OFF

-- Add 318 rows to [dbo].[Lookups]
SET IDENTITY_INSERT [dbo].[Lookups] ON
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (1, N'COUNTRY', N'AF', N'Afghanistan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (2, N'COUNTRY', N'AL', N'Albania', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (3, N'COUNTRY', N'DZ', N'Algeria', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (4, N'COUNTRY', N'AS', N'American Samoa', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (5, N'COUNTRY', N'AD', N'Andorra', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (6, N'COUNTRY', N'AO', N'Angola', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (7, N'COUNTRY', N'AI', N'Anguilla', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (8, N'COUNTRY', N'AQ', N'Antarctica', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (9, N'COUNTRY', N'AG', N'Antigua and Barbuda', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (10, N'COUNTRY', N'AR', N'Argentina', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (11, N'COUNTRY', N'AM', N'Armenia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (12, N'COUNTRY', N'AW', N'Aruba', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (13, N'COUNTRY', N'AU', N'Australia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (14, N'COUNTRY', N'AT', N'Austria', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (15, N'COUNTRY', N'AZ', N'Azerbaijan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (16, N'COUNTRY', N'BS', N'Bahamas', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (17, N'COUNTRY', N'BH', N'Bahrain', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (18, N'COUNTRY', N'BD', N'Bangladesh', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (19, N'COUNTRY', N'BB', N'Barbados', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (20, N'COUNTRY', N'BY', N'Belarus', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (21, N'COUNTRY', N'BE', N'Belgium', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (22, N'COUNTRY', N'BZ', N'Belize', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (23, N'COUNTRY', N'BJ', N'Benin', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (24, N'COUNTRY', N'BM', N'Bermuda', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (25, N'COUNTRY', N'BT', N'Bhutan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (26, N'COUNTRY', N'BO', N'Bolivia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (27, N'COUNTRY', N'BW', N'Botswana', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (28, N'COUNTRY', N'BV', N'Bouvet Island', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (29, N'COUNTRY', N'BR', N'Brazil', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (30, N'COUNTRY', N'IO', N'British Indian Ocean Territory', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (31, N'COUNTRY', N'BN', N'Brunei Darussalam', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (32, N'COUNTRY', N'BG', N'Bulgaria', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (33, N'COUNTRY', N'BF', N'Burkina Faso', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (34, N'COUNTRY', N'BI', N'Burundi', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (35, N'COUNTRY', N'KH', N'Cambodia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (36, N'COUNTRY', N'CM', N'Cameroon', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (37, N'COUNTRY', N'CA', N'Canada', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (38, N'COUNTRY', N'CV', N'Cape Verde', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (39, N'COUNTRY', N'KY', N'Cayman Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (40, N'COUNTRY', N'CF', N'Central African Republic', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (41, N'COUNTRY', N'TD', N'Chad', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (42, N'COUNTRY', N'CL', N'Chile', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (43, N'COUNTRY', N'CN', N'China', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (44, N'COUNTRY', N'CX', N'Christmas Island', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (45, N'COUNTRY', N'CC', N'Cocos (Keeling) Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (46, N'COUNTRY', N'CO', N'Colombia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (47, N'COUNTRY', N'KM', N'Comoros', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (48, N'COUNTRY', N'CG', N'Congo', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (49, N'COUNTRY', N'CK', N'Cook Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (50, N'COUNTRY', N'CR', N'Costa Rica', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (51, N'COUNTRY', N'CI', N'Cote D''ivoire', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (52, N'COUNTRY', N'CU', N'Cuba', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (53, N'COUNTRY', N'CY', N'Cyprus', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (54, N'COUNTRY', N'CZ', N'Czech Republic', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (55, N'COUNTRY', N'DK', N'Denmark', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (56, N'COUNTRY', N'DJ', N'Djibouti', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (57, N'COUNTRY', N'DM', N'Dominica', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (58, N'COUNTRY', N'DO', N'Dominican Republic', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (59, N'COUNTRY', N'EC', N'Ecuador', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (60, N'COUNTRY', N'EG', N'Egypt', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (61, N'COUNTRY', N'SV', N'El Salvador', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (62, N'COUNTRY', N'GQ', N'Equatorial Guinea', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (63, N'COUNTRY', N'ER', N'Eritrea', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (64, N'COUNTRY', N'EE', N'Estonia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (65, N'COUNTRY', N'ET', N'Ethiopia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (66, N'COUNTRY', N'FO', N'Faroe Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (67, N'COUNTRY', N'FK', N'Falkland Islands (Malvinas)', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (68, N'COUNTRY', N'FJ', N'Fiji', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (69, N'COUNTRY', N'FI', N'Finland', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (70, N'COUNTRY', N'FR', N'France', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (71, N'COUNTRY', N'GF', N'French Guiana', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (72, N'COUNTRY', N'PF', N'French Polynesia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (73, N'COUNTRY', N'TF', N'French Southern Territories', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (74, N'COUNTRY', N'GA', N'Gabon', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (75, N'COUNTRY', N'GM', N'Gambia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (76, N'COUNTRY', N'GE', N'Georgia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (77, N'COUNTRY', N'DE', N'Germany', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (78, N'COUNTRY', N'GH', N'Ghana', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (79, N'COUNTRY', N'GI', N'Gibraltar', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (80, N'COUNTRY', N'GR', N'Greece', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (81, N'COUNTRY', N'GL', N'Greenland', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (82, N'COUNTRY', N'GD', N'Grenada', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (83, N'COUNTRY', N'GP', N'Guadeloupe', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (84, N'COUNTRY', N'GU', N'Guam', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (85, N'COUNTRY', N'GT', N'Guatemala', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (86, N'COUNTRY', N'GN', N'Guinea', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (87, N'COUNTRY', N'GW', N'Guinea-Bissau', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (88, N'COUNTRY', N'GY', N'Guyana', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (89, N'COUNTRY', N'HT', N'Haiti', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (90, N'COUNTRY', N'VA', N'Holy See (Vatican City State)', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (91, N'COUNTRY', N'HN', N'Honduras', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (92, N'COUNTRY', N'HK', N'Hong Kong', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (93, N'COUNTRY', N'HU', N'Hungary', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (94, N'COUNTRY', N'IS', N'Iceland', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (95, N'COUNTRY', N'IN', N'India', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (96, N'COUNTRY', N'ID', N'Indonesia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (97, N'COUNTRY', N'IQ', N'Iraq', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (98, N'COUNTRY', N'IE', N'Ireland', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (99, N'COUNTRY', N'IL', N'Israel', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (100, N'COUNTRY', N'IT', N'Italy', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (101, N'COUNTRY', N'JM', N'Jamaica', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (102, N'COUNTRY', N'JP', N'Japan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (103, N'COUNTRY', N'JO', N'Jordan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (104, N'COUNTRY', N'KZ', N'Kazakhstan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (105, N'COUNTRY', N'KE', N'Kenya', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (106, N'COUNTRY', N'KI', N'Kiribati', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (107, N'COUNTRY', N'KP', N'Korea', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (108, N'COUNTRY', N'KR', N'Korea', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (109, N'COUNTRY', N'KW', N'Kuwait', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (110, N'COUNTRY', N'KG', N'Kyrgyzstan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (111, N'COUNTRY', N'LA', N'Lao People''s Democratic Republic', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (112, N'COUNTRY', N'LV', N'Latvia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (113, N'COUNTRY', N'LB', N'Lebanon', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (114, N'COUNTRY', N'LS', N'Lesotho', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (115, N'COUNTRY', N'LR', N'Liberia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (116, N'COUNTRY', N'LY', N'Libyan Arab Jamahiriya', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (117, N'COUNTRY', N'LI', N'Liechtenstein', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (118, N'COUNTRY', N'LT', N'Lithuania', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (119, N'COUNTRY', N'LU', N'Luxembourg', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (120, N'COUNTRY', N'MG', N'Madagascar', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (121, N'COUNTRY', N'MW', N'Malawi', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (122, N'COUNTRY', N'MY', N'Malaysia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (123, N'COUNTRY', N'MV', N'Maldives', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (124, N'COUNTRY', N'ML', N'Mali', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (125, N'COUNTRY', N'MT', N'Malta', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (126, N'COUNTRY', N'MH', N'Marshall Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (127, N'COUNTRY', N'MQ', N'Martinique', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (128, N'COUNTRY', N'MR', N'Mauritania', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (129, N'COUNTRY', N'MU', N'Mauritius', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (130, N'COUNTRY', N'YT', N'Mayotte', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (131, N'COUNTRY', N'MX', N'Mexico', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (132, N'COUNTRY', N'MC', N'Monaco', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (133, N'COUNTRY', N'MN', N'Mongolia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (134, N'COUNTRY', N'MS', N'Montserrat', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (135, N'COUNTRY', N'MA', N'Morocco', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (136, N'COUNTRY', N'MZ', N'Mozambique', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (137, N'COUNTRY', N'MM', N'Myanmar', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (138, N'COUNTRY', N'NA', N'Namibia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (139, N'COUNTRY', N'NR', N'Nauru', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (140, N'COUNTRY', N'NP', N'Nepal', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (141, N'COUNTRY', N'NL', N'Netherlands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (142, N'COUNTRY', N'AN', N'Netherlands Antilles', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (143, N'COUNTRY', N'NC', N'New Caledonia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (144, N'COUNTRY', N'NZ', N'New Zealand', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (145, N'COUNTRY', N'NI', N'Nicaragua', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (146, N'COUNTRY', N'NE', N'Niger', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (147, N'COUNTRY', N'NG', N'Nigeria', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (148, N'COUNTRY', N'NU', N'Niue', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (149, N'COUNTRY', N'NF', N'Norfolk Island', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (150, N'COUNTRY', N'MP', N'Northern Mariana Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (151, N'COUNTRY', N'NO', N'Norway', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (152, N'COUNTRY', N'OM', N'Oman', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (153, N'COUNTRY', N'PK', N'Pakistan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (154, N'COUNTRY', N'PW', N'Palau', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (155, N'COUNTRY', N'PS', N'Palestinian Territory', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (156, N'COUNTRY', N'PA', N'Panama', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (157, N'COUNTRY', N'PG', N'Papua New Guinea', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (158, N'COUNTRY', N'PY', N'Paraguay', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (159, N'COUNTRY', N'PE', N'Peru', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (160, N'COUNTRY', N'PH', N'Philippines', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (161, N'COUNTRY', N'PN', N'Pitcairn', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (162, N'COUNTRY', N'PL', N'Poland', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (163, N'COUNTRY', N'PT', N'Portugal', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (164, N'COUNTRY', N'PR', N'Puerto Rico', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (165, N'COUNTRY', N'QA', N'Qatar', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (166, N'COUNTRY', N'RE', N'Reunion', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (167, N'COUNTRY', N'RO', N'Romania', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (168, N'COUNTRY', N'RU', N'Russian Federation', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (169, N'COUNTRY', N'RW', N'Rwanda', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (170, N'COUNTRY', N'SH', N'Saint Helena', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (171, N'COUNTRY', N'KN', N'Saint Kitts And Nevis', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (172, N'COUNTRY', N'LC', N'Saint Lucia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (173, N'COUNTRY', N'PM', N'Saint Pierre and Miquelon', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (174, N'COUNTRY', N'VC', N'Saint Vincent And The Grenadines', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (175, N'COUNTRY', N'WS', N'Samoa', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (176, N'COUNTRY', N'SM', N'San Marino', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (177, N'COUNTRY', N'ST', N'Sao Tome And Principe', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (178, N'COUNTRY', N'SA', N'Saudi Arabia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (179, N'COUNTRY', N'SN', N'Senegal', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (180, N'COUNTRY', N'SC', N'Seychelles', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (181, N'COUNTRY', N'SL', N'Sierra Leone', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (182, N'COUNTRY', N'SG', N'Singapore', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (183, N'COUNTRY', N'SI', N'Slovenia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (184, N'COUNTRY', N'SB', N'Solomon Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (185, N'COUNTRY', N'SO', N'Somalia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (186, N'COUNTRY', N'ZA', N'South Africa', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (187, N'COUNTRY', N'ES', N'Spain', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (188, N'COUNTRY', N'LK', N'Sri Lanka', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (189, N'COUNTRY', N'SD', N'Sudan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (190, N'COUNTRY', N'SR', N'Suriname', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (191, N'COUNTRY', N'SZ', N'Swaziland', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (192, N'COUNTRY', N'SE', N'Sweden', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (193, N'COUNTRY', N'CH', N'Switzerland', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (194, N'COUNTRY', N'SY', N'Syrian Arab Republic', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (195, N'COUNTRY', N'TJ', N'Tajikistan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (196, N'COUNTRY', N'TH', N'Thailand', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (197, N'COUNTRY', N'TL', N'Timor-Leste', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (198, N'COUNTRY', N'TG', N'Togo', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (199, N'COUNTRY', N'TK', N'Tokelau', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (200, N'COUNTRY', N'TO', N'Tonga', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (201, N'COUNTRY', N'TT', N'Trinidad And Tobago', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (202, N'COUNTRY', N'TN', N'Tunisia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (203, N'COUNTRY', N'TR', N'Turkey', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (204, N'COUNTRY', N'TM', N'Turkmenistan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (205, N'COUNTRY', N'TC', N'Turks And Caicos Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (206, N'COUNTRY', N'TV', N'Tuvalu', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (207, N'COUNTRY', N'UG', N'Uganda', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (208, N'COUNTRY', N'UA', N'Ukraine', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (209, N'COUNTRY', N'AE', N'United Arab Emirates', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (210, N'COUNTRY', N'GB', N'United Kingdom', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (211, N'COUNTRY', N'US', N'United States', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (212, N'COUNTRY', N'UM', N'United States Minor Outlying Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (213, N'COUNTRY', N'UY', N'Uruguay', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (214, N'COUNTRY', N'UZ', N'Uzbekistan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (215, N'COUNTRY', N'VU', N'Vanuatu', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (216, N'COUNTRY', N'VE', N'Venezuela', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (217, N'COUNTRY', N'VN', N'Viet Nam', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (218, N'COUNTRY', N'CD', N'Congo', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (219, N'COUNTRY', N'EH', N'Western Sahara', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (220, N'COUNTRY', N'YE', N'Yemen', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (221, N'COUNTRY', N'YU', N'Yugoslavia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (222, N'COUNTRY', N'ZM', N'Zambia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (223, N'COUNTRY', N'ZW', N'Zimbabwe', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (224, N'COUNTRY', N'BA', N'Bosnia and Herzegovina', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (225, N'COUNTRY', N'FM', N'Micronesia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (226, N'COUNTRY', N'HM', N'Heard And Mc Donald Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (227, N'COUNTRY', N'HR', N'Croatia (Hrvatska)', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (228, N'COUNTRY', N'IR', N'Iran (Islamic Republic Of)', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (229, N'COUNTRY', N'MO', N'Macau', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (230, N'COUNTRY', N'MD', N'Moldova', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (231, N'COUNTRY', N'MK', N'Macedonia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (232, N'COUNTRY', N'GS', N'South Georgia & South Sandwich Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (233, N'COUNTRY', N'SJ', N'Svalbard And Jan Mayen Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (234, N'COUNTRY', N'SK', N'Slovakia (Slovak Republic)', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (235, N'COUNTRY', N'TP', N'East Timor', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (236, N'COUNTRY', N'TW', N'Taiwan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (237, N'COUNTRY', N'TZ', N'Tanzania', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (238, N'COUNTRY', N'VG', N'Virgin Islands (British)', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (239, N'COUNTRY', N'VI', N'Virgin Islands (U.S.)', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (240, N'COUNTRY', N'WF', N'Wallis And Futuna Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (241, N'COUNTRY', N'ZZ', N'Other - not listed', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (242, N'STATE', N'AE', N'Armed Forces Middle East', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (243, N'STATE', N'AP', N'Armed Forces Pacific', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (244, N'STATE', N'BC', N'British Columbia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (245, N'STATE', N'CA', N'California', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (246, N'STATE', N'CO', N'Colorado', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (247, N'STATE', N'CT', N'Connecticut', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (248, N'STATE', N'DE', N'Delaware', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (249, N'STATE', N'DC', N'District of Columbia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (250, N'STATE', N'FM', N'Federated States of Micronesia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (251, N'STATE', N'FL', N'Florida', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (252, N'STATE', N'GA', N'Georgia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (253, N'STATE', N'GU', N'Guam', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (254, N'STATE', N'HI', N'Hawaii', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (255, N'STATE', N'ID', N'Idaho', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (256, N'STATE', N'IL', N'Illinois', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (257, N'STATE', N'IN', N'Indiana', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (258, N'STATE', N'IA', N'Iowa', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (259, N'STATE', N'KS', N'Kansas', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (260, N'STATE', N'KY', N'Kentucky', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (261, N'STATE', N'LA', N'Louisiana', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (262, N'STATE', N'ME', N'Maine', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (263, N'STATE', N'MB', N'Manitoba', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (264, N'STATE', N'MH', N'Marshall Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (265, N'STATE', N'MD', N'Maryland', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (266, N'STATE', N'MA', N'Massachusetts', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (267, N'STATE', N'MI', N'Michigan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (268, N'STATE', N'MN', N'Minnesota', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (269, N'STATE', N'MS', N'Mississippi', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (270, N'STATE', N'MO', N'Missouri', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (271, N'STATE', N'MT', N'Montana', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (272, N'STATE', N'NE', N'Nebraska', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (273, N'STATE', N'NB', N'New Brunswick', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (274, N'STATE', N'NV', N'Nevada', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (275, N'STATE', N'NF', N'Newfoundland', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (276, N'STATE', N'NH', N'New Hampshire', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (277, N'STATE', N'NJ', N'New Jersey', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (278, N'STATE', N'NM', N'New Mexico', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (279, N'STATE', N'NY', N'New York', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (280, N'STATE', N'NC', N'North Carolina', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (281, N'STATE', N'ND', N'North Dakota', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (282, N'STATE', N'MP', N'Northern Mariana Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (283, N'STATE', N'NT', N'Northwest Territories', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (284, N'STATE', N'NS', N'Nova Scotia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (285, N'STATE', N'OH', N'Ohio', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (286, N'STATE', N'OK', N'Oklahoma', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (287, N'STATE', N'ON', N'Ontario', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (288, N'STATE', N'OR', N'Oregon', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (289, N'STATE', N'PW', N'Palau', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (290, N'STATE', N'PA', N'Pennsylvania', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (291, N'STATE', N'PE', N'Prince Edward Island', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (292, N'STATE', N'PR', N'Puerto Rico', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (293, N'STATE', N'QC', N'Quebec', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (294, N'STATE', N'RI', N'Rhode Island', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (295, N'STATE', N'SK', N'Saskatchewan', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (296, N'STATE', N'SC', N'South Carolina', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (297, N'STATE', N'SD', N'South Dakota', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (298, N'STATE', N'TN', N'Tennessee', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (299, N'STATE', N'TX', N'Texas', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (300, N'STATE', N'UT', N'Utah', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (301, N'STATE', N'VT', N'Vermont', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (302, N'STATE', N'VI', N'Virgin Islands', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (303, N'STATE', N'VA', N'Virginia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (304, N'STATE', N'WA', N'Washington', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (305, N'STATE', N'WV', N'West Virginia', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (306, N'STATE', N'WI', N'Wisconsin', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (307, N'STATE', N'WY', N'Wyoming', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (308, N'STATE', N'YT', N'Yukon', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (309, N'STATE', N'AL', N'Alabama', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (310, N'STATE', N'AK', N'Alaska', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (311, N'STATE', N'AB', N'Alberta', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (312, N'STATE', N'AS', N'American Samoa', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (313, N'STATE', N'AZ', N'Arizona', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (314, N'STATE', N'AR', N'Arkansas', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (315, N'STATE', N'AE', N'Armed Forces Africa', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (316, N'STATE', N'AA', N'Armed Forces Americas', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (317, N'STATE', N'AE', N'Armed Forces Canada', N'', 0)
INSERT INTO [dbo].[Lookups] ([pk], [type], [cdata], [cdata1], [cdata2], [idata]) VALUES (318, N'STATE', N'AE', N'Armed Forces Europe', N'', 0)
SET IDENTITY_INSERT [dbo].[Lookups] OFF

-- Add 14 rows to [dbo].[Projects]
SET IDENTITY_INSERT [dbo].[Projects] ON
INSERT INTO [dbo].[Projects] ([Pk], [CustomerPk], [ProjectName], [Entered], [StartDate], [EndDate], [Status]) VALUES (16, 4, N'SummaLPHttp Manager', '2007-09-07 01:11:40.793', '2007-03-01 00:00:00.000', '2007-06-22 00:00:00.000', 2)
INSERT INTO [dbo].[Projects] ([Pk], [CustomerPk], [ProjectName], [Entered], [StartDate], [EndDate], [Status]) VALUES (17, 4, N'Ofx Parsing', '2007-09-07 01:11:41.680', '2007-09-07 04:02:24.023', '1900-01-01 00:00:00.000', 0)
INSERT INTO [dbo].[Projects] ([Pk], [CustomerPk], [ProjectName], [Entered], [StartDate], [EndDate], [Status]) VALUES (18, 15, N'Omni Web Service', '2007-09-07 01:11:42.400', '2007-09-07 04:02:24.023', '1900-01-01 00:00:00.000', 1)
INSERT INTO [dbo].[Projects] ([Pk], [CustomerPk], [ProjectName], [Entered], [StartDate], [EndDate], [Status]) VALUES (34, 1, N'West Wind Web Connection', '2007-09-01 00:00:00.000', '2007-09-01 00:00:00.000', '1900-01-01 00:00:00.000', 0)
INSERT INTO [dbo].[Projects] ([Pk], [CustomerPk], [ProjectName], [Entered], [StartDate], [EndDate], [Status]) VALUES (35, 1, N'West Wind Html Help Builder', '2007-10-02 11:43:57.097', '2007-09-01 00:00:00.000', '2008-03-04 00:00:00.000', 1)
INSERT INTO [dbo].[Projects] ([Pk], [CustomerPk], [ProjectName], [Entered], [StartDate], [EndDate], [Status]) VALUES (36, 1, N'West Wind Web Store', '2007-09-01 00:00:00.000', '2007-09-01 00:00:00.000', '1900-01-01 00:00:00.000', 0)
INSERT INTO [dbo].[Projects] ([Pk], [CustomerPk], [ProjectName], [Entered], [StartDate], [EndDate], [Status]) VALUES (37, 3, N'Mere Mortals Framework', '2007-09-01 00:00:00.000', '2007-09-01 00:00:00.000', '1900-01-01 00:00:00.000', 0)
INSERT INTO [dbo].[Projects] ([Pk], [CustomerPk], [ProjectName], [Entered], [StartDate], [EndDate], [Status]) VALUES (38, 1, N'Conference Preparations', '2007-08-01 00:00:00.000', '2007-09-25 00:00:00.000', '2007-11-10 00:00:00.000', 1)
INSERT INTO [dbo].[Projects] ([Pk], [CustomerPk], [ProjectName], [Entered], [StartDate], [EndDate], [Status]) VALUES (39, 16, N'Mentoring and Phone Consultations', '2007-09-01 00:00:00.000', '2007-09-01 00:00:00.000', '1900-01-01 00:00:00.000', 1)
INSERT INTO [dbo].[Projects] ([Pk], [CustomerPk], [ProjectName], [Entered], [StartDate], [EndDate], [Status]) VALUES (46, 23, N'POP3 Messaging Server', '2007-07-01 00:00:00.000', '2007-07-02 00:00:00.000', '2007-08-25 00:00:00.000', 1)
INSERT INTO [dbo].[Projects] ([Pk], [CustomerPk], [ProjectName], [Entered], [StartDate], [EndDate], [Status]) VALUES (49, 24, N'Web Service Integration', '2007-01-01 00:00:00.000', '2007-12-06 00:00:00.000', '2007-12-20 00:00:00.000', 2)
INSERT INTO [dbo].[Projects] ([Pk], [CustomerPk], [ProjectName], [Entered], [StartDate], [EndDate], [Status]) VALUES (50, 25, N'TradeOne Integration', '2008-01-24 12:20:03.660', '2008-01-24 00:00:00.000', '2008-01-24 00:00:00.000', 0)
INSERT INTO [dbo].[Projects] ([Pk], [CustomerPk], [ProjectName], [Entered], [StartDate], [EndDate], [Status]) VALUES (52, 26, N'Web Service XML Signature', NULL, '2008-01-29 00:00:00.000', '2008-03-30 00:00:00.000', 2)
INSERT INTO [dbo].[Projects] ([Pk], [CustomerPk], [ProjectName], [Entered], [StartDate], [EndDate], [Status]) VALUES (53, 4, N'PRA Data Service', NULL, '2008-03-26 00:00:00.000', '2008-04-23 00:00:00.000', 1)
SET IDENTITY_INSERT [dbo].[Projects] OFF

-- Add 2 rows to [dbo].[Users]
INSERT INTO [dbo].[Users] ([Pk], [UserId], [Password], [UserName], [UserCompany], [LastCustomer], [LastProject]) VALUES (1, N'rstrahl@west-wind.com', N'ww', N'Rick Strahl', N'West Wind Technologies', 26, 38)
INSERT INTO [dbo].[Users] ([Pk], [UserId], [Password], [UserName], [UserCompany], [LastCustomer], [LastProject]) VALUES (2, N'jdoe@doe.com', N'doe', N'John Doe', N'Doe Boy Inc.', 25, 50)

-- Add constraints to [dbo].[Projects]
ALTER TABLE [dbo].[Projects] ADD CONSTRAINT [FK_Projects_Customers] FOREIGN KEY ([CustomerPk]) REFERENCES [dbo].[Customers] ([Pk])

-- Add constraints to [dbo].[Entries]
ALTER TABLE [dbo].[Entries] WITH NOCHECK ADD CONSTRAINT [FK_Entries_Invoices] FOREIGN KEY ([InvoicePk]) REFERENCES [dbo].[Invoices] ([Pk])
ALTER TABLE [dbo].[Entries] NOCHECK CONSTRAINT [FK_Entries_Invoices]
ALTER TABLE [dbo].[Entries] WITH NOCHECK ADD CONSTRAINT [FK_tt_entries_tt_Projects] FOREIGN KEY ([ProjectPk]) REFERENCES [dbo].[Projects] ([Pk]) ON DELETE CASCADE
ALTER TABLE [dbo].[Entries] NOCHECK CONSTRAINT [FK_tt_entries_tt_Projects]
ALTER TABLE [dbo].[Entries] WITH NOCHECK ADD CONSTRAINT [FK_tt_entries_tt_users] FOREIGN KEY ([UserPk]) REFERENCES [dbo].[Users] ([Pk])
ALTER TABLE [dbo].[Entries] NOCHECK CONSTRAINT [FK_tt_entries_tt_users]

COMMIT TRANSACTION
GO
