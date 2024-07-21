USE [TopCB_frienly_url]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[UserName] [nvarchar](256) NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Brokers]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brokers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OriginalName] [nvarchar](450) NOT NULL,
	[Mark] [int] NULL,
	[Foundation] [int] NULL,
	[PhoneNumbersHtml] [nvarchar](max) NULL,
	[EmailHtml] [nvarchar](max) NULL,
 CONSTRAINT [PK_Brokers_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Brokers.en-US]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brokers.en-US](
	[Id] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Instruments] [nvarchar](max) NULL,
	[RegulatedBy] [nvarchar](max) NULL,
	[UrlImg] [nvarchar](max) NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Visible] [bit] NOT NULL,
	[MinDepositListing] [nvarchar](150) NULL,
	[RegulationListing] [nvarchar](150) NULL,
	[SpreadsListing] [nvarchar](150) NULL,
	[Regulation] [nvarchar](max) NULL,
	[BusinessNature] [nvarchar](max) NULL,
	[GoogleAddress] [nvarchar](max) NULL,
	[Leverage] [nvarchar](max) NULL,
	[Platforms] [nvarchar](max) NULL,
	[AccountTypes] [nvarchar](max) NULL,
	[LotSizes] [nvarchar](max) NULL,
	[DemoAccount] [nvarchar](max) NULL,
	[AccountCurrency] [nvarchar](max) NULL,
	[PairsOffered] [nvarchar](max) NULL,
	[PaymentMethods] [nvarchar](max) NULL,
	[Commissions] [nvarchar](max) NULL,
	[AdditionalText] [nvarchar](max) NULL,
	[ShortInfo1] [nvarchar](max) NULL,
	[ShortInfo2] [nvarchar](max) NULL,
	[EndInfo] [nvarchar](max) NULL,
 CONSTRAINT [PK_Brokers.en-US] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Brokers.ru-RU]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brokers.ru-RU](
	[Id] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Instruments] [nvarchar](max) NULL,
	[RegulatedBy] [nvarchar](max) NULL,
	[UrlImg] [nvarchar](max) NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Visible] [bit] NOT NULL,
	[MinDepositListing] [nvarchar](150) NULL,
	[RegulationListing] [nvarchar](150) NULL,
	[SpreadsListing] [nvarchar](150) NULL,
	[Regulation] [nvarchar](max) NULL,
	[BusinessNature] [nvarchar](max) NULL,
	[GoogleAddress] [nvarchar](max) NULL,
	[Leverage] [nvarchar](max) NULL,
	[Platforms] [nvarchar](max) NULL,
	[AccountTypes] [nvarchar](max) NULL,
	[LotSizes] [nvarchar](max) NULL,
	[DemoAccount] [nvarchar](max) NULL,
	[AccountCurrency] [nvarchar](max) NULL,
	[PairsOffered] [nvarchar](max) NULL,
	[PaymentMethods] [nvarchar](max) NULL,
	[Commissions] [nvarchar](max) NULL,
	[AdditionalText] [nvarchar](max) NULL,
	[ShortInfo1] [nvarchar](max) NULL,
	[ShortInfo2] [nvarchar](max) NULL,
	[EndInfo] [nvarchar](max) NULL,
 CONSTRAINT [PK_Brokers.ru-RU] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BrokersCrypto]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BrokersCrypto](
	[BrokerId] [int] NOT NULL,
	[CryptoId] [nvarchar](50) NOT NULL,
	[value] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CoinApiCache]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CoinApiCache](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[date] [datetime] NULL,
	[price] [decimal](10, 5) NULL,
	[coin_Id] [nvarchar](10) NOT NULL,
	[time_type] [int] NOT NULL,
	[coin_graph_id] [nvarchar](150) NULL,
 CONSTRAINT [PK_CoinApiCache] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CoinApiMarketCache]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CoinApiMarketCache](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[asset_id_base] [nvarchar](150) NOT NULL,
	[symbol_id] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_CoinApiMarketCache] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CryptoCurrencyPrices]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CryptoCurrencyPrices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[timestamp] [datetime] NOT NULL,
	[cryptoId] [int] NOT NULL,
	[price] [float] NOT NULL,
	[lastUpdatedApi] [bigint] NOT NULL,
 CONSTRAINT [PK_CryptoCurrencyPrices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CryptoСurrency]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CryptoСurrency](
	[Id] [int] NOT NULL,
	[coin_abb] [nvarchar](150) NOT NULL,
	[coin_graph_id] [nvarchar](150) NULL,
 CONSTRAINT [PK_CryptoСurrency] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CryptoСurrencyIdsCache]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CryptoСurrencyIdsCache](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](150) NOT NULL,
	[symbol] [nvarchar](50) NOT NULL,
	[coin_id] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_CryptoСurrencyIdsCache] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbLog]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StackTrace] [nvarchar](max) NULL,
	[TimeStamp] [datetime] NULL,
	[Message] [nvarchar](max) NULL,
	[ActionName] [nvarchar](50) NULL,
	[EceptionType] [nvarchar](50) NULL,
 CONSTRAINT [PK_DbLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Languages]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Languages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Culture] [nvarchar](10) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[EnglishName] [nvarchar](50) NOT NULL,
	[LanguageSelectorImage] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQ_Culture] UNIQUE NONCLUSTERED 
