public class RoomTypeModel
{
    public int ID { get; set; }
    public string Name { get; set; }

    public RoomTypeModel() { }

    public RoomTypeModel(int id, string name)
    {
        ID = id;
        Name = name;
    }
}
