
CREATE procedure [dbo].[Sp_InsertMasterActivityToProjectMonitor]
(
@InsID varchar(50),
@PhaID int,
@CreBy varchar(50)
)  
AS   
BEGIN  
	Declare @Summary varChar(max), @BuildingBlock_Id varchar(50),@Version_Id uniqueidentifier;
	set @BuildingBlock_Id =(select BuildingBlock_Id from ScenarioMaster where  ScenarioId=(select ScenarioId from Project where Project_Id=(select Project_ID from Instance where Instance_id=@InsID)))
	SET @Version_Id=(SELECT version_id from Instance where Instance_id=@InsID)

	INSERT ProjectMonitor( 
		[Id]
      ,[ActivityID]
      ,[InstanceID]
      ,[PhaseId]
	  ,RoleId
      ,[UserID]
      ,[StatusId]
	  ,EST_hours
	  ,Planed__St_Date
	  ,Planed__En_Date
	  ,Actual_St_Date
	  ,Actual_En_Date
	  ,Version_Id
      ,[Cre_By]
	  )
	 SELECT
	  NEWID()
	  ,[Activity_ID]
	  ,@InsID
	  ,[PhaseID]
	  ,RoleID
	  ,'00000000-0000-0000-0000-000000000000'
	  ,(select id from StatusMaster WHERE StatusName='Yet To Start')
	  ,EST_hours
	  ,GETUTCDATE()
	 -- ,CASE WHEN (select LEFT(Right(EST_hours,2),1) from ActivityMaster where PhaseID=1 and PM_Add=0) >= 5 then 
		--[dbo].[WeakDayCount](GETUTCDATE(),(CAST(ROUND([EST_hours],1)AS INT)/8.001))
		--ELSE [dbo].[WeakDayCount](GETUTCDATE(),(CAST(ROUND(([EST_hours]),0)AS INT)/8.001))
		--End
	  ,[dbo].[WeakDayCount](GETUTCDATE(),(CAST(ROUND(([EST_hours]),0)AS INT)/8.001))
	  --,DATEADD(DAY,CAST(ROUND(([EST_hours]),0)AS INT)/8.001,GETUTCDATE())
	  --,GETUTCDATE()
	  ,GETUTCDATE()
	  ,GETUTCDATE()
	  ,Version_Id
	  ,@CreBy
	  --FROM ActivityMaster where isactive=1 AND PhaseID=@PhaID AND Sequence_Num IS NOT NULL and PM_Add=0 order by Sequence_Num
	  FROM ActivityMaster 
	  where isactive=1 
	  AND PhaseID=@PhaID 
	  AND Sequence_Num IS NOT NULL 
	  and PM_Add=0 
	  and BuildingBlock_id in (select * from STRING_SPLIT(@BuildingBlock_Id, ',')) 
	  --AND Version_Id=(select id from AMVersion where isActive=1)
	  AND Version_Id=@Version_Id
	  order by Sequence_Num
END; 

--Use ProAcc2_Dev_Dev
--select * from ActivityMaster where PhaseID=1 and isActive=1 and PM_Add=0 order by Sequence_Num 

--select 
--CASE WHEN (select LEFT(Right(EST_hours,2),1) from ActivityMaster where PhaseID=1 and Activity_ID=273 and PM_Add=0) >= 5 then 
--[dbo].[WeakDayCount](GETUTCDATE(),(CAST(ROUND(10,1)AS INT)/8.001))
--ELSE [dbo].[WeakDayCount](GETUTCDATE(),(CAST(ROUND((9.4),0)AS INT)/8.001))
--END 
--First  = LEFT(Right(EST_hours,2),1) from ActivityMaster where PhaseID=1 and Activity_ID=273 and PM_Add=0 order by Sequence_Num 

--SELECT DATEADD(DAY,CAST(ROUND(([EST_hours]),0)AS INT)/8.001,GETUTCDATE())
