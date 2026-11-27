public class HotelReservationLogic
{
    public const string StatusReserved = "Reserved";
    public const string StatusCheckedIn = "Checked in";
    public const string StatusCheckedOut = "Checked out";

    private readonly HotelReservationAccess _access = new();

    public bool WriteHotelReservation(HotelReservationModel reservation)
    {
        if (reservation == null)
            return false;

        if (reservation.User_Id <= 0 || reservation.Room_Id <= 0)
            return false;

        if (reservation.EndDate <= reservation.StartDate)
            return false;

        if (!_access.IsRoomAvailable(reservation.Room_Id, reservation.StartDate, reservation.EndDate))
            return false;

        if (string.IsNullOrWhiteSpace(reservation.Status))
            reservation.Status = StatusReserved;

        _access.Write(reservation);
        return true;
    }

    public bool WriteHotelReservation(Int64 userId, Int64 roomId,
                                    DateTime startDate, DateTime endDate,
                                    double price)
    {
        HotelReservationModel reservation = new()
        {
            User_Id = userId,
            Room_Id = roomId,
            StartDate = startDate,
            EndDate = endDate,
            Price = price
        };

        return WriteHotelReservation(reservation);
    }

    public HotelReservationModel SearchHotelReservationByID(Int64 id)
    {
        if (id <= 0)
            return null;

        return _access.GetHotelReservationByID(id);
    }

    public bool UpdateHotelReservation(HotelReservationModel reservation)
    {
        if (reservation == null || reservation.Id <= 0)
            return false;

        if (reservation.User_Id <= 0 || reservation.Room_Id <= 0)
            return false;

        if (reservation.EndDate <= reservation.StartDate)
            return false;

        if (!_access.IsRoomAvailableForUpdate(
            reservation.Id, reservation.Room_Id,
            reservation.StartDate, reservation.EndDate))
            return false;

        _access.UpdateHotelReservation(reservation);
        return true;
    }

    public bool UpdateHotelReservation(Int64 id, Int64 userId, Int64 roomId,
                                    DateTime startDate, DateTime endDate,
                                    double price)
    {
        HotelReservationModel reservation = new()
        {
            Id = id,
            User_Id = userId,
            Room_Id = roomId,
            StartDate = startDate,
            EndDate = endDate,
            Price = price
        };

        return UpdateHotelReservation(reservation);
    }

    public bool DeleteHotelReservation(HotelReservationModel reservation)
    {
        if (reservation == null || reservation.Id <= 0)
            return false;

        return DeleteHotelReservationByID(reservation.Id);
    }

    public bool DeleteHotelReservationByID(Int64 id)
    {
        if (id <= 0)
            return false;

        return _access.DeleteByID(id);
    }

    public bool IsRoomAvailable(Int64 roomId, DateTime startDate, DateTime endDate)
    {
        if (roomId <= 0 || endDate <= startDate)
            return false;

        return _access.IsRoomAvailable(roomId, startDate, endDate);
    }

    public List<HotelReservationModel> GetReservationsByUserID(Int64 userId)
    {
        if (userId <= 0)
            return new List<HotelReservationModel>();

        return _access.GetReservationsByUserID(userId);
    }

    public List<HotelReservationModel> GetAllReservations()
    {
        return _access.GetAllReservations();
    }

    // a guest can only check in on a reserved booking, during the days of the stay,
    // and only when the room is not occupied by another guest
    public bool CheckIn(Int64 reservationId)
    {
        HotelReservationModel reservation = SearchHotelReservationByID(reservationId);

        if (reservation == null || reservation.Status != StatusReserved)
            return false;

        if (DateTime.Today < reservation.StartDate.Date || DateTime.Today >= reservation.EndDate.Date)
            return false;

        if (_access.RoomIsOccupied(reservation.Room_Id))
            return false;

        return _access.CheckIn(reservationId);
    }

    // check out is only possible after the guest has checked in
    public bool CheckOut(Int64 reservationId)
    {
        HotelReservationModel reservation = SearchHotelReservationByID(reservationId);

        if (reservation == null || reservation.Status != StatusCheckedIn)
            return false;

        return _access.CheckOut(reservationId);
    }

    public bool RoomIsOccupied(Int64 roomId)
    {
        if (roomId <= 0)
            return false;

        return _access.RoomIsOccupied(roomId);
    }
}
