public class ActivityLogic
{
    private ActivityAccess _activityAccess = new ActivityAccess();

    public List<ActivityModel> GetAll()
    {
        return _activityAccess.GetAll();
    }

    public ActivityModel GetByID(int id)
    {
        return _activityAccess.GetByID(id);
    }

    public void Add(ActivityModel activity)
    {
        _activityAccess.Write(activity);
    }

    public void Update(ActivityModel activity)
    {
        _activityAccess.Update(activity);
    }

    public bool Delete(int id)
    {
        return _activityAccess.DeleteByID(id);
    }
}