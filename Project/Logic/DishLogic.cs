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

    public void Add(DishModel dish)
    {
        _dishAccess.Write(dish);
    }

    public void Update(DishModel dish)
    {
        _dishAccess.Update(dish);
    }

    public bool Delete(int id)
    {
        return _dishAccess.DeleteByID(id);
    }
}