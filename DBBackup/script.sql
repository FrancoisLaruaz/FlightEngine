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


USE [Template]
GO
/****** Object:  StoredProcedure [dbo].[InsertTripWithTransaction]    Script Date: 2018-10-18 11:30:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[InsertTripWithTransaction]
        @SearchTripProviderId int,
		@Price decimal (25,2),
		@CurrencyCode varchar(50)='',
		@Url varchar(500),
		@OneWayTrip_FromAirportCode varchar(5),
		@OneWayTrip_ToAirportCode varchar(5),
		@OneWayTrip_DepartureDate varchar(30),
		@OneWayTrip_ArrivalDate varchar(30),
		@OneWayTrip_Duration int,
		@OneWayTrip_StopInformation  varchar(500)=null,
		@OneWayTrip_FlightNumber  varchar(500)=null,
		@OneWayTrip_AirlineName varchar(100),
		@OneWayTrip_AirlineLogoSrc varchar(500)=null,
		@OneWayTrip_Stops integer = 0,
		@ReturnTrip_FromAirportCode varchar(5) = null,
		@ReturnTrip_ToAirportCode varchar(5) =null,
		@ReturnTrip_DepartureDate varchar(30)=null,
		@ReturnTrip_ArrivalDate varchar(30)=null,
		@ReturnTrip_Duration int =null ,
		@ReturnTrip_AirlineName varchar(100)=null,
		@ReturnTrip_AirlineLogoSrc varchar(500) =null,
		@ReturnTrip_Stops integer =null,
		@ReturnTrip_StopInformation  varchar(500)=null,
		@ReturnTrip_FlightNumber  varchar(500)=null
        AS
        BEGIN
    SET NOCOUNT ON;  
	SET XACT_ABORT ON; --> the only change
    declare @trancount int,
			@CurrencyId int,
			@OneWayTrip_FromAirportId int,
			@OneWayTrip_ToAirportId int,
			@ReturnTrip_ToAirportId int,
			@ReturnTrip_FromAirportId int,
			@StrParameters varchar(8000),
			@Error_message varchar(max),
			@Exception varchar(max),
			@EuroPrice decimal (25,2),
			@EuroConversationRate decimal (25,2),
			@TripId int,
			@OneWayTrip_AirlineId int,
			@ReturnTrip_AirlineId int

	-- execute [dbo].[InsertTripWithTransaction] '1','974.99','€','https://www.edreams.com/travel/#results/type=R;dep=2018-10-10;from=RNS;to=NRT;ret=2018-10-18;collectionmethod=false;airlinescodes=false;internalSearch=true','RNS','NRT','10/10/2018 10:00','11/10/2018 08:40','940','Klm Royal Dutch Airlines','https://www.edreams.com/images/onefront/airlines/smKL.gif','1','NRT','RNS','18/10/2018 10:30','18/10/2018 17:55','865','Klm Royal Dutch Airlines','https://www.edreams.com/images/onefront/airlines/smKL.gif','1'

    set @trancount = @@trancount;
    begin try
	print '@trancount : '+cast(@trancount as varchar(20))
			BEGIN TRAN   
			set @StrParameters='@SearchTripProviderId = '+cast(@SearchTripProviderId as varchar(256))+' and @currencyCode = '+@CurrencyCode+' and @price = '+cast(@Price as varchar(250))+' and @url = '+@Url+' and @trancount = '+cast(@trancount as varchar(250)) 
			
			-- CURRENCY
			(select top 1 @CurrencyId=id, @EuroConversationRate=EuroConversationRate from dbo.Currency where ltrim(rtrim(UPPER(Code)))=@CurrencyCode or (Symbol is not null and ltrim(rtrim(UPPER(Symbol)))=@CurrencyCode))
			if @CurrencyId is null
				begin
					set @CurrencyId=1
					set @Exception='Currency not found : '+@CurrencyCode
					execute [dbo].[InsertInfo] @StrParameters,@Exception
				end
			print '@CurrencyId = '+cast(@CurrencyId as varchar(10))

			-- FROM AIRPORT ONE WAY
			set @OneWayTrip_FromAirportId=(select top 1 id from dbo.Airport where ltrim(rtrim(UPPER(Code)))=@OneWayTrip_FromAirportCode )
			if @OneWayTrip_FromAirportId is null
				begin
					set @OneWayTrip_FromAirportId=0
					set @Exception='@OneWayTrip_FromAirportId not found : '+@OneWayTrip_FromAirportCode
					execute [dbo].[InsertInfo] @StrParameters,@Exception
				end
			print '@OneWayTrip_FromAirportId = '+cast(@OneWayTrip_FromAirportId as varchar(10))

			-- TO AIRPORT ONE WAY
			set @OneWayTrip_ToAirportId=(select top 1 id from dbo.Airport where ltrim(rtrim(UPPER(Code)))=@OneWayTrip_ToAirportCode )
			if @OneWayTrip_ToAirportId is null
				begin
					set @OneWayTrip_ToAirportId=0
					set @Exception='@OneWayTrip_ToAirportId not found : '+@OneWayTrip_ToAirportCode
					execute [dbo].[InsertInfo] @StrParameters,@Exception
				end
			print '@OneWayTrip_ToAirportId = '+cast(@OneWayTrip_ToAirportId as varchar(10))

		   -- AIRLINE ONE WAY
			set @OneWayTrip_AirlineId=(select top 1 id from dbo.Airline where ltrim(rtrim(UPPER(Code)))=@OneWayTrip_AirlineName 
			or ltrim(rtrim(UPPER(Name)))=@OneWayTrip_AirlineName 
			or replace(replace(replace(replace(ltrim(rtrim(UPPER(Name))),'airline',''),'airlines',''),'air line',''),'air lines','')=replace(replace(replace(replace(@OneWayTrip_AirlineName,'airline',''),'airlines',''),'air line',''),'air lines',''))
			if @OneWayTrip_AirlineId is null
				begin
						INSERT INTO [dbo].[Airline]
							([Name]
							,[Code]
							,[ImageSrc])
						VALUES
							 (ltrim(rtrim(@OneWayTrip_AirlineName))
							 ,ltrim(rtrim(@OneWayTrip_AirlineName))
							 ,@OneWayTrip_AirlineLogoSrc)
						set @OneWayTrip_AirlineId=@@IDENTITY
				end
			print '@OneWayTrip_AirlineId = '+cast(@OneWayTrip_AirlineId as varchar(10))

			if @ReturnTrip_FromAirportCode is not null and @ReturnTrip_ToAirportCode is not null begin
				-- FROM AIRPORT RETURN WAY
				if @OneWayTrip_FromAirportCode=@ReturnTrip_FromAirportCode 
				begin
					set @ReturnTrip_FromAirportId=@OneWayTrip_FromAirportId
				end
				else
					begin
						set @ReturnTrip_FromAirportId=(select top 1 id from dbo.Airport where ltrim(rtrim(UPPER(Code)))=@ReturnTrip_FromAirportCode )
						if @ReturnTrip_FromAirportId is null
							begin
								set @ReturnTrip_FromAirportId=0
								set @Exception='@ReturnTrip_FromAirportId not found : '+@ReturnTrip_FromAirportCode
								execute [dbo].[InsertInfo] @StrParameters,@Exception
							end
					end
					print '@ReturnTrip_FromAirportId = '+cast(@ReturnTrip_FromAirportId as varchar(10))

				-- TO AIRPORT RETURN WAY
				if @OneWayTrip_FromAirportCode=@ReturnTrip_ToAirportCode 
				begin
					set @ReturnTrip_ToAirportId=@OneWayTrip_FromAirportId
				end
				else
					begin
						set @ReturnTrip_ToAirportId=(select top 1 id from dbo.Airport where ltrim(rtrim(UPPER(Code)))=@ReturnTrip_ToAirportCode )
						if @ReturnTrip_ToAirportId is null
							begin
								set @ReturnTrip_ToAirportId=0
								set @Exception='@ReturnTrip_ToAirportId not found : '+@ReturnTrip_ToAirportCode
								execute [dbo].[InsertInfo] @StrParameters,@Exception
							end
					end
				print '@ReturnTrip_ToAirportId = '+cast(@ReturnTrip_ToAirportId as varchar(10))


				-- Airlines RETURN WAY
				if @OneWayTrip_AirlineName=@ReturnTrip_AirlineName
				begin
					set @ReturnTrip_AirlineId=@OneWayTrip_AirlineId
				end
				else
					begin
						set @ReturnTrip_AirlineId=(select top 1 id from dbo.Airline where ltrim(rtrim(UPPER(Code)))=@ReturnTrip_AirlineName or ltrim(rtrim(UPPER(Name)))=@ReturnTrip_AirlineName )
						if @ReturnTrip_AirlineId is null
							begin
								INSERT INTO [dbo].[Airline]
										   ([Name]
										   ,[Code]
										   ,[ImageSrc])
									 VALUES
										   (@ReturnTrip_AirlineName
										   ,@ReturnTrip_AirlineName
										   ,@ReturnTrip_AirlineLogoSrc)
								set @ReturnTrip_AirlineId=@@IDENTITY
							end
					end
				print '@ReturnTrip_AirlineId = '+cast(@ReturnTrip_AirlineId as varchar(10))
				
			end 

			-- GET THE PRICE IN EURO
			if @CurrencyId<>1 and @EuroConversationRate is not null
			begin
				set @EuroPrice=@Price*@EuroConversationRate
			end

			if ((@ReturnTrip_ToAirportId>0 and @ReturnTrip_FromAirportId>0) or (@ReturnTrip_FromAirportCode is null and @ReturnTrip_ToAirportCode is null)    ) and @OneWayTrip_AirlineId>0 and @OneWayTrip_FromAirportId>0 and @OneWayTrip_ToAirportId>0
			begin

				INSERT INTO [dbo].[Trip]
				   ([CurrencyId]
				   ,[Price]
				   ,[Url]
				   ,[SearchTripProviderId]
				   ,[EuroPrice])
				 VALUES
					   (@CurrencyId
					   ,@Price
					   ,@Url
					   ,@SearchTripProviderId
					   ,@EuroPrice)

				set @TripId=@@IDENTITY

				INSERT INTO [dbo].[Flight]
				   ([FlightNumber]
				   ,[FromAirportId]
				   ,[ToAirportId]
				   ,[DepartureDate]
				   ,[ArrivalDate]
				   ,[AirlineId]
				   ,[StopNumber]
				   ,[TripId]
				   ,[Duration]
				   ,[FlightTypeId]
				   ,[StopInformation])
				VALUES
				   (@OneWayTrip_FlightNumber
				   ,@OneWayTrip_FromAirportId
				   ,@OneWayTrip_ToAirportId
				   ,substring(@OneWayTrip_DepartureDate,7,4)+'-'+substring(@OneWayTrip_DepartureDate,4,2)+'-'+substring(@OneWayTrip_DepartureDate,1,2)+' '+substring(@OneWayTrip_DepartureDate,12,6)+':00.000'
				   ,substring(@OneWayTrip_ArrivalDate,7,4)+'-'+substring(@OneWayTrip_ArrivalDate,4,2)+'-'+substring(@OneWayTrip_ArrivalDate,1,2)+' '+substring(@OneWayTrip_ArrivalDate,12,6)+':00.000'
				   ,@OneWayTrip_AirlineId
				   ,@OneWayTrip_Stops
				   ,@TripId
				   ,@OneWayTrip_Duration
				   ,9001
				   , @OneWayTrip_StopInformation)

				if @ReturnTrip_FromAirportCode is not null and @ReturnTrip_ToAirportCode is not null
				begin
					INSERT INTO [dbo].[Flight]
					   ([FlightNumber]
					   ,[FromAirportId]
					   ,[ToAirportId]
					   ,[DepartureDate]
					   ,[ArrivalDate]
					   ,[AirlineId]
					   ,[StopNumber]
					   ,[TripId]
					   ,[Duration]
					   ,[FlightTypeId]
					   ,[StopInformation])
					VALUES
					   (@ReturnTrip_FlightNumber
					   ,@ReturnTrip_FromAirportId
					   ,@ReturnTrip_ToAirportId
					   ,substring(@ReturnTrip_DepartureDate,7,4)+'-'+substring(@ReturnTrip_DepartureDate,4,2)+'-'+substring(@ReturnTrip_DepartureDate,1,2)+' '+substring(@ReturnTrip_DepartureDate,12,6)+':00.000'
					   ,substring(@ReturnTrip_ArrivalDate,7,4)+'-'+substring(@ReturnTrip_ArrivalDate,4,2)+'-'+substring(@ReturnTrip_ArrivalDate,1,2)+' '+substring(@ReturnTrip_ArrivalDate,12,6)+':00.000'
					   ,@ReturnTrip_AirlineId
					   ,@ReturnTrip_Stops
					   ,@TripId
					   ,@ReturnTrip_Duration
					   ,9002
					   ,@ReturnTrip_StopInformation)
				end

				print 'end : SUCCESS'

				COMMIT TRAN
				return 1
			end

		   return 0


    end try
    begin catch
		IF @@TRANCOUNT > 0 ROLLBACK TRAN   
			set @Error_message='Error in [dbo].[InsertTripWithTransaction] stored procedure. '+@StrParameters
			set @Exception=ERROR_MESSAGE()
			execute [dbo].[InsertLog] @Error_message,@Exception
		    print 'FAILURE : rollback => '+ERROR_MESSAGE()
		   return 0
    end catch
end


