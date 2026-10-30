using Microsoft.Data.Sqlite;

using Dapper;
public class MenuAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private string Table = "Menu";

    public void Write(MenuModel Menu)
    {
        string sql = $"INSERT INTO {Table} (name, type,description,price) VALUES (@Name, @Type, @Description,@Price)";
        _connection.Execute(sql, Menu);
    }
    public MenuModel GetMenuByID(int id)
    {
        string sql = $"SELECT * FROM {Table} WHERE id = @ID";
        return _connection.QueryFirstOrDefault<MenuModel>(sql, new { ID = id });
    }

    public List<MenuModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table} ORDER BY id";
        return _connection.Query<MenuModel>(sql).AsList();
    }

    public List<MenuModel> GetAvailable()
    {
        string sql = $"SELECT * FROM {Table} WHERE IsAvailable = 1 ORDER BY id";
        return _connection.Query<MenuModel>(sql).AsList();
    }

    public void UpdateMenu(MenuModel Menu)
    {
        string sql = $@"
        UPDATE {Table}
        SET name = @Name,
            type = @Type,
            description = @Description,
            price = @Price,
            IsAvailable = @IsAvailable
        WHERE id = @Id";

        _connection.Execute(sql, new
        {
            Name = Menu.Name,
            Type = Menu.Type,
            Description = Menu.Description,
            Price = Menu.Price,
            IsAvailable = Menu.IsAvailable,
            Id = Menu.ID
        });
    }
    public void Delete(MenuModel Menu)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";
        _connection.Execute(sql, new { Id = Menu.ID });
    }
    public bool DeleteByID(int id)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";
        int rowsAffected = _connection.Execute(sql, new { Id = id });

        return rowsAffected > 0;
    }

}
