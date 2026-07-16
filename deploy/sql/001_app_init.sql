IF DB_ID('FhirPlatform') IS NULL CREATE DATABASE FhirPlatform;
GO
USE FhirPlatform;
GO
IF OBJECT_ID('ExtensionRegistry', 'U') IS NULL
CREATE TABLE ExtensionRegistry (Id uniqueidentifier NOT NULL PRIMARY KEY, CanonicalUrl nvarchar(450) NOT NULL UNIQUE, Name nvarchar(200) NOT NULL, Description nvarchar(max) NULL, ApplicableResourceTypes nvarchar(max) NOT NULL, ValueType nvarchar(100) NOT NULL, Active bit NOT NULL DEFAULT 1);
IF OBJECT_ID('SavedApiRequests', 'U') IS NULL
CREATE TABLE SavedApiRequests (Id uniqueidentifier NOT NULL PRIMARY KEY, Name nvarchar(200) NOT NULL, ResourceType nvarchar(100) NOT NULL, Interaction nvarchar(100) NOT NULL, ParametersJson nvarchar(max) NULL, CreatedAt datetimeoffset NOT NULL DEFAULT SYSUTCDATETIME());
IF OBJECT_ID('OperationalAuditCorrelations', 'U') IS NULL
CREATE TABLE OperationalAuditCorrelations (Id uniqueidentifier NOT NULL PRIMARY KEY, CorrelationId nvarchar(100) NOT NULL UNIQUE, EventType nvarchar(200) NOT NULL, FhirAuditEventReference nvarchar(200) NULL, UserSubject nvarchar(200) NULL, OccurredAt datetimeoffset NOT NULL DEFAULT SYSUTCDATETIME());
IF OBJECT_ID('IngestionJobs', 'U') IS NULL
CREATE TABLE IngestionJobs (Id uniqueidentifier NOT NULL PRIMARY KEY, IdempotencyKey nvarchar(200) NOT NULL UNIQUE, Status nvarchar(50) NOT NULL, OperationOutcomeJson nvarchar(max) NULL, CreatedAt datetimeoffset NOT NULL DEFAULT SYSUTCDATETIME(), CompletedAt datetimeoffset NULL);
