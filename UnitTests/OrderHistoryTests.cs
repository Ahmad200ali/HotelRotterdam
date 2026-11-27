namespace UnitTests;

[TestClass]
public sealed class OrderHistoryTests
{
    // every test gets its own dish, so other orders in the database dont get in the way
    private static DishModel AddTestDish(string name, double price)
    {
        DishLogic dishLogic = new();
        dishLogic.Add(new DishModel(0, name, "Test", "made by a test", price));

        return dishLogic.GetByName(name);
    }

    private static long GetUserId()
    {
        UserLogic userLogic = new();

        return userLogic.CheckLogin("kevin@kevin.nl", "kevin").Id;
    }

    [TestMethod]
    public void EveryOrderIsKeptInTheHistory()
    {
        // arrange
        UserDishLogic logic = new();
        DishModel dish = AddTestDish("Test dish history", 12.50);
        long userId = GetUserId();
        DateTime before = DateTime.Now.AddSeconds(-1);

        // act: the same dish ordered twice
        bool first = logic.PlaceOrder(userId, dish, 2);
        bool second = logic.PlaceOrder(userId, dish, 3);
        List<UserDishModel> orders = logic.GetByUserID(userId)
            .Where(o => o.Dish_Id == dish.ID).ToList();

        // assert: two separate orders, newest first, with date, count and price
        Assert.IsTrue(first);
        Assert.IsTrue(second);
        Assert.AreEqual(2, orders.Count);
        Assert.AreEqual(3, orders[0].Count);
        Assert.AreEqual(2, orders[1].Count);
        Assert.AreEqual(12.50, orders[0].Price);
        Assert.IsNotNull(orders[0].OrderedAt);
        Assert.IsTrue(orders[0].OrderedAt >= before);

        foreach (UserDishModel order in orders)
            logic.Delete(order.Id);
        new DishLogic().Delete(dish.ID);
    }

    [TestMethod]
    public void OldOrderKeepsItsPriceWhenDishPriceChanges()
    {
        // arrange
        UserDishLogic logic = new();
        DishLogic dishLogic = new();
        DishModel dish = AddTestDish("Test dish price change", 10.00);
        long userId = GetUserId();

        // act
        logic.PlaceOrder(userId, dish, 1);
        dish.Price = 15.00;
        dishLogic.Update(dish);
        UserDishModel order = logic.GetByUserID(userId).First(o => o.Dish_Id == dish.ID);

        // assert
        Assert.AreEqual(10.00, order.Price);

        logic.Delete(order.Id);
        dishLogic.Delete(dish.ID);
    }

    [DataTestMethod]
    [DataRow(0)] // no count
    [DataRow(-1)]
    public void OrderWithWrongCountIsNotSaved(int count)
    {
        // arrange
        UserDishLogic logic = new();
        DishModel dish = AddTestDish("Test dish wrong count", 10.00);
        long userId = GetUserId();

        // act
        bool result = logic.PlaceOrder(userId, dish, count);

        // assert
        Assert.IsFalse(result);
        Assert.AreEqual(0, logic.GetByUserID(userId).Count(o => o.Dish_Id == dish.ID));

        new DishLogic().Delete(dish.ID);
    }

    [TestMethod]
    public void UnavailableDishCanNotBeOrdered()
    {
        // arrange
        UserDishLogic logic = new();
        DishModel dish = AddTestDish("Test dish unavailable", 10.00);
        dish.IsAvailable = false;

        // act
        bool result = logic.PlaceOrder(GetUserId(), dish, 1);

        // assert
        Assert.IsFalse(result);

        new DishLogic().Delete(dish.ID);
    }
}
