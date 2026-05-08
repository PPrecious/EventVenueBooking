CREATE VIEW BookingDetailsView AS
SELECT 
    b.BookingId,
    e.EventName,
    e.EventDate,
    v.VenueName,
    v.Location,
    v.Capacity,
    b.StartDate,
    b.EndDate,
    b.BookingDate
FROM Booking b
INNER JOIN Event e ON b.EventId = e.EventId
INNER JOIN Venue v ON b.VenueId = v.VenueId;