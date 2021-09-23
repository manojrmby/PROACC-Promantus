CREATE TYPE [dbo].[UploadSAPIssueTrackUpload] AS TABLE (
    [IssueNo]            INT            NULL,
    [IssueName]          NVARCHAR (MAX) NULL,
    [Category]           NVARCHAR (MAX) NULL,
    [Priority]           NVARCHAR (MAX) NULL,
    [Assignee]           NVARCHAR (MAX) NULL,
    [RaisedBy]           NVARCHAR (MAX) NULL,
    [ApplicationArea]    NVARCHAR (MAX) NULL,
    [OpenDt]             VARCHAR (500)  NULL,
    [CloseDt]            VARCHAR (500)  NULL,
    [SAPIssueDumpStatus] NVARCHAR (MAX) NULL,
    [Resolution]         NVARCHAR (MAX) NULL,
    [Comments]           NVARCHAR (MAX) NULL);





