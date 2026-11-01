
public class RoomTypeLogic
{
    private readonly RoomTypeAccess _access = new();

    public List<RoomTypeModel> GetAllRoomTypes()
    {
        return _access.GetAll();
    }

    public bool AddRoomType(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        // same type twice is not allowed
        if (_access.GetByName(name.Trim()) != null)
            return false;

        _access.Write(new RoomTypeModel { Name = name.Trim() });
        return true;
    }

    public bool DeleteRoomType(int id)
    {
        if (id <= 0)
            return false;

        return _access.DeleteByID(id);
    }
}
