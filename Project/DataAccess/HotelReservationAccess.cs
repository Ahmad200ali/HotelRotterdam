
using Microsoft.Data.Sqlite;
using Dapper;

public class HotelReservationAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private string Table = "HotelReservation";

    public void Write(HotelReservationModel reservation)
    {
        string sql = $@"
        INSERT INTO {Table} (User_Id, Room_Id, Start_Date, End_Date, Price)
        VALUES (@User_Id, @Room_Id, @StartDate, @EndDate, @Price)";

        _connection.Execute(sql, reservation);
    }

    public HotelReservationModel GetHotelReservationByID(Int64 id)
    {
        string sql = $"SELECT * FROM {Table} WHERE Id = @ID";

        return _connection.QueryFirstOrDefault<HotelReservationModel>(
            sql, new { ID = id });
    }

    public void UpdateHotelReservation(HotelReservationModel reservation)
    {
        string sql = $@"
        UPDATE {Table}
        SET User_Id = @User_Id,
            Room_Id = @Room_Id,
            Start_Date = @StartDate,
            End_Date = @EndDate,
            Price = @Price
        WHERE Id = @Id";

        _connection.Execute(sql, reservation);
    }

    public void Delete(HotelReservationModel reservation)
    {
        string sql = $"DELETE FROM {Table} WHERE Id = @Id";

        _connection.Execute(sql, new { Id = reservation.Id });
    }

    public bool DeleteByID(Int64 id)
    {
        string sql = $"DELETE FROM {Table} WHERE Id = @Id";

        int rowsAffected = _connection.Execute(sql, new { Id = id });

        return rowsAffected > 0;
    }

    public bool IsRoomAvailable(Int64 roomId, DateTime startDate, DateTime endDate)
    {
        string sql = $@"
        SELECT EXISTS (
            SELECT 1
            FROM {Table}
            WHERE Room_Id = @Room_Id
            AND Start_Date < @EndDate
            AND End_Date > @StartDate
        )";

        bool isBooked = _connection.ExecuteScalar<bool>(sql, new
        {
            Room_Id = roomId,
            StartDate = startDate.ToString("yyyy-MM-dd HH:mm:ss"),
            EndDate = endDate.ToString("yyyy-MM-dd HH:mm:ss")
        });

        return !isBooked;
    }

    public bool IsRoomAvailableForUpdate(Int64 id, Int64 roomId,
                                         DateTime startDate, DateTime endDate)
    {
        string sql = $@"
        SELECT EXISTS (
            SELECT 1
            FROM {Table}
            WHERE Room_Id = @Room_Id
            AND Start_Date < @EndDate
            AND End_Date > @StartDate
            AND Id != @Id
        )";

        bool isBooked = _connection.ExecuteScalar<bool>(sql, new
        {
            Id = id,
            Room_Id = roomId,
            StartDate = startDate.ToString("yyyy-MM-dd HH:mm:ss"),
            EndDate = endDate.ToString("yyyy-MM-dd HH:mm:ss")
        });

        return !isBooked;
    }

    public List<HotelReservationModel> GetReservationsByUserID(Int64 userId)
    {
        string sql = $"SELECT * FROM {Table} WHERE User_Id = @User_Id";

        return _connection.Query<HotelReservationModel>(
            sql, new { User_Id = userId }).ToList();
    }
}
