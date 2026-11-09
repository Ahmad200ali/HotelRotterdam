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
        string sql = $"SELECT id, name, type, description, CAST(price AS REAL) AS price FROM {Table} WHERE id = @ID";
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
        // CAST is needed, a price like 80 is saved as a whole number and then Dapper crashes
        string sql = $"SELECT id, name, type, description, CAST(price AS REAL) AS price FROM {Table}";

        return _connection.Query<RoomModel>(sql).ToList();
    }

    // true when there is still a room with this type
    public bool TypeIsUsed(string type)
    {
        string sql = $"SELECT EXISTS (SELECT 1 FROM {Table} WHERE type = @Type)";
        return _connection.ExecuteScalar<bool>(sql, new { Type = type });
    }
}