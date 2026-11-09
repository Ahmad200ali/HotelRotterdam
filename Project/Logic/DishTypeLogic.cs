
public class DishTypeLogic
{
    private readonly DishTypeAccess _access = new();
    private readonly MenuAccess _menuAccess = new();

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

        // a type that is still used by a dish can not be deleted
        if (IsInUse(id))
            return false;

        return _access.DeleteByID(id);
    }

    public bool IsInUse(int id)
    {
        DishTypeModel type = _access.GetByID(id);
        if (type == null)
            return false;

        return _menuAccess.TypeIsUsed(type.Name);
    }
}
