namespace UnitTests;



[TestClass]
public sealed class Test1
{


    [DataTestMethod]
    [DataRow("kevin@kevin.nl", "kevin")]
    public void LoginValidCredentials(string m, string p)
    {
        // arrange
        UserLogic l = new();
        UserAccess access = new();

        // act 
        UserModel result = l.CheckLogin(m, p);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(m, result.EmailAddress);
        Assert.AreEqual(p, result.Password);
    }

    [DataTestMethod]
    [DataRow("kevin@kevin.nl", "wrong")] // wrong password
    [DataRow("wrong1", "kevin")] // wrong email
    [DataRow("wrong2", "wrong")] // everything wrong
    [DataRow("", "")]
    [DataRow(null, null)]
    public void LoginInvalidCredentials(string m, string p)
    {
        // arrange
        UserLogic l = new();

        // act 
        UserModel result = l.CheckLogin(m, p);

        // assert
        Assert.IsNull(result);
    }
}
