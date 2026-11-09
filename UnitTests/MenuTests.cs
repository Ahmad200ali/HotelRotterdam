namespace UnitTests;

[TestClass]
public sealed class MenuTests
{
    [DataTestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow(null)]
    public void AddDishWithoutName(string name)
    {
        // arrange
        MenuLogic logic = new();
        int count = logic.GetAllMenus().Count;

        // act
        bool result = logic.WriteMenu(name, "Starter", "test", 5);

        // assert
        Assert.IsFalse(result);
        Assert.AreEqual(count, logic.GetAllMenus().Count);
    }

    [TestMethod]
    public void AddAndDeleteDish()
    {
        // arrange
        MenuLogic logic = new();

        // act
        bool added = logic.WriteMenu("Test soup", "Starter", "made by a test", 6.75);
        MenuModel dish = logic.GetAllMenus().Last(d => d.Name == "Test soup");

        // assert
        Assert.IsTrue(added);
        Assert.AreEqual("Starter", dish.Type);
        Assert.AreEqual("made by a test", dish.Description);
        Assert.AreEqual(6.75, dish.Price);
        Assert.IsTrue(dish.IsAvailable);

        // delete the dish again, so the database stays the same
        Assert.IsTrue(logic.DeleteMenuByID(dish.ID));
        Assert.IsNull(logic.SearchMenuByID(dish.ID));
    }

    [TestMethod]
    public void UpdateDish()
    {
        // arrange
        MenuLogic logic = new();
        logic.WriteMenu("Test dish update", "Starter", "old", 5);
        MenuModel dish = logic.GetAllMenus().Last(d => d.Name == "Test dish update");

        // act
        dish.Name = "Test dish new";
        dish.Price = 8.5;
        dish.IsAvailable = false;
        bool updated = logic.UpdateMenu(dish);

        dish.Name = "";
        bool updatedWithoutName = logic.UpdateMenu(dish);
        MenuModel result = logic.SearchMenuByID(dish.ID);

        // assert
        Assert.IsTrue(updated);
        Assert.IsFalse(updatedWithoutName);
        Assert.AreEqual("Test dish new", result.Name);
        Assert.AreEqual(8.5, result.Price);
        Assert.IsFalse(result.IsAvailable);

        logic.DeleteMenuByID(dish.ID);
    }

    [TestMethod]
    public void UnavailableDishIsNotShownToGuests()
    {
        // arrange
        MenuLogic logic = new();
        logic.WriteMenu("Test dish hidden", "Starter", "test", 5);
        MenuModel dish = logic.GetAllMenus().Last(d => d.Name == "Test dish hidden");

        // act
        dish.IsAvailable = false;
        logic.UpdateMenu(dish);

        // assert
        Assert.IsFalse(logic.GetAvailableMenus().Any(d => d.ID == dish.ID));
        Assert.IsTrue(logic.GetAllMenus().Any(d => d.ID == dish.ID));

        logic.DeleteMenuByID(dish.ID);
    }

    [TestMethod]
    public void DishTypeInUseCanNotBeDeleted()
    {
        // arrange
        MenuLogic menuLogic = new();
        DishTypeLogic typeLogic = new();
        bool first = typeLogic.AddDishType("Test dish type");
        bool second = typeLogic.AddDishType("Test dish type");
        DishTypeModel type = typeLogic.GetAllDishTypes().Last(t => t.Name == "Test dish type");

        menuLogic.WriteMenu("Test dish with type", "Test dish type", "test", 5);
        MenuModel dish = menuLogic.GetAllMenus().Last(d => d.Name == "Test dish with type");

        // assert
        Assert.IsTrue(first);
        Assert.IsFalse(second); // same type twice is not allowed
        Assert.IsTrue(typeLogic.IsInUse(type.ID));
        Assert.IsFalse(typeLogic.DeleteDishType(type.ID));

        // after deleting the dish the type is not used anymore
        menuLogic.DeleteMenuByID(dish.ID);
        Assert.IsFalse(typeLogic.IsInUse(type.ID));
        Assert.IsTrue(typeLogic.DeleteDishType(type.ID));
    }
}
