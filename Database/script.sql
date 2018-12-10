CREATE TABLE [dbo].[WeekOfYear](
	[Id] [int]  NOT NULL,
	[MonthStart] int not null,
	[MonthEnd] int not null,
	[DayStart] int not null,
	[DayEnd] int not null,
	[IndexStart] int not null default -1,
	[IndexEnd] int not null default -1,
 CONSTRAINT [PK_WeekOfYear_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

delete from [dbo].[WeekOfYear]

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           (1
           ,1
           ,1
           ,7
           ,1
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           (2
           ,8
           ,1
           ,14
           ,1
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           (3
           ,15
           ,1
           ,21
           ,1
           ,0
           ,0)
GO



INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           (4
           ,22
           ,1
           ,28
           ,1
           ,0
           ,0)
GO



INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           (5
           ,29
           ,1
           ,5
           ,2
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,6
           ,2
           ,12
           ,2
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,13
           ,2
           ,19
           ,2
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,20
           ,2
           ,26
           ,2
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,27
           ,2
           ,5
           ,3
           ,0
           ,0)
GO



INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,6
           ,3
           ,12
           ,3
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,13
           ,3
           ,19
           ,3
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,20
           ,3
           ,26
           ,3
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,27
           ,3
           ,2
           ,4
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,3
           ,4
           ,10
           ,4
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,11
           ,4
           ,17
           ,4
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,18
           ,4
           ,24
           ,4
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,25
           ,4
           ,1
           ,5
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,2
           ,5
           ,8
           ,5
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,9
           ,5
           ,15
           ,5
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,16
           ,5
           ,22
           ,5
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,23
           ,5
           ,29
           ,5
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,30
           ,5
           ,5
           ,6
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,6
           ,6
           ,13
           ,6
           ,0
           ,0)
GO




INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,14
           ,6
           ,20
           ,6
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,21
           ,6
           ,27
           ,6
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,28
           ,6
           ,4
           ,7
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,5
           ,7
           ,11
           ,7
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,12
           ,7
           ,18
           ,7
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,19
           ,7
           ,25
           ,7
           ,0
           ,0)
GO





INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,26
           ,7
           ,1
           ,8
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,2
           ,8
           ,8
           ,8
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,9
           ,8
           ,15
           ,8
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,16
           ,8
           ,22
           ,8
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,23
           ,8
           ,29
           ,8
           ,0
           ,0)
GO



INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,30
           ,8
           ,5
           ,9
           ,0
           ,0)
GO
INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,6
           ,9
           ,12
           ,9
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,13
           ,9
           ,19
           ,9
           ,0
           ,0)
GO



INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,20
           ,9
           ,26
           ,9
           ,0
           ,0)
GO




INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,27
           ,9
           ,3
           ,10
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,4
           ,10
           ,10
           ,10
           ,0
           ,0)
GO



INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,11
           ,10
           ,17
           ,10
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,18
           ,10
           ,24
           ,10
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,25
           ,10
           ,31
           ,10
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,1
           ,11
           ,7
           ,11
           ,0
           ,0)
GO



INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,8
           ,11
           ,14
           ,11
           ,0
           ,0)
GO



INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,15
           ,11
           ,21
           ,11
           ,0
           ,0)
GO




INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,22
           ,11
           ,28
           ,11
           ,0
           ,0)
GO

INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,29
           ,11
           ,5
           ,12
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,6
           ,12
           ,12
           ,12
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,13
           ,12
           ,19
           ,12
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,20
           ,12
           ,25
           ,12
           ,0
           ,0)
GO


INSERT INTO [dbo].[WeekOfYear]
           ([Id]
		   ,[DayStart]
           ,[MonthStart]
		   ,[DayEnd]
           ,[MonthEnd]
           ,[IndexStart]
           ,[IndexEnd])
     VALUES
           ((select max(id)+1 from [dbo].[WeekOfYear])
           ,26
           ,12
           ,31
           ,12
           ,0
           ,0)
GO














update .[WeekOfYear]
set IndexStart=MonthStart*100+DayStart

update .[WeekOfYear]
set IndexEnd=MonthEnd*100+DayEnd



alter table [dbo].[HistoricWeather]
add WeekOfYearId int null CONSTRAINT [FK_HistoricWeather_weekofyearid] 
REFERENCES [dbo].[WeekOfYear] ([Id])







