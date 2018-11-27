CREATE TABLE [dbo].[AirportsTripProvider](
	[AirportsTripId] [int] NOT NULL,
	[ProviderId] [int] NOT NULL,
 CONSTRAINT [PK_AirportsTripProvider_Id] PRIMARY KEY CLUSTERED 
(
	[AirportsTripId], [ProviderId]  ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AirportsTripProvider]  WITH CHECK ADD  CONSTRAINT [FK_AirportsTripProvider_AirportsTrip] FOREIGN KEY([AirportsTripId])
REFERENCES [dbo].[AirportsTrip] ([Id])
GO

ALTER TABLE [dbo].[AirportsTripProvider] CHECK CONSTRAINT [FK_AirportsTripProvider_AirportsTrip]
GO

ALTER TABLE [dbo].[AirportsTripProvider]  WITH CHECK ADD  CONSTRAINT [FK_AirportsTripProvider_Provider] FOREIGN KEY([ProviderId])
REFERENCES [dbo].[Provider] ([Id])
GO

ALTER TABLE [dbo].[AirportsTripProvider] CHECK CONSTRAINT [FK_AirportsTripProvider_Provider]
GO

uPDATE dbo.Provider
set Name='AirFrance'
where id=4

INSERT INTO [dbo].[Provider]
           ([Name]
           ,[ImageSrc]
           ,[HasAPI]
           ,[IsSearchEngine]
           ,[Active])
     VALUES
           ('KLM'
           ,'https://www.sebastienbouyssou.com/wp-content/uploads/2009/02/airfranceklm.jpg'
           ,1
           ,0
           ,1)
GO
INSERT INTO [dbo].[ProviderCountry]
           ([ProviderId]
           ,[CountryId])
select 5,CountryId
from [dbo].[ProviderCountry]where ProviderId=4

USE [Template]
GO
/****** Object:  StoredProcedure [dbo].[InsertTripWithTransaction]    Script Date: 2018-11-26 9:48:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[InsertTripWithTransaction]
        @SearchTripProviderId int,
		@Price decimal (25,2),
		@CurrencyCode varchar(50)='',
		@Url varchar(4000),
		@Comment nvarchar(2000)=null,
		@OneWayTrip_FromAirportCode varchar(5),
		@OneWayTrip_ToAirportCode varchar(5),
		@OneWayTrip_DepartureDate varchar(30),
		@OneWayTrip_ArrivalDate varchar(30),
		@OneWayTrip_Duration int,
		@ProviderId int,
		@OneWayTrip_AirlineName varchar(100),
		@OneWayTrip_StopInformation varchar(1000) =null,
		@OneWayTrip_AirlineLogoSrc varchar(500)=null,
		@OneWayTrip_FlightNumber varchar(100)=null,
		@OneWayTrip_Stops integer = 0,
		@ReturnTrip_FromAirportCode varchar(5) = null,
		@ReturnTrip_ToAirportCode varchar(5) =null,
		@ReturnTrip_DepartureDate varchar(30)=null,
		@ReturnTrip_ArrivalDate varchar(30)=null,
		@ReturnTrip_Duration int =null ,
		@ReturnTrip_AirlineName varchar(100)=null,
		@ReturnTrip_AirlineLogoSrc varchar(500) =null,
		@ReturnTrip_StopInformation varchar(1000) =null,
		@ReturnTrip_FlightNumber varchar(100)=null,
		@ReturnTrip_Stops integer =null
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
			@ReturnTrip_AirlineId int,
			@AirportsTripId int

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
				   ,[EuroPrice]
				   ,[Comment])
				 VALUES
					   (@CurrencyId
					   ,@Price
					   ,@Url
					   ,@SearchTripProviderId
					   ,@EuroPrice
					   ,@Comment)

				set @TripId=@@IDENTITY

				

				INSERT INTO [dbo].[Flight]
				   (
				    [FromAirportId]
				   ,[ToAirportId]
				   ,[DepartureDate]
				   ,[ArrivalDate]
				   ,[AirlineId]
				   ,[StopNumber]
				   ,[TripId]
				   ,[Duration]
				   ,[FlightTypeId],
				    [StopInformation],
					[FlightNumber])
				VALUES
				   (
				   @OneWayTrip_FromAirportId
				   ,@OneWayTrip_ToAirportId
				   ,substring(@OneWayTrip_DepartureDate,7,4)+'-'+substring(@OneWayTrip_DepartureDate,4,2)+'-'+substring(@OneWayTrip_DepartureDate,1,2)+' '+substring(@OneWayTrip_DepartureDate,12,6)+':00.000'
				   ,substring(@OneWayTrip_ArrivalDate,7,4)+'-'+substring(@OneWayTrip_ArrivalDate,4,2)+'-'+substring(@OneWayTrip_ArrivalDate,1,2)+' '+substring(@OneWayTrip_ArrivalDate,12,6)+':00.000'
				   ,@OneWayTrip_AirlineId
				   ,@OneWayTrip_Stops
				   ,@TripId
				   ,@OneWayTrip_Duration
				   ,9001
				   	,@OneWayTrip_StopInformation
				   ,@OneWayTrip_FlightNumber)

				if @ReturnTrip_FromAirportCode is not null and @ReturnTrip_ToAirportCode is not null
				begin
					INSERT INTO [dbo].[Flight]
					   (
					    [FromAirportId]
					   ,[ToAirportId]
					   ,[DepartureDate]
					   ,[ArrivalDate]
					   ,[AirlineId]
					   ,[StopNumber]
					   ,[TripId]
					   ,[Duration]
					   ,[FlightTypeId],
						[StopInformation],
						[FlightNumber])
					VALUES
					   (
					    @ReturnTrip_FromAirportId
					   ,@ReturnTrip_ToAirportId
					   ,substring(@ReturnTrip_DepartureDate,7,4)+'-'+substring(@ReturnTrip_DepartureDate,4,2)+'-'+substring(@ReturnTrip_DepartureDate,1,2)+' '+substring(@ReturnTrip_DepartureDate,12,6)+':00.000'
					   ,substring(@ReturnTrip_ArrivalDate,7,4)+'-'+substring(@ReturnTrip_ArrivalDate,4,2)+'-'+substring(@ReturnTrip_ArrivalDate,1,2)+' '+substring(@ReturnTrip_ArrivalDate,12,6)+':00.000'
					   ,@ReturnTrip_AirlineId
					   ,@ReturnTrip_Stops
					   ,@TripId
					   ,@ReturnTrip_Duration
					   ,9002
					   ,@ReturnTrip_StopInformation
					   , @ReturnTrip_FlightNumber)
				end

				set @AirportsTripId=(select top 1 id from dbo.AirportsTrip 
					where 
					(FromAirportId=@OneWayTrip_FromAirportId and ToAirportId=@OneWayTrip_ToAirportId)
					or
					(FromAirportId=@OneWayTrip_ToAirportId and ToAirportId=@OneWayTrip_FromAirportId)
					)

				if @AirportsTripId is null
				begin
					INSERT INTO [dbo].[AirportsTrip]
					   ([FromAirportId]
					   ,[ToAirportId]
					   ,[Attractiveness])
				 VALUES
					   (@OneWayTrip_FromAirportId
					   ,@OneWayTrip_ToAirportId
					   ,100)
					set @AirportsTripId=@@IDENTITY
				end



				if (select count(*) from dbo.AirportsTripProvider 
					where ProviderId=@ProviderId and @AirportsTripId=AirportsTripId
					)=0
				begin
					INSERT INTO [dbo].[AirportsTripProvider]
					   ([ProviderId]
					   ,[AirportsTripId])
				 VALUES
					   (@ProviderId
					   ,@AirportsTripId)
				end

				update dbo.Trip
				set Attractiveness= [dbo].[GetTripAttractiveness](@TripId)
				where id=@TripId

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







