USE ClothingStoreDB
GO

IF EXISTS (
	SELECT * FROM sys.database_principals
	WHERE name = 'ClothingStoreOwners'
)
DROP ROLE ClothingStoreOwners


CREATE ROLE ClothingStoreOwners

GRANT ALL PRIVILEGES ON SCHEMA :: [dbo] TO ClothingStoreOwners




IF EXISTS (
	SELECT * FROM sys.database_principals
	WHERE name = 'ClothingStoreCustomers'
)
DROP ROLE ClothingStoreCustomers

CREATE ROLE ClothingStoreCustomers

GRANT SELECT ON SCHEMA :: [dbo] TO ClothingStoreCustomers



IF EXISTS (
	SELECT * FROM sys.database_principals
	WHERE name = 'ClothingStoreManagers'
)
DROP ROLE ClothingStoreManagers

CREATE ROLE ClothingStoreManagers

GRANT SELECT ON SCHEMA :: [dbo] TO ClothingStoreManagers
