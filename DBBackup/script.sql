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