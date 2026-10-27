public class RoomModel
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public RoomModel() { }

    public RoomModel(int id, string name, string type, string description, decimal price)
    {
        ID = id;
        Name = name;
        Type = type;
        Description = description;
        Price = price;
    }
}