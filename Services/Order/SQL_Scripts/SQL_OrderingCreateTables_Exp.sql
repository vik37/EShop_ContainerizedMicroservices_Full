use [EShop_OrderingDb]
go



drop table if exists [ordering].[Buyers]
go
drop table if exists [ordering].[CardTypes]
go
drop table if exists [ordering].[Address]
go
drop table if exists [ordering].[OrderStatus]
go
drop table if exists [ordering].[PaymentMethods]
go
drop table if exists [ordering].[Orders]
go
drop table if exists [ordering].[OrderItems]
go

create table [ordering].[Buyers]
(
	Id int  identity(1,1) not null,
	IdentityGuid nvarchar(200) not null

	constraint [PK_Buyers] primary key clustered
	(
	    [Id]
	)
)

create table [ordering].[CardTypes]
(
	Id int  identity(1,1) not null,
	[Name] nvarchar(200) not null

	constraint [PK_CardTypes] primary key clustered
	(
	    [Id]
	)
)

create table [ordering].[Address]
(
	Id int  identity(1,1) not null,
	City nvarchar(500) null,
	Country nvarchar(550) null,
	[State] nvarchar(550) null,
	Street nvarchar(650) null,
	ZipCode nvarchar(450) null

	constraint [PK_Address] primary key clustered
	(
	    [Id]
	)
)

create table [ordering].[OrderStatus]
(
	Id int  identity(1,1) not null,
	[Name] nvarchar(200) not null

	constraint [PK_OrderStatus] primary key clustered
	(
	    [Id]
	)
)

create table [ordering].[PaymentMethods]
(
	Id int  identity(1,1) not null,
	[Alias] nvarchar(200) not null,
	[BuyerId] int not null,
	CardHolderName nvarchar(250) not null,
	CardNumber nvarchar(25) not null,
	CardTypeId int not null,
	Expiration DateTime not null

	constraint [PK_PaymentMethods] primary key clustered
	(
	    [Id]
	),
	constraint FK_PaymentMethods_BuyerId foreign key(BuyerId) references [ordering].[Buyers],
	constraint FK_PaymentMethods_CardTypeId foreign key(CardTypeId) references [ordering].[CardTypes]
)

create table [ordering].[Orders]
(
	Id int identity(1,1) not null,
	AddressId int null,
	BuyerId int not null,
	OrderDate DateTime  not null,
	OrderStatusId int not null,
	PaymentMethodId int not null

	constraint [PK_Orders] primary key clustered
	(
	    [Id]
	),
	constraint FK_Order_BuyerId foreign key(BuyerId) references [ordering].[Buyers],
	constraint FK_Order_OrderStatuId foreign key(OrderStatusId) references [ordering].[OrderStatus],
	constraint FK_Order_PaymentMethodId foreign key(PaymentMethodId) references [ordering].[PaymentMethods]
)

create table [ordering].[OrderItems]
(
	Id int identity(1,1) not null,
	Discount decimal(18,2) not null,
	OrderId int null,
	ProductName nvarchar(350) not null,
	UnitPrice decimal(18,2) not null,
	Units int not null,

	constraint [PK_OrderItems] primary key clustered
	(
	    [Id]
	),
	constraint FK_OrderItems_OrderId foreign key(OrderId) references [ordering].[Orders]
)