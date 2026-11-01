using Microsoft.Data.Sqlite;

using Dapper;
public class DishTypeAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private string Table = "DishType";

    public void Write(DishTypeModel dishType)
    {
        string sql = $"INSERT INTO {Table} (name) VALUES (@Name)";
        _connection.Execute(sql, dishType);
    }

    public List<DishTypeModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table} ORDER BY id";
        return _connection.Query<DishTypeModel>(sql).ToList();
    }

    public DishTypeModel GetByName(string name)
    {
        string sql = $"SELECT * FROM {Table} WHERE name = @Name";
        return _connection.QueryFirstOrDefault<DishTypeModel>(sql, new { Name = name });
    }

    public bool DeleteByID(int id)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";
        int rowsAffected = _connection.Execute(sql, new { Id = id });

        return rowsAffected > 0;
    }
}
