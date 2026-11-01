public class DishTypeModel
{
    public int ID { get; set; }
    public string Name { get; set; }

    public DishTypeModel() { }

    public DishTypeModel(int id, string name)
    {
        ID = id;
        Name = name;
    }
}
