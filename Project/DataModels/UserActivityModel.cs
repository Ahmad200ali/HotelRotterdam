public class UserActivityModel
{
    public int Id { get; set; }
    public int User_Id { get; set; }
    public int Activity_Id { get; set; }

    public UserActivityModel() { }

    public UserActivityModel(int id, int userId, int activityId)
    {
        Id = id;
        User_Id = userId;
        Activity_Id = activityId;
    }
}