(
	[Culture] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MonetaryCurrency]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MonetaryCurrency](
	[Id] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[News]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[imgLink] [nvarchar](max) NULL,
	[dateForShowing] [datetime] NOT NULL,
	[date] [datetime] NOT NULL,
	[note] [nvarchar](max) NULL,
 CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[News.en-US]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News.en-US](
	[id] [int] NOT NULL,
	[title] [nvarchar](250) NULL,
	[isHidden] [bit] NOT NULL,
	[body] [nvarchar](max) NULL,
	[previewBody] [nvarchar](max) NULL,
	[newsUrl] [nvarchar](270) NULL,
 CONSTRAINT [PK_News_en] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[News.ru-RU]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News.ru-RU](
	[id] [int] NOT NULL,
	[title] [nvarchar](250) NULL,
	[isHidden] [bit] NOT NULL,
	[body] [nvarchar](max) NULL,
	[previewBody] [nvarchar](max) NULL,
	[newsUrl] [nvarchar](270) NULL,
 CONSTRAINT [PK_News_ru-RU] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[id] [nvarchar](256) NOT NULL,
	[value] [nvarchar](max) NULL,
	[query] [nvarchar](256) NULL,
 CONSTRAINT [UNI_Settings] UNIQUE NONCLUSTERED 
(
	[id] ASC,
	[query] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Brokers.en-US] ADD  DEFAULT ((0)) FOR [Visible]
GO
ALTER TABLE [dbo].[Brokers.ru-RU] ADD  DEFAULT ((0)) FOR [Visible]
GO
ALTER TABLE [dbo].[CryptoCurrencyPrices] ADD  CONSTRAINT [DF_CryptoCurrencyPrices_timestamp]  DEFAULT (getdate()) FOR [timestamp]
GO
ALTER TABLE [dbo].[DbLog] ADD  DEFAULT (getdate()) FOR [TimeStamp]
GO
ALTER TABLE [dbo].[Languages] ADD  DEFAULT ('assets/img/flag.png') FOR [LanguageSelectorImage]
GO
ALTER TABLE [dbo].[News] ADD  CONSTRAINT [DF_News_date]  DEFAULT (getdate()) FOR [date]
GO
ALTER TABLE [dbo].[News.en-US] ADD  CONSTRAINT [DF__News.en-U__isVis__1881A0DE]  DEFAULT ((1)) FOR [isHidden]
GO
ALTER TABLE [dbo].[News.ru-RU] ADD  CONSTRAINT [DF__News.ru-R__isVis__14B10FFA]  DEFAULT ((1)) FOR [isHidden]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Brokers.en-US]  WITH CHECK ADD FOREIGN KEY([Id])
REFERENCES [dbo].[Brokers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Brokers.ru-RU]  WITH CHECK ADD FOREIGN KEY([Id])
REFERENCES [dbo].[Brokers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BrokersCrypto]  WITH CHECK ADD  CONSTRAINT [BrokersReference] FOREIGN KEY([BrokerId])
REFERENCES [dbo].[Brokers] ([Id])
GO
ALTER TABLE [dbo].[BrokersCrypto] CHECK CONSTRAINT [BrokersReference]
GO
ALTER TABLE [dbo].[News.en-US]  WITH CHECK ADD FOREIGN KEY([id])
REFERENCES [dbo].[News] ([id])
GO
ALTER TABLE [dbo].[News.ru-RU]  WITH CHECK ADD FOREIGN KEY([id])
REFERENCES [dbo].[News] ([id])
GO
/****** Object:  StoredProcedure [dbo].[Add_records_into_CryptoCurrencyPrices]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Add_records_into_CryptoCurrencyPrices] 
	-- Add the parameters for the stored procedure here
	@JsonValue nvarchar(max)
AS
BEGIN

/*
	declare @JsonValue nvarchar(max) = '[
		{"cryptoId":1, "price":1.1111, "lastUpdatedApi":2222},
		{"cryptoId":158, "price":1.1111, "lastUpdatedApi":2222}
		]';
*/
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into [dbo].[CryptoCurrencyPrices] (cryptoId,price, lastUpdatedApi)
		SELECT cryptoId, price, lastUpdatedApi
		FROM OPENJSON(@JsonValue,'$')
			WITH (cryptoId int '$.cryptoId',
			price float '$.price',
			lastUpdatedApi bigint '$.lastUpdatedApi') as nv
		where not exists( select * from CryptoCurrencyPrices where cryptoId = nv.cryptoId AND lastUpdatedApi = nv.lastUpdatedApi)
			and price is not null
			and lastUpdatedApi is not null;;

	delete [dbo].[CryptoCurrencyPrices]
		where timestamp < DATEADD(dd, -7, getdate());

	/*
	SELECT [cryptoId]
		  ,[price]
	  FROM [dbo].[CryptoCurrencyPrices] 
		where [cryptoId] in (
			SELECT cryptoId
			FROM OPENJSON(@JsonValue,'$')
				WITH (cryptoId int '$.cryptoId')
		)
		Order by [timestamp];
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[GetWeekAverageValues]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GetWeekAverageValues]
(
    -- Add the parameters for the stored procedure here
   @countOfDot int = 17
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    DECLARE @cryptoTableCached TABLE
	(
		timestamp datetime,
		cryptoId int, 
		price float,
		lastUpdatedApi bigint
	)

	Insert @cryptoTableCached(timestamp, cryptoId, price, lastUpdatedApi)
		select timestamp, cryptoId, price, lastUpdatedApi from CryptoCurrencyPrices;


	--store of number of records per currency
	DECLARE @cryptoCountTable TABLE
	(
	  cryptoId int, 
	  cryptoCount int
	)

	Insert @cryptoCountTable (cryptoId, cryptoCount)
		SELECT cryptoId, COUNT(*)
		  FROM @cryptoTableCached 
			group by cryptoId
			order by cryptoId;
	/*
		(select 1, 7)
		union all
		(select 3, 4);
	*/


	DECLARE @cryptoTableResult TABLE
	(
		cryptoId int,
		price float,
		iteration int
	);

	Declare @cryptoId int, @price float;

	DECLARE crypto_prices_cursor CURSOR LOCAL FOR
		SELECT cryptoId, price
		  FROM @cryptoTableCached
			where cryptoId in (select Id from CryptoСurrency)
			order by cryptoId, timestamp;
	/*
	(select 1 "cryptoId", 2 "price"
	union ALL
	select 1, 2
	union ALL
	select 1, 2
	union ALL
	select 1, 2
	union ALL
	select 1, 2
	union ALL
	select 1, 2
	union ALL
	select 1, 2 
	
	union ALL
	select 3, 2 
	union ALL
	select 3, 2 
	union ALL
	select 3, 2 
	union ALL
	select 3, 4 )
	order by cryptoId, price
	*/

    OPEN crypto_prices_cursor  
    FETCH NEXT FROM crypto_prices_cursor INTO @cryptoId, @price

	DECLARE @cryptoCount int = (Select cryptoCount from @cryptoCountTable where cryptoId = @cryptoId),
		@i int = 0;

	DECLARE @last_cryptoId int = -1, 
			@summ float = 0, 
			@quotient int, 
			@remainder int,
			@count int,
			@i2 int = 0;

	while @@FETCH_STATUS = 0  AND @i < @cryptoCount
	BEGIN

		if(@last_cryptoId != @cryptoId)
		BEGIN
			set @quotient = @cryptoCount/@countOfDot;
			set @remainder =  @cryptoCount%@countOfDot;
			set @last_cryptoId = @cryptoId;
		END
				
		set	@summ = 0;

	
		if(@remainder > 0)
		BEGIN
			set @count = @quotient +1;
			set @remainder = @remainder -1;
		END
		else
		BEGIN
			set @count = @quotient;
		END

		WHILE @@FETCH_STATUS = 0 AND @count > @i2
		BEGIN
			set @summ = @summ + @price;
			set @i2 = @i2 + 1;

			FETCH NEXT FROM crypto_prices_cursor INTO @cryptoId, @price 
		END

		set @i = @i + @i2;
		Set @i2 = 0;

		--select @last_cryptoId as "@last_cryptoId", @summ as summ, @count as count,  (@summ / @count), @quotient as "@quotient", @remainder as "@remainder";
		--select @last_cryptoId, (@summ / @count);

		insert @cryptoTableResult(cryptoId, price, iteration) values(@last_cryptoId, @summ / @count, @i);

		if(@last_cryptoId != @cryptoId)
		BEGIN
			set @cryptoCount = (Select cryptoCount from @cryptoCountTable where cryptoId = @cryptoId);
			set @i = 0;
		END
	END

	CLOSE crypto_prices_cursor  
	DEALLOCATE crypto_prices_cursor 

	select cryptoId, price/*, iteration*/ from @cryptoTableResult order by cryptoId, iteration;
