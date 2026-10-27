
public class RoomLogic
{
    private readonly RoomAccess _access = new();

    public bool WriteRoom(RoomModel room)
    {
        if (room == null)
            return false;

        _access.Write(room);
        return true;
    }

    public bool WriteRoom(string name, string type,
                          string description, decimal price)
    {
        RoomModel room = new()
        {
            Name = name,
            Type = type,
            Description = description,
            Price = price
        };

        return WriteRoom(room);
    }

    public RoomModel SearchRoomByID(int id)
    {
        if (id <= 0)
            return null;

        return _access.GetRoomByID(id);
    }

    public bool UpdateRoom(RoomModel room)
    {
        if (room == null || room.ID <= 0)
            return false;

        _access.UpdateRoom(room);
        return true;
    }

    public bool UpdateRoom(int id, string name, string type,
                           string description, decimal price)
    {
        RoomModel room = new()
        {
            ID = id,
            Name = name,
            Type = type,
            Description = description,
            Price = price
        };

        return UpdateRoom(room);
    }

    public bool DeleteRoom(RoomModel room)
    {
        if (room == null || room.ID <= 0)
            return false;

        return DeleteRoomByID(room.ID);
    }

    public bool DeleteRoomByID(int id)
    {
        if (id <= 0)
            return false;

        return _access.DeleteByID(id);
    }
}
