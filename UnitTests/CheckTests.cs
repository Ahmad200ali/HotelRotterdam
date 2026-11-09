namespace UnitTests;

[TestClass]
public sealed class CheckTests
{
    [DataTestMethod]
    [DataRow("kevin@kevin.nl")]
    [DataRow("test@test.com")]
    [DataRow("TEST@TEST.NL")] // capital letters are fine
    public void ValidEmail(string email)
    {
        // act
        bool result = Check.CheckValidEmail(email);

        // assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("")] // empty
    [DataRow("   ")]
    [DataRow(null)]
    [DataRow("kevin.nl")] // no @
    [DataRow("kevin@kevin")] // no .nl or .com
    [DataRow("kevin@kevin.de")] // wrong ending
    public void InvalidEmail(string email)
    {
        // act
        bool result = Check.CheckValidEmail(email);

        // assert
        Assert.IsFalse(result);
    }

    [DataTestMethod]
    [DataRow("Welkom12!")]
    [DataRow("p@ssword1")]
    public void ValidPassword(string password)
    {
        // act
        bool result = Check.CheckValidPassword(password);

        // assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("")] // empty
    [DataRow(null)]
    [DataRow("kort1!")] // too short
    [DataRow("geencijfer!")] // no number
    [DataRow("geenteken123")] // no special character
    public void InvalidPassword(string password)
    {
        // act
        bool result = Check.CheckValidPassword(password);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidNameAndPhoneNumber()
    {
        Assert.IsTrue(Check.CheckValidName("Kevin de Vries"));
        Assert.IsTrue(Check.CheckValidPhoneNumber("0612345678"));
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow(null)]
    public void EmptyNameAndPhoneNumber(string input)
    {
        Assert.IsFalse(Check.CheckValidName(input));
        Assert.IsFalse(Check.CheckValidPhoneNumber(input));
    }

    [DataTestMethod]
    [DataRow("31-12-2000")]
    [DataRow("01-01-1990")]
    [DataRow("29-02-2024")] // leap year
    public void ValidDateOfBirth(string date)
    {
        // act
        bool result = Check.CheckValidDateOfBirth(date);

        // assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("")] // empty
    [DataRow("abc")]
    [DataRow("2000-12-31")] // year first
    [DataRow("31/12/2000")] // wrong sign between the numbers
    [DataRow("32-01-2000")] // day doesnt exist
    [DataRow("29-02-2023")] // not a leap year
    public void InvalidDateOfBirth(string date)
    {
        // act
        bool result = Check.CheckValidDateOfBirth(date);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void DateOfBirthCanNotBeInTheFuture()
    {
        // arrange
        string today = DateTime.Today.ToString("dd-MM-yyyy");
        string tomorrow = DateTime.Today.AddDays(1).ToString("dd-MM-yyyy");

        // assert
        Assert.IsTrue(Check.CheckValidDateOfBirth(today));
        Assert.IsFalse(Check.CheckValidDateOfBirth(tomorrow));
    }

    [DataTestMethod]
    [DataRow("12.50", 12.50)] // with a dot
    [DataRow("12,50", 12.50)] // with a comma
    [DataRow("80", 80)]
    [DataRow("0", 0)]
    [DataRow(" 5,5 ", 5.5)] // spaces around it
    public void ValidPrice(string input, double expected)
    {
        // act
        bool result = Check.TryParsePrice(input, out decimal price);

        // assert
        Assert.IsTrue(result);
        Assert.AreEqual((decimal)expected, price);
    }

    [DataTestMethod]
    [DataRow("")] // empty
    [DataRow("abc")]
    [DataRow("12.5.0")]
    [DataRow("1.000,50")]
    [DataRow("-5")] // negative
    public void InvalidPrice(string input)
    {
        // act
        bool result = Check.TryParsePrice(input, out decimal price);

        // assert
        Assert.IsFalse(result);
    }
}
