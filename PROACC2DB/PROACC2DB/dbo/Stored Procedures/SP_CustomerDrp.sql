
CREATE PROCEDURE [dbo].[SP_CustomerDrp]
	@Type varchar(50)=null,
	@Id varchar(50)=null
AS
BEGIN
IF @Type='CustomerDrp_Admin'
BEGIN
	select Customer_ID As id,Company_Name AS[Name] from Customer  where isActive=1 AND IsDeleted=0 order by Company_Name
END
	IF @Type='CustomerDrp_Consultant'
	BEGIN
		select DISTINCT Customer.Customer_ID  As id,Company_Name AS[Name] from Customer 
		join Project on (Project.Customer_Id = Customer.Customer_ID)
		join Instance on (Instance.Project_ID=Project.Project_Id)
		join ProjectMonitor on ProjectMonitor.InstanceID=Instance.Instance_id where UserID=@Id AND Customer.isActive=1 and Instance.isActive=1 order by Company_Name
	END
	IF @Type='CustomerDrp_Customer'
	BEGIN
		SELECT * from Customer where isActive=1 AND IsDeleted=0 AND Customer_ID=@Id order by Company_Name
	END
END
