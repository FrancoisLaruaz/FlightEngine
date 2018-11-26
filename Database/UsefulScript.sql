delete from dbo.Flight
delete from dbo.Trip
delete from dbo.SearchTripProvider
delete from dbo.SearchTrip
delete from dbo.SearchTripWishes

-- https://dev.maxmind.com/geoip/legacy/codes/country_continent/

select * from dbo.Airport
where id  not in (select ToAirportId from dbo.Trip t 
				inner join dbo.SearchTripProvider stp on stp.Id=t.SearchTripProviderId
				inner join dbo.SearchTrip st on st.id=stp.SearchTripId
				inner join dbo.SearchTripWishes stw on stw.id=st.SearchTripWishesId)

INSERT INTO [dbo].[AirportsTrip]
           ([FromAirportId]
           ,[ToAirportId]
           ,[Attractiveness])
     select distinct base.FromAirportId,base.ToAirportId, 100  from dbo.SearchTripWishes base
	where base.ToAirportId in  (select ToAirportId from dbo.Trip t 
				inner join dbo.SearchTripProvider stp on stp.Id=t.SearchTripProviderId
				inner join dbo.SearchTrip st on st.id=stp.SearchTripId
				inner join dbo.SearchTripWishes stw on stw.id=st.SearchTripWishesId
				where base.id=stw.id)


				
select stw.distance, t.Price,t.EuroPrice,t.Url,t.Attractiveness,a.Name,a.Code, (select sum(StopNumber) from dbo.Flight where TripId=t.Id) as stops,  (select sum(f.Duration)/60 from dbo.Flight f where TripId=t.Id) as 'flights_duration (hours)'
				 from dbo.Trip t 
				inner join dbo.SearchTripProvider stp on stp.Id=t.SearchTripProviderId
				inner join dbo.SearchTrip st on st.id=stp.SearchTripId
				inner join dbo.SearchTripWishes stw on stw.id=st.SearchTripWishesId
				inner join dbo.Airport a on a.id=stw.ToAirportId
				inner join dbo.City ci on ci.id=a.CityId
				inner join dbo.Country co on co.id=ci.CountryId
				inner join dbo.Continent con on con.id=co.ContinentId
				where 1=1 
				-- stw.FromAirportId=7921
			--	and stw.Distance>6000
			--	and con.id=232
				--and co.Name like '%canada%'
			--	and ci.name like '%atlanta%'
				order by Attractiveness desc		
				
				
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
           ,FromAirportId
           ,ToAirportId
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
		   from dbo.AirportsTrip
		   where FromAirportId= 7921 

		   
		   
		   