END
GO
/****** Object:  StoredProcedure [dbo].[SaveListOfNames]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SaveListOfNames]
	-- Add the parameters for the stored procedure here
	@listOfNames nvarchar(max), 
	@tblName nvarchar(150)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	/*
	--DEBUG
    DECLARE @listOfNames nvarchar(max) = 'bitcoin,bitcoin-cash,cardano,ethereum,litecoin,Ripple';
	DECLARE @tblName varchar(max) = 'CryptoСurrency';
	*/

	if(Exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @tblName))
	BEGIN 

		DECLARE @SQL nvarchar(max)= N'delete [' + @tblName + '];
		INSERT INTO [' + @tblName + ' ] (Id) 
		   select * from STRING_SPLIT (@listOfNames, '','') cs;';

		--select @SQL

		EXEC SP_EXECUTESQL @SQL,  @params = N'@listOfNames nvarchar(max)', @listOfNames = @listOfNames;
	END
	else
	begin
		RAISERROR(15600,-1,-1, 'SaveListOfNames'); 
	end
END
GO
/****** Object:  Trigger [dbo].[UpdateLanguageBrokersAfterINSERT]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[UpdateLanguageBrokersAfterINSERT]
   ON  [dbo].[Brokers]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ID int,
		@OriginalName nvarchar(450),
		@TableName varchar(50),
		@SQL nvarchar(max),
		@ParmDefinition nvarchar(500);

	DECLARE table_cursor CURSOR Local FOR select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_NAME like 'Brokers.%';

	DECLARE broker_cursor CURSOR local
		FOR SELECT ID, OriginalName FROM inserted
	OPEN broker_cursor

	FETCH NEXT FROM broker_cursor
	INTO @ID, @OriginalName;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- table cursor
		OPEN table_cursor
		FETCH NEXT FROM table_cursor
			INTO @TableName;

		WHILE @@FETCH_STATUS = 0
			BEGIN
		
			set @SQL = N'insert ['+@TableName+'](ID, Name) values(@ID, @OriginalName)';
			--exec(@SQL)

			SET @ParmDefinition = N'@OriginalName nvarchar(450), @ID int';  

			EXECUTE sp_executesql @SQL, @ParmDefinition,  
						  @OriginalName = @OriginalName,
						  @ID = @ID;  

			FETCH NEXT FROM table_cursor 
					INTO @TableName;
		END
		CLOSE table_cursor;
		
		--table cursor end
		FETCH NEXT FROM broker_cursor 
				INTO @ID, @OriginalName;
	END
	CLOSE broker_cursor;
	DEALLOCATE broker_cursor;

	DEALLOCATE table_cursor;
END
GO
ALTER TABLE [dbo].[Brokers] ENABLE TRIGGER [UpdateLanguageBrokersAfterINSERT]
GO
/****** Object:  Trigger [dbo].[Languages_create_Brokers]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[Languages_create_Brokers] 
   ON  [dbo].[Languages]
   AFTER INSERT,UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	--Example: 'fr-FR'
	DECLARE @culture varchar(15);

	DECLARE culture_cursor CURSOR
		FOR SELECT culture FROM inserted
	OPEN culture_cursor

	FETCH NEXT FROM culture_cursor
	INTO @culture;

	WHILE @@FETCH_STATUS = 0
	BEGIN

	IF (NOT EXISTS (SELECT * 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'dbo' 
					 AND TABLE_NAME = Concat('Brokers.', @culture) ))
		begin

			DECLARE @SQL varchar(1600)
			select @SQL = 'CREATE TABLE [dbo].[Brokers.' + @culture + '](
				[Id] [int] NOT NULL	FOREIGN KEY REFERENCES [dbo].Brokers(Id) ON DELETE CASCADE,
				[Description] [nvarchar](max) NULL,
				[Instruments] [nvarchar](max) NULL,
				[RegulatedBy] [nvarchar](max) NULL,
				[UrlImg] [nvarchar](max) NULL,
				[Name] [nvarchar](450) NOT NULL,
				[Visible] [bit] NOT NULL,
				[MinDepositListing] [nvarchar](150) NULL,
				[RegulationListing] [nvarchar](150) NULL,
				[SpreadsListing] [nvarchar](150) NULL,
				[Regulation] [nvarchar](max) NULL,
				[BusinessNature] [nvarchar](max) NULL,
				[GoogleAddress] [nvarchar](max) NULL,
				[Leverage] [nvarchar](max) NULL,
				[Platforms] [nvarchar](max) NULL,
				[AccountTypes] [nvarchar](max) NULL,
				[LotSizes] [nvarchar](max) NULL,
				[DemoAccount] [nvarchar](max) NULL,
				[AccountCurrency] [nvarchar](max) NULL,
				[PairsOffered] [nvarchar](max) NULL,
				[PaymentMethods] [nvarchar](max) NULL,
				[Commissions] [nvarchar](max) NULL,
				[AdditionalText] [nvarchar](max) NULL,
				[ShortInfo1] [nvarchar](max) NULL,
				[ShortInfo2] [nvarchar](max) NULL,
				[EndInfo] [nvarchar](max) NULL,
			 CONSTRAINT [PK_Brokers.'+@culture+'] PRIMARY KEY CLUSTERED
			(
				[Id] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
			
			ALTER TABLE [dbo].[Brokers.'+@culture+'] ADD  DEFAULT ((0)) FOR [Visible];
			
			Insert [Brokers.'+@culture+'](Id, Name, Visible) select Id, OriginalName, 1 from Brokers;
			'
			exec (@SQL)

		END

		FETCH NEXT FROM culture_cursor 
				INTO @culture;
	END
	CLOSE culture_cursor;
	DEALLOCATE culture_cursor;

END
GO
ALTER TABLE [dbo].[Languages] ENABLE TRIGGER [Languages_create_Brokers]
GO
/****** Object:  Trigger [dbo].[Languages_create_News]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[Languages_create_News] 
   ON  [dbo].[Languages]
   AFTER INSERT,UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	--Example: 'fr-FR'
	DECLARE @culture varchar(15);

	DECLARE culture_cursor CURSOR
		FOR SELECT culture FROM inserted
	OPEN culture_cursor

	FETCH NEXT FROM culture_cursor
	INTO @culture;

	WHILE @@FETCH_STATUS = 0
	BEGIN

	IF (NOT EXISTS (SELECT * 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'dbo' 
					 AND TABLE_NAME = Concat('News.', @culture) ))
		begin

			DECLARE @SQL varchar(1600)
			select @SQL = 'CREATE TABLE [dbo].[News.'+@culture+'](
	[id] [int] NOT NULL,
	[title] [nvarchar](250) NULL,
	[isHidden] [bit] NOT NULL,
	[body] [nvarchar](max) NULL,
	[previewBody] [nvarchar](max) NULL,
 CONSTRAINT [PK_News_'+@culture+'] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

ALTER TABLE [dbo].[News.'+@culture+'] ADD  CONSTRAINT [DF__News.'+@culture+']  DEFAULT ((1)) FOR [isHidden];

ALTER TABLE [dbo].[News.'+@culture+']  WITH CHECK ADD FOREIGN KEY([id]) REFERENCES [dbo].[News] ([id]);

Insert [News.'+@culture+'](Id, IsHidden) select Id, 1 from News;
			'
			exec (@SQL)

		END

		FETCH NEXT FROM culture_cursor 
				INTO @culture;
	END
	CLOSE culture_cursor;
	DEALLOCATE culture_cursor;

END
GO
ALTER TABLE [dbo].[Languages] ENABLE TRIGGER [Languages_create_News]
GO
/****** Object:  Trigger [dbo].[UpdateLanguageNewsAfterINSERT]    Script Date: 13.06.2019 20:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[UpdateLanguageNewsAfterINSERT]
   ON  [dbo].[News]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ID int,
		@TableName varchar(50),
		@SQL nvarchar(max),
		@ParmDefinition nvarchar(500);

	DECLARE table_cursor CURSOR Local FOR select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_NAME like 'News.%';

	DECLARE news_cursor CURSOR local
		FOR SELECT ID FROM inserted
	OPEN news_cursor

	FETCH NEXT FROM news_cursor INTO @ID;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- table cursor
		OPEN table_cursor
		FETCH NEXT FROM table_cursor INTO @TableName;

		WHILE @@FETCH_STATUS = 0
		BEGIN
		
			set @SQL = N'insert ['+@TableName+'](ID) values(@ID)';
			--exec(@SQL)

			SET @ParmDefinition = N'@ID int';  

			EXECUTE sp_executesql @SQL, @ParmDefinition,
						  @ID = @ID;  

			FETCH NEXT FROM table_cursor 
					INTO @TableName;
		END
		CLOSE table_cursor;
		
		--table cursor end
		FETCH NEXT FROM news_cursor INTO @ID;
	END
	CLOSE news_cursor;
	DEALLOCATE news_cursor;

	DEALLOCATE table_cursor;
END
GO
ALTER TABLE [dbo].[News] ENABLE TRIGGER [UpdateLanguageNewsAfterINSERT]
GO
