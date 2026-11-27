using System.Collections.Generic;

public class DishLogic
{
    private DishAccess _dishAccess = new DishAccess();

    public List<DishModel> GetAll()
    {
        return _dishAccess.GetAll();
    }

    public List<DishModel> GetAvailable()
    {
        return _dishAccess.GetAvailable();
    }

    public DishModel GetByID(int id)
    {
        return _dishAccess.GetByID(id);
    }

    public DishModel GetByName(string name)
    {
        return _dishAccess.GetByName(name);
    }

    public bool Add(DishModel dish)
    {
        // a dish needs a name
        if (dish == null || string.IsNullOrWhiteSpace(dish.Name))
            return false;

        _dishAccess.Write(dish);
        return true;
    }

    public bool Update(DishModel dish)
    {
        if (dish == null || dish.ID <= 0 || string.IsNullOrWhiteSpace(dish.Name))
            return false;

        _dishAccess.Update(dish);
        return true;
    }

    public bool Delete(int id)
    {
        return _dishAccess.DeleteByID(id);
    }
}