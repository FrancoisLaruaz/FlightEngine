INSERT INTO [dbo].[Provider]
           ([Name]
           ,[ImageSrc]
           ,[HasAPI]
           ,[IsSearchEngine]
           ,[Active])
     VALUES
           ('Transavia'
           ,'https://cms-media.lyonaeroports.com/media/cache/image_company_logo_squared/1439488401/file.jpg'
           ,1
           ,0
           ,1)
GO


alter table [dbo].[Provider]
add NeedFixedDates bit not null default 0
go
update dbo.Provider
set NeedFixedDates=0
update dbo.Provider
set NeedFixedDates=1
where id in (1,2)

INSERT INTO [dbo].[Provider]
           ([Name]
           ,[ImageSrc]
           ,[HasAPI]
           ,[IsSearchEngine]
           ,[Active])
     VALUES
           ('Turkish Airlines'
           ,null
           ,1
           ,0
           ,1)
		   
INSERT INTO [dbo].[Provider]
           ([Name]
           ,[ImageSrc]
           ,[HasAPI]
           ,[IsSearchEngine]
           ,[Active])
     VALUES
           ('Ryan Air'
           ,null
           ,1
           ,0
           ,1)
GO		   
		   
GO

USE [Template]
GO
/****** Object:  Trigger [dbo].[SearchTripWishesInsert]    Script Date: 2018-11-28 6:26:16 PM ******/
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
				set @NewMaxStopNumber=1

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
					set @NewMaxStopNumber=2
				end
			end

			update [dbo].[SearchTripWishes]
			set Distance=@Distance,MaxStopNumber=@NewMaxStopNumber,DurationMin=@DurationMin, DurationMax=@DurationMax
			where id=@SearchTripWishesdId


			if @CreationUserId is not null or (select count(*) from dbo.AirportsTripProvider atp
												inner join dbo.AirportsTrip at on atp.AirportsTripId=at.Id
												inner join dbo.Provider p on p.id=atp.ProviderId
												where p.Active=1 and NeedFixedDates=1
												and ((at.FromAirportId=@FromAirportId and at.ToAirportId=@ToAirportId)  or (at.FromAirportId=@ToAirportId and at.ToAirportId=@FromAirportId) )
												)>0
			begin
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
go
INSERT INTO [dbo].[Provider]
           ([Name]
           ,[ImageSrc]
           ,[HasAPI]
           ,[IsSearchEngine]
           ,[Active])
     VALUES
           ('British Airways'
           ,null
           ,1
           ,0
           ,1)
GO		   