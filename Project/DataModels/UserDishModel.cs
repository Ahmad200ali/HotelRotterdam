public class UserDishModel
{
    public int Id { get; set; }
    public long User_Id { get; set; }
    public int Dish_Id { get; set; }

    public UserDishModel() { }

    public UserDishModel(int id, long userId, int dishId)
    {
        Id = id;
        User_Id = userId;
        Dish_Id = dishId;
    }
}