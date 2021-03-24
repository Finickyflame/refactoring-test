namespace LegacyApp
{
    public class UserData : IUserData
    {
        public void AddUser(User user) => UserDataAccess.AddUser(user);
    }
}