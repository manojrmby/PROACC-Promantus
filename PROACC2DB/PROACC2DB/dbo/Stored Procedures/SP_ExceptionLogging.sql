
CREATE Procedure [dbo].[SP_ExceptionLogging]  
(  
@ExceptionMsg varchar(100)=null,  
@ExceptionType varchar(100)=null,  
@ExceptionSource nvarchar(max)=null,  
@ExceptionURL varchar(100)=null,

@ReturnID int=null output

)  
as  
begin  
Insert into ExceptionLogging  
(  
ExceptionMsg ,  
ExceptionType,   
ExceptionSource,  
ExceptionURL,  
Logdate  
)  
select  
@ExceptionMsg,  
@ExceptionType,  
@ExceptionSource,  
@ExceptionURL,  
GETUTCDATE()  

SET @ReturnID=SCOPE_IDENTITY()
Return @ReturnID
End 


