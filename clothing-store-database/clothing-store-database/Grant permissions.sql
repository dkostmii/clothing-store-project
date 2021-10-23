USE ClothingStoreDB
GO

GRANT BACKUP DATABASE ON DATABASE::ClothingStoreDB TO [ClothingStoreOwners]
GRANT BACKUP LOG ON SCHEMA :: [dbo] TO ClothingStoreOwners
GRANT CREATE DATABASE ON SCHEMA :: [dbo] TO ClothingStoreOwners
GRANT CREATE DEFAULT ON SCHEMA :: [dbo] TO ClothingStoreOwners
GRANT CREATE FUNCTION ON SCHEMA :: [dbo] TO ClothingStoreOwners
GRANT CREATE PROCEDURE ON SCHEMA :: [dbo] TO ClothingStoreOwners
GRANT CREATE RULE ON SCHEMA :: [dbo] TO ClothingStoreOwners
GRANT CREATE TABLE ON SCHEMA :: [dbo] TO ClothingStoreOwners
GRANT CREATE VIEW ON SCHEMA :: [dbo] TO ClothingStoreOwners
GO
