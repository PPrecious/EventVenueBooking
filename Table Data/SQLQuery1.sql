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