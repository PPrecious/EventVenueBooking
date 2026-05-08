CREATE TABLE [dbo].[Bookings] (
    [BookingId] INT      NOT NULL,
    [EventId]   INT      NOT NULL,
    [VenueId]   INT      NOT NULL,
    [StartDate] DATETIME NOT NULL,
    [EndDate]   DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([BookingId] ASC),
    CONSTRAINT [FK_Bookings_Events] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Events] ([EventId]),
    CONSTRAINT [FK_Bookings_Venues] FOREIGN KEY ([VenueId]) REFERENCES [dbo].[Venues] ([VenueId])
);


GO
CREATE TRIGGER PreventDoubleBooking
ON Bookings
AFTER INSERT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM Bookings b
        JOIN inserted i ON b.VenueId = i.VenueId
        WHERE 
            i.StartDate < b.EndDate
            AND i.EndDate > b.StartDate
            AND b.BookingId <> i.BookingId
    )
    BEGIN
        RAISERROR ('Double booking detected for this venue!', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;