CREATE PROCEDURE [Catalog].[LoadClient]
(
           @CityId int
           ,@Code nvarchar(12)
           ,@Name nvarchar(max)
           ,@BusinessName nvarchar(50)
           ,@LegalRepresentative nvarchar(50)
           ,@FTR nvarchar(13)
           ,@TaxAddress nvarchar(max)
           ,@Address nvarchar(max)
           ,@ZipCode nvarchar(max)
           ,@Entrance datetime
           ,@Email nvarchar(30)
           ,@Phone nvarchar(20)
           ,@IsActive bit
           ,@UpdDate datetime
		   ,@UpdUser varchar(100)
)
AS
BEGIN


DECLARE @ClientId INT = 0

SELECT TOP 1 @ClientId = ISNULL(ClientId,0)
FROM Catalog.Client
WHERE Name LIKE '%'+@Name+'%' OR Code = @Code

If @ClientId = 0
BEGIN
	INSERT INTO [Catalog].[Client]
           ([CityId]
           ,[Code]
           ,[Name]
           ,[BusinessName]
           ,[LegalRepresentative]
           ,[FTR]
           ,[TaxAddress]
           ,[Address]
           ,[ZipCode]
           ,[Entrance]
           ,[Email]
           ,[Phone]
           ,[IsActive]
           ,[UpdDate]
		   ,[UpdUser])
     VALUES
           (@CityId
           ,@Code
           ,@Name
           ,@BusinessName
           ,@LegalRepresentative
           ,@FTR
           ,@TaxAddress
           ,@Address
           ,@ZipCode
           ,@Entrance
           ,@Email
           ,@Phone
           ,@IsActive
           ,@UpdDate
		   ,@UpdUser)
END
END



GO
CREATE PROCEDURE [Catalog].[LoadProvider]
(
           @CityId int
           ,@Code nvarchar(12)
           ,@Name nvarchar(max)
           ,@BusinessName nvarchar(50)
           ,@FTR nvarchar(13)
           ,@Address nvarchar(max)
           ,@ZipCode nvarchar(max)
           ,@Email nvarchar(30)
           ,@Phone nvarchar(20)
           ,@IsActive bit
           ,@UpdDate datetime
		   ,@UpdUser varchar(100)
		   
)
AS
BEGIN


DECLARE @ProviderId INT = 0

SELECT TOP 1 @ProviderId = ISNULL(ProviderId,0)
FROM Catalog.Provider
WHERE Name LIKE '%'+@Name+'%' OR Code = @Code

If @ProviderId = 0
BEGIN
	INSERT INTO [Catalog].[Provider]
           ([CityId]
           ,[Code]
           ,[Name]
           ,[BusinessName]
           ,[FTR]
           ,[Address]
           ,[ZipCode]
           ,[Email]
           ,[Phone]
           ,[IsActive]
           ,[UpdDate]
		   ,[UdpUser])
     VALUES
           (@CityId
           ,@Code
           ,@Name
           ,@BusinessName
           ,@FTR
           ,@Address
           ,@ZipCode
           ,@Email
           ,@Phone
           ,@IsActive
           ,@UpdDate
		   ,@UpdUser)
END
END

GO

CREATE PROCEDURE [Catalog].[LoadProduct]
(
	@CategoryId int
    ,@Code nvarchar(30)
    ,@Name nvarchar(200)
    ,@Description nvarchar(200)
    ,@MinQuantity float
    ,@BarCode nvarchar(max)
    ,@BuyPrice float
    ,@StorePercentage int
    ,@DealerPercentage int
    ,@WholesalerPercentage int
    ,@StorePrice float
    ,@WholesalerPrice float
    ,@DealerPrice float
    ,@TradeMark nvarchar(50)
	,@Ledge nvarchar(50)
    ,@Row nvarchar(50)
    ,@Unit nvarchar(20)
	,@IsActive bit
    ,@UpdDate datetime
	,@UpdUser varchar(100)
)
AS
BEGIN

DECLARE @ProductId INT = 0

SELECT @ProductId = ISNULL(ProductId,0)
FROM Catalog.Product
WHERE Code=@Code

IF @ProductId = 0
BEGIN
	INSERT INTO [Catalog].[Product]
			   ([CategoryId]
			   ,[Code]
			   ,[Name]
			   ,[Description]
			   ,[MinQuantity]
			   ,[BarCode]
			   ,[BuyPrice]
			   ,[StorePercentage]
			   ,[DealerPercentage]
			   ,[WholesalerPercentage]
			   ,[StorePrice]
			   ,[WholesalerPrice]
			   ,[DealerPrice]
			   ,[TradeMark]
			   ,[Unit]
			   ,[Row]
			   ,[Ledge]
			   ,[IsActive]
			   ,[UpdDate]
			   ,[UpdUser])
		 VALUES
			   (@CategoryId
			   ,@Code
			   ,@Name
			   ,@Description
			   ,@MinQuantity
			   ,@BarCode
			   ,@BuyPrice
			   ,@StorePercentage
			   ,@DealerPercentage
			   ,@WholesalerPercentage
			   ,@StorePrice
			   ,@WholesalerPrice
			   ,@DealerPrice
			   ,@TradeMark
			   ,@Unit
			   ,@Row
			   ,@Ledge
			   ,@IsActive
			   ,@UpdDate
			   ,@UpdUser)

END
END

