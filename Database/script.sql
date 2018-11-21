CREATE TABLE [dbo].[Continent](
	[Id] [int] IDENTITY(231,1) NOT NULL,
	[Name] [nvarchar](255) NULL DEFAULT (NULL),
	[Code] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Continent_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Continent$Id_UNIQUE] UNIQUE NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_Continent] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[Continent]
           ([Name]
           ,[Code])
     VALUES
           ('Unknown'
           ,'--')
GO



INSERT INTO [dbo].[Continent]
           ([Name]
           ,[Code])
     VALUES
           ('Europe'
           ,'EU')
GO


INSERT INTO [dbo].[Continent]
           ([Name]
           ,[Code])
     VALUES
           ('Asia'
           ,'AS')
GO


INSERT INTO [dbo].[Continent]
           ([Name]
           ,[Code])
     VALUES
           ('North America'
           ,'NA')
GO



INSERT INTO [dbo].[Continent]
           ([Name]
           ,[Code])
     VALUES
           ('Oceania'
           ,'OC')
GO

INSERT INTO [dbo].[Continent]
           ([Name]
           ,[Code])
     VALUES
           ('Africa'
           ,'AF')
GO

INSERT INTO [dbo].[Continent]
           ([Name]
           ,[Code])
     VALUES
           ('South America'
           ,'SA')
GO

INSERT INTO [dbo].[Continent]
           ([Name]
           ,[Code])
     VALUES
           ('Antartica'
           ,'AN')
GO

INSERT INTO [dbo].[Continent]
           ([Name]
           ,[Code])
     VALUES
           ('Central America'
           ,'CA')
GO




if not exists (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='Country' and COLUMN_NAME='ContinentId')
BEGIN
	Alter table Country
	add ContinentId  int not null default 231 CONSTRAINT FK_Country_ContinentId  
	REFERENCES [dbo].[Continent] ([Id])
END
Go
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='--') where code='A1'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='--') where code='A2'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='AD'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='AE'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='AF'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='AG'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='AI'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='AL'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='AM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='AN'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='AO'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='AP'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AN') where code='AQ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='SA') where code='AR'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='AS'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='AT'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='AU'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='AW'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='AX'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='AZ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='BA'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='BB'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='BD'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='BE'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='BF'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='BG'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='BH'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='BI'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='BJ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='BL'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='BM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='BN'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='SA') where code='BO'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='SA') where code='BR'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='BS'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='BT'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AN') where code='BV'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='BW'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='BY'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='BZ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='CA'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='CC'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='CD'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='CF'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='CG'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='CH'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='CI'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='CK'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='SA') where code='CL'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='CM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='CN'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='SA') where code='CO'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='CR'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='CU'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='CV'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='CX'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='CY'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='CZ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='DE'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='DJ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='DK'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='DM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='DO'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='DZ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='SA') where code='EC'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='EE'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='EG'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='EH'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='ER'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='ES'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='ET'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='EU'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='FI'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='FJ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='SA') where code='FK'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='FM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='FO'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='FR'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='FX'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='GA'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='GB'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='GD'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='GE'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='SA') where code='GF'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='GG'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='GH'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='GI'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='GL'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='GM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='GN'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='GP'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='GQ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='GR'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AN') where code='GS'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='GT'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='GU'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='GW'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='SA') where code='GY'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='HK'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AN') where code='HM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='HN'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='HR'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='HT'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='HU'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='ID'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='IE'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='IL'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='IM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='IN'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='IO'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='IQ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='IR'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='IS'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='IT'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='JE'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='JM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='JO'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='JP'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='KE'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='KG'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='KH'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='KI'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='KM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='KN'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='KP'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='KR'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='KW'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='KY'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='KZ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='LA'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='LB'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='LC'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='LI'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='LK'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='LR'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='LS'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='LT'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='LU'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='LV'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='LY'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='MA'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='MC'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='MD'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='ME'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='MF'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='MG'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='MH'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='MK'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='ML'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='MM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='MN'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='MO'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='MP'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='MQ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='MR'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='MS'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='MT'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='MU'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='MV'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='MW'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='MX'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='MY'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='MZ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='NA'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='NC'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='NE'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='NF'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='NG'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='NI'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='NL'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='NO'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='NP'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='NR'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='NU'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='NZ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='--') where code='O1'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='OM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='PA'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='SA') where code='PE'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='PF'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='PG'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='PH'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='PK'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='PL'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='PM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='PN'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='PR'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='PS'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='PT'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='PW'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='SA') where code='PY'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='QA'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='RE'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='RO'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='RS'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='RU'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='RW'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='SA'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='SB'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='SC'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='SD'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='SE'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='SG'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='SH'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='SI'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='SJ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='SK'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='SL'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='SM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='SN'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='SO'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='SA') where code='SR'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='ST'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='SV'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='SY'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='SZ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='TC'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='TD'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AN') where code='TF'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='TG'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='TH'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='TJ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='TK'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='TL'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='TM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='TN'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='TO'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='TR'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='TT'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='TV'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='TW'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='TZ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='UA'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='UG'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='UM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='US'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='SA') where code='UY'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='UZ'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='EU') where code='VA'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='VC'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='SA') where code='VE'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='VG'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='NA') where code='VI'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='VN'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='VU'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='WF'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='OC') where code='WS'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AS') where code='YE'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='YT'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='ZA'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='ZM'
update dbo.Country set continentId=(select top 1 id  from dbo.continent where code='AF') where code='ZW'
    
	
update dbo.Country
set ContinentId=232
where ContinentId=231	

