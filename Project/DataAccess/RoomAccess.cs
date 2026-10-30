using Microsoft.Data.Sqlite;

using Dapper;
public class RoomAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private string Table = "Room";
    public void Write(RoomModel room)
    {
        string sql = $"INSERT INTO {Table} (name, type,description,price) VALUES (@Name, @Type, @Description,@Price)";
        _connection.Execute(sql, room);
    }
    public RoomModel GetRoomByID(int id)
    {
        string sql = $"SELECT * FROM {Table} WHERE id = @ID";
        return _connection.QueryFirstOrDefault<RoomModel>(sql, new { ID = id });
    }

    public void UpdateRoom(RoomModel room)
    {
        string sql = $@"
        UPDATE {Table}
        SET name = @Name,
            type = @Type,
            description = @Description,
            price = @Price
        WHERE id = @Id";

        _connection.Execute(sql, new
        {
            Name = room.Name,
            Type = room.Type,
            Description = room.Description,
            Price = room.Price,
            Id = room.ID
        });
    }
    public void Delete(RoomModel room)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";
        _connection.Execute(sql, new { Id = room.ID });
    }
    public bool DeleteByID(int id)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";
        int rowsAffected = _connection.Execute(sql, new { Id = id });

        return rowsAffected > 0;
    }
    public List<RoomModel> GetAllRooms()
    {
        string sql = $"SELECT * FROM {Table}";

        return _connection.Query<RoomModel>(sql).ToList();
    }
}