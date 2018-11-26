INSERT INTO [dbo].[Provider]
           ([Name]
           ,[ImageSrc]
           ,[HasAPI]
           ,[IsSearchEngine]
           ,[Active])
     VALUES
           ('AirFrance/KLM'
           ,'https://www.sebastienbouyssou.com/wp-content/uploads/2009/02/airfranceklm.jpg'
           ,1
           ,0
           ,1)
GO

deleet from [dbo].[ProviderCountry] where providerid=4
INSERT INTO [dbo].[ProviderCountry]
           ([ProviderId]
           ,[CountryId])
select 4, id from dbo.Country
where ContinentId=232