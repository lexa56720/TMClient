namespace ApiWrapper.Interfaces
{
    public interface IUsersApi
    {
        public Task<bool> ChangeName(string name);
        public ValueTask<User?> GetUser(int userId);
        public ValueTask<User[]> GetUser(int[] userId);
        public Task<User[]> GetByName(string name);
    }
}
