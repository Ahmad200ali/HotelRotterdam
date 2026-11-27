namespace UnitTests;

[TestClass]
public sealed class UserDishCancelTests
{
    // UserDish has foreign keys, so the tests use users that are in the database
    private long TestUserId;
    private long OtherUserId;

    private UserDishLogic logic = new();
    private DishLogic dishLogic = new();
    private int dishId;
    private List<int> madeOrders = new();

    [TestInitialize]
    public void Setup()
    {
        UserLogic userLogic = new();
        TestUserId = userLogic.CheckLogin("kevin@kevin.nl", "kevin").Id;
        OtherUserId = userLogic.CheckLogin("molme@molme.nl", "123456").Id;

        // every test gets its own dish to order
        dishLogic.Add(new DishModel(0, "Test order dish", "Main course", "dish for the tests", 9.5));
        dishId = dishLogic.GetAll().Last(d => d.Name == "Test order dish").ID;
    }

    [TestCleanup]
    public void Cleanup()
    {
        // only remove what the tests made themselves
        foreach (int id in madeOrders)
            logic.Delete(id);

        dishLogic.Delete(dishId);
    }

    // makes an order and gives back the id
    private int MakeOrder(long userId)
    {
        logic.Add(new UserDishModel(0, userId, dishId, 1));
        int id = logic.GetByUserID(userId).Last().Id;
        madeOrders.Add(id);
        return id;
    }

    [TestMethod]
    public void NewOrderIsReceived()
    {
        // act
        int id = MakeOrder(TestUserId);

        // assert
        Assert.AreEqual(UserDishLogic.StatusReceived, logic.GetByID(id).Status);
    }

    [TestMethod]
    public void CancelReceivedOrder()
    {
        // arrange
        int id = MakeOrder(TestUserId);

        // act
        bool result = logic.CancelOrder(id, TestUserId);

        // assert
        Assert.IsTrue(result);
        Assert.IsNull(logic.GetByID(id));
        Assert.AreEqual(0, logic.GetByUserID(TestUserId).Count(o => o.Dish_Id == dishId));
    }

    [TestMethod]
    public void CancelOrderInPreparation()
    {
        // arrange
        int id = MakeOrder(TestUserId);
        logic.UpdateStatus(id, UserDishLogic.StatusInPreparation);

        // act
        bool result = logic.CancelOrder(id, TestUserId);

        // assert
        Assert.IsFalse(result);
        Assert.AreEqual(UserDishLogic.StatusInPreparation, logic.GetByID(id).Status);
    }

    [TestMethod]
    public void CancelDeliveredOrder()
    {
        // arrange
        int id = MakeOrder(TestUserId);
        logic.UpdateStatus(id, UserDishLogic.StatusDelivered);

        // act
        bool result = logic.CancelOrder(id, TestUserId);

        // assert
        Assert.IsFalse(result);
        Assert.IsNotNull(logic.GetByID(id));
    }

    [TestMethod]
    public void CancelOrderTwice()
    {
        // arrange
        int id = MakeOrder(TestUserId);
        logic.CancelOrder(id, TestUserId);

        // act
        bool result = logic.CancelOrder(id, TestUserId);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void CancelOrderOfAnotherGuest()
    {
        // arrange
        int id = MakeOrder(TestUserId);

        // act
        bool result = logic.CancelOrder(id, OtherUserId);

        // assert
        Assert.IsFalse(result);
        Assert.IsNotNull(logic.GetByID(id));
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(999999)] // does not exist
    public void CancelOrderInvalidId(int id)
    {
        // act
        bool result = logic.CancelOrder(id, TestUserId);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void CancelOnlyRemovesThatOrder()
    {
        // arrange
        int first = MakeOrder(TestUserId);
        int second = MakeOrder(TestUserId);

        // act
        logic.CancelOrder(first, TestUserId);

        // assert
        Assert.IsNull(logic.GetByID(first));
        Assert.IsNotNull(logic.GetByID(second));
    }

    [TestMethod]
    public void UpdateStatusInvalidStatus()
    {
        // arrange
        int id = MakeOrder(TestUserId);

        // act
        bool result = logic.UpdateStatus(id, "Flying");

        // assert
        Assert.IsFalse(result);
        Assert.AreEqual(UserDishLogic.StatusReceived, logic.GetByID(id).Status);
    }

    [TestMethod]
    public void UpdateStatusInvalidId()
    {
        // act
        bool result = logic.UpdateStatus(0, UserDishLogic.StatusDelivered);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void OrdersOnlyOfThisGuest()
    {
        // arrange
        MakeOrder(TestUserId);
        MakeOrder(OtherUserId);
        MakeOrder(OtherUserId);

        // act
        List<UserDishModel> result = logic.GetByUserID(TestUserId).Where(o => o.Dish_Id == dishId).ToList();

        // assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(TestUserId, result[0].User_Id);
    }
}
