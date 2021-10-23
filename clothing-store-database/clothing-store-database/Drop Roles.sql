USE ClothingStoreDB
GO

IF EXISTS (
	SELECT * FROM sys.database_principals
	WHERE name = 'ClothingStoreOwners'
)
DROP ROLE ClothingStoreOwners


IF EXISTS (
	SELECT * FROM sys.database_principals
	WHERE name = 'ClothingStoreCustomers'
)
DROP ROLE ClothingStoreCustomers


IF EXISTS (
	SELECT * FROM sys.database_principals
	WHERE name = 'ClothingStoreManagers'
)
DROP ROLE ClothingStoreManagers


IF EXISTS (
	SELECT * FROM sys.database_principals
	WHERE name = 'ClothingStoreSystem'
)
DROP ROLE ClothingStoreSystem
