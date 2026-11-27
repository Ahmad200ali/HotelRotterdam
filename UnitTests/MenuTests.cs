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
        DishLogic logic = new();
        int count = logic.GetAll().Count;

        // act
        bool result = logic.Add(new DishModel(0, name, "Starter", "test", 5));

        // assert
        Assert.IsFalse(result);
        Assert.AreEqual(count, logic.GetAll().Count);
    }

    [TestMethod]
    public void AddAndDeleteDish()
    {
        // arrange
        DishLogic logic = new();

        // act
        bool added = logic.Add(new DishModel(0, "Test soup", "Starter", "made by a test", 6.75));
        DishModel dish = logic.GetAll().Last(d => d.Name == "Test soup");

        // assert
        Assert.IsTrue(added);
        Assert.AreEqual("Starter", dish.Type);
        Assert.AreEqual("made by a test", dish.Description);
        Assert.AreEqual(6.75, dish.Price);
        Assert.IsTrue(dish.IsAvailable);

        // delete the dish again, so the database stays the same
        Assert.IsTrue(logic.Delete(dish.ID));
        Assert.IsNull(logic.GetByID(dish.ID));
    }

    [TestMethod]
    public void UpdateDish()
    {
        // arrange
        DishLogic logic = new();
        logic.Add(new DishModel(0, "Test dish update", "Starter", "old", 5));
        DishModel dish = logic.GetAll().Last(d => d.Name == "Test dish update");

        // act
        dish.Name = "Test dish new";
        dish.Price = 8.5;
        dish.IsAvailable = false;
        bool updated = logic.Update(dish);

        dish.Name = "";
        bool updatedWithoutName = logic.Update(dish);
        DishModel result = logic.GetByID(dish.ID);

        // assert
        Assert.IsTrue(updated);
        Assert.IsFalse(updatedWithoutName);
        Assert.AreEqual("Test dish new", result.Name);
        Assert.AreEqual(8.5, result.Price);
        Assert.IsFalse(result.IsAvailable);

        logic.Delete(dish.ID);
    }

    [TestMethod]
    public void UnavailableDishIsNotShownToGuests()
    {
        // arrange
        DishLogic logic = new();
        logic.Add(new DishModel(0, "Test dish hidden", "Starter", "test", 5));
        DishModel dish = logic.GetAll().Last(d => d.Name == "Test dish hidden");

        // act
        dish.IsAvailable = false;
        logic.Update(dish);

        // assert
        Assert.IsFalse(logic.GetAvailable().Any(d => d.ID == dish.ID));
        Assert.IsTrue(logic.GetAll().Any(d => d.ID == dish.ID));

        logic.Delete(dish.ID);
    }

    [TestMethod]
    public void DishTypeInUseCanNotBeDeleted()
    {
        // arrange
        DishLogic menuLogic = new();
        DishTypeLogic typeLogic = new();
        bool first = typeLogic.AddDishType("Test dish type");
        bool second = typeLogic.AddDishType("Test dish type");
        DishTypeModel type = typeLogic.GetAllDishTypes().Last(t => t.Name == "Test dish type");

        menuLogic.Add(new DishModel(0, "Test dish with type", "Test dish type", "test", 5));
        DishModel dish = menuLogic.GetAll().Last(d => d.Name == "Test dish with type");

        // assert
        Assert.IsTrue(first);
        Assert.IsFalse(second); // same type twice is not allowed
        Assert.IsTrue(typeLogic.IsInUse(type.ID));
        Assert.IsFalse(typeLogic.DeleteDishType(type.ID));

        // after deleting the dish the type is not used anymore
        menuLogic.Delete(dish.ID);
        Assert.IsFalse(typeLogic.IsInUse(type.ID));
        Assert.IsTrue(typeLogic.DeleteDishType(type.ID));
    }
}
