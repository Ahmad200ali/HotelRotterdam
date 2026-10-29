
public class MenuLogic
{
    private readonly MenuAccess _access = new();

    public bool WriteMenu(MenuModel Menu)
    {
        if (Menu == null)
            return false;

        _access.Write(Menu);
        return true;
    }

    public bool WriteMenu(string name, string type,
                          string description, double price)
    {
        MenuModel Menu = new()
        {
            Name = name,
            Type = type,
            Description = description,
            Price = price
        };

        return WriteMenu(Menu);
    }

    public MenuModel SearchMenuByID(int id)
    {
        if (id <= 0)
            return null;

        return _access.GetMenuByID(id);
    }

    public List<MenuModel> GetAllMenus() => _access.GetAll();

    public bool UpdateMenu(MenuModel Menu)
    {
        if (Menu == null || Menu.ID <= 0)
            return false;

        _access.UpdateMenu(Menu);
        return true;
    }

    public bool UpdateMenu(int id, string name, string type,
                           string description, double price)
    {
        MenuModel Menu = new()
        {
            ID = id,
            Name = name,
            Type = type,
            Description = description,
            Price = price
        };

        return UpdateMenu(Menu);
    }

    public bool DeleteMenu(MenuModel Menu)
    {
        if (Menu == null || Menu.ID <= 0)
            return false;

        return DeleteMenuByID(Menu.ID);
    }

    public bool DeleteMenuByID(int id)
    {
        if (id <= 0)
            return false;

        return _access.DeleteByID(id);
    }
}
