CREATE TABLE [dbo].[Customer] (
    [Customer_ID]       UNIQUEIDENTIFIER NOT NULL,
    [Company_Name]      NVARCHAR (50)    NOT NULL,
    [IndustrySector_ID] INT              NOT NULL,
    [Contact]           NVARCHAR (50)    NULL,
    [Countrycode]       NVARCHAR (50)    NULL,
    [DialCode]          NCHAR (10)       NULL,
    [Phone]             NVARCHAR (20)    NULL,
    [Email]             NVARCHAR (50)    NULL,
    [isActive]          BIT              CONSTRAINT [DF_Customer_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]            DATETIME         CONSTRAINT [DF_Customer_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]            UNIQUEIDENTIFIER NOT NULL,
    [Modified_On]       DATETIME         NULL,
    [Modified_by]       UNIQUEIDENTIFIER NULL,
    [IsDeleted]         BIT              CONSTRAINT [DF_Customer_IsDeleted] DEFAULT ((0)) NOT NULL,
    [Ts]                ROWVERSION       NOT NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Customer_ID] ASC),
    CONSTRAINT [FK_Customer_IndustrySector] FOREIGN KEY ([IndustrySector_ID]) REFERENCES [dbo].[IndustrySector] ([IndustrySector_ID])
);






GO
Create TRIGGER [dbo].[Trg_Customer]
	ON [dbo].[Customer]
	FOR DELETE, INSERT, UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		DECLARE @UserId uniqueidentifier;
		DECLARE @Summary varchar(max);
		DECLARE @Tablename varchar(150)='customer';
		DECLARE @TablID varchar(150);
		DECLARE @Action varchar(150);

		DECLARE @Operation varchar(7) = 
		CASE WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) 
			THEN 'UPDATE'
		WHEN EXISTS(SELECT * FROM inserted) 
			THEN 'INSERT'
		WHEN EXISTS(SELECT * FROM deleted)
			THEN 'DELETE'
		ELSE 
			NULL --Unknown
		END;

		IF @Operation='UPDATE' OR @Operation='INSERT'
			BEGIN
				--SELECT * from UserMaster
				SET @Summary=(
				SELECT 
					ISNULL(CAST(i.Customer_ID as varchar(max)),'')+','
					+ISNULL(CAST(i.Company_Name as varchar(max)),'')+','
					+ISNULL(CAST(i.IndustrySector_ID as varchar(max)),'')+','
					+ISNULL(CAST(i.Contact as varchar(max)),'')+','
					+ISNULL(CAST(i.Countrycode as varchar(max)),'')+','
				    +ISNULL(CAST(i.DialCode as varchar(max)),'')+','
					+ISNULL(CAST(i.Phone as varchar(max)),'')+','
					+ISNULL(CAST(i.Email as varchar(max)),'')+','

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from Customer A inner join inserted i on A.Customer_ID=i.Customer_ID)
				SELECT  
				@UserId= 
				CASE 
				WHEN @Operation='INSERT' OR @Operation='UPDATE' THEN  i.Cre_By
				ELSE i.Modified_by  END,
				@TablID=i.Customer_ID,
				@Action=@Operation
				from Customer A inner join inserted i on A.Customer_ID=i.Customer_ID
			END
		Else IF @Operation='DELETE'
		BEGIN
		SET @Summary=(
				SELECT 
					ISNULL(CAST(i.Customer_ID as varchar(max)),'')+','
					+ISNULL(CAST(i.Company_Name as varchar(max)),'')+','
					+ISNULL(CAST(i.IndustrySector_ID as varchar(max)),'')+','
					+ISNULL(CAST(i.Contact as varchar(max)),'')+','
					+ISNULL(CAST(i.Countrycode as varchar(max)),'')+','
					+ISNULL(CAST(i.DialCode as varchar(max)),'')+','
					+ISNULL(CAST(i.Phone as varchar(max)),'')+','
					+ISNULL(CAST(i.Email as varchar(max)),'')+','

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from deleted i)
			SET @UserId=(select cast(cast(0 as binary) as uniqueidentifier));
				SELECT  
				@TablID=i.Customer_ID,
				@Action=@Operation
				from deleted i
		END
		IF @Operation IS NOT NULL
		BEGIN
		EXEC SP_Audit 
		@Type='AddToAduit',
		@Summary=@Summary,
		@UserId=@UserId,
		@Tablename=@Tablename,
		@TablID=@TablID,
		@Action=@Action
		END
	END