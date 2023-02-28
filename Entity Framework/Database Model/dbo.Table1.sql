CREATE TABLE [dbo].[Table1] (
    [ID]       INT        NOT NULL IDENTITY,
    [PID]      NCHAR (30) NULL,
    [Groups]   NCHAR (30) NULL,
    [Name]     NCHAR (30) NULL,
    [Quantity] INT        NULL,
    [Price]    FLOAT (53) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