update dbo.Country
set ContinentId=(select top 1 id from dbo.Continent where code='CA' order by id desc)
where name like '%Panama%'
or name like '%Nicaragua%'
or name like '%Mexico%'
or name like '%Belize%'
or name like '%Costa Rica%'
or name like '%El Salvador%'
or name like '%Guatemala%'
or name like '%Honduras%'


CREATE TABLE [dbo].[AirportsTrip](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FromAirportId] [int] NOT NULL,
	[ToAirportId] [int] NOT NULL,
	[Attractiveness] [int] NOT NULL DEFAULT (100),
 CONSTRAINT [PK_AirportsTrip_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[AirportsTrip]  WITH CHECK ADD  CONSTRAINT [FK_AirportsTrip_FromAirport] FOREIGN KEY([FromAirportId])
REFERENCES [dbo].[Airport] ([Id])
GO

ALTER TABLE [dbo].[AirportsTrip] CHECK CONSTRAINT [FK_AirportsTrip_FromAirport]
GO

ALTER TABLE [dbo].[AirportsTrip]  WITH CHECK ADD  CONSTRAINT [FK_AirportsTrip_ToAirport] FOREIGN KEY([ToAirportId])
REFERENCES [dbo].[Airport] ([Id])
GO

ALTER TABLE [dbo].[AirportsTrip] CHECK CONSTRAINT [FK_AirportsTrip_ToAirport]
GO



				alter table dbo.airport
				add Active bit not null default 1

drop procedure SetTripAttractiveness

create function  [dbo].[GetTripAttractiveness]
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
			@TripDurationCoefficient decimal(25,12)

			select @Distance=Distance,@Price=Price  from dbo.Trip t 
				inner join dbo.SearchTripProvider stp on stp.Id=t.SearchTripProviderId
				inner join dbo.SearchTrip st on st.id=stp.SearchTripId
				inner join dbo.SearchTripWishes stw on stw.id=st.SearchTripWishesId
				where  t.id=@TripId
			
			if @Price>0 and @Distance is not null and @Distance>0
			begin
				set @DayOfTheWeekOneWayFlight=(select top 1 DATEPART(dw,f.DepartureDate) from dbo.Flight f where TripId=@TripId and FlightTypeId=9001 order by f.DepartureDate asc)
				set @DayOfTheWeekReturnFlight=(select top 1 DATEPART(dw,f.ArrivalDate) from dbo.Flight f where TripId=@TripId and FlightTypeId=9002 order by f.ArrivalDate desc)
				set @StopsNumber=isnull((select sum(StopNumber) from dbo.Flight where TripId=@TripId),0)
				set @TripDurationCoefficient=isnull((select sum(Duration) from dbo.Flight where TripId=@TripId),0)


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

				set @Attractiveness=((@Distance/@Price)*POWER(0.85, @StopsNumber)*@DayOfTheWeekReturnFlightCoefficient*@DayOfTheWeekOneWayFlightCoefficient)-@TripDurationCoefficient/20
			end

			return @Attractiveness
	end



ALTER procedure [dbo].[InsertTripWithTransaction]
        @SearchTripProviderId int,
		@Price decimal (25,2),
		@CurrencyCode varchar(50)='',
		@Url varchar(4000),
		@OneWayTrip_FromAirportCode varchar(5),
		@OneWayTrip_ToAirportCode varchar(5),
		@OneWayTrip_DepartureDate varchar(30),
		@OneWayTrip_ArrivalDate varchar(30),
		@OneWayTrip_Duration int,
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







