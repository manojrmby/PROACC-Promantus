

CREATE PROCEDURE [dbo].[SP_Audit]
	@Type varchar(50)=null,
	@Summary varchar(max)=null,
	@UserId varchar(50)=null,
	@Tablename varchar(150)=null,
	@TablID varchar(150)=null,
	@Action varchar(150)=null,
	@Startdate datetime =null,
	@enddate datetime =null

	--@UserName varchar(50)=null
	--@Password varchar(50)=null
AS
BEGIN

	--IF @Type='OverAllReport'
	--BEGIN
	--	select P.Change_id, I.InstaceName,A.Task,PH.PhaseName,R.RoleName,P.Operation,P.OperationAt ,M.[Name] as [BY] 
	--	from ProjectMonitor_Log P 
	--	Join UserMaster M on M.UserId=P.Cre_By 
	--	Join ActivityMaster A on A.Activity_ID=P.ActivityID
	--	Join Instance I on I.Instance_id=P.InstanceID
	--	JOIN PhaseMaster PH on PH.Id=p.PhaseId
	--	JOIN RoleMaster R on R.RoleId=P.RoleId
	--	Where P.isActive=1
	--END
	IF @Type='AddToAduit'
	BEGIN
	INSERT INTO AuditData(USERID,TABLE_NAME,TABLE_ID,SUMMARY,ACTION)
	values 
	(@UserId,@Tablename,@TablID,@Summary,@Action)
	END

	IF @Type='AuditReport'
	BEGIN	
	EXEC SP_GetAuditData
	select t.AUDIT_ID,u.Name, t.TABLE_NAME,t.SUMMARY,t.ACTION,t.CREATED_DATE from ##TempPM t
	join UserMaster u on t.USERID =u.UserId
	where CREATED_DATE>=@Startdate+'00:00:00.000' And CREATED_DATE<=@enddate+'23:59:59.000'
	END

	IF @Type='AuditReportsearch'
	BEGIN
	EXEC SP_GetAuditData
	if @Tablename is not null and @Action is null
		begin
			select t.AUDIT_ID,u.Name, t.TABLE_NAME,t.SUMMARY,t.ACTION,t.CREATED_DATE from ##TempPM t
			join UserMaster u on t.USERID =u.UserId
			where TABLE_NAME like '%'+@Tablename+'%' and CREATED_DATE>=@Startdate+'00:00:00.000' And CREATED_DATE<=@enddate+'23:59:59.000'
		end
	else if @Action is not null and @Tablename is null
		begin
			select t.AUDIT_ID,u.Name, t.TABLE_NAME,t.SUMMARY,t.ACTION,t.CREATED_DATE from ##TempPM t
			join UserMaster u on t.USERID =u.UserId
			where Action=@Action and CREATED_DATE>=@Startdate+'00:00:00.000' And CREATED_DATE<=@enddate+'23:59:59.000'
		end	
	else if @Action is not null and  @Tablename is not null
		begin
			select t.AUDIT_ID,u.Name, t.TABLE_NAME,t.SUMMARY,t.ACTION,t.CREATED_DATE from ##TempPM t
			join UserMaster u on t.USERID =u.UserId
			where TABLE_NAME like '%'+@Tablename+'%' and Action=@Action and CREATED_DATE>=@Startdate+'00:00:00.000' And CREATED_DATE<=@enddate+'23:59:59.000'
		end
	END
END

