namespace UnitTests;

[TestClass]
public sealed class ReservationTests
{
    // every test gets its own room, so the reservations in the database dont get in the way
    private static RoomModel AddTestRoom(string name)
    {
        RoomLogic roomLogic = new();
        roomLogic.WriteRoom(name, "Single room", "made by a test", 100m);

        return roomLogic.GetAllRooms().Last(r => r.Name == name);
    }

    private static Int64 GetUserId()
    {
        UserLogic userLogic = new();

        return userLogic.CheckLogin("kevin@kevin.nl", "kevin").Id;
    }

    [DataTestMethod]
    [DataRow(5, 5)] // check-out on the same day
    [DataRow(5, 3)] // check-out before check-in
    public void ReservationWithWrongDates(int startDays, int endDays)
    {
        // arrange
        HotelReservationLogic logic = new();
        DateTime start = DateTime.Today.AddDays(startDays);
        DateTime end = DateTime.Today.AddDays(endDays);

        // act
        bool result = logic.WriteHotelReservation(GetUserId(), 1, start, end, 100);

        // assert
        Assert.IsFalse(result);
        Assert.IsFalse(logic.IsRoomAvailable(1, start, end));
    }

    [DataTestMethod]
    [DataRow(0, 1)] // no user
    [DataRow(1, 0)] // no room
    [DataRow(-1, -1)]
    public void ReservationWithoutUserOrRoom(int userId, int roomId)
    {
        // arrange
        HotelReservationLogic logic = new();
        DateTime start = DateTime.Today.AddDays(5);
        DateTime end = DateTime.Today.AddDays(7);

        // act
        bool result = logic.WriteHotelReservation(userId, roomId, start, end, 100);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void CreateAndCancelReservation()
    {
        // arrange
        HotelReservationLogic logic = new();
        RoomLogic roomLogic = new();
        RoomModel room = AddTestRoom("Test room reservation");
        Int64 userId = GetUserId();
        DateTime start = DateTime.Today.AddDays(10);
        DateTime end = DateTime.Today.AddDays(13);

        // act
        bool created = logic.WriteHotelReservation(userId, room.ID, start, end, 300);
        HotelReservationModel reservation = logic.GetReservationsByUserID(userId)
            .Last(r => r.Room_Id == room.ID);

        // assert
        Assert.IsTrue(created);
        Assert.AreEqual(start, reservation.StartDate);
        Assert.AreEqual(end, reservation.EndDate);
        Assert.AreEqual(300, reservation.Price);

        // cancel it again
        Assert.IsTrue(logic.DeleteHotelReservationByID(reservation.Id));
        Assert.IsNull(logic.SearchHotelReservationByID(reservation.Id));
        Assert.IsFalse(logic.DeleteHotelReservationByID(reservation.Id));

        roomLogic.DeleteRoomByID(room.ID);
    }

    [TestMethod]
    public void RoomCanNotBeBookedTwice()
    {
        // arrange
        HotelReservationLogic logic = new();
        RoomLogic roomLogic = new();
        RoomModel room = AddTestRoom("Test room twice");
        Int64 userId = GetUserId();
        DateTime today = DateTime.Today;

        // act
        bool first = logic.WriteHotelReservation(userId, room.ID, today.AddDays(10), today.AddDays(13), 300);
        bool second = logic.WriteHotelReservation(userId, room.ID, today.AddDays(12), today.AddDays(15), 300);

        // assert
        Assert.IsTrue(first);
        Assert.IsFalse(second);
        Assert.IsFalse(logic.IsRoomAvailable(room.ID, today.AddDays(11), today.AddDays(12)));
        Assert.IsFalse(logic.IsRoomAvailable(room.ID, today.AddDays(8), today.AddDays(11)));

        // the room is free again on the day the other guest leaves
        Assert.IsTrue(logic.IsRoomAvailable(room.ID, today.AddDays(13), today.AddDays(15)));
        Assert.IsTrue(logic.IsRoomAvailable(room.ID, today.AddDays(7), today.AddDays(10)));

        HotelReservationModel reservation = logic.GetReservationsByUserID(userId)
            .Last(r => r.Room_Id == room.ID);
        logic.DeleteHotelReservationByID(reservation.Id);
        roomLogic.DeleteRoomByID(room.ID);
    }

    [TestMethod]
    public void RoomWithReservationCanNotBeDeleted()
    {
        // arrange
        HotelReservationLogic logic = new();
        RoomLogic roomLogic = new();
        RoomModel room = AddTestRoom("Test room delete");
        Int64 userId = GetUserId();

        logic.WriteHotelReservation(userId, room.ID,
            DateTime.Today.AddDays(10), DateTime.Today.AddDays(13), 300);
        HotelReservationModel reservation = logic.GetReservationsByUserID(userId)
            .Last(r => r.Room_Id == room.ID);

        // assert
        Assert.IsTrue(roomLogic.HasReservations(room.ID));
        Assert.IsFalse(roomLogic.DeleteRoomByID(room.ID));
        Assert.IsNotNull(roomLogic.SearchRoomByID(room.ID));

        // without the reservation the room can be deleted
        logic.DeleteHotelReservationByID(reservation.Id);
        Assert.IsFalse(roomLogic.HasReservations(room.ID));
        Assert.IsTrue(roomLogic.DeleteRoomByID(room.ID));
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(999999)] // this user doesnt exist
    public void NoReservationsForWrongUser(int userId)
    {
        // arrange
        HotelReservationLogic logic = new();

        // act
        List<HotelReservationModel> reservations = logic.GetReservationsByUserID(userId);

        // assert
        Assert.AreEqual(0, reservations.Count);
    }
}
