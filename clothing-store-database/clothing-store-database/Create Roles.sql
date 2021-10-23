USE ClothingStoreDB
GO

IF EXISTS (
	SELECT * FROM sys.database_principals
	WHERE name = 'ClothingStoreOwners'
)
DROP ROLE ClothingStoreOwners

CREATE ROLE ClothingStoreOwners
GO


GRANT BACKUP DATABASE ON Database::ClothingStoreDB TO ClothingStoreOwners
GRANT BACKUP LOG ON Database::ClothingStoreDB TO ClothingStoreOwners
GRANT CREATE DEFAULT ON Database::ClothingStoreDB TO ClothingStoreOwners
GRANT CREATE FUNCTION ON Database::ClothingStoreDB TO ClothingStoreOwners
GRANT CREATE PROCEDURE ON Database::ClothingStoreDB TO ClothingStoreOwners
GRANT CREATE RULE ON Database::ClothingStoreDB TO ClothingStoreOwners
GRANT CREATE TABLE ON Database::ClothingStoreDB TO ClothingStoreOwners
GRANT CREATE VIEW ON Database::ClothingStoreDB TO ClothingStoreOwners
GO

GRANT SELECT, INSERT, DELETE, REFERENCES ON SCHEMA::dbo TO ClothingStoreOwners
GO

GRANT ALTER ON SCHEMA::dbo TO ClothingStoreOwners
GO

IF EXISTS (
	SELECT * FROM sys.database_principals
	WHERE name = 'ClothingStoreCustomers'
)
DROP ROLE ClothingStoreCustomers

CREATE ROLE ClothingStoreCustomers

GRANT SELECT ON SCHEMA :: [dbo] TO ClothingStoreCustomers
GO



IF EXISTS (
	SELECT * FROM sys.database_principals
	WHERE name = 'ClothingStoreManagers'
)
DROP ROLE ClothingStoreManagers

CREATE ROLE ClothingStoreManagers

GRANT SELECT ON SCHEMA :: [dbo] TO ClothingStoreManagers
GO


IF EXISTS (
	SELECT * FROM sys.database_principals
	WHERE name = 'ClothingStoreSystem'
)
DROP ROLE ClothingStoreSystem

CREATE ROLE ClothingStoreSystem

GRANT SELECT ON SCHEMA :: [dbo] TO ClothingStoreSystem
GO

GRANT SELECT, UPDATE, INSERT, DELETE ON SCHEMA :: [dbo] TO ClothingStoreSystem
GO

GRANT SELECT, UPDATE, INSERT, DELETE ON SCHEMA :: [dbo] TO ClothingStoreManagers
GO
