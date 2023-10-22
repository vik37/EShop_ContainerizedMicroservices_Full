use [EShop_OrderDb]

insert into [ordering].[Buyers] (Id,IdentityGuid,[Name])
values (1,'d2f4f0e3-9c9c-406c-bd0a-409a1942305f','Pavel'),
	   (2,'0fb9a52c-5eaf-4ca0-89e0-b78066922682','Kristina'),
	   (3,'57f74177-cd42-4261-8f34-f778ac4ef669','Simona'),
	   (4,'661a9359-9dd4-4f3c-a592-722ce4f9709d','Marko'),
	   (5,'afc9ed91-f00f-4475-aa1e-6682a89a8c34','Dimitar'),
	   (6,'ae5e8b08-6590-4537-af1b-a8194a48afe8','Spase'),
	   (7,'8a1f3828-e7ee-4e0f-ab8c-72700705e8b8','Brendon'),
	   (8,'5565b9db-d611-4169-ba56-996b0060787c','Bruce'),
	   (9,'63cbebb3-2e7d-4e60-bfd9-56b7361c8da6','Jennifer'),
	   (10,'d9d4a02a-d59a-4614-9422-eed28761a160','Göran'),
	   (11,'2b1d649b-5f82-4fa2-826e-0b026cd3c0af','Issabela'),
	   (12,'cfa3bab6-14c2-4c44-9c7b-5fc0c1d18f01','Hasan'),
	   (13,'e7631793-40bf-4bee-85c8-35c46cc9a4fd','Ibraim'),
	   (14,'d2cf89a7-e5f0-4cb6-a780-7f5a5b210d25','Orce');
go

select *
from [ordering].[Buyers]
go

insert into [ordering].[PaymentMethods] (Id,CardTypeId,BuyerId,Alias,CardHolderName,CardNumber,Expiration)
values (1,1,1,'Payment Method on: 10/09/2023 13:07:56','Pavel Sotirovski','6781-0000-2821-8844','2025-12-9 00:00:00.0000000'),
		(2,1,2,'Payment Method on: 09/23/2023 17:21:16','Kristina Spasovska','0000-2200-2899-2904','2026-12-01 00:00:00.0000000'),
		(3,1,3,'Payment Method on: 10/20/2023 19:27:06','Simona Kostova','7892-3242-9992-8093','2025-02-01 00:00:00.0000000'),
		(4,1,4,'Payment Method on: 10/20/2023 13:07:56','Marko Sotirovski','4489-4389-3421-1846','2025-05-01 00:00:00.0000000'),
		(5,1,5,'Payment Method on: 10/21/2023 13:07:56','Dimitar Miladinov','6666-0430-0431-5523','2026-12-01 00:00:00.0000000'),
		(6,1,6,'Payment Method on: 10/21/2023 08:27:54','Spase Arsovski','6494-8023-8873-2238','2027-03-01 00:00:00.0000000'),
		(7,1,7,'Payment Method on: 10/03/2023 13:27:28','Mr. Brendon Hill','4443-0300-2055-3541','2025-09-01 00:00:00.0000000'),
		(8,1,8,'Payment Method on: 10/20/2023 15:37:50','Bruce Willis','7898-3924-3429-4394','2028-12-01 00:00:00.0000000'),
		(9,1,9,'Payment Method on: 10/21/2023 05:07:19','Jenifer Lewis','8903-3904-7893-6271','2026-04-01 00:00:00.0000000'),
		(10,1,10,'Payment Method on: 10/21/2023 18:29:06','Göran Håkansson','4879-37982-8362','2025-12-01 00:00:00.0000000'),
		(11,1,11,'Payment Method on: 10/21/2023 17:07:26','Issabela Flores','7893-2292-3902-3892','2025-10-01 00:00:00.0000000'),
		(12,1,12,'Payment Method on: 10/20/2023 15:09:56','Hasan Hussain','7893-1119-4888-0134','2025-02-01 00:00:00.0000000'),
		(13,1,13,'Payment Method on: 10/14/2023 07:27:32','Ibraim Mustafa','4402-7821-9883-4372','2026-10-01 00:00:00.0000000'),
		(14,1,14,'Payment Method on: 10/21/2023 13:27:14','Orce Nikolov','0000-1111-2222-9943','2028-05-01 00:00:00.0000000'),
		(15,1,4,'Payment Method on: 10/20/2023 13:07:56','Marko Sotirovski','4489-4389-3421-1846','2025-05-01 00:00:00.0000000'),
		(16,1,1,'Payment Method on: 10/09/2023 13:07:56','Pavel Sotirovski','6781-0000-2821-8844','2025-12-9 00:00:00.0000000');
		
go

select *
from [ordering].[PaymentMethods]
go

