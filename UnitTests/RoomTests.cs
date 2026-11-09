namespace UnitTests;

[TestClass]
public sealed class RoomTests
{
    [DataTestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow(null)]
    public void AddRoomWithoutName(string name)
    {
        // arrange
        RoomLogic logic = new();
        int count = logic.GetAllRooms().Count;

        // act
        bool result = logic.WriteRoom(name, "Single room", "test", 50m);

        // assert
        Assert.IsFalse(result);
        Assert.AreEqual(count, logic.GetAllRooms().Count);
    }

    [TestMethod]
    public void AddAndDeleteRoom()
    {
        // arrange
        RoomLogic logic = new();

        // act
        bool added = logic.WriteRoom("Test room", "Single room", "made by a test", 75.50m);
        RoomModel room = logic.GetAllRooms().Last(r => r.Name == "Test room");

        // assert
        Assert.IsTrue(added);
        Assert.AreEqual("Single room", room.Type);
        Assert.AreEqual("made by a test", room.Description);
        Assert.AreEqual(75.50m, room.Price);

        // delete the room again, so the database stays the same
        Assert.IsTrue(logic.DeleteRoomByID(room.ID));
        Assert.IsNull(logic.SearchRoomByID(room.ID));
    }

    [TestMethod]
    public void UpdateRoom()
    {
        // arrange
        RoomLogic logic = new();
        logic.WriteRoom("Test room update", "Single room", "old", 50m);
        RoomModel room = logic.GetAllRooms().Last(r => r.Name == "Test room update");

        // act
        bool updated = logic.UpdateRoom(room.ID, "Test room new", "Double room", "new", 99m);
        bool updatedWithoutName = logic.UpdateRoom(room.ID, "", "Double room", "new", 99m);
        RoomModel result = logic.SearchRoomByID(room.ID);

        // assert
        Assert.IsTrue(updated);
        Assert.IsFalse(updatedWithoutName);
        Assert.AreEqual("Test room new", result.Name);
        Assert.AreEqual("Double room", result.Type);
        Assert.AreEqual("new", result.Description);
        Assert.AreEqual(99m, result.Price);

        logic.DeleteRoomByID(room.ID);
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(999999)] // this id doesnt exist
    public void RoomThatDoesNotExist(int id)
    {
        // arrange
        RoomLogic logic = new();

        // assert
        Assert.IsNull(logic.SearchRoomByID(id));
        Assert.IsFalse(logic.DeleteRoomByID(id));
    }

    [TestMethod]
    public void AddRoomTypeTwice()
    {
        // arrange
        RoomTypeLogic logic = new();

        // act
        bool first = logic.AddRoomType("Test type");
        bool second = logic.AddRoomType("Test type");
        bool empty = logic.AddRoomType("   ");
        RoomTypeModel type = logic.GetAllRoomTypes().Last(t => t.Name == "Test type");

        // assert
        Assert.IsTrue(first);
        Assert.IsFalse(second);
        Assert.IsFalse(empty);

        Assert.IsTrue(logic.DeleteRoomType(type.ID));
    }

    [TestMethod]
    public void RoomTypeInUseCanNotBeDeleted()
    {
        // arrange
        RoomLogic roomLogic = new();
        RoomTypeLogic typeLogic = new();
        typeLogic.AddRoomType("Test type in use");
        RoomTypeModel type = typeLogic.GetAllRoomTypes().Last(t => t.Name == "Test type in use");

        roomLogic.WriteRoom("Test room with type", "Test type in use", "test", 50m);
        RoomModel room = roomLogic.GetAllRooms().Last(r => r.Name == "Test room with type");

        // assert
        Assert.IsTrue(typeLogic.IsInUse(type.ID));
        Assert.IsFalse(typeLogic.DeleteRoomType(type.ID));

        // after deleting the room the type is not used anymore
        roomLogic.DeleteRoomByID(room.ID);
        Assert.IsFalse(typeLogic.IsInUse(type.ID));
        Assert.IsTrue(typeLogic.DeleteRoomType(type.ID));
    }
}
