using System.Collections.Generic;

public class UserDishLogic
{
    private UserDishAccess _userDishAccess = new UserDishAccess();

    public void Add(UserDishModel userDish)
    {
        _userDishAccess.Write(userDish);
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