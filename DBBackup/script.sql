update dbo.Airline
set code='AC'
where id=13

update dbo.Airline
set code='DL'
where id=14

update dbo.Airline
set code='AC'
where id=13

update dbo.Airline
set code='DL'
where id=14



update dbo.Airline
set code='AA'
where id=15



update dbo.Airline
set code='Alaska'
where id=15

delete from dbo.Airline
where id in (93,94,9)


if not exists (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='EmailAudit' and COLUMN_NAME='GuidId')
BEGIN
	Alter table EmailAudit
	add GuidId varchar(256) not null default NEWID()
END
Go


if not exists (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='EmailAudit' and COLUMN_NAME='EmailSent')
BEGIN
	Alter table EmailAudit
	add EmailSent  bit not null default 0
END
Go

update EmailAudit set EmailSent=1


USE [Template]
GO

INSERT INTO [dbo].[CategoryType]
           ([Id]
           ,[Name])
     VALUES
           (10
           ,'Email Watcher Status')
GO



INSERT INTO [dbo].[Category]
           ([Id]
           ,[Name]
           ,[Active]
           ,[CategoryTypeId]
           ,[DateModification])
     VALUES
           (10001
           ,'Not Watched'
           ,1
           ,10
           ,GETUTCDATE())
GO

INSERT INTO [dbo].[Category]
           ([Id]
           ,[Name]
           ,[Active]
           ,[CategoryTypeId]
           ,[DateModification])
     VALUES
           (10002
           ,'Email Not Opened'
           ,1
           ,10
           ,GETUTCDATE())
GO



INSERT INTO [dbo].[Category]
           ([Id]
           ,[Name]
           ,[Active]
           ,[CategoryTypeId]
           ,[DateModification])
     VALUES
           (10003
           ,'Link Clicked'
           ,1
           ,10
           ,GETUTCDATE())
GO



INSERT INTO [dbo].[Category]
           ([Id]
           ,[Name]
           ,[Active]
           ,[CategoryTypeId]
           ,[DateModification])
     VALUES
           (10004
           ,'Email Opened'
           ,1
           ,10
           ,GETUTCDATE())
GO


if not exists (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='EmailAudit' and COLUMN_NAME='EmailWatcherStatusId')
BEGIN
	Alter table EmailAudit
	add EmailWatcherStatusId int not null default 10001  CONSTRAINT [FK_EmailAudit_EmailWatcherStatusId] 
		REFERENCES  dbo.Category ([Id])
END
Go


if not exists (SELECT * 
FROM sys.indexes 
WHERE name='UniqueIndex_EmailAudit_Guid' AND object_id = OBJECT_ID('dbo.EmailAudit'))
begin
	CREATE UNIQUE INDEX UniqueIndex_EmailAudit_Guid ON dbo.EmailAudit (GuidId DESC);  

end

if not exists (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='EmailAudit' and COLUMN_NAME='EmailOpenedDate')
BEGIN
	Alter table EmailAudit
	add EmailOpenedDate DateTime null
END
Go


if not exists (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='EmailAudit' and COLUMN_NAME='EmailLinkClickedDate')
BEGIN
	Alter table EmailAudit
	add EmailLinkClickedDate DateTime null
END
Go