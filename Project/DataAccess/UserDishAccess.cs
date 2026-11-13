using Microsoft.Data.Sqlite;
using Dapper;
using System.Collections.Generic;
using System.Linq;

public class UserDishAccess
{
    private SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/project.db");

    private string Table = "UserDish";

    public void Write(UserDishModel userDish)
    {
        string sql = $@"INSERT INTO {Table}
                        (User_Id, Dish_Id)
                        VALUES (@User_Id, @Dish_Id)";

        _connection.Execute(sql, userDish);
    }

    public List<UserDishModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table} ORDER BY Id";

        return _connection.Query<UserDishModel>(sql).ToList();
    }

    public List<UserDishModel> GetByUserID(long userId)
    {
        string sql = $"SELECT * FROM {Table} WHERE User_Id = @UserId";

        return _connection.Query<UserDishModel>(
            sql, new { UserId = userId }).ToList();
    }

    public List<UserDishModel> GetByDishID(int dishId)
    {
        string sql = $"SELECT * FROM {Table} WHERE Dish_Id = @DishId";

        return _connection.Query<UserDishModel>(
            sql, new { DishId = dishId }).ToList();
    }

    public bool DeleteByID(int id)
    {
        string sql = $"DELETE FROM {Table} WHERE Id = @Id";

        int rowsAffected = _connection.Execute(sql, new { Id = id });

        return rowsAffected > 0;
    }
}