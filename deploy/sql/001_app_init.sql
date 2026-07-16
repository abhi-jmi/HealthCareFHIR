IF DB_ID('FhirPlatform') IS NULL CREATE DATABASE FhirPlatform;
GO
USE FhirPlatform;
GO
IF OBJECT_ID('ExtensionRegistry', 'U') IS NULL
CREATE TABLE ExtensionRegistry (Id uniqueidentifier NOT NULL PRIMARY KEY, CanonicalUrl nvarchar(450) NOT NULL UNIQUE, Name nvarchar(200) NOT NULL, Description nvarchar(max) NULL, ApplicableResourceTypes nvarchar(max) NOT NULL, ValueType nvarchar(100) NOT NULL, Active bit NOT NULL DEFAULT 1);
