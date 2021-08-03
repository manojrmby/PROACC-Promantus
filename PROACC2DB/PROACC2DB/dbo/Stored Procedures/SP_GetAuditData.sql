

CREATE PROCEDURE [dbo].[SP_GetAuditData]
	@Audit int =null
AS
BEGIN
	IF OBJECT_ID(N'tempdb..##TempPM') IS NOT NULL
	BEGIN
		DROP TABLE ##TempPM
	END
	SELECT AUDIT_ID,USERID,TABLE_NAME,TABLE_ID,
	SUMMARY
	,[ACTION],CREATED_DATE INTO ##TempPM from AuditData
		
		DECLARE @Audit_Count int
		DECLARE @newSummary varchar(max)
		DECLARE @Counter INT 
		DECLARE @TableName varchar(max)
		DECLARE @Task varchar(max)
		CREATE TABLE #tmp
			(
			ID INT IDENTITY(1, 1) ,
			[AUDIT_ID] [int]  NOT NULL,
			[USERID] [uniqueidentifier] NOT NULL,
			[TABLE_NAME] [nvarchar](150) NOT NULL,
			[TABLE_ID] [nvarchar](max) NOT NULL,
			[SUMMARY] [nvarchar](max) NOT NULL,
			[ACTION] [nvarchar](150) NOT NULL,
			[CREATED_DATE] [datetime] NOT NULL
			);

			Insert INTO #tmp SELECT * from AuditData

		SELECT @Audit_Count=count(*) FROM #tmp
		SET @Counter=1
		WHILE ( @Counter <= @Audit_Count)
		BEGIN
			IF OBJECT_ID(N'tempdb..#TableOne') IS NOT NULL
			BEGIN
			DROP TABLE #TableOne
			END
			IF OBJECT_ID(N'tempdb..#FinalLine') IS NOT NULL
			BEGIN
			DROP TABLE #FinalLine
			END
			CREATE TABLE #TableOne
			( 
			ID_IDENTITY int not null IDENTITY (1,1) , 
			InFo varchar(max)
			)
			--Drop table #TableOne
			--Drop table #FinalLine
		     SELECT @TableName=TABLE_NAME from #tmp where  ID=@Counter
			
			 IF @TableName='ActivityMaster'
			 BEGIN
			 Print 'ActivityMaster'
					IF OBJECT_ID(N'tempdb..#AM_Line') IS NOT NULL
					BEGIN
					DROP TABLE #AM_Line
					END
				    Insert into #TableOne (InFo)(select value As Info  from #tmp CROSS APPLY STRING_SPLIT(summary, ',') where  ID=@Counter)
					SELECT Pvt.* Into #AM_Line
					FROM (
					SELECT *
					FROM #TableOne T
					)AS P
					PIVOT (MIN(InFo) FOR ID_IDENTITY IN (
					[1],[2],[3],[4],[5],[6],[7],
					[8],[9],[10],[11],[12]
					))Pvt
				SELECT @newSummary=
				--' Activity ID:'+ISNULL(CAST(F.[1] as varchar(max)),'')+','
				' Task :'+ISNULL(CAST(F.[2] as varchar(max)),'')+','
				+' Block:'+ISNULL(CAST(B.Block_Name as varchar(max)),'')+','
				+' PhaseName:'+ISNULL(CAST(P.PhaseName as varchar(max)),'')+','
				----+' Sequence:'+ISNULL(CAST(F.[5] as varchar(max)),'')+','
				+' Application Area:'+ISNULL(CAST(AM.ApplicationArea as varchar(max)),'')+','
				+' Role :'+ISNULL(CAST(R.RoleName as varchar(max)),'')+','
				+' Is Active :'+
				CASE 
				WHEN F.[12] =1 THEN CAST('Active' as varchar(max))
				ELSE CAST('DELETED' as varchar(max)) END
				from #AM_Line F
				INNER JOIN BuildingBlock B on F.[3]=B.block_ID
				INNER JOIN PhaseMaster P on F.[4]=P.Id
				INNER JOIN ApplicationAreaMaster AM on F.[6]=AM.Id
				INNER JOIN RoleMaster R on F.[7]=R.RoleId
			 END
			 
			ELSE IF @TableName='Customer'
			 BEGIN
			 Print 'Customer'
					IF OBJECT_ID(N'tempdb..#CM_Line') IS NOT NULL
					BEGIN
					DROP TABLE #CM_Line
					END
				    Insert into #TableOne (InFo)(select value As Info  from #tmp CROSS APPLY STRING_SPLIT(summary, ',') where  ID=@Counter)
					SELECT Pvt.* Into #CM_Line
					FROM (
					SELECT *
					FROM #TableOne T
					)AS P
					PIVOT (MIN(InFo) FOR ID_IDENTITY IN (
					[1],[2],[3],[4],[5],[6],[7],
					[8],[9],[10],[11],[12],[13],[14]
					))Pvt
				SELECT @newSummary=
				--' Customer ID:'+ISNULL(CAST(U.[1] as varchar(max)),'')+','
				' Company :'+ISNULL(CAST(U.[2] as varchar(max)),'')+','
				+' Industry Sector :'+ISNULL(CAST(I.Industry_Sector as varchar(max)),'')+','
				+' Contact :'+ISNULL(CAST(U.[4] as varchar(max)),'')+','
				----+' Countrycode:'+ISNULL(CAST(F.[5] as varchar(max)),'')+','
				+' Phone :'+ISNULL(CAST(U.[7] as varchar(max)),'')+','
				+' E Mail:'+ISNULL(CAST(U.[8] as varchar(max)),'')+','
				
				
				
				+' Is Active :'+
				CASE 
				WHEN U.[9] =1 THEN CAST('Active' as varchar(max))
				ELSE CAST('DELETED' as varchar(max)) END

				from #CM_Line U
				INNER JOIN IndustrySector I on U.[3]=I.IndustrySector_ID
					
			 END
			 
			 ELSE IF @TableName='Instance'
			 BEGIN
			 Print 'Instance'
					IF OBJECT_ID(N'tempdb..#I_Line') IS NOT NULL
					BEGIN
					DROP TABLE #I_Line
					END
				    Insert into #TableOne (InFo)(select value As Info  from #tmp CROSS APPLY STRING_SPLIT(summary, ',') where  ID=@Counter)
					SELECT Pvt.* Into #I_Line
					FROM (
					SELECT *
					FROM #TableOne T
					)AS P
					PIVOT (MIN(InFo) FOR ID_IDENTITY IN (
					[1],[2],[3],[4],[5],[6],[7],
					[8],[9],[10],[11],[12]
					))Pvt
				SELECT @newSummary=
				--' Instance ID:'+ISNULL(CAST(U.[1] as varchar(max)),'')+','
				' Instance name :'+ISNULL(CAST(U.[2] as varchar(max)),'')+','
				+' Project:'+ISNULL(CAST(P.Project_Name as varchar(max)),'')+','
				+' Description:'+ISNULL(CAST(U.[8] as varchar(max)),'')+','
				--+' Customer :'+ISNULL(CAST(c.Company_Name as varchar(max)),'')+','
				--+' Project Manager :'+ISNULL(CAST(UM.[Name] as varchar(max)),'')+','
				
				+' Is Active :'+
				CASE 
				WHEN U.[7] =1 THEN CAST('Active' as varchar(max))
				ELSE CAST('DELETED' as varchar(max)) END

				from #I_Line U
				INNER JOIN Project P on U.[3]=P.Project_Id
				
				
			 END

			ELSE IF @TableName='PMTaskMaster'
			 BEGIN
			 Print 'PMTaskMaster'
					IF OBJECT_ID(N'tempdb..#PMM_Line') IS NOT NULL
					BEGIN
					DROP TABLE #PMM_Line
					END
				    Insert into #TableOne (InFo)(select value As Info  from #tmp CROSS APPLY STRING_SPLIT(summary, ',') where  ID=@Counter)
					SELECT Pvt.* Into #PMM_Line
					FROM (
					SELECT *
					FROM #TableOne T
					)AS P
					PIVOT (MIN(InFo) FOR ID_IDENTITY IN (
					[1],[2],[3],[4],[5],[6],[7],
					[8],[9],[10]
					))Pvt
				SELECT @newSummary=
				
				' PM Task Name :'+ISNULL(CAST(U.[2] as varchar(max)),'')+','
				+' PM Task Category :'+ISNULL(CAST(PMTC.[Name] as varchar(max)),'')+','
				+' ESt hours :'+ISNULL(CAST(U.[4] as varchar(max)),'')+','
				
				+' Is Active :'+
				CASE 
				WHEN U.[5] =1 THEN CAST('Active' as varchar(max))
				ELSE CAST('DELETED' as varchar(max)) END
				from #PMM_Line U
				INNER JOIN PMTaskCategory PMTC on U.[3]=PMTC.ID
			 END
			 ELSE IF @TableName='Project'
			 BEGIN
			 Print 'Project'
					IF OBJECT_ID(N'tempdb..#P_Line') IS NOT NULL
					BEGIN
					DROP TABLE #P_Line
					END
				    Insert into #TableOne (InFo)(select value As Info  from #tmp CROSS APPLY STRING_SPLIT(summary, ',') where  ID=@Counter)
					SELECT Pvt.* Into #P_Line
					FROM (
					SELECT *
					FROM #TableOne T
					)AS P
					PIVOT (MIN(InFo) FOR ID_IDENTITY IN (
					[1],[2],[3],[4],[5],[6],[7],
					[8],[9],[10],[11],[12]
					))Pvt
				SELECT @newSummary=
				--' Project ID:'+ISNULL(CAST(U.[1] as varchar(max)),'')+','
				' Project name :'+ISNULL(CAST(U.[2] as varchar(max)),'')+','
				+' Description:'+ISNULL(CAST(U.[3] as varchar(max)),'')+','
				+' Customer :'+ISNULL(CAST(c.Company_Name as varchar(max)),'')+','
				+' Project Manager :'+ISNULL(CAST(UM.[Name] as varchar(max)),'')+','
				+' Scenario :'+ISNULL(CAST(s.ScenarioName as varchar(max)),'')+','
				
				+' Is Active :'+
				CASE 
				WHEN U.[7] =1 THEN CAST('Active' as varchar(max))
				ELSE CAST('DELETED' as varchar(max)) END

				from #P_Line U
				INNER JOIN Customer c on U.[4]=c.Customer_ID
				INNER JOIN UserMaster UM on U.[5]=UM.UserId
				INNER JOIN ScenarioMaster s on U.[6]=s.ScenarioId
					
			 END

			 ELSE IF @TableName='RoleMaster'
			 BEGIN
			 Print 'RoleMaster'
					IF OBJECT_ID(N'tempdb..#R_Line') IS NOT NULL
					BEGIN
					DROP TABLE #R_Line
					END
				    Insert into #TableOne (InFo)(select value As Info  from #tmp CROSS APPLY STRING_SPLIT(summary, ',') where  ID=@Counter)
					SELECT Pvt.* Into #R_Line
					FROM (
					SELECT *
					FROM #TableOne T
					)AS P
					PIVOT (MIN(InFo) FOR ID_IDENTITY IN (
					[1],[2],[3],[4],[5],[6],[7],[8],[9]
					))Pvt
				SELECT @newSummary=
				' Role name :'+ISNULL(CAST(U.[2] as varchar(max)),'')+','
				+' Description:'+ISNULL(CAST(U.[9] as varchar(max)),'')+','
				+' Is Active :'+
				CASE 
				WHEN U.[3] =1 THEN CAST('Active' as varchar(max))
				ELSE CAST('DELETED' as varchar(max)) END
				from #R_Line U
				
				
			 END

			 ELSE IF @TableName='UserMaster'
			 BEGIN
			 Print 'UserMaster'
					IF OBJECT_ID(N'tempdb..#UM_Line') IS NOT NULL
					BEGIN
					DROP TABLE #UM_Line
					END
				    Insert into #TableOne (InFo)(select value As Info  from #tmp CROSS APPLY STRING_SPLIT(summary, ',') where  ID=@Counter)
					SELECT Pvt.* Into #UM_Line
					FROM (
					SELECT *
					FROM #TableOne T
					)AS P
					PIVOT (MIN(InFo) FOR ID_IDENTITY IN (
					[1],[2],[3],[4],[5],[6],[7],
					[8],[9],[10],[11],[12],[13],[14],[15],[16],[17]
					))Pvt
				SELECT @newSummary=
				--' USER ID:'+ISNULL(CAST(U.[1] as varchar(max)),'')+','
				' Name :'+ISNULL(CAST(U.[2] as varchar(max)),'')+','
				+' E Mail:'+ISNULL(CAST(U.[3] as varchar(max)),'')+','
				--+' Countrycode:'+ISNULL(CAST(F.[4] as varchar(max)),'')+','
				+' Phone :'+ISNULL(CAST(U.[6] as varchar(max)),'')+','
				+' Login Id :'+ISNULL(CAST(U.[7] as varchar(max)),'')+','
				--+' Password:'+ISNULL(CAST(U.[8] as varchar(max)),'')+','
				--+' Role :'+ISNULL(CAST(R.RoleName as varchar(max)),'')+','
				+' User Type:'+ISNULL(CAST(UT.UserType as varchar(max)),'')
				--+' Customer:'+ISNULL(CAST(C.Company_Name as varchar(max)),'')
				+CASE 
				WHEN U.[11]!='00000000-0000-0000-0000-000000000000'THEN +','+' Customer :'+ (SELECT Company_Name FROM customer  WHERE Customer_ID= U.[11])
				ELSE ''END
				+','
				+' Is Active :'+
				CASE 
				WHEN U.[12] =1 THEN CAST('Active' as varchar(max))
				ELSE CAST('DELETED' as varchar(max)) END

				from #UM_Line U
				--INNER JOIN RoleMaster R on U.[9]=R.RoleId
				INNER JOIN User_Type UT on U.[10]=UT.Usertypeid 
				--INNER JOIN Customer C on U.[11]=C.Customer_ID
 
				--INNER JOIN Buldingblock B on F.[3]=B.block_ID
				--INNER JOIN PhaseMaster P on F.[4]=P.Id
				--INNER JOIN ApplicationAreaMaster AM on F.[6]=AM.Id
				
			 END
			
			
			ELSE IF @TableName='ProjectMonitor'
			 BEGIN

				Insert into #TableOne (InFo)(select value As Info  from #tmp CROSS APPLY STRING_SPLIT(summary, ',') where  ID=@Counter)
				SELECT Pvt.* Into #FinalLine
				FROM (
				SELECT *
				FROM #TableOne T
				)AS P
				PIVOT (MIN(InFo) FOR ID_IDENTITY IN (
				[1],[2],[3],[4],[5],[6],[7],
				[8],[9],[10],[11],[12],[13],[14],
				[15],[16],[17],[18],[19],[20],[21],
				[22],[23],[24],[25]
				))Pvt

				SELECT @newSummary=
				'Task: '+ISNULL( CAST(A.[Task] as varchar(max)),'')+','+
				'Instance: '+ISNULL( CAST(I.InstaceName as varchar(max)),'')+','+
				' PhaseName:'+ISNULL(CAST(P.PhaseName as varchar(max)),'')+','+

				--' Task_Other_Environment:'+ISNULL(CAST(F.[5] as varchar(max)),'')+','+
				--' Dependency:'+ISNULL(CAST(F.[6] as varchar(max)),'')+','+
				--' Pending:'+ISNULL(CAST(F.[7] as varchar(max)),'')+','+
				' Delay occurred:'+ISNULL(CAST(F.[8] as varchar(max)),'')+','+
				--' Delayed Reason:'+ISNULL(CAST(F.[9] as varchar(max)),'')+','+

				' Role :'+ISNULL(CAST(R.RoleName as varchar(max)),'')+','+
				' Owner :'+ISNULL(CAST(U.[Name] as varchar(max)),'')+','+
				' Status :'+ISNULL(CAST(s.StatusName as varchar(max)),'')+','+

				' EST hours:'+ISNULL(CAST(F.[13] as varchar(max)),'')+','+
				' Actual Start hours:'+ISNULL(CAST(F.[14] as varchar(max)),'')+','+
				' Planed Start Date:'+ISNULL(CAST(F.[15] as varchar(max)),'')+','+
				' Actual Start Date:'+ISNULL(CAST(F.[16] as varchar(max)),'')+','+
				' Planed End Date:'+ISNULL(CAST(F.[17] as varchar(max)),'')+','+
				' Actual End Date:'+ISNULL(CAST(F.[18] as varchar(max)),'')
				--' Comments:'+ISNULL(CAST(F.[19] as varchar(max)),'')

				from #FinalLine F 
				INNER JOIN Instance I on convert(uniqueidentifier, F.[3]) =I.Instance_id
				INNER JOIN PhaseMaster P on F.[4]=P.Id
				INNER JOIN RoleMaster R on F.[10]=R.RoleId
				LEFT JOIN UserMaster U on F.[11]=U.UserId 
				LEFT JOIN StatusMaster s on F.[12]=s.Id
				LEFT JOIN ActivityMaster A on F.[2]=A.Activity_ID
			 END
			
			 ELSE IF @TableName='PMTaskMonitor'
			 BEGIN
			 IF OBJECT_ID(N'tempdb..#PMTask_Line') IS NOT NULL
					BEGIN
					DROP TABLE #PMTask_Line
					END
				Insert into #TableOne (InFo)(select value As Info  from #tmp CROSS APPLY STRING_SPLIT(summary, ',') where  ID=@Counter)
				SELECT Pvt.* Into #PMTask_Line
				FROM (
				SELECT *
				FROM #TableOne T
				)AS P
				PIVOT (MIN(InFo) FOR ID_IDENTITY IN (
				[1],[2],[3],[4],[5],[6],[7],
				[8],[9],[10],[11],[12],[13],[14],
				[15],[16],[17],[18],[19],[20],[21],[22]
				))Pvt

				SELECT @newSummary=
				'Task: '+ISNULL( CAST(A.[PMTaskName] as varchar(max)),'')+','+
				' Project:'+ISNULL(CAST(P.Project_Name as varchar(max)),'')+','+
				' Status :'+ISNULL(CAST(s.StatusName as varchar(max)),'')+','+
				' EST hours:'+ISNULL(CAST(F.[10] as varchar(max)),'')+','+
				' Actual Start hours:'+ISNULL(CAST(F.[11] as varchar(max)),'')+','+
				' Planed Start Date:'+ISNULL(CAST(F.[12] as varchar(max)),'')+','+
				' Actual Start Date:'+ISNULL(CAST(F.[13] as varchar(max)),'')+','+
				' Planed End Date:'+ISNULL(CAST(F.[14] as varchar(max)),'')+','+
				' Actual End Date:'+ISNULL(CAST(F.[15] as varchar(max)),'')

				from #PMTask_Line F 
				INNER JOIN PMTaskMaster A on F.[2]=A.PMTaskId
				INNER JOIN Project P on F.[3]=P.Project_Id
				INNER JOIN StatusMaster s on F.[9]=s.Id			
				
				
			 END
			
			 
			  ELSE IF @TableName='Issuetrack'
			 BEGIN
					IF OBJECT_ID(N'tempdb..#It_Line') IS NOT NULL
					BEGIN
					DROP TABLE #It_Line
					END
				    Insert into #TableOne (InFo)(select value As Info  from #tmp CROSS APPLY STRING_SPLIT(summary, ',') where  ID=@Counter)
					SELECT Pvt.* Into #It_Line
					FROM (
					SELECT *
					FROM #TableOne T
					)AS P
					PIVOT (MIN(InFo) FOR ID_IDENTITY IN (
					[1],[2],[3],[4],[5],[6],[7],
					[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18]
					))Pvt
				SELECT @newSummary=
				
				' Issue name :'+ISNULL(CAST(U.[3] as varchar(max)),'')+','
				+' Start Date:'+ISNULL(CAST(U.[7] as varchar(max)),'')+','
				+' End Date :'+ISNULL(CAST(U.[8] as varchar(max)),'')+','
				+' Assigned To :'+ISNULL(CAST(UM.[Name] as varchar(max)),'')+','
				+' Status :'+ISNULL(CAST(U.[11] as varchar(max)),'')+','
				
				+' Is Active :'+
				CASE 
				WHEN U.[13] =1 THEN CAST('Active' as varchar(max))
				ELSE CAST('DELETED' as varchar(max)) END
				from #It_Line U
				INNER JOIN UserMaster UM on U.[10]=UM.UserId
			 END

			  ELSE IF @TableName='FileUploadMaster'
			 BEGIN
					IF OBJECT_ID(N'tempdb..#FUM_Line') IS NOT NULL
					BEGIN
					DROP TABLE #FUM_Line
					END
				    Insert into #TableOne (InFo)(select value As Info  from #tmp CROSS APPLY STRING_SPLIT(summary, ',') where  ID=@Counter)
					SELECT Pvt.* Into #FUM_Line
					FROM (
					SELECT *
					FROM #TableOne T
					)AS P
					PIVOT (MIN(InFo) FOR ID_IDENTITY IN (
					[1],[2],[3],[4],[5],[6],[7],
					[8],[9],[10],[11]
					))Pvt
				SELECT @newSummary=
				
				' Instance :'+ISNULL(CAST(I.InstaceName as varchar(max)),'')+','
				+' File Name:'+ISNULL(CAST(U.[3] as varchar(max)),'')+','
				
				+' Is Active :'+
				CASE 
				WHEN U.[6] =1 THEN CAST('Active' as varchar(max))
				ELSE CAST('DELETED' as varchar(max)) END
				from #FUM_Line U
				INNER JOIN Instance I on U.[2]=I.Instance_id				
			 END
			--PRINT 'The counter value is = ' + CONVERT(VARCHAR,@Counter) 
			--select * from ProjectMonitor

			declare @AuID int 

			Select @AuID =AUDIT_ID from #tmp Where ID=@Counter
			Update ##TempPM SET SUMMARY=@newSummary WHERE AUDIT_ID=@AuID

			--BREAK
		SET @Counter  = @Counter  + 1
		END
--SELECT * from ##TempPM
		
		

END


--truncate table AuditData

