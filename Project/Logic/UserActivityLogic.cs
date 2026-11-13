public class UserActivityLogic
{
    private UserActivityAccess _userActivityAccess =
        new UserActivityAccess();

    public void Add(UserActivityModel userActivity)
    {
        _userActivityAccess.Write(userActivity);
    }

    public List<UserActivityModel> GetAll()
    {
        return _userActivityAccess.GetAll();
    }

    public List<UserActivityModel> GetByUserID(int userId)
    {
        return _userActivityAccess.GetByUserID(userId);
    }

    public List<UserActivityModel> GetByActivityID(int activityId)
    {
        return _userActivityAccess.GetByActivityID(activityId);
    }

    public bool Delete(int id)
    {
        return _userActivityAccess.DeleteByID(id);
    }
}