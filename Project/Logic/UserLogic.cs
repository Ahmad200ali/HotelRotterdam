public class UserLogic
{
    public static UserModel? CurrentAccount { get; private set; }
    private UserAccess _access = new();

    public UserLogic()
    {

    }

    public UserModel CheckLogin(string email, string password)
    {


        UserModel acc = _access.GetByEmail(email);
        if (acc != null && acc.Password == password)
        {
            CurrentAccount = acc;
            return acc;
        }
        return null!;
    }

    public bool Register(string email, string pass, string name, string phone, string date)
    {
        UserModel? existingUser = _access.GetByEmail(email);
        if (existingUser != null)
        {
            return false;
        }

        UserModel newUser = new UserModel
        {
            EmailAddress = email,
            Password = pass,
            FullName = name,
            DateOfBirth = date,
            PhoneNumber = phone,
            Role = "USER"
        };

        _access.Write(newUser);
        return true;

    }
}




