



CREATE STATISTICS [_dta_stat_75147313_8_15] ON [dbo].[Trip]([CurrencyId], [SearchTripProviderId])
go

CREATE STATISTICS [_dta_stat_1095674951_3_2] ON [dbo].[AirportsTrip]([ToAirportId], [FromAirportId])
go

CREATE STATISTICS [_dta_stat_2126630619_4_5] ON [dbo].[Flight]([FromAirportId], [ToAirportId])
go

CREATE STATISTICS [_dta_stat_2126630619_8_4_5] ON [dbo].[Flight]([AirlineId], [FromAirportId], [ToAirportId])
go

CREATE STATISTICS [_dta_stat_2126630619_13_8_4_5] ON [dbo].[Flight]([FlightTypeId], [AirlineId], [FromAirportId], [ToAirportId])
go

CREATE STATISTICS [_dta_stat_2126630619_12_8_4_5_13] ON [dbo].[Flight]([TripId], [AirlineId], [FromAirportId], [ToAirportId], [FlightTypeId])
go

CREATE TABLE [dbo].[HistoricWeather](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Summary] [varchar](2000) NULL,
	[Icon] [varchar](2000) NULL,
	[PrecipType] [varchar](2000) NULL,
	[PrecipIntensity] [decimal](25, 12) NOT NULL,
	[PrecipProbability] [decimal](25, 12) NOT NULL,
	[TemperatureHigh] [decimal](25, 12) NOT NULL,
	[TemperatureLow] [decimal](25, 12) NOT NULL,
	[Humidity] [decimal](25, 12) NOT NULL,
	[CloudCover] [decimal](25, 12) NOT NULL,
	[AirportId] [int] NOT NULL,
	[WindSpeed] [decimal](25, 12) NOT NULL,
 CONSTRAINT [PK_HistoricWeather_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_HistoricWeather] UNIQUE NONCLUSTERED 
(
	[Date] ASC,
	[AirportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[HistoricWeather]  WITH CHECK ADD  CONSTRAINT [FK_HistoricWeather_Airport] FOREIGN KEY([AirportId])
REFERENCES [dbo].[Airport] ([Id])
GO

ALTER TABLE [dbo].[HistoricWeather] CHECK CONSTRAINT [FK_HistoricWeather_Airport]
GO


alter table [dbo].[City]
add OffSetHours decimal(5,2) null

update [dbo].[City]
set OffSetHours=OffSet

alter table [dbo].[City]
drop column OffSet