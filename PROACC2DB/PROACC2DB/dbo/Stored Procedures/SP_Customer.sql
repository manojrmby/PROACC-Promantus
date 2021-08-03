CREATE PROCEDURE [dbo].[SP_Customer]
	@Type varchar(100) =null,
	@Id uniqueidentifier =null,
	@CompanyName varchar(Max) =null,
	@IndustrySector_ID int =null,
	@Contact varchar(Max) =null,
	@Countrycode varchar(Max) =null,
	@Customer_DialCode varchar(Max)=null,
	@Phone varchar(Max) =null,
	@Email varchar(Max) =null,
	@Cre_By uniqueidentifier =null,
	@Ts timestamp=null
AS
BEGIN
	
	SET NOCOUNT ON;
	if @Type='GetCustomers'
	BEGIN
	select *, 
	(select Industry_Sector from industrysector i where i.IndustrySector_ID = C.IndustrySector_ID)[IndustrySector]
	from Customer C where isActive=1 order by Cre_on desc
	END

	if @Type='GetIndustrySector'
	BEGIN
	select *, IndustrySector_ID from IndustrySector where IsActive=1
	END

	if @Type='CustomerCreate'
	BEGIN
	insert into Customer (Customer_ID,Company_Name,IndustrySector_ID,Contact,Countrycode,DialCode, Phone,Email,Cre_on,Cre_By)
	values (NEWID(),@CompanyName,@IndustrySector_ID,@Contact,@Countrycode,@Customer_DialCode,@Phone,@Email,GETUTCDATE(),@Cre_By)
	END
	
	if @Type='CheckCustomersNameAvailability'
	BEGIN
	select * from Customer where isActive=1 and Company_Name=@CompanyName
	END

	if @Type='CheckCustomersNameAvailability1'
	BEGIN
	select * from Customer where isActive=1 and Company_Name=@CompanyName and Customer_ID!=@Id
	END

	if @Type='GetCustomerById'
	BEGIN
	select * from Customer where Customer_ID=@Id and isActive=1
	END

	if @Type='CustomerUpdate'
	BEGIN
	DECLARE @t TABLE (myKey uniqueidentifier);  
	Update Customer set Company_Name=@CompanyName,IndustrySector_ID=@IndustrySector_ID,Contact=@Contact,Countrycode=@Countrycode,DialCode=@Customer_DialCode ,Phone=@Phone,Email=@Email,
	Modified_by=@Cre_By,Modified_On=GETUTCDATE() 
	OUTPUT inserted.Customer_ID INTO @t(myKey)   
	where Customer_ID=@Id and Ts=@Ts
	--return 2
	--select * from @t
	if (select count(*) from @t)>0
	BEGIN
	RETURN 0
	END
	ELSE IF(select count(*) from Customer where Customer_ID=@Id)=0
	BEGIN
	RETURN 1
	END
	ELSE IF(select count(*) from Customer where Customer_ID=@Id and TS=@Ts)=0
	BEGIN
	RETURN 2
	END

	END
		
	if @Type='DeleteCustomerById'
	BEGIN
	Declare @Cust_Id int
	set @Cust_Id=(select Count(P.Customer_Id) from Customer C left join Project P on C.Customer_ID = P.Customer_Id where P.isactive=1 and C.Customer_ID=@Id)
	if @Cust_Id=0
	Begin
	Update Customer set isActive=0,IsDeleted=1 where Customer_ID=@Id
	End
	Else
	Begin
	SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)
	End
	END
END
