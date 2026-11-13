using Microsoft.Data.Sqlite;
using Dapper;

public class ActivityAccess
{
    private SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/project.db");

    private string Table = "Activity";

    public void Write(ActivityModel activity)
    {
        string sql = $@"INSERT INTO {Table}
                        (name, description, price)
                        VALUES (@Name, @Description, @Price)";

        _connection.Execute(sql, activity);
    }

    public List<ActivityModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table} ORDER BY id";

        return _connection.Query<ActivityModel>(sql).ToList();
    }

    public ActivityModel GetByID(int id)
    {
        string sql = $"SELECT * FROM {Table} WHERE id = @Id";

        return _connection.QueryFirstOrDefault<ActivityModel>(
            sql, new { Id = id });
    }

    public ActivityModel GetByName(string name)
    {
        string sql = $"SELECT * FROM {Table} WHERE name = @Name";

        return _connection.QueryFirstOrDefault<ActivityModel>(
            sql, new { Name = name });
    }

    public void Update(ActivityModel activity)
    {
        string sql = $@"UPDATE {Table}
                        SET name = @Name,
                            description = @Description,
                            price = @Price
                        WHERE id = @ID";

        _connection.Execute(sql, activity);
    }

    public bool DeleteByID(int id)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";

        int rowsAffected = _connection.Execute(sql, new { Id = id });

        return rowsAffected > 0;
    }
}