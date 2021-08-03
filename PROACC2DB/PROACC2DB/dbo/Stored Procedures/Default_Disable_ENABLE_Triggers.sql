CREATE PROCEDURE [Default_Disable_ENABLE_Triggers] 
@disable BIT = 1
AS 
    DECLARE
        @sql VARCHAR(500),
        @tableName VARCHAR(128),
        @triggerName VARCHAR(128),
        @tableSchema VARCHAR(128)

    -- List of all triggers and tables that exist on them
    DECLARE triggerCursor CURSOR
        FOR
    SELECT
        so_tr.name AS TriggerName,
        so_tbl.name AS TableName,
        t.TABLE_SCHEMA AS TableSchema
    FROM
        sysobjects so_tr
    INNER JOIN sysobjects so_tbl ON so_tr.parent_obj = so_tbl.id
    INNER JOIN INFORMATION_SCHEMA.TABLES t 
    ON 
        t.TABLE_NAME = so_tbl.name
    WHERE
        so_tr.type = 'TR'
    ORDER BY
        so_tbl.name ASC,
        so_tr.name ASC

    OPEN triggerCursor

    FETCH NEXT FROM triggerCursor 
    INTO @triggerName, @tableName, @tableSchema

    WHILE ( @@FETCH_STATUS = 0 )
        BEGIN
            IF @disable = 1 
                SET @sql = 'DISABLE TRIGGER [' 
                    + @triggerName + '] ON ' 
                    + @tableSchema + '.[' + @tableName + ']'
            ELSE 
                SET @sql = 'ENABLE TRIGGER [' 
                    + @triggerName + '] ON ' 
                    + @tableSchema + '.[' + @tableName + ']'

            PRINT 'Executing Statement - ' + @sql
            EXECUTE ( @sql )
            FETCH NEXT FROM triggerCursor 
            INTO @triggerName, @tableName,  @tableSchema
        END

    CLOSE triggerCursor
    DEALLOCATE triggerCursor