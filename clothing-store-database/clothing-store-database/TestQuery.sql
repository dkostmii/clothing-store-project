USE ClothingStoreDB
GO

/*
SELECT * FROM Products
GO

SELECT * FROM Sizes
GO

SELECT * FROM dbo.Types
GO

Select * from Materials
GO

DELETE FROM Managers
GO

DELETE FROM Users
GO


SELECT * FROM Users
GO

SELECT * FROM Managers
GO

SELECT * FROM Customers
GO
*/
/*
DELETE FROM ProductImages
GO

DELETE FROM Products
GO*/

/*
SELECT * FROM Products
GO

SELECT * FROM ProductImages
GO*/

SELECT * FROM Users
GO

SELECT * FROM Customers
GO

UPDATE Customers
SET ShippingInfoId = NULL
WHERE Id = 1

DELETE FROM ShippingInfos
GO

SELECT * FROM Products
GO

SELECT * FROM Orders
GO

DELETE FROM Orders

SELECT * FROM CartPositions
GO

DELETE FROM CartPositions
GO

SELECT * FROM ShippingInfos
GO



UPDATE Customers
SET ShippingInfoId = 1
WHERE UserId = 3;
GO

USE ClothingStoreDB
GO

UPDATE Customers
SET Balance = 9999.99
GO
