using Microsoft.Data.Sqlite;

using Dapper;
public class RoomTypeAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private string Table = "RoomType";

    public void Write(RoomTypeModel roomType)
    {
        string sql = $"INSERT INTO {Table} (name) VALUES (@Name)";
        _connection.Execute(sql, roomType);
    }

    public List<RoomTypeModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table} ORDER BY id";
        return _connection.Query<RoomTypeModel>(sql).ToList();
    }

    public RoomTypeModel GetByID(int id)
    {
        string sql = $"SELECT * FROM {Table} WHERE id = @Id";
        return _connection.QueryFirstOrDefault<RoomTypeModel>(sql, new { Id = id });
    }

    public RoomTypeModel GetByName(string name)
    {
        string sql = $"SELECT * FROM {Table} WHERE name = @Name";
        return _connection.QueryFirstOrDefault<RoomTypeModel>(sql, new { Name = name });
    }

    public bool DeleteByID(int id)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";
        int rowsAffected = _connection.Execute(sql, new { Id = id });

        return rowsAffected > 0;
    }
}
