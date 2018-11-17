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

alter table [dbo].[Airport]
alter column Longitude float(18)


alter table [dbo].[Airport]
alter column Latitude float(18)



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

if not exists (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='Flight' and COLUMN_NAME='StopInformation')
BEGIN
	Alter table Flight
	add StopInformation varchar(1000) null
END
Go

alter table [dbo].[SearchTripProvider]
alter column Url nvarchar(4000) null

alter table dbo.trip
alter column Url nvarchar(4000) null




alter table dbo.SearchTripWishes
add Active bit not null default 1



	
	
	Create Function [dbo].[GetDistanceKM] 
( 
      @Lat1 Float(18),  
      @Lat2 Float(18), 
      @Long1 Float(18), 
      @Long2 Float(18)
)
Returns Float(18)
AS
Begin
      Declare @R Float(8); 
      Declare @dLat Float(18); 
      Declare @dLon Float(18); 
      Declare @a Float(18); 
      Declare @c Float(18); 
      Declare @d Float(18);

	  if @Lat1!=0 and @Lat2!=0 and @Long1!=0 and @Long2!=0
	  begin
		  Set @R =  6367.45
				--Miles 3956.55  
				--Kilometers 6367.45 
				--Feet 20890584 
				--Meters 6367450 


		  Set @dLat = Radians(@lat2 - @lat1);
		  Set @dLon = Radians(@long2 - @long1);
		  Set @a = Sin(@dLat / 2)  
					 * Sin(@dLat / 2)  
					 + Cos(Radians(@lat1)) 
					 * Cos(Radians(@lat2))  
					 * Sin(@dLon / 2)  
					 * Sin(@dLon / 2); 
		  Set @c = 2 * Asin(Min(Sqrt(@a))); 

		  Set @d = @R * @c;
	  end		
      Return @d; 

End
GO



alter table [dbo].[SearchTripWishes]
add Distance float(18) null

alter table [dbo].[Trip]
add Attractiveness decimal(25,12) not null default 0



