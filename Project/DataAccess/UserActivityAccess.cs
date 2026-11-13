using Microsoft.Data.Sqlite;
using Dapper;

public class UserActivityAccess
{
    private SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/project.db");

    private string Table = "UserActivity";

    public void Write(UserActivityModel userActivity)
    {
        string sql = $@"INSERT INTO {Table}
                        (User_Id, Activity_Id)
                        VALUES (@User_Id, @Activity_Id)";

        _connection.Execute(sql, userActivity);
    }

    public List<UserActivityModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table} ORDER BY Id";

        return _connection.Query<UserActivityModel>(sql).ToList();
    }

    public List<UserActivityModel> GetByUserID(int userId)
    {
        string sql = $"SELECT * FROM {Table} WHERE User_Id = @UserId";

        return _connection.Query<UserActivityModel>(
            sql, new { UserId = userId }).ToList();
    }

    public List<UserActivityModel> GetByActivityID(int activityId)
    {
        string sql = $"SELECT * FROM {Table} WHERE Activity_Id = @ActivityId";

        return _connection.Query<UserActivityModel>(
            sql, new { ActivityId = activityId }).ToList();
    }

    public bool DeleteByID(int id)
    {
        string sql = $"DELETE FROM {Table} WHERE Id = @Id";

        int rowsAffected = _connection.Execute(sql, new { Id = id });

        return rowsAffected > 0;
    }
}