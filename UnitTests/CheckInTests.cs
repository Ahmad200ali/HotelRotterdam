namespace UnitTests;

[TestClass]
public sealed class CheckInTests
{
    // this user and these rooms do not exist in the database, so the tests can not touch real data
    private const long TestUserId = 9001;
    private const long TestRoomId = 9001;

    private HotelReservationLogic logic = new();
    private HotelReservationAccess access = new();

    [TestCleanup]
    public void Cleanup()
    {
        foreach (HotelReservationModel r in logic.GetReservationsByUserID(TestUserId))
            logic.DeleteHotelReservationByID(r.Id);
    }

    // makes a reservation and gives back the id
    private long MakeReservation(DateTime start, DateTime end)
    {
        logic.WriteHotelReservation(TestUserId, TestRoomId, start, end, 100);
        List<HotelReservationModel> reservations = logic.GetReservationsByUserID(TestUserId);
        return reservations.Last().Id;
    }

    [TestMethod]
    public void NewReservationStartsAsReserved()
    {
        // arrange
        long id = MakeReservation(DateTime.Today, DateTime.Today.AddDays(2));

        // act
        HotelReservationModel result = logic.SearchHotelReservationByID(id);

        // assert
        Assert.AreEqual(HotelReservationLogic.StatusReserved, result.Status);
        Assert.IsNull(result.CheckInTime);
        Assert.IsNull(result.CheckOutTime);
    }

    [TestMethod]
    public void CheckInValidReservation()
    {
        // arrange
        long id = MakeReservation(DateTime.Today, DateTime.Today.AddDays(2));

        // act
        bool result = logic.CheckIn(id);

        // assert
        Assert.IsTrue(result);
        Assert.AreEqual(HotelReservationLogic.StatusCheckedIn, logic.SearchHotelReservationByID(id).Status);
    }

    [TestMethod]
    public void CheckInTimeIsSavedAutomatically()
    {
        // arrange
        long id = MakeReservation(DateTime.Today, DateTime.Today.AddDays(2));
        DateTime before = DateTime.Now.AddMinutes(-1);

        // act
        logic.CheckIn(id);
        HotelReservationModel result = logic.SearchHotelReservationByID(id);

        // assert
        Assert.IsNotNull(result.CheckInTime);
        Assert.IsTrue(result.CheckInTime >= before && result.CheckInTime <= DateTime.Now.AddMinutes(1));
        Assert.IsNull(result.CheckOutTime);
    }

    [TestMethod]
    public void CheckInBeforeStartDate()
    {
        // arrange
        long id = MakeReservation(DateTime.Today.AddDays(5), DateTime.Today.AddDays(7));

        // act
        bool result = logic.CheckIn(id);

        // assert
        Assert.IsFalse(result);
        Assert.AreEqual(HotelReservationLogic.StatusReserved, logic.SearchHotelReservationByID(id).Status);
    }

