

CREATE PROCEDURE [dbo].[SP_Dashboard]
	@Type varchar(50)=null,
	@loginID uniqueidentifier=null,
	@CustomerId varchar(MAX)=null,
	@UserId varchar(MAX)=null
AS
BEGIN
		DECLARE 
		@Instance uniqueidentifier, 
		@Project   uniqueidentifier,
		@Cre_on DATETIME

		DECLARE @User_Type int ,@cnt_proj int,@cnt_T_Task_Comp int,@cnt_con int,@cnt_client int,@cnt_total_Task int
		DECLARE @ClientIDs varchar(Max),@ConsuIDs varchar(Max),@ClientNames varchar(Max),@ConsuNames varchar(Max),@All_ClientIDs varchar(Max),@All_UserIDs varchar(Max)

		DROP TABLE IF EXISTS tempdb.dbo.#Return_Table_
			CREATE TABLE #Return_Table_ (
			customerId uniqueidentifier,
			customerName varchar(500),

			ProjectId uniqueidentifier,
			ProjectName varchar(500),
			InstanceId uniqueidentifier,
			InstanceName varchar(500),
			ReadinessCheck int,
			PhaseId int,
			Total_Task int,
			Comp_Task int,
			Top5Con_ID varchar(Max),
			Top5Con_Name varchar(Max),

			Color nvarchar(MAX),
		Start_Dt nvarchar(MAX),
		End_Dt nvarchar(MAX),
		Completed int,
		TotalTask int,
		YetToStart int
			);
		

		select @User_Type=UT.UserTypeID from UserMaster M 
		LEFT JOIN User_Type UT ON M.UserTypeID=UT.UserTypeID
		where M.UserId=@loginID
		PRINT @User_Type
 IF @Type='GetDasboard_Top'
	BEGIN
			IF	@User_Type=1
			BEGIN
				DROP TABLE IF EXISTS tempdb.dbo.#Cus_Admin
				DROP TABLE IF EXISTS tempdb.dbo.#Use_Admin

				SELECT @cnt_proj=count(*) from Project where isActive=1 AND IsDeleted=0
				select  @cnt_T_Task_Comp=COUNT(*) from ProjectMonitor  where isActive=1 AND IsDeleted=0 AND StatusId= (select id from statusmaster where statusname='Completed') AND  INSTANCEID IN (select Instance_id from Instance where isActive=1)
				select @cnt_client=COUNT(*) from Customer where isActive=1 AND IsDeleted=0 
				select @cnt_con=COUNT(*) from UserMaster where isActive=1 AND IsDeleted=0 AND UserTypeID=(select UserTypeID from User_Type where UserType='Consultant')AND USERID !='00000000-0000-0000-0000-000000000000'
				select @cnt_total_Task=COUNT(*) from ProjectMonitor  where isActive=1 AND IsDeleted=0 AND StatusId != (select id from statusmaster where statusname='Not Applicable') AND  INSTANCEID IN (select Instance_id from Instance where isActive=1)

				select TOP(5) Customer_ID,Company_Name INTO #Cus_Admin from Customer where isActive=1 AND IsDeleted=0 
				select  Customer_ID INTO #All_Cus_Admin from Customer where isActive=1 AND IsDeleted=0 


				SELECT @ClientIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),Customer_ID) FROM(SELECT Customer_ID from #Cus_Admin) AS T FOR XML PATH('')),1,1,'')),
				@ClientNames=(STUFF((SELECT  ', ' + CONVERT(varchar(100),Company_Name)FROM(SELECT Company_Name from #Cus_Admin ) AS T FOR XML PATH('')),1,1,''))

				SELECT @All_ClientIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),Customer_ID) FROM(SELECT Customer_ID from #All_Cus_Admin) AS T FOR XML PATH('')),1,1,''))

				select TOP(5) USERID,[Name] INTO #Use_Admin from UserMaster  where isActive=1 AND IsDeleted=0 AND UserTypeID=(select UserTypeID from User_Type where UserType='Consultant')
				select  UserId INTO #All_User_Admin from UserMaster where isActive=1 AND IsDeleted=0 AND UserTypeID=(select UserTypeID from User_Type where UserType='Consultant')



				SELECT @ConsuIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),UserID) FROM(SELECT UserID from #Use_Admin) AS T FOR XML PATH('')),1,1,'')),
				@ConsuNames=(STUFF((SELECT  ', ' + CONVERT(varchar(100),[Name])FROM(SELECT [Name] from #Use_Admin) AS T FOR XML PATH('')),1,1,''))

				SELECT @All_UserIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),UserId) FROM(SELECT UserId from #All_User_Admin) AS T FOR XML PATH('')),1,1,''))


				SELECT @cnt_proj As proj ,@cnt_T_Task_Comp AS TotalTaskComp,@cnt_client AS client, @cnt_con AS consult, @cnt_total_Task AS TotalTask,@All_ClientIDs as All_ClientIDs
				,@ClientIDs As ClientIDs,@ClientNames as ClientNames
				,@ConsuIDs as ConsuIDs,@ConsuNames as ConsuNames,@All_UserIDs as ALL_UserIDs

				
			END

			ELSE IF @User_Type=2 --Consultant
			BEGIN
			PRINT 'IN'

				DROP TABLE IF EXISTS tempdb.dbo.#Cus_CUN
				DROP TABLE IF EXISTS tempdb.dbo.#Use_CUN

				select @cnt_proj=count(DISTINCT(P.Project_Id)) from ProjectMonitor PM RIGHT JOIN					-----Project Count
						Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
						Project P on I.Project_ID=P.Project_Id
						where USERID=@loginID AND P.isActive=1 AND I.isActive=1



				--SELECT @cnt_proj=count(*) from Project where isActive=1 AND IsDeleted=0
				select @cnt_T_Task_Comp=COUNT(*) from ProjectMonitor  where isActive=1 AND IsDeleted=0 AND StatusId= (select id from statusmaster where statusname='Completed')				-------Completed Task Count
				AND USERID=@loginID AND  INSTANCEID IN (select Instance_id from Instance where isActive=1)
				--select @cnt_client=COUNT(*) from Customer where isActive=1 AND IsDeleted=0 
				select @cnt_client=count( DISTINCT(c.Customer_ID)) from ProjectMonitor PM RIGHT JOIN					----Customer Count
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id RIGHT JOIN
					Customer c on c.Customer_ID=p.Customer_Id 
					where USERID=@loginID AND P.isActive=1 AND I.isActive=1

					

				select @cnt_con=COUNT(DISTINCT(PM.Userid)) from ProjectMonitor PM RIGHT JOIN							----Consultant	Count
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id 
					WHERE P.Project_Id IN(select DISTINCT(P.Project_Id) from ProjectMonitor PM RIGHT JOIN 
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id 
					where USERID=@loginID AND P.isActive=1) AND I.isActive=1 AND USERID !='00000000-0000-0000-0000-000000000000'

				--select @cnt_con=COUNT(DISTINCT(PM.Userid)) from ProjectMonitor PM RIGHT JOIN							----Consultant	Count
				--Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
				--Project P on I.Project_ID=P.Project_Id RIGHT JOIN 
				--UserMaster UM on UM.UserId=PM.UserID
				--WHERE P.Project_Id IN(select DISTINCT(P.Project_Id) from ProjectMonitor PM RIGHT JOIN 
				--Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
				--Project P on I.Project_ID=P.Project_Id 
				--where USERID=@loginID AND UM.isActive=1) 

					--Print @User_Type


				--select @cnt_con=COUNT(*) from UserMaster where isActive=1 AND IsDeleted=0 AND UserTypeID=(select UserTypeID from User_Type where UserType='Consultant')
				select @cnt_total_Task=COUNT(*) from ProjectMonitor  where isActive=1 AND IsDeleted=0 AND StatusId != (select id from statusmaster where statusname='Not Applicable')AND USERID=@loginID	----Chart
				AND  INSTANCEID IN (select Instance_id from Instance where isActive=1)AND USERID !='00000000-0000-0000-0000-000000000000'

				--select TOP(5) Customer_ID,Company_Name INTO #Cus_CUN from Customer where isActive=1 AND IsDeleted=0 
				select DISTINCT TOP (5)(c.Customer_ID),c.Company_Name INTO #Cus_CUN from ProjectMonitor PM RIGHT JOIN	---Top	5	Customer
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id RIGHT JOIN
					Customer c on c.Customer_ID=p.Customer_Id 
					where USERID=@loginID AND P.isActive=1
				SELECT @ClientIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),Customer_ID) FROM(SELECT Customer_ID from #Cus_CUN) AS T FOR XML PATH('')),1,1,'')),
				@ClientNames=(STUFF((SELECT  ', ' + CONVERT(varchar(100),Company_Name)FROM(SELECT Company_Name from #Cus_CUN ) AS T FOR XML PATH('')),1,1,''))



				select DISTINCT(c.Customer_ID) as Customer_ID INTO #All_Cus_Consultant from ProjectMonitor PM RIGHT JOIN	---All	Customer
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id RIGHT JOIN
					Customer c on c.Customer_ID=p.Customer_Id 
					where USERID=@loginID AND P.isActive=1
				SELECT @All_ClientIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),Customer_ID) FROM(SELECT Customer_ID from #All_Cus_Consultant) AS T FOR XML PATH('')),1,1,''))
					
					

				select DISTINCT TOP(5)(UM.UserId),UM.[Name] INTO #Use_CUN  from ProjectMonitor PM RIGHT JOIN		---TOP	5	User
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id  RIGHT JOIN 
					UserMaster UM on UM.UserId=PM.UserID
					WHERE P.Project_Id IN(select DISTINCT(P.Project_Id) from ProjectMonitor PM RIGHT JOIN 
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id 
					where USERID=@loginID AND UM.isActive=1) AND I.isActive=1 
				SELECT @ConsuIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),UserID) FROM(SELECT UserID from #Use_CUN) AS T FOR XML PATH('')),1,1,'')),
				@ConsuNames=(STUFF((SELECT  ', ' + CONVERT(varchar(100),[Name])FROM(SELECT [Name] from #Use_CUN) AS T FOR XML PATH('')),1,1,''))

				select DISTINCT(UM.UserId) INTO #All_User_Consultant  from ProjectMonitor PM RIGHT JOIN		---All	User
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id  RIGHT JOIN 
					UserMaster UM on UM.UserId=PM.UserID
					WHERE P.Project_Id IN(select DISTINCT(P.Project_Id) from ProjectMonitor PM RIGHT JOIN 
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id 
					where USERID=@loginID AND UM.isActive=1) AND I.isActive=1
				SELECT @All_UserIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),UserId) FROM(SELECT UserId from #All_User_Consultant) AS T FOR XML PATH('')),1,1,''))


				--Result
				SELECT @cnt_proj As proj ,@cnt_T_Task_Comp AS TotalTaskComp,@cnt_client AS client, @cnt_con AS consult, @cnt_total_Task AS TotalTask,
				@All_ClientIDs as All_ClientIDs
				,@ClientIDs As ClientIDs,@ClientNames as ClientNames

				,@ConsuIDs as ConsuIDs,@ConsuNames as ConsuNames,
				@All_UserIDs as ALL_UserIDs
			END
			ELSE IF @User_Type=3 --Customer
			BEGIN
				DROP TABLE IF EXISTS tempdb.dbo.#Cus_CUN1
				DROP TABLE IF EXISTS tempdb.dbo.#Use_CUN1
				
				select @cnt_proj=count(DISTINCT(P.Project_Id)) from Project P 
						RIGHT JOIN UserMaster U on U.UserId=@loginID
						where P.Customer_Id=U.Customer_Id AND p.isactive=1



				select @cnt_T_Task_Comp=COUNT(*) from ProjectMonitor  where isActive=1 AND IsDeleted=0 AND StatusId= (select id from statusmaster where statusname='Completed')				-------Completed Task Count
				AND InstanceID in (select Instance_id from Instance I 
						WHERE I.Project_Id IN (select Project_Id from Project P 
						RIGHT JOIN UserMaster U on U.UserId=@loginID
						where P.Customer_Id=U.Customer_Id AND P.isActive=1)AND I.isActive=1)


				select @cnt_client=count( DISTINCT(c.Customer_ID)) from Customer c
						RIGHT JOIN UserMaster U on U.UserId=@loginID
						where c.Customer_Id=U.Customer_Id

					select @cnt_con=COUNT(DISTINCT(PM.Userid)) from ProjectMonitor PM WHERE 						----Consultant	Count
					InstanceID in (select Instance_id from Instance I 
						WHERE I.Project_Id IN (select Project_Id from Project P 
						RIGHT JOIN UserMaster U on U.UserId=@loginID
						where P.Customer_Id=U.Customer_Id AND P.isActive=1)AND I.isActive=1) AND USERID !='00000000-0000-0000-0000-000000000000'

				select @cnt_total_Task=COUNT(*) from ProjectMonitor  where isActive=1 AND IsDeleted=0 AND StatusId != (select id from statusmaster where statusname='Not Applicable')--AND USERID=@loginID	----Chart
				AND InstanceID in (select Instance_id from Instance I 
						WHERE I.Project_Id IN (select Project_Id from Project P 
						RIGHT JOIN UserMaster U on U.UserId=@loginID
						where P.Customer_Id=U.Customer_Id AND P.isActive=1)AND I.isActive=1)


					 SELECT DISTINCT TOP (5)(c.Customer_ID),c.Company_Name INTO #Cus_CUN1 from Customer c
						RIGHT JOIN UserMaster U on U.UserId=@loginID
						where c.Customer_Id=U.Customer_Id 


				SELECT @ClientIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),Customer_ID) FROM(SELECT Customer_ID from #Cus_CUN1) AS T FOR XML PATH('')),1,1,'')),
				@ClientNames=(STUFF((SELECT  ', ' + CONVERT(varchar(100),Company_Name)FROM(SELECT Company_Name from #Cus_CUN1 ) AS T FOR XML PATH('')),1,1,''))



				select DISTINCT(c.Customer_ID) as Customer_ID INTO #All_Cus_Consultant1 from ProjectMonitor PM RIGHT JOIN	---All	Customer
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id RIGHT JOIN
					Customer c on c.Customer_ID=p.Customer_Id 
					where USERID=@loginID AND P.isActive=1

				--select DISTINCT(c.Customer_ID) as Customer_ID INTO #All_Cus_Consultant1 from Customer c
				--		RIGHT JOIN UserMaster U on U.UserId=@loginID
				--		where c.Customer_Id=U.Customer_Id


				SELECT @All_ClientIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),Customer_ID) FROM(SELECT Customer_ID from #All_Cus_Consultant1) AS T FOR XML PATH('')),1,1,''))

				--select DISTINCT TOP(5)(UM.UserId),UM.[Name] INTO #Use_CUN1  from ProjectMonitor PM RIGHT JOIN		---TOP	5	User
				--	Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
				--	Project P on I.Project_ID=P.Project_Id  RIGHT JOIN 
				--	UserMaster UM on UM.UserId=PM.UserID
				--	WHERE P.Project_Id IN(select DISTINCT(P.Project_Id) from ProjectMonitor PM RIGHT JOIN 
				--	Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
				--	Project P on I.Project_ID=P.Project_Id 
				--	where USERID=@loginID AND UM.isActive=1)

				select DISTINCT TOP(5)(UM.UserId) AS UserID,UM.[Name] INTO #Use_CUN1 from ProjectMonitor PM
						LEFT JOIN Instance I on I.Instance_id=PM.InstanceID
						LEFT JOIN UserMaster UM on UM.UserId=PM.UserID
						where Project_ID IN (select Project_Id from Project P 
						RIGHT JOIN UserMaster U on U.UserId=@loginID
						where P.Customer_Id=U.Customer_Id AND p.isactive=1) AND UM.UserId IS NOT NULL AND I.isActive=1

				SELECT @ConsuIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),UserID) FROM(SELECT UserID from #Use_CUN1) AS T FOR XML PATH('')),1,1,'')),
				@ConsuNames=(STUFF((SELECT  ', ' + CONVERT(varchar(100),[Name])FROM(SELECT [Name] from #Use_CUN1) AS T FOR XML PATH('')),1,1,''))

				select DISTINCT(UM.UserId) INTO #All_User_Consultant1  from ProjectMonitor PM --RIGHT JOIN		---All	User
					LEFT JOIN UserMaster UM on UM.UserId=PM.UserID
					WHERE PM.InstanceID in (select Instance_id from Instance I 
						WHERE I.Project_Id IN (select Project_Id from Project P 
						RIGHT JOIN UserMaster U on U.UserId=@loginID
						where P.Customer_Id=U.Customer_Id AND P.isActive=1)AND I.isActive=1) AND UM.USERID !='00000000-0000-0000-0000-000000000000'

				SELECT @All_UserIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),UserId) FROM(SELECT UserId from #All_User_Consultant1) AS T FOR XML PATH('')),1,1,''))


				--Result
				SELECT @cnt_proj As proj ,@cnt_T_Task_Comp AS TotalTaskComp,@cnt_client AS client, @cnt_con AS consult, @cnt_total_Task AS TotalTask,
				@All_ClientIDs as All_ClientIDs
				,@ClientIDs As ClientIDs,@ClientNames as ClientNames

				,@ConsuIDs as ConsuIDs,@ConsuNames as ConsuNames,
				@All_UserIDs as ALL_UserIDs
			END
			ELSE IF @User_Type=4
			BEGIN
				DROP TABLE IF EXISTS tempdb.dbo.#Cus_PM
				DROP TABLE IF EXISTS tempdb.dbo.#Use_PM

				select @cnt_proj=count(*) from Project where isActive=1 and ProjectManager_Id=@loginID
				--select @cnt_proj=count(DISTINCT(P.Project_Id)) from ProjectMonitor PM RIGHT JOIN 
				--		Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
				--		Project P on I.Project_ID=P.Project_Id
				--		where ProjectManager_Id=@loginID AND P.isActive=1 AND I.isActive=1

				--SELECT @cnt_proj=count(*) from Project where isActive=1 AND IsDeleted=0
				--select @cnt_T_Task_Comp=COUNT(*) from ProjectMonitor  where isActive=1 AND IsDeleted=0 AND StatusId= (select id from statusmaster where statusname='Completed') AND USERID=@loginID

					select @cnt_T_Task_Comp=COUNT(*) from ProjectMonitor  where isActive=1 AND IsDeleted=0 
					AND StatusId= (select id from statusmaster where statusname='Completed') 
							AND InstanceID IN(select DISTINCT(I.Instance_id) from ProjectMonitor PM RIGHT JOIN 
								Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
								Project P on I.Project_ID=P.Project_Id
								where I.Instance_id IS NOT NULL AND ProjectManager_Id=@loginID AND P.isActive=1 AND I.isActive=1) 


				--select @cnt_client=COUNT(*) from Customer where isActive=1 AND IsDeleted=0 
				select @cnt_client=count( DISTINCT(c.Customer_ID)) from ProjectMonitor PM RIGHT JOIN 
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id RIGHT JOIN
					Customer c on c.Customer_ID=p.Customer_Id 
					WHERE  P.ProjectManager_Id=@loginID AND P.isActive=1 AND I.isActive=1





				select @cnt_con=COUNT(DISTINCT(PM.Userid)) from ProjectMonitor PM RIGHT JOIN 
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id 
					WHERE  P.ProjectManager_Id=@loginID AND P.isActive=1 AND I.isActive=1 AND USERID !='00000000-0000-0000-0000-000000000000'



				--select @cnt_con=COUNT(*) from UserMaster where isActive=1 AND IsDeleted=0 AND UserTypeID=(select UserTypeID from User_Type where UserType='Consultant')
				select @cnt_total_Task=COUNT(*) from ProjectMonitor  where isActive=1 AND IsDeleted=0 AND StatusId != (select id from statusmaster where statusname='Not Applicable')
				AND InstanceID IN(select DISTINCT(I.Instance_id) from ProjectMonitor PM RIGHT JOIN 
								Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
								Project P on I.Project_ID=P.Project_Id
								where I.Instance_id IS NOT NULL AND ProjectManager_Id=@loginID AND P.isActive=1 AND I.isActive=1) 

				--select TOP(5) Customer_ID,Company_Name INTO #Cus_PM from Customer where isActive=1 AND IsDeleted=0 
				select DISTINCT TOP (5)(c.Customer_ID),c.Company_Name INTO #Cus_PM from ProjectMonitor PM RIGHT JOIN 
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id RIGHT JOIN
					Customer c on c.Customer_ID=p.Customer_Id 
					where  P.ProjectManager_Id=@loginID AND P.isActive=1

				
				select  Customer_ID INTO #All_Cus_PM from Customer where isActive=1 AND IsDeleted=0 


				SELECT @ClientIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),Customer_ID) FROM(SELECT Customer_ID from #Cus_PM) AS T FOR XML PATH('')),1,1,'')),
				@ClientNames=(STUFF((SELECT  ', ' + CONVERT(varchar(100),Company_Name)FROM(SELECT Company_Name from #Cus_PM ) AS T FOR XML PATH('')),1,1,''))

				SELECT @All_ClientIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),Customer_ID) FROM(SELECT Customer_ID from #All_Cus_PM) AS T FOR XML PATH('')),1,1,''))

				--select  UserId INTO #All_User_PM from UserMaster where isActive=1 AND IsDeleted=0 AND UserTypeID=(select UserTypeID from User_Type where UserType='Consultant')

				select DISTINCT (UM.UserId),UM.[Name] INTO #All_User_PM  from ProjectMonitor PM RIGHT JOIN 
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id  RIGHT JOIN 
					UserMaster UM on UM.UserId=PM.UserID
					WHERE P.ProjectManager_Id=@loginID AND P.isActive=1 AND I.isActive=1
				SELECT @All_UserIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),UserId) FROM(SELECT UserId from #All_User_PM) AS T FOR XML PATH('')),1,1,''))


				select DISTINCT TOP(5)(UM.UserId),UM.[Name] INTO #Use_PM  from ProjectMonitor PM RIGHT JOIN 
					Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
					Project P on I.Project_ID=P.Project_Id  RIGHT JOIN 
					UserMaster UM on UM.UserId=PM.UserID
					WHERE P.ProjectManager_Id=@loginID AND P.isActive=1 AND I.isActive=1

				--select TOP(5) USERID,[Name] INTO #Use_PM from UserMaster  where isActive=1 AND IsDeleted=0 AND UserTypeID=(select UserTypeID from User_Type where UserType='Consultant')

				SELECT @ConsuIDs=(STUFF((SELECT  ', ' + CONVERT(varchar(100),UserID) FROM(SELECT UserID from #Use_PM) AS T FOR XML PATH('')),1,1,'')),
				@ConsuNames=(STUFF((SELECT  ', ' + CONVERT(varchar(100),[Name])FROM(SELECT [Name] from #Use_PM) AS T FOR XML PATH('')),1,1,''))

				SELECT @cnt_proj As proj ,@cnt_T_Task_Comp AS TotalTaskComp,@cnt_client AS client, @cnt_con AS consult, @cnt_total_Task AS TotalTask,@All_ClientIDs as All_ClientIDs
				,@ClientIDs As ClientIDs,@ClientNames as ClientNames
				,@ConsuIDs as ConsuIDs,@ConsuNames as ConsuNames,@All_UserIDs as ALL_UserIDs
			END
	END
ELSE IF @Type='GetDasboard_Donut'
BEGIN

	IF @User_Type=1
	BEGIN
			
		DECLARE cursor_Inst CURSOR
		FOR select  DISTINCT(i.Instance_id),p.Project_Id,i.Cre_on from Project p LEFT JOIN Instance i on i.Project_ID=p.Project_Id WHERE Instance_id IS NOT NULL AND i.isActive=1

		OPEN cursor_Inst;
		FETCH NEXT FROM cursor_Inst INTO 
		@Instance,@Project,@Cre_on;

		WHILE @@FETCH_STATUS = 0
		BEGIN
		INSERT INTO #Return_Table_ EXEC SP_Dashboard_Card  @Type ='Get_CardDetails',@InstanceID =@Instance
		FETCH NEXT FROM cursor_Inst INTO 
			@Instance,@Project,@Cre_on;
		END;
		SELECT * from #Return_Table_ 
		CLOSE cursor_Inst;
		DEALLOCATE cursor_Inst;
	END
	ELSE IF @User_Type=2 
	BEGIN
			
		DECLARE cursor_Inst CURSOR
		FOR select DISTINCT(I.Instance_id),P.Project_Id,i.Cre_on from ProjectMonitor PM RIGHT JOIN 
				Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
				Project P on I.Project_ID=P.Project_Id  
				WHERE P.Project_Id IN (select DISTINCT(P.Project_Id) from ProjectMonitor PM RIGHT JOIN 
				Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
				Project P on I.Project_ID=P.Project_Id 
		where USERID=@loginID AND i.isActive=1) ORDER BY i.Cre_on 

		OPEN cursor_Inst;
		FETCH NEXT FROM cursor_Inst INTO 
		@Instance, 
		@Project,
		@Cre_on;

		WHILE @@FETCH_STATUS = 0
		BEGIN
		INSERT INTO #Return_Table_ EXEC SP_Dashboard_Card  @Type ='Get_CardDetails',@InstanceID =@Instance 
		FETCH NEXT FROM cursor_Inst INTO 
			@Instance, 
			@Project,
			@Cre_on;
		END;
		SELECT * from #Return_Table_ 
		CLOSE cursor_Inst;
		DEALLOCATE cursor_Inst;
	END
	ELSE IF @User_Type=3
	BEGIN
		DECLARE cursor_Inst CURSOR
		FOR select  DISTINCT(i.Instance_id),p.Project_Id,i.Cre_on from Project p 
		LEFT JOIN Instance i on i.Project_ID=p.Project_Id 
		WHERE Instance_id IS NOT NULL AND i.isActive=1
		AND Instance_id IN(select Instance_id from Instance I 
						WHERE I.Project_Id IN (select Project_Id from Project P 
						RIGHT JOIN UserMaster U on U.UserId=@loginID
						where P.Customer_Id=U.Customer_Id AND P.isActive=1)AND I.isActive=1)ORDER BY i.Cre_on 
		
		--select DISTINCT(I.Instance_id),P.Project_Id,i.Cre_on from ProjectMonitor PM RIGHT JOIN 
		--		Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
		--		Project P on I.Project_ID=P.Project_Id  
		--		WHERE P.Project_Id IN (select DISTINCT(P.Project_Id) from ProjectMonitor PM RIGHT JOIN 
		--		Instance I on PM.InstanceID=I.Instance_id RIGHT JOIN 
		--		Project P on I.Project_ID=P.Project_Id 
		--where USERID=@loginID AND i.isActive=1) ORDER BY i.Cre_on 

		OPEN cursor_Inst;
		FETCH NEXT FROM cursor_Inst INTO 
		@Instance, 
		@Project,
		@Cre_on;

		WHILE @@FETCH_STATUS = 0
		BEGIN
		INSERT INTO #Return_Table_ EXEC SP_Dashboard_Card  @Type ='Get_CardDetails',@InstanceID =@Instance 
		FETCH NEXT FROM cursor_Inst INTO 
			@Instance, 
			@Project,
			@Cre_on;
		END;
		SELECT * from #Return_Table_ 
		CLOSE cursor_Inst;
		DEALLOCATE cursor_Inst;
	END
	ELSE IF @User_Type=4
	BEGIN
		DECLARE cursor_Inst CURSOR
		FOR select  DISTINCT(i.Instance_id),p.Project_Id,i.Cre_on from Project p LEFT JOIN Instance i on i.Project_ID=p.Project_Id WHERE Instance_id IS NOT NULL AND P.ProjectManager_Id=@loginID AND P.isActive=1 AND i.isActive=1
		ORDER BY i.Cre_on 

		OPEN cursor_Inst;
		FETCH NEXT FROM cursor_Inst INTO 
		@Instance, 
		@Project,
		@Cre_on;

		WHILE @@FETCH_STATUS = 0
		BEGIN
		INSERT INTO #Return_Table_ EXEC SP_Dashboard_Card  @Type ='Get_CardDetails',@InstanceID =@Instance
		FETCH NEXT FROM cursor_Inst INTO 
			@Instance, 
			@Project,
			@Cre_on;
		END;
		SELECT * from #Return_Table_ 
		CLOSE cursor_Inst;
		DEALLOCATE cursor_Inst;
	END
END

 IF @Type='GetCustomerDetailDashboard_Top'
	BEGIN			
Select * ,
(select Industry_Sector from  IndustrySector where IndustrySector_ID=C.IndustrySector_ID)[IndustryName]
from Customer C  where  CAST(C.Customer_ID as nvarchar(MAX)) IN (@CustomerId) AND C.isActive=1 order by C.Cre_on desc 
END

IF @Type='GetConsultantDetailDashboard_Top'
	BEGIN
Select * ,
--(select UserType from  User_Type where UserTypeID=(select UserTypeID from User_Type where UserType='Consultant'))[UserType],
(select UserType from User_Type where UserTypeID=U.UsertypeId)[UserType],
(select RoleName from RoleMaster where RoleID=U.RoleID)[RoleName]
from UserMaster U  where  CAST(U.UserId as nvarchar(MAX)) IN (@UserId) AND U.isActive=1 order by U.Cre_on desc 
END


END

