

CREATE  PROCEDURE [dbo].[SP_Activity_Loop]
	@PhaseID int=null,
	@PreviousId int =null,
	@Cre_By varchar(50)=null,
	@Task_Id int =null,
	@Parallel_Id uniqueidentifier=null, --'00000000-0000-0000-0000-000000000000' 
	@VersionID uniqueidentifier=null
AS
BEGIN
PRINT @PhaseID
		DECLARE @PRE_SQUID int,@count int,@Start int,@innerCount int 
		DECLARE @PRE_SQU_Num int,@AC_ID int
		IF OBJECT_ID(N'tempdb..#temp_Activity') IS NOT NULL
		BEGIN
		DROP TABLE #temp_Activity
		END
		IF OBJECT_ID(N'tempdb..#temp_Acti') IS NOT NULL
		BEGIN
		DROP TABLE #temp_Acti
		END

		IF @PreviousId =0
		BEGIN
			IF @PhaseID=1
			BEGIN
				SET @PRE_SQUID=0
			END
			ELSE IF @PhaseID=2
			BEGIN
				SET @PRE_SQUID=1000
			END
			ELSE IF @PhaseID=3
			BEGIN
				SET @PRE_SQUID=2000
			END
			ELSE IF @PhaseID=4
			BEGIN
				SET @PRE_SQUID=3000
			END
			ELSE IF @PhaseID=5
			BEGIN
				SET @PRE_SQUID=4000
			END
			SET @innerCount=(SELECT COUNT(*) FROM ActivityMaster WHERE PhaseID=@PhaseID AND Version_Id=@VersionID)
			IF	@innerCount>0
			BEGIN
				SELECT ROW_NUMBER () OVER (ORDER BY Activity_ID) AS RowNum, * into #temp_Acti from ActivityMaster WHERE PhaseID=@PhaseID AND Version_Id=@VersionID  ORDER BY Sequence_Num
				SET @count =(SELECT COUNT(*) from #temp_Acti)
		
				SET @Start=1
				WHILE @Start<=@count
					BEGIN
					SET @PRE_SQU_Num=(SELECT Sequence_Num FROM #temp_Acti WHERE RowNum=@Start);
					SET @AC_ID=(SELECT Activity_ID FROM #temp_Acti WHERE RowNum=@Start);
					Update ActivityMaster SET Sequence_Num=@PRE_SQU_Num+1 ,Modified_by=@Cre_By WHERE Activity_ID=@AC_ID;
					SET @Start=@Start+1;
					END
			END
		END
		ELSE 
			BEGIN
			--SET @PreviousId=(select Max(Sequence_Num) from ActivityMaster where Parallel_Id=(select Parallel_Id from ActivityMaster where Activity_ID=@PreviousId))
			--SET @PRE_SQUID=(SELECT Sequence_Num from ActivityMaster WHERE Activity_ID=@PreviousId)

			--SET @PRE_SQUID=(SELECT Sequence_Num from ActivityMaster WHERE Activity_ID=@PreviousId)
			--if @Task_Id=2 and (select count(*) from ActivityMaster where Parallel_Id=@Parallel_Id)>=1
			print @Parallel_Id
				if @Task_Id=2 and (select count(*) from ActivityMaster where Parallel_Id=@Parallel_Id AND Version_Id=@VersionID)>=1  and  @Parallel_Id IS NOT NULL 
					BEGIN
					print '@PRE_SQUID 1'
					SET @PRE_SQUID=(SELECT Sequence_Num from ActivityMaster WHERE Activity_ID=@PreviousId AND Version_Id=@VersionID)
					print '@PRE_SQUID 1'
					print @PRE_SQUID
					END
				ELSE if @Parallel_Id is null and (select Task_id from ActivityMaster where Activity_ID=@PreviousId AND Version_Id=@VersionID)!=2
					BEGIN 
					print '@PRE_SQUID 2'
					SET @PRE_SQUID=(SELECT Sequence_Num from ActivityMaster WHERE Activity_ID=@PreviousId AND Version_Id=@VersionID)
					print '@PRE_SQUID 2'
					print @PRE_SQUID
					END
				Else if (select count(*) from ActivityMaster where Version_Id=@VersionID  AND Parallel_Id=@Parallel_Id )=0 and @Task_Id=2 and  (select Task_id from ActivityMaster where Activity_ID=@PreviousId AND Version_Id=@VersionID)!=2
				Begin
					print '@PRE_SQUID 3'
					SET @PRE_SQUID=(SELECT Sequence_Num from ActivityMaster WHERE Activity_ID=@PreviousId AND Version_Id=@VersionID)
					print '@PRE_SQUID 3'
					print @PRE_SQUID
				End
				ELSE
					BEGIN 
					print '@PRE_SQUID else'

					SET @PRE_SQUID=(select Max(Sequence_Num) from ActivityMaster where   Version_Id=@VersionID  AND Parallel_Id=(select Parallel_Id from ActivityMaster where  Version_Id=@VersionID  AND Activity_ID=@PreviousId))
					print '@PRE_SQUID else'
					print @PRE_SQUID
					END
				
				SELECT ROW_NUMBER () OVER (ORDER BY Activity_ID) AS RowNum, * into #temp_Activity from ActivityMaster WHERE PhaseID=@PhaseID AND Version_Id=@VersionID  AND Sequence_Num > @PRE_SQUID ORDER BY Sequence_Num

				SET @count =(SELECT COUNT(*) from #temp_Activity)
		
				SET @Start=1
				select * from #temp_Activity
				WHILE @Start<=@count
					BEGIN
					SET @PRE_SQU_Num=(SELECT Sequence_Num FROM #temp_Activity WHERE RowNum=@Start);
					SET @AC_ID=(SELECT Activity_ID FROM #temp_Activity WHERE RowNum=@Start);
					Update ActivityMaster SET Sequence_Num=@PRE_SQU_Num+1 ,Modified_by=@Cre_By WHERE Activity_ID=@AC_ID;
					SET @Start=@Start+1;
					END
			END
			
 RETURN @PRE_SQUID
END

--Use ProAcc2_DevDevNew
--select * from parallelType
--select * from ProjectMonitor order by Cre_on desc
--select * from ActivityMaster order by Cre_on desc

--truncate table ParallelType
--truncate table ActivityMaster
