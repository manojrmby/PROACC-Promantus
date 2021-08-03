CREATE FUNCTION [dbo].[WeakDayCount] (
	@fromDate_ datetime,
	@daysToAdd_ int
)
RETURNS DATETIME AS
BEGIN
DECLARE @return_value DATETIME;

declare @fromDate datetime
declare @daysToAdd int
--SET @fromDate=@fromDate_
--SET @daysToAdd_=@daysToAdd_


select @fromDate = @fromDate_,@DaysToAdd = @daysToAdd_
declare @Saturday int


select @Saturday = DATEPART(weekday,'20130126');with Numbers as (select 0 as n union all select 1 union all select 2 union all select 3 union all select 4
), Split as (
    select @DaysToAdd%5 as PartialDays,@DaysToAdd/5 as WeeksToAdd
), WeekendCheck as (
    select WeeksToAdd,PartialDays,MAX(CASE WHEN DATEPART(weekday,DATEADD(day,n.n,@fromDate))=@Saturday THEN 1 ELSE 0 END) as HitWeekend
    from
    Split t
        left join
    Numbers n
        on
            t.PartialDays >= n.n
group by WeeksToAdd,PartialDays
)
select @return_value=DATEADD(day,WeeksToAdd*7+PartialDays+CASE WHEN HitWeekend=1 THEN 2 ELSE 0 END,@fromDate)
from WeekendCheck
--SET @return_value=GETDATE();
RETURN @return_value
END;