CREATE TABLE [dbo].[Weather](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Summary] [varchar](2000) NULL,
	[PrecipType] [varchar](2000) NULL,
	[PrecipIntensity] [decimal](25, 12) NOT NULL,
	[TemperatureHigh] [decimal](25, 12) NOT NULL,
	[TemperatureLow] [decimal](25, 12) NOT NULL,
	[Humidity] [decimal](25, 12) NOT NULL,
	[CloudCover] [decimal](25, 12) NOT NULL,
	[AirportId] [int] NOT NULL,
	[WindSpeed] [decimal](25, 12) NOT NULL,
	[WeekOfYearId] [int] NULL,
 CONSTRAINT [PK_Weather_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Weather]  WITH CHECK ADD  CONSTRAINT [FK_Weather_Airport] FOREIGN KEY([AirportId])
REFERENCES [dbo].[Airport] ([Id])
GO

ALTER TABLE [dbo].[Weather] CHECK CONSTRAINT [FK_Weather_Airport]
GO

ALTER TABLE [dbo].[Weather]  WITH CHECK ADD  CONSTRAINT [FK_Weather_weekofyearid] FOREIGN KEY([WeekOfYearId])
REFERENCES [dbo].[WeekOfYear] ([Id])
GO

ALTER TABLE [dbo].[Weather] CHECK CONSTRAINT [FK_Weather_weekofyearid]
GO


