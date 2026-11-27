public class UserDishModel
{
    public int Id { get; set; }
    public long User_Id { get; set; }
    public int Dish_Id { get; set; }
    public int Count { get; set; } = 1;

    // ontvangen, in bereiding or bezorgd
    public string Status { get; set; } = "ontvangen";

    public UserDishModel() { }

    public UserDishModel(int id, long userId, int dishId, int count = 1, string status = "ontvangen")
    {
        Id = id;
        User_Id = userId;
        Dish_Id = dishId;
        Count = count;
        Status = status;
    }
}
