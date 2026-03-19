CREATE TABLE [dbo].[Events] (
    [EventId]          INT           NOT NULL,
    [EventName]        VARCHAR (150) NOT NULL,
    [EventDate]        DATE          NOT NULL,
    [EventDescription] VARCHAR (500) NULL,
    [VenueId]          INT           NULL,
    PRIMARY KEY CLUSTERED ([EventId] ASC),
    CONSTRAINT [FK_Events_Venues] FOREIGN KEY ([VenueId]) REFERENCES [dbo].[Venues] ([VenueId])
);