CREATE TABLE [dbo].[WeatherSummaryType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] varchar(100) not null,
	[ImageSrc] varchar(200)  null,
	[Attractiveness] decimal(25,2) NOT NULL default 5,
 CONSTRAINT [PK_WeatherSummaryType_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[WeatherRainType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] varchar(100) not null,
 CONSTRAINT [PK_WeatherRainType_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



update [dbo].[WeatherSummaryType]
set Attractiveness=10
where name='clear-day'

update [dbo].[WeatherSummaryType]
set Attractiveness=7.5
where name='partly-cloudy-day'

update [dbo].[WeatherSummaryType]
set Attractiveness=7
where name='partly-cloudy-night'

update [dbo].[WeatherSummaryType]
set Attractiveness=5
where  name in ('cloudy','snow','wind')

update [dbo].[WeatherSummaryType]
set Attractiveness=3.5
where name in ('fog','rain','sleet')



alter table dbo.Weather
drop column summary

alter table dbo.Weather
drop column preciptype


alter table [dbo].weather
add SummaryTypeId int not null CONSTRAINT [FK_Weather_SummaryTypeId] 
REFERENCES [dbo].[WeatherSummaryType] ([Id])

alter table [dbo].weather
add RainTypeId int null CONSTRAINT [FK_Weather_RainTypeId] 
REFERENCES [dbo].[WeatherRainType] ([Id])

ALTER TABLE [dbo].[Weather]  ADD Attractiveness decimal(25,12) NOT NULL DEFAULT 0


create function  [dbo].[GetWeatherAttractiveness]
		( 
        @WeatherId integer
		)
		Returns decimal(25,12)
        AS
        BEGIN

	declare @SummaryAttractiveness decimal(25,12),
			@Attractiveness decimal(25,12),
			@TemperatureLow decimal(25,12),
			@TemperatureHigh decimal(25,12),
			@TemperatureAttractiveness decimal(25,12)


			(select @SummaryAttractiveness=ws.Attractiveness,
					@TemperatureLow=w.TemperatureLow,
					@TemperatureHigh=w.TemperatureHigh
			 from dbo.Weather w inner join dbo.WeatherSummaryType ws on ws.id=w.SummaryTypeId where w.id=@WeatherId)

			 set @TemperatureAttractiveness=5
			if @TemperatureHigh>=45
			begin
				set @TemperatureAttractiveness=2
			end
			else if @TemperatureHigh<45 and @TemperatureHigh>=40
			begin
				set @TemperatureAttractiveness=3
			end
			else if @TemperatureHigh<39 and @TemperatureHigh>=39
			begin
				set @TemperatureAttractiveness=4
			end
			else if @TemperatureHigh<38 and @TemperatureHigh>=38
			begin
				set @TemperatureAttractiveness=5
			end
			else if @TemperatureHigh<37 and @TemperatureHigh>=37
			begin
				set @TemperatureAttractiveness=6
			end
			else if @TemperatureHigh<37 and @TemperatureHigh>=35
			begin
				set @TemperatureAttractiveness=7
			end
			else if @TemperatureHigh<35 and @TemperatureHigh>=34
			begin
				set @TemperatureAttractiveness=8
			end
			else if @TemperatureHigh<34 and @TemperatureHigh>=30
			begin
				set @TemperatureAttractiveness=9
			end
			else if @TemperatureHigh<30 and @TemperatureHigh>=25
			begin
				set @TemperatureAttractiveness=10
			end
			else if @TemperatureHigh<23 and @TemperatureHigh>=25
			begin
				set @TemperatureAttractiveness=9
			end
			else if @TemperatureHigh<23 and @TemperatureHigh>=20
			begin
				set @TemperatureAttractiveness=8
			end
			else if @TemperatureHigh<20 and @TemperatureHigh>=17
			begin
				set @TemperatureAttractiveness=7
			end
			else if @TemperatureHigh<17 and @TemperatureHigh>=13
			begin
				set @TemperatureAttractiveness=6
			end
			else if @TemperatureHigh<13 and @TemperatureHigh>=9
			begin
				set @TemperatureAttractiveness=5
			end
			else if @TemperatureHigh<9 and @TemperatureHigh>=4
			begin
				set @TemperatureAttractiveness=4
			end
			else if @TemperatureHigh<4 and @TemperatureHigh>=-2
			begin
				set @TemperatureAttractiveness=3
			end
			else if @TemperatureHigh<-2 and @TemperatureHigh>=-10
			begin
				set @TemperatureAttractiveness=2
			end
			else if @TemperatureHigh<-10
			begin
				set @TemperatureAttractiveness=1
			end

			set @TemperatureAttractiveness=@TemperatureAttractiveness+5
			set @Attractiveness=@SummaryAttractiveness/10*@TemperatureAttractiveness/15

			return @Attractiveness
	end
go

create  procedure [dbo].[AggregateWeather]
        AS
        BEGIN

    declare @Error_message varchar(max),
			@Exception varchar(max),
			@WeekOfYearId int,
			@AirportId int,
			@WeatherSummaryTypeId int,
			@WeatherRainTypeId  int,
			@PrecipIntensity decimal(25,12),
			@TemperatureHigh decimal(25,12),
			@TemperatureLow decimal(25,12),
			@Humidity  decimal(25,12),
			@CloudCover decimal(25,12),
			@WindSpeed decimal(25,12),
			@nbData int,
			@Attractiveness decimal(25,12),
			@WeatherId integer

    begin try
			-- execute  [dbo].[AggregateWeather]

			update [dbo].[HistoricWeather] 
			set WeekOfYearId=(select top 1 id from dbo.WeekOfYear w where w.IndexStart<=(100*MONTH( [dbo].[HistoricWeather].Date)+DAY( [dbo].[HistoricWeather].DATE)) and w.IndexEnd>=(100*MONTH([dbo].[HistoricWeather].DATE)+DAY([dbo].[HistoricWeather].DATE)))
			where WeekOfYearId is null

			INSERT INTO [dbo].[WeatherRainType]
           ([Name])
			select
				distinct PrecipType
				from [dbo].[HistoricWeather]
			where PrecipType not in (select name from dbo.WeatherRainType)
			and PrecipType is not null

			INSERT INTO [dbo].[WeatherSummaryType]
           ([Name]
           ,[ImageSrc]
           ,[Attractiveness])
			select
				distinct Icon,null,5
				from [dbo].[HistoricWeather]
			where Icon not in (select name from  [dbo].[WeatherSummaryType])
			and Icon is not null


			delete from [dbo].[Weather]

			DECLARE WeekOfYear_cursor CURSOR LOCAL FOR   
			SELECT Id  FROM [dbo].[WeekOfYear] where id in (select distinct WeekOfYearid from dbo.HistoricWeather)
			OPEN WeekOfYear_cursor
			FETCH NEXT FROM WeekOfYear_cursor   
			INTO @WeekOfYearId

			WHILE @@FETCH_STATUS = 0  -- meaning successful
				BEGIN
				print '*** WEEK : '+cast(@WeekOfYearId as varchar(10))+' ***'

				DECLARE Airport_cursor CURSOR LOCAL FOR   
				SELECT Id  FROM [dbo].[Airport] where Active=1
				-- and id =7921
				OPEN Airport_cursor
				FETCH NEXT FROM Airport_cursor   
				INTO @AirportId
				WHILE @@FETCH_STATUS = 0  -- meaning successful
					BEGIN
					set @nbData=(select count(*) from dbo.HistoricWeather where AirportId=@AirportId and WeekOfYearId=@WeekOfYearId)
					print cast(getdate() as varchar(30)) +' : ** Airport : '+cast(@AirportId as varchar(10))+'  and WeekOfYearId : '+cast(@WeekOfYearId as varchar(10))+' and data number = '+cast(@nbData as varchar(10))+' ***'
					if @nbData>0
					begin
						SELECT top 1 @WeatherSummaryTypeId=S.Id,
									 @nbData=COUNT(S.Id)  ,
									 @Attractiveness=s.Attractiveness
							FROM     dbo.HistoricWeather h
									inner join dbo.WeatherSummaryType s on LOWER(s.Name)=LOWER(h.Icon)
							where  AirportId=@AirportId and WeekOfYearId=@WeekOfYearId
							GROUP BY S.Id,s.Attractiveness
							ORDER BY COUNT(S.Id)  DESC, s.Attractiveness desc
	
						SET @WeatherRainTypeId=NULL
						if (select count(*)  FROM  dbo.HistoricWeather h
							where  AirportId=@AirportId and WeekOfYearId=@WeekOfYearId and h.PrecipType is not null) >0
						 begin
							  SELECT top 1 @WeatherRainTypeId=r.Id,
										 @nbData=COUNT(r.Id)  
								FROM     dbo.HistoricWeather h
									inner join dbo.WeatherRainType r on LOWER(r .Name)=LOWER(h.PrecipType)
								where  AirportId=@AirportId and WeekOfYearId=@WeekOfYearId
								GROUP BY r.Id
								ORDER BY COUNT(r.Id)  DESC
                       END


						set @nbData=(select count(*) from dbo.HistoricWeather h where h.PrecipIntensity is not null  and AirportId=@AirportId and WeekOfYearId=@WeekOfYearId  and h.PrecipIntensity<>0)
						if @nbData>0
						begin
							set @PrecipIntensity=(select sum(h.PrecipIntensity) from dbo.HistoricWeather h where h.PrecipIntensity is not null  and AirportId=@AirportId and WeekOfYearId=@WeekOfYearId  and h.PrecipIntensity<>0) / @nbData
						end
						else
						begin
							set @PrecipIntensity=0
						end


						set @nbData= (select count(*) from dbo.HistoricWeather h where h.TemperatureHigh is not null  and AirportId=@AirportId and WeekOfYearId=@WeekOfYearId )
						if @nbData>0
						begin
							set @TemperatureHigh=(select sum(h.TemperatureHigh) from dbo.HistoricWeather h where h.TemperatureHigh is not null  and AirportId=@AirportId and WeekOfYearId=@WeekOfYearId )  / @nbData
						end
						else
						begin
							set @TemperatureHigh=0
						end

						set @nbData= (select count(*) from dbo.HistoricWeather h where h.TemperatureLow is not null  and AirportId=@AirportId and WeekOfYearId=@WeekOfYearId )
						if @nbData>0
						begin
							set @TemperatureLow=(select sum(h.TemperatureLow) from dbo.HistoricWeather h where h.TemperatureLow is not null  and AirportId=@AirportId and WeekOfYearId=@WeekOfYearId )  / @nbData
						end
						else
						begin
							set @TemperatureLow=0
						end

						set @nbData= (select count(*) from dbo.HistoricWeather h where h.WindSpeed is not null  and AirportId=@AirportId and WeekOfYearId=@WeekOfYearId )
						if @nbData>0
						begin
							set @WindSpeed=(select sum(h.WindSpeed) from dbo.HistoricWeather h where h.WindSpeed is not null  and AirportId=@AirportId and WeekOfYearId=@WeekOfYearId )  / @nbData
						end
						else
						begin
							set @WindSpeed=0
						end

				
						set @nbData= (select count(*) from dbo.HistoricWeather h where h.CloudCover is not null  and AirportId=@AirportId and WeekOfYearId=@WeekOfYearId )
						if @nbData>0
						begin
							set @CloudCover=(select sum(h.CloudCover) from dbo.HistoricWeather h where h.WindSpeed is not null  and AirportId=@AirportId and WeekOfYearId=@WeekOfYearId )  / @nbData
						end
						else
						begin
							set @CloudCover=0
						end

						set @nbData= (select count(*) from dbo.HistoricWeather h where h.Humidity is not null  and AirportId=@AirportId and WeekOfYearId=@WeekOfYearId )
						if @nbData>0
						begin
							set @Humidity=(select sum(h.Humidity) from dbo.HistoricWeather h where h.Humidity is not null  and AirportId=@AirportId and WeekOfYearId=@WeekOfYearId )  / @nbData
						end
						else
						begin
							set @Humidity=0
						end

						INSERT INTO [dbo].[Weather]
						   ([SummaryTypeId]
						   ,[RainTypeId]
						   ,[PrecipIntensity]
						   ,[TemperatureHigh]
						   ,[TemperatureLow]
						   ,[Humidity]
						   ,[CloudCover]
						   ,[AirportId]
						   ,[WindSpeed]
						   ,[WeekOfYearId],
						   Attractiveness)
					 VALUES
						   (@WeatherSummaryTypeId
						   ,@WeatherRainTypeId
						   ,@PrecipIntensity
						   ,@TemperatureHigh
						   ,@TemperatureLow
						   ,@Humidity
						   ,@CloudCover
						   ,@AirportId
						   ,@WindSpeed
						   ,@WeekOfYearId
						   ,0)
					   end

					   set @WeatherId=@@identity

					   update dbo.Weather
					   set Attractiveness=[dbo].[GetWeatherAttractiveness](@WeatherId)
					   where id=@WeatherId

					FETCH NEXT FROM Airport_cursor   
					INTO @AirportId
					END   
				CLOSE Airport_cursor;
				DEALLOCATE Airport_cursor;               
				FETCH NEXT FROM WeekOfYear_cursor   
				INTO @WeekOfYearId
				END   
			CLOSE WeekOfYear_cursor;
			DEALLOCATE WeekOfYear_cursor;

		   return 1
    end try
    begin catch 
			set @Error_message='Error in [dbo].[AggregateWeather] stored procedure. '
			set @Exception=ERROR_MESSAGE()
			execute [dbo].[InsertLog] @Error_message,@Exception
		    print 'error in AggregateWeather : rollback => '+ERROR_MESSAGE()
		   return 0
    end catch
end

go


USE [Template]
GO
/****** Object:  UserDefinedFunction [dbo].[GetTripAttractiveness]    Script Date: 2018-12-09 7:24:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER  function  [dbo].[GetTripAttractiveness]
		( 
        @TripId integer
		)
		Returns decimal(25,12)
        AS
        BEGIN

	declare @Price decimal(25,2),
			@Distance float(18),
			@StopsNumber int,
			@Attractiveness decimal(25,12),
			@DayOfTheWeekOneWayFlight int,
			@DayOfTheWeekReturnFlight int,
			@DayOfTheWeekOneWayFlightCoefficient decimal(25,12),
			@DayOfTheWeekReturnFlightCoefficient decimal(25,12),
			@WeatherCoefficient decimal(25,12),
			@WeatherCoefficientFrom decimal(25,12),
			@WeatherCoefficientTo decimal(25,12),
			@TripDurationCoefficient decimal(25,12),
			@WeatherAttractiveness  decimal(25,12),
			@TripAttractiveness int,
			@FromAirportId int,
			@ToAirportId int,
			@IndexDateFrom int,
			@IndexDateTo int,
			@WeatherAttractivenessFrom  decimal(25,12),
			 @WeatherAttractivenessTo  decimal(25,12),
			 @DateFrom datetime,
			 @DateTo datetime

			select @Distance=Distance,@Price=Price, @FromAirportId=stw.FromAirportId, @ToAirportId=ToAirportId, @DateFrom=st.FromDate, @DateTo=st.ToDate  from dbo.Trip t 
				inner join dbo.SearchTripProvider stp on stp.Id=t.SearchTripProviderId
				inner join dbo.SearchTrip st on st.id=stp.SearchTripId
				inner join dbo.SearchTripWishes stw on stw.id=st.SearchTripWishesId
				where  t.id=@TripId
			
			if @Price>0 and @Distance is not null and @Distance>0
			begin
				set @DayOfTheWeekOneWayFlight=(select top 1 DATEPART(dw,f.DepartureDate) from dbo.Flight f where TripId=@TripId and FlightTypeId=9001 order by f.DepartureDate asc)
				set @DayOfTheWeekReturnFlight=(select top 1 DATEPART(dw,f.ArrivalDate) from dbo.Flight f where TripId=@TripId and FlightTypeId=9002 order by f.ArrivalDate desc)
				set @StopsNumber=isnull((select sum(StopNumber) from dbo.Flight where TripId=@TripId),0)
				set @TripDurationCoefficient=isnull((select sum(Duration)/60 from dbo.Flight where TripId=@TripId and Duration>0),0)
				set @TripAttractiveness=isnull((select top 1 Attractiveness from dbo.AirportsTrip where (FromAirportId=@FromAirportId and ToAirportId=@ToAirportId)or(FromAirportId=@ToAirportId and ToAirportId=@FromAirportId)),100)
				set @WeatherCoefficient=1

				set @IndexDateFrom = 100*MONTH(@DateFrom)+Day(@DateFrom)
				set @IndexDateTo = 100*MONTH(@DateTo)+Day(@DateTo)

				set @WeatherAttractivenessFrom=(select top 1 Attractiveness from dbo.Weather w inner join dbo.WeekOfYear wy on wy.id=w.WeekOfYearId 
								where wy.IndexStart<=@IndexDateFrom and wy.IndexEnd>=@IndexDateFrom and w.AirportId=@ToAirportId)
				set @WeatherAttractivenessTo=(select top 1 Attractiveness from dbo.Weather w inner join dbo.WeekOfYear wy on wy.id=w.WeekOfYearId 
								where wy.IndexStart<=@IndexDateTo and wy.IndexEnd>=@IndexDateTo  and w.AirportId=@ToAirportId)
				
				if @WeatherAttractivenessFrom is not null
				begin
					set @WeatherCoefficientFrom=@WeatherAttractivenessFrom/(select top 1 Attractiveness from dbo.Weather w 
								where w.AirportId=@ToAirportId order by Attractiveness desc)
				end
				if @WeatherAttractivenessTo is not null
				begin
					set @WeatherCoefficientTo=@WeatherAttractivenessTo/(select top 1 Attractiveness from dbo.Weather w 
								where w.AirportId=@ToAirportId order by Attractiveness desc)
				end
				set @WeatherCoefficient=isnull((isnull(@WeatherCoefficientFrom,1)+isnull(@WeatherCoefficientTo,1))/2,1)


				if @DayOfTheWeekReturnFlight is not null
				begin
					if @DayOfTheWeekReturnFlight=1 or @DayOfTheWeekReturnFlight=7
					begin
						set @DayOfTheWeekReturnFlightCoefficient=1.1
					end
					if @DayOfTheWeekReturnFlight=2
					begin
						set @DayOfTheWeekReturnFlightCoefficient=1.05
					end
					else
					begin
						set @DayOfTheWeekReturnFlightCoefficient=1
					end
				end
				else 
				begin
					set @DayOfTheWeekReturnFlightCoefficient=1
				end

				if @DayOfTheWeekOneWayFlight is not null
				begin
					if @DayOfTheWeekOneWayFlight=7
					begin
						set @DayOfTheWeekOneWayFlightCoefficient=1.1
					end
					if @DayOfTheWeekOneWayFlight=1 or  @DayOfTheWeekOneWayFlight=6
					begin
						set @DayOfTheWeekOneWayFlightCoefficient=1.05
					end
					else
					begin
						set @DayOfTheWeekOneWayFlightCoefficient=1
					end
				end
				else 
				begin
					set @DayOfTheWeekOneWayFlightCoefficient=1
				end

				set @Attractiveness=@WeatherCoefficient*((@Distance/@Price)*POWER(0.85, @StopsNumber)*@DayOfTheWeekReturnFlightCoefficient*@DayOfTheWeekOneWayFlightCoefficient*@TripAttractiveness/100)-@TripDurationCoefficient/20
			end

			return @Attractiveness
	end
go
end


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
