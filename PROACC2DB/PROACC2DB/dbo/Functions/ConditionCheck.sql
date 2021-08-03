CREATE FUNCTION [dbo].[ConditionCheck] (
	@first_ varchar(200),
	@second_ varchar(200),
	@Third_ varchar(200)
)
RETURNS int AS
BEGIN
DECLARE @t TABLE (myKey uniqueidentifier);  
DECLARE @return_value int;
if(@first_)>0
BEGIN
set @return_value=0
END
else if (@second_)=0
BEGIN
set @return_value=1
END
else if (@Third_)=0
BEGIN
set @return_value=2
END
RETURN @return_value
END;