    [TestMethod]
    public void CheckInAfterEndDate()
    {
        // arrange
        long id = MakeReservation(DateTime.Today.AddDays(-5), DateTime.Today.AddDays(-2));

        // act
        bool result = logic.CheckIn(id);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void CheckInTwice()
    {
        // arrange
        long id = MakeReservation(DateTime.Today, DateTime.Today.AddDays(2));
        logic.CheckIn(id);

        // act
        bool result = logic.CheckIn(id);

        // assert
        Assert.IsFalse(result);
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(999999)] // does not exist
    public void CheckInInvalidId(long id)
    {
        // act
        bool result = logic.CheckIn(id);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void CheckInWhenRoomIsStillOccupied()
    {
        // arrange: the guest before did not check out yet
        long first = MakeReservation(DateTime.Today.AddDays(-3), DateTime.Today);
        access.CheckIn(first);
        long second = MakeReservation(DateTime.Today, DateTime.Today.AddDays(2));

        // act
        bool result = logic.CheckIn(second);

        // assert
        Assert.IsFalse(result);
        Assert.AreEqual(HotelReservationLogic.StatusReserved, logic.SearchHotelReservationByID(second).Status);
    }

    [TestMethod]
    public void CheckInWhenRoomIsFreeAgain()
    {
        // arrange: the guest before did check out
        long first = MakeReservation(DateTime.Today.AddDays(-3), DateTime.Today);
        access.CheckIn(first);
        logic.CheckOut(first);
        long second = MakeReservation(DateTime.Today, DateTime.Today.AddDays(2));

        // act
        bool result = logic.CheckIn(second);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CheckOutAfterCheckIn()
    {
        // arrange
        long id = MakeReservation(DateTime.Today, DateTime.Today.AddDays(2));
        logic.CheckIn(id);

        // act
        bool result = logic.CheckOut(id);

        // assert
        Assert.IsTrue(result);
        Assert.AreEqual(HotelReservationLogic.StatusCheckedOut, logic.SearchHotelReservationByID(id).Status);
    }

    [TestMethod]
    public void CheckOutTimeIsSavedAutomatically()
    {
        // arrange
        long id = MakeReservation(DateTime.Today, DateTime.Today.AddDays(2));
        logic.CheckIn(id);
        DateTime before = DateTime.Now.AddMinutes(-1);

        // act
        logic.CheckOut(id);
        HotelReservationModel result = logic.SearchHotelReservationByID(id);

        // assert
        Assert.IsNotNull(result.CheckInTime);
        Assert.IsNotNull(result.CheckOutTime);
        Assert.IsTrue(result.CheckOutTime >= before && result.CheckOutTime <= DateTime.Now.AddMinutes(1));
        Assert.IsTrue(result.CheckOutTime >= result.CheckInTime);
    }

    [TestMethod]
    public void CheckOutWithoutCheckIn()
    {
        // arrange
        long id = MakeReservation(DateTime.Today, DateTime.Today.AddDays(2));

        // act
        bool result = logic.CheckOut(id);

        // assert
        Assert.IsFalse(result);
        Assert.AreEqual(HotelReservationLogic.StatusReserved, logic.SearchHotelReservationByID(id).Status);
    }

    [TestMethod]
    public void CheckOutTwice()
    {
        // arrange
        long id = MakeReservation(DateTime.Today, DateTime.Today.AddDays(2));
        logic.CheckIn(id);
        logic.CheckOut(id);

        // act
        bool result = logic.CheckOut(id);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void CheckInAfterCheckOut()
    {
        // arrange
        long id = MakeReservation(DateTime.Today, DateTime.Today.AddDays(2));
        logic.CheckIn(id);
        logic.CheckOut(id);

        // act
        bool result = logic.CheckIn(id);

        // assert
        Assert.IsFalse(result);
        Assert.AreEqual(HotelReservationLogic.StatusCheckedOut, logic.SearchHotelReservationByID(id).Status);
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(999999)]
    public void CheckOutInvalidId(long id)
    {
        // act
        bool result = logic.CheckOut(id);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void RoomIsOccupiedWhileGuestIsCheckedIn()
    {
        // arrange
        long id = MakeReservation(DateTime.Today, DateTime.Today.AddDays(2));
        bool before = logic.RoomIsOccupied(TestRoomId);

        // act
        logic.CheckIn(id);
        bool during = logic.RoomIsOccupied(TestRoomId);
        logic.CheckOut(id);
        bool after = logic.RoomIsOccupied(TestRoomId);

        // assert
        Assert.IsFalse(before);
        Assert.IsTrue(during);
        Assert.IsFalse(after);
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void RoomIsOccupiedInvalidRoom(long roomId)
    {
        // act
        bool result = logic.RoomIsOccupied(roomId);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void GetAllReservationsContainsNewReservation()
    {
        // arrange
        long id = MakeReservation(DateTime.Today, DateTime.Today.AddDays(2));

        // act
        List<HotelReservationModel> result = logic.GetAllReservations();

        // assert
        Assert.IsTrue(result.Any(r => r.Id == id));
    }
}