USE [Template]
GO
/****** Object:  Trigger [dbo].[SearchTripWishesInsert]    Script Date: 2018-11-15 7:21:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[SearchTripWishesInsert] ON  [dbo].[SearchTripWishes]
FOR INSERT
AS  
	declare @FromDateMin datetime,
			@FromDateMax datetime,
			@ToDateMin datetime,
			@ToDateMax datetime,
			@Error_message varchar(max),
			@Exception varchar(max),
			@DurationMin int,
			@DurationMax int,
			@FromDate datetime,
			@ToDate datetime,
			@Duration int,
			@FromAirportId int,
			@ToAirportId int,
			@FromCityId int,
			@ToCityId int,
			@MaxStopNumber int,
			@SearchTripWishesdId int,
			@LatFrom Float(18),
			@LongFrom Float(18),
			@LatTo Float(18),
			@LongTo Float(18),
			@Distance Float(18)
	begin try
		print 'Start SearchTripWishesInsert'

		DECLARE Wishes_cursor CURSOR LOCAL FOR   
		select Id, ToAirportId,	FromAirportId,  FromCityId, ToCityId,  MaxStopNumber, FromDateMin,FromDateMax, ToDateMin, ToDateMax, DurationMin, DurationMax  from inserted i
		OPEN Wishes_cursor
		FETCH NEXT FROM Wishes_cursor   
		INTO @SearchTripWishesdId,@ToAirportId,@FromAirportId,@FromCityId,@ToCityId,@MaxStopNumber,@FromDateMin,@FromDateMax,@ToDateMin,@ToDateMax,@DurationMin,@DurationMax

		WHILE @@FETCH_STATUS = 0  -- meaning successful
			BEGIN
	
			select top 1  @LongTo=Longitude, @LatTo=Latitude from dbo.Airport where id=@ToAirportId
			select top 1 @LongFrom=Longitude, @LatFrom=Latitude from dbo.Airport where id=@FromAirportId

			set @Distance=[dbo].[GetDistanceKM] (@LatFrom,@LatTo,@LongFrom,@LongTo)

			update [dbo].[SearchTripWishes]
			set Distance=@Distance
			where id=@SearchTripWishesdId

			set @FromDate=@FromDateMin
			set @ToDate=@ToDateMin

			WHILE @FromDate<=@FromDateMax
			BEGIN
				set @ToDate=@ToDateMin
				--print '*** @FromDate = '+cast(@FromDate as varchar(50))
				WHILE @ToDate<=@ToDateMax
				BEGIN
					set @Duration= DATEDIFF(dd, @FromDate, @ToDate)
					--print '** @ToDate = '+cast(@ToDate as varchar(50))+' and duration = '+cast(@Duration as varchar(50))
					if @Duration<=@DurationMax
					begin
						if  @Duration>=@DurationMin
						begin
							INSERT INTO [dbo].[SearchTrip]
							   (
							   [FromDate]
							   ,[ToDate]
							   ,[SearchTripWishesId])
						 VALUES
							   (
							   @FromDate
							   ,@ToDate 
							   ,@SearchTripWishesdId)
						end
					end

					set @ToDate=DATEADD (day , 1 , @ToDate )
				END
				set @FromDate=DATEADD (day , 1 , @FromDate )  
			END

			FETCH NEXT FROM Wishes_cursor   
			INTO @SearchTripWishesdId,@ToAirportId,@FromAirportId,@FromCityId,@ToCityId,@MaxStopNumber,@FromDateMin,@FromDateMax,@ToDateMin,@ToDateMax,@DurationMin,@DurationMax
		END   
        CLOSE Wishes_cursor;
        DEALLOCATE Wishes_cursor;

		print 'End SearchTripWishesInsert'
    end try
    begin catch
			set @Error_message='Error in [dbo].[SearchTripWishesInsert] stored procedure.'
			set @Exception=ERROR_MESSAGE()
			execute [dbo].[InsertLog] @Error_message,@Exception
		    print 'FAILURE : rollback => '+ERROR_MESSAGE()
    end catch

	
	
ALTER TRIGGER [dbo].[SearchTripWishesInsert] ON  [dbo].[SearchTripWishes]
FOR INSERT
AS  
	declare @FromDateMin datetime,
			@FromDateMax datetime,
			@ToDateMin datetime,
			@ToDateMax datetime,
			@Error_message varchar(max),
			@Exception varchar(max),
			@DurationMin int,
			@DurationMax int,
			@FromDate datetime,
			@ToDate datetime,
			@Duration int,
			@FromAirportId int,
			@ToAirportId int,
			@FromCityId int,
			@ToCityId int,
			@MaxStopNumber int,
			@SearchTripWishesdId int,
			@LatFrom Float(18),
			@LongFrom Float(18),
			@LatTo Float(18),
			@LongTo Float(18),
			@Distance Float(18),
			@NewMaxStopNumber integer,
			@CreationUserId integer
	begin try
		print 'Start SearchTripWishesInsert'

		DECLARE Wishes_cursor CURSOR LOCAL FOR   
		select Id, ToAirportId,	FromAirportId,  FromCityId, ToCityId,  MaxStopNumber, FromDateMin,FromDateMax, ToDateMin, ToDateMax, DurationMin, DurationMax,CreationUserId  from inserted i
		OPEN Wishes_cursor
		FETCH NEXT FROM Wishes_cursor   
		INTO @SearchTripWishesdId,@ToAirportId,@FromAirportId,@FromCityId,@ToCityId,@MaxStopNumber,@FromDateMin,@FromDateMax,@ToDateMin,@ToDateMax,@DurationMin,@DurationMax,@CreationUserId

		WHILE @@FETCH_STATUS = 0  -- meaning successful
			BEGIN
	
			select top 1  @LongTo=Longitude, @LatTo=Latitude from dbo.Airport where id=@ToAirportId
			select top 1 @LongFrom=Longitude, @LatFrom=Latitude from dbo.Airport where id=@FromAirportId

			set @Distance=[dbo].[GetDistanceKM] (@LatFrom,@LatTo,@LongFrom,@LongTo)
			set @NewMaxStopNumber=@MaxStopNumber

			if @CreationUserId is null 
			begin
				if @Distance<=2500
				begin
					set @NewMaxStopNumber=0
                END

				IF @Distance<=1000
				begin
					set @DurationMin=2
					set @DurationMax=7
				end
				else if @Distance>1000 and @Distance<2500
				begin
					set @DurationMin=4
					set @DurationMax=10
				end
				else if @Distance>2500 and @Distance<5000
				begin
					set @DurationMin=5
					set @DurationMax=14
				end
				else if @Distance>5000 and @Distance<7500
				begin
					set @DurationMin=7
					set @DurationMax=16
				end
				else if @Distance>7500
				begin
					set @DurationMin=9
					set @DurationMax=16
				end
			end

			update [dbo].[SearchTripWishes]
			set Distance=@Distance,MaxStopNumber=@NewMaxStopNumber,DurationMin=@DurationMin, DurationMax=@DurationMax
			where id=@SearchTripWishesdId



			set @FromDate=@FromDateMin
			set @ToDate=@ToDateMin

			WHILE @FromDate<=@FromDateMax
			BEGIN
				set @ToDate=@ToDateMin
				--print '*** @FromDate = '+cast(@FromDate as varchar(50))
				WHILE @ToDate<=@ToDateMax
				BEGIN
					set @Duration= DATEDIFF(dd, @FromDate, @ToDate)
					--print '** @ToDate = '+cast(@ToDate as varchar(50))+' and duration = '+cast(@Duration as varchar(50))
					if @Duration<=@DurationMax
					begin
						if  @Duration>=@DurationMin
						begin
							INSERT INTO [dbo].[SearchTrip]
							   (
							   [FromDate]
							   ,[ToDate]
							   ,[SearchTripWishesId])
						 VALUES
							   (
							   @FromDate
							   ,@ToDate 
							   ,@SearchTripWishesdId)
						end
					end

					set @ToDate=DATEADD (day , 1 , @ToDate )
				END
				set @FromDate=DATEADD (day , 1 , @FromDate )  
			END

			FETCH NEXT FROM Wishes_cursor   
			INTO @SearchTripWishesdId,@ToAirportId,@FromAirportId,@FromCityId,@ToCityId,@MaxStopNumber,@FromDateMin,@FromDateMax,@ToDateMin,@ToDateMax,@DurationMin,@DurationMax,@CreationUserId
		END   
        CLOSE Wishes_cursor;
        DEALLOCATE Wishes_cursor;

		print 'End SearchTripWishesInsert'
    end try
    begin catch
			set @Error_message='Error in [dbo].[SearchTripWishesInsert] stored procedure.'
			set @Exception=ERROR_MESSAGE()
			execute [dbo].[InsertLog] @Error_message,@Exception
		    print 'FAILURE : rollback => '+ERROR_MESSAGE()
    end catch


create  procedure  [dbo].[SetTripAttractiveness]
        @TripId integer
        AS
        BEGIN

	declare @Price decimal(25,2),
			@Distance float(18),
			@Attractiveness decimal(25,12)


			select @Distance=Distance,@Price=Price from dbo.Trip t 
				inner join dbo.SearchTripProvider stp on stp.Id=t.SearchTripProviderId
				inner join dbo.SearchTrip st on st.id=stp.SearchTripId
				inner join dbo.SearchTripWishes stw on stw.id=st.SearchTripWishesId
				where  t.id=@TripId
		
			if @Price>0 and @Distance is not null and @Distance>0
			begin
				set @Attractiveness=@Distance/@Price
				update dbo.Trip
				set Attractiveness=@Attractiveness
				where id=@TripId
			end


	end


