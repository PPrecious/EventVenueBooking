CREATE TABLE [dbo].[Venues] (
    [VenueId]       INT           NOT NULL,
    [VenueName]     VARCHAR (100) NOT NULL,
    [VenueLocation] VARCHAR (150) NULL,
    [VenueCapacity] INT           NULL,
    [ImageURL]      VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([VenueId] ASC),
    UNIQUE NONCLUSTERED ([VenueName] ASC)
);

