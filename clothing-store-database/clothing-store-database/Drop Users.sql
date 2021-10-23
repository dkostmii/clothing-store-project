USE ClothingStoreDB
GO

IF USER_ID('clothstorea') IS NOT NULL
BEGIN
	ALTER ROLE ClothingStoreOwners DROP MEMBER clothstorea
	DROP USER clothstorea
END
GO

IF SUSER_ID('clothstorea') IS NOT NULL
BEGIN
	DROP LOGIN clothstorea
END
GO

-----------------------------------------------------------------------------------------------

IF USER_ID('clothstore_customer') IS NOT NULL
BEGIN
	ALTER ROLE ClothingStoreCustomers DROP MEMBER clothstore_customer
	DROP USER clothstore_customer
END
GO

IF SUSER_ID('clothstore_customer') IS NOT NULL
BEGIN
	DROP LOGIN clothstore_customer
END
GO

-----------------------------------------------------------------------------------------------

IF USER_ID('clothstore_manager') IS NOT NULL
BEGIN
	ALTER ROLE ClothingStoreManagers DROP MEMBER clothstore_manager
	DROP USER clothstore_manager
END
GO

IF SUSER_ID('clothstore_manager') IS NOT NULL
BEGIN
	DROP LOGIN clothstore_manager
END
GO

-----------------------------------------------------------------------------------------------

IF USER_ID('clothstores') IS NOT NULL
BEGIN
	ALTER ROLE ClothingStoreSystem DROP MEMBER clothstores
	DROP USER clothstores
END
GO

IF SUSER_ID('clothstores') IS NOT NULL
BEGIN
	DROP LOGIN clothstores
END
GO
