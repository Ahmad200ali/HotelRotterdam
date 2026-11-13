public class ActivityModel
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public ActivityModel() { }

    public ActivityModel(int id, string name, string description, decimal price)
    {
        ID = id;
        Name = name;
        Description = description;
        Price = price;
    }
}