USE FHIR;
GO
SELECT s.name AS SchemaName, t.name AS TableName
FROM sys.tables t
INNER JOIN sys.schemas s ON s.schema_id = t.schema_id
ORDER BY s.name, t.name;
GO
