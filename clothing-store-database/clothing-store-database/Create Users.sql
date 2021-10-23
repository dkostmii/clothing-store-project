USE ClothingStoreDB
GO

CREATE LOGIN clothstorea
	WITH PASSWORD = 'qwerty1234'
GO

CREATE USER clothstorea
FOR LOGIN clothstorea
GO

ALTER ROLE ClothingStoreOwners ADD MEMBER clothstorea
GO

-----------------------------------------------------------------------------------------------

CREATE LOGIN clothstore_customer
	WITH PASSWORD = 'qwerty1234'
GO

CREATE USER clothstore_customer
FOR LOGIN clothstore_customer
GO

ALTER ROLE ClothingStoreCustomers ADD MEMBER clothstore_customer
GO

-----------------------------------------------------------------------------------------------

CREATE LOGIN clothstore_manager
	WITH PASSWORD = 'qwerty1234'
GO

CREATE USER clothstore_manager
FOR LOGIN clothstore_manager
GO

ALTER ROLE ClothingStoreManagers ADD MEMBER clothstore_manager
GO

-----------------------------------------------------------------------------------------------

CREATE LOGIN clothstores
	WITH PASSWORD = 'qwerty1234'
GO

CREATE USER clothstores
FOR LOGIN clothstores
GO

ALTER ROLE ClothingStoreSystem ADD MEMBER clothstores
GO
