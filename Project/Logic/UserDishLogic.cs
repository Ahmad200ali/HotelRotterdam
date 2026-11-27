using System.Collections.Generic;

public class UserDishLogic
{
    public const string StatusReceived = "Received";
    public const string StatusInPreparation = "In preparation";
    public const string StatusDelivered = "Delivered";

    private UserDishAccess _userDishAccess = new UserDishAccess();

    public void Add(UserDishModel userDish)
    {
        _userDishAccess.Write(userDish);
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

    public UserDishModel GetByID(int id)
    {
        if (id <= 0)
            return null;

        return _userDishAccess.GetByID(id);
    }

    public bool UpdateStatus(int id, string status)
    {
        if (id <= 0)
            return false;

        if (status != StatusReceived && status != StatusInPreparation && status != StatusDelivered)
            return false;

        return _userDishAccess.UpdateStatus(id, status);
    }

    // a guest can only cancel his own order, as long as it is not being prepared yet
    // (old rows without a status count as received)
    public bool CancelOrder(int id, long userId)
    {
        UserDishModel order = GetByID(id);

        if (order == null || order.User_Id != userId)
            return false;

        if (!string.IsNullOrEmpty(order.Status) && order.Status != StatusReceived)
            return false;

        return _userDishAccess.DeleteByID(id);
    }
}
