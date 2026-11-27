using System.Collections.Generic;

public class UserDishLogic
{
    private UserDishAccess _userDishAccess = new UserDishAccess();

    public void Add(UserDishModel userDish)
    {
        _userDishAccess.Write(userDish);
    }

    // every order is saved as a new row, so the guest keeps a history of all orders
    public bool PlaceOrder(long userId, DishModel dish, int count)
    {
        if (userId <= 0 || dish == null || dish.ID <= 0 || !dish.IsAvailable || count <= 0)
            return false;

        _userDishAccess.Write(new UserDishModel
        {
            User_Id = userId,
            Dish_Id = dish.ID,
            Count = count,
            Status = "ontvangen",
            OrderedAt = DateTime.Now,
            Price = dish.Price
        });

        return true;
    }

    public void Update(UserDishModel userDish)
    {
        _userDishAccess.Update(userDish);
    }

    public List<UserDishModel> GetAll()
    {
        return _userDishAccess.GetAll();
    }

    public List<UserDishModel> GetByUserID(long userId)
    {
        return _userDishAccess.GetByUserID(userId);
    }

    public List<UserDishModel> GetByDishID(int dishId)
    {
        return _userDishAccess.GetByDishID(dishId);
    }

    public bool Delete(int id)
    {
        return _userDishAccess.DeleteByID(id);
    }
}
