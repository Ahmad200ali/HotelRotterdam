public class MenuModel
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }

    public MenuModel() { }

    public MenuModel(int id, string name, string type, string description, double price)
    {
        ID = id;
        Name = name;
        Type = type;
        Description = description;
        Price = price;
    }
}