insert into [ordering].[Orders] (Id,Address_City,Address_State,Address_Country,Address_Street,Address_ZipCode,OrderStatusId,[Description],BuyerId,OrderDate,PaymentMethodId)
values (1,'Skopje','Skopje District','Macedonia','Prespanska 15','1000',1,null,1, DATEADD(DAY, -20, GETDATE()),1),
		(2,'Skopje','Skopje District','Macedonia','ASNOM 12/A','1000',5,null,2,DATEADD(MONTH, -1, GETDATE()),2),
		(3,'Prilep','Pelagonija District','Macedonia','Prilepska 235','7500',2,null,3,DATEADD(DAY, -4, GETDATE()),3),
		(4,'Skopje','Skopje District','Macedonia','Partizanska 195','1000',3,null,4,DATEADD(DAY, -15, GETDATE()),4),
		(5,'Struga','Ohrid District','Macedonia','Crn Drim 48','6330',1,null,5,DATEADD(DAY, 0, GETDATE()),5),
		(6,'Skopje','Skopje District','Macedonia','ASNOM 94/A','1000',1,null,6,DATEADD(DAY, -3, GETDATE()),6),
		(7,'Cork City','Munster','Republic of Ireland','st. Patrick 200','D08',6,null,7,DATEADD(WEEK, -3, GETDATE()),7),
		(8,'London','London Town','United Kingdon','Oxford 98','EC1A 1AE ',1,null,8,DATEADD(DAY, -1, GETDATE()),8),
		(9,'Detroit','Michigen','USA','Aaron 402','48223',1,null,9,DATEADD(HOUR, -4, GETDATE()),9),
		(10,'Borås','Götaland','Sverige','Fatbursgatan 539','503',1,null,10,DATEADD(DAY, -10, GETDATE()),10),
		(11,'Seville','Andalusia','Spain','Avenida de la Constitución 15','41001',1,null,11,DATEADD(WEEK, -5, GETDATE()),11),
		(12,'Alexandria','Mediteran','Egypt','15th street','22206',1,null,12,DATEADD(DAY, -9, GETDATE()),12),
		(13,'Kicevo','Kicevo District','Macedonia','Treska 15','6250',5,null,13,DATEADD(WEEK, -1, GETDATE()),13),
		(14,'Skopje','Skopje District','Macedonia','Maksim Gorki 22','1000',1,null,14,DATEADD(MONTH, -1, GETDATE()),14),
		(15,'Skopje','Skopje District','Macedonia','Fushtanksa 12','1000',3,null,4,DATEADD(DAY, -5, GETDATE()),15),
		(16,'Skopje','Skopje District','Macedonia','Prespanska 15','1000',1,null,1, DATEADD(DAY, -2, GETDATE()),6);
go

select *
from [ordering].[Orders]
go

insert into [ordering].[OrderItems] (Id,ProductId,OrderId,Discount,PictureUrl,ProductName,UnitPrice,Units)
values (1,4,1,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/4/image','.NET Foundation T-shirt',12.0000,3),
		(2,7,2,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/7/image','Roslyn Red T-Shirt',12.0000,2),
		(3,4,2,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/4/image','.NET Foundation T-shirt',12.0000,1),
		(4,12,3,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/12/image','Prism White TShirt',12.0000,3),
		(5,11,3,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/11/image','Cup<T> Sheet',8.5000,2),
		(6,8,3,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/8/image','Kudu Purple Hoodie',8.5000,1),
		(7,1,4,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/1/image','.NET Bot Black Hoodie',19.5000,2),
		(8,10,4,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/10/image','.NET Foundation Sheet',12.0000,1),
		(9,5,5,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/5/image','Roslyn Red Sheet',8.5000,5),
		(10,7,6,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/7/image','Roslyn Red T-Shirt',12.0000,1),
		(11,8,6,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/8/image','Kudu Purple Hoodie',8.5000,1),
		(12,3,7,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/3/image','Prism White T-Shirt',12.0000,3),
		(13,11,8,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/11/image','Cup<T> Sheet',8.5000,2),
		(14,2,8,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/2/image','.NET Black & White Mug',8.5000,2),
		(15,8,8,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/8/image','Kudu Purple Hoodie',8.5000,1),
		(16,12,9,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/12/image','Prism White TShirt',12.0000,1),
		(17,9,9,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/9/image','Cup<T> White Mug',12.0000,4),
		(18,8,10,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/8/image','Kudu Purple Hoodie',8.5000,3),
		(19,11,10,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/11/image','Cup<T> Sheet',8.5000,1),
		(20,10,12,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/10/image','.NET Foundation Sheet',12.0000,1),
		(21,6,13,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/6/image','.NET Blue Hoodie',12.0000,5),
		(22,7,14,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/7/image','Roslyn Red T-Shirt',12.0000,1),
		(23,11,15,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/11/image','Cup<T> Sheet',8.5000,1),
		(24,5,5,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/5/image','Roslyn Red Sheet',8.5000,5),
		(25,4,16,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/4/image','.NET Foundation T-shirt',12.0000,2),
		(26,8,16,0,'http://host.docker.internal:9010/gw/v1.0/catalog/items/8/image','Kudu Purple Hoodie',8.5000,2);

select *
from [ordering].[OrderItems]