
public class DishTypeLogic
{
    private readonly DishTypeAccess _access = new();

    public List<DishTypeModel> GetAllDishTypes()
    {
        return _access.GetAll();
    }

    public bool AddDishType(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        if (_access.GetByName(name.Trim()) != null)
            return false;

        _access.Write(new DishTypeModel { Name = name.Trim() });
        return true;
    }

    public bool DeleteDishType(int id)
    {
        if (id <= 0)
            return false;

        return _access.DeleteByID(id);
    }
}
