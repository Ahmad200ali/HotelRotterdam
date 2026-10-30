public class HotelReservationModel
{
    public Int64 Id { get; set; }

    public Int64 User_Id { get; set; }

    public Int64 Room_Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public double Price { get; set; }


    public HotelReservationModel() { }


    public HotelReservationModel(Int64 id, Int64 userId, Int64 roomId,
        DateTime startDate, DateTime endDate, double price)
    {
        Id = id;
        User_Id = userId;
        Room_Id = roomId;
        StartDate = startDate;
        EndDate = endDate;
        Price = price;
    }
}