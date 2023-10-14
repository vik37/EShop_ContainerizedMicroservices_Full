use [EShop_OrderDb]

DROP VIEW IF EXISTS [ordering].[VW_OrderIntendedForAdminByOrderNumber];
go

CREATE VIEW [ordering].[VW_OrderIntendedForAdminByOrderNumber] 
as
	select o.Id as OrderNumber, o.OrderDate, o.[Description], 
		o.Address_Country as Country, o.Address_State as [State], o.Address_Street as Street, o.Address_City as City, o.Address_ZipCode as ZipCode, 
	os.[Name] as [Status], 
	b.[Name] as BuyerName,
	sum(oi.UnitPrice * oi.Units) as TotalPrice, sum(oi.Units) as TotalProducts, MAX(oi.UnitPrice) as MaximumPrice, MIN(oi.UnitPrice) as MinimumPrice, AVG(oi.UnitPrice) as AveragePrice
	from [ordering].[Orders] as o
	left join [ordering].[Buyers] as b
	on o.BuyerId = b.Id
	left join [ordering].[OrderStatus] as os
	on o.OrderStatusId = os.Id
	left join [ordering].[OrderItems] as oi
	on oi.OrderId = o.Id
	group by o.Id, o.OrderDate, o.[Description], 
		o.Address_Country, o.Address_State, o.Address_Street, o.Address_City, o.Address_ZipCode, 
	os.[Name], 
	b.[Name]
go

declare @Id int;
set @Id = 15;

select * from [ordering].[VW_OrderIntendedForAdminByOrderNumber] where OrderNumber = @Id
go 

DROP VIEW IF EXISTS [ordering].[VW_AdminOrdersByOrderStatus]
go

CREATE VIEW [ordering].[VW_AdminOrdersByOrderStatus]
WITH SCHEMABINDING AS
  SELECT os.Id as StatusId, os.[Name] as StatusName, O.Id as OrderNumber, O.OrderDate, B.[Name] as BuyerName
  FROM [ordering].[Orders] as o
  inner join [ordering].[OrderStatus] as os
  on o.OrderStatusId = os.Id
  inner join [ordering].[Buyers] as b
  on o.BuyerId = b.Id
go

select * from [ordering].[VW_AdminOrdersByOrderStatus] where StatusId =1 order by OrderDate asc
go

CREATE UNIQUE CLUSTERED INDEX IX__VWAdminOrdersByOrderStatus_OrderNumber ON [ordering].[VW_AdminOrdersByOrderStatus] (OrderNumber)
go
CREATE NONCLUSTERED INDEX IX_VWAdminOrdersByOrderStatus_StatusId ON [ordering].[VW_AdminOrdersByOrderStatus] (StatusId)
go
