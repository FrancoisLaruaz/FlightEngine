delete from dbo.Flight
delete from dbo.Trip
delete from dbo.SearchTripProvider
delete from dbo.SearchTrip
delete from dbo.SearchTripWishes


--DBCC SHRINKFILE(Template_log, 2);


INSERT INTO [dbo].[SearchTripWishes]
           ([CreationDate]
           ,[FromAirportId]
           ,[ToAirportId]
           ,[FromDateMin]
           ,[FromDateMax]
           ,[ToDateMin]
           ,[ToDateMax]
           ,[DurationMin]
           ,[DurationMax]
           ,[FromCityId]
           ,[ToCityId]
           ,[MaxStopNumber]
           ,[CreationUserId]
           ,[WishedPriceCurrencyId]
           ,[WishedPrice]
           ,[EuroWishedPrice]
           ,[Active])
select
           GETUTCDATE()
           ,7921
           ,Id
           , DATEADD(day, 7, GETDATE())
           , DATEADD(day, 200, GETDATE())
           , DATEADD(day, 7, GETDATE())
           , DATEADD(day, 200, GETDATE())
           ,4
           ,16
           ,null
           ,null
           ,1
           ,null
           ,null
           ,null
           ,null
           ,1
		   from dbo.Airport
		   where id<>7921 AND Longitude!=0 

		   
		   