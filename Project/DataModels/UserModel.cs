public class UserModel
{

    public Int64 Id { get; set; }
    public string EmailAddress { get; set; }

    public string Password { get; set; }

    public string FullName { get; set; }
    public string PhoneNumber { get; set; }

    public string DateOfBirth { get; set; }
    public string Role { get; set; }



    public UserModel() { }


    public UserModel(Int64 id, string email, string password, string fullname,string phone,string date,string role)
    {
        Id = id;
        EmailAddress = email;
        Password = password;
        FullName = fullname;
        PhoneNumber = phone;
        DateOfBirth = date;
        Role = role;
    }


}



