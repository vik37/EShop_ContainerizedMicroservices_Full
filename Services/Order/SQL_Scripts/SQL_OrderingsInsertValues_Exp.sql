use [EShop_OrderingDb]

select *
from [ordering].[Address]

insert into [ordering].[Address] (City,Country,[State],Street,ZipCode)
values ('Skopje','Macedonia',null,'bul. ASNOM 22/A','1000')

select *
from [ordering].[Buyers]

insert into  [ordering].[Buyers] (IdentityGuid)
values ('7e1161ec-3118-490a-935b-ba931c275018')

select *
from [ordering].[CardTypes]

insert into [ordering].[CardTypes] ([Name])
values ('Amex')
go

insert into [ordering].[CardTypes] ([Name])
values ('Visa')
go

insert into [ordering].[CardTypes] ([Name])
values ('MasterCard')
go

insert into [ordering].[CardTypes] ([Name])
values ('Capital One')
go

select *
from [ordering].[OrderStatus]

insert into [ordering].[OrderStatus] ([Name])
values ('Submitted')
go

insert into [ordering].[OrderStatus] ([Name])
values ('AwaitingValidation')
go

insert into [ordering].[OrderStatus] ([Name])
values ('StockConfirmed')
go

insert into [ordering].[OrderStatus] ([Name])
values ('Paid')
go

insert into [ordering].[OrderStatus] ([Name])
values ('Shipped')
go

insert into [ordering].[OrderStatus] ([Name])
values ('Cancelled')
go

select *
from [ordering].[PaymentMethods]

insert into [ordering].[PaymentMethods](Alias,BuyerId,CardHolderName,CardNumber,CardTypeId,Expiration)
values ('A37317672402294',1,'marce','123',3,'2025-05-12')

select *
from [ordering].[OrderItems]

insert into [ordering].[OrderItems] (Discount,OrderId,ProductName,UnitPrice,Units)
values (8.23,3,'Mudd',12.45,3)
insert into [ordering].[OrderItems] (Discount,OrderId,ProductName,UnitPrice,Units)
values (12.23,3,'Dot Net Mravka',2.75,1)

select *
from [ordering].[Orders]

insert into [ordering].[Orders] (AddressId,BuyerId,OrderDate,OrderStatusId,PaymentMethodId)
values(1,1,'2017-05-29',3,1)