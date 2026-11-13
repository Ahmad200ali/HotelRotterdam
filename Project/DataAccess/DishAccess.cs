using Microsoft.Data.Sqlite;
using Dapper;
using System.Collections.Generic;
using System.Linq;

public class DishAccess
{
    private SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/project.db");

    private string Table = "Dish";

    public void Write(DishModel dish)
    {
        string sql = $@"INSERT INTO {Table}
                        (name, type, description, price, isAvailable)
                        VALUES (@Name, @Type, @Description, @Price, @IsAvailable)";

        _connection.Execute(sql, dish);
    }

    public List<DishModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table} ORDER BY id";

        return _connection.Query<DishModel>(sql).ToList();
    }

    public List<DishModel> GetAvailable()
    {
        string sql = $@"SELECT * FROM {Table}
                        WHERE isAvailable = 1
                        ORDER BY id";

        return _connection.Query<DishModel>(sql).ToList();
    }

    public DishModel GetByID(int id)
    {
        string sql = $"SELECT * FROM {Table} WHERE id = @Id";

        return _connection.QueryFirstOrDefault<DishModel>(
            sql, new { Id = id });
    }

    public DishModel GetByName(string name)
    {
        string sql = $"SELECT * FROM {Table} WHERE name = @Name";

        return _connection.QueryFirstOrDefault<DishModel>(
            sql, new { Name = name });
    }

    public void Update(DishModel dish)
    {
        string sql = $@"UPDATE {Table}
                        SET name = @Name,
                            type = @Type,
                            description = @Description,
                            price = @Price,
                            isAvailable = @IsAvailable
                        WHERE id = @ID";

        _connection.Execute(sql, dish);
    }

    public bool DeleteByID(int id)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";

        int rowsAffected = _connection.Execute(sql, new { Id = id });

        return rowsAffected > 0;
    }
    public bool TypeIsUsed(string type)
    {
        string sql = $"SELECT COUNT(*) FROM {Table} WHERE type = @Type";

        int count = _connection.ExecuteScalar<int>(
            sql, new { Type = type });

        return count > 0;
    }
}