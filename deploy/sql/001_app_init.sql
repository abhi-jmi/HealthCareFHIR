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
IF OBJECT_ID('RuleConfigurations', 'U') IS NULL
CREATE TABLE RuleConfigurations (Id uniqueidentifier NOT NULL PRIMARY KEY, RuleId nvarchar(100) NOT NULL, Version nvarchar(50) NOT NULL, DisplayName nvarchar(200) NOT NULL, ConfigurationJson nvarchar(max) NOT NULL, Active bit NOT NULL DEFAULT 1, CONSTRAINT UQ_RuleConfigurations UNIQUE (RuleId, Version));
IF OBJECT_ID('RuleExecutionAudits', 'U') IS NULL
CREATE TABLE RuleExecutionAudits (Id uniqueidentifier NOT NULL PRIMARY KEY, PatientReference nvarchar(200) NOT NULL, RuleId nvarchar(100) NOT NULL, RuleVersion nvarchar(50) NOT NULL, InputResourceIds nvarchar(max) NOT NULL, ExecutedAt datetimeoffset NOT NULL DEFAULT SYSUTCDATETIME(), Result nvarchar(100) NOT NULL, OutputResourceIds nvarchar(max) NOT NULL, InitiatedBy nvarchar(200) NOT NULL);
IF NOT EXISTS (SELECT 1 FROM RuleConfigurations WHERE RuleId = 'hba1c-review' AND Version = '1.0.0')
INSERT INTO RuleConfigurations (Id, RuleId, Version, DisplayName, ConfigurationJson, Active)
VALUES (NEWID(), 'hba1c-review', '1.0.0', 'HbA1c review reminder', '{"observationCode":"http://loinc.org|4548-4","reviewMessage":"Latest HbA1c result is available for configured clinical review. This is not medical advice."}', 1);
