namespace Movies.DataAccess;

public interface IUserRepository
{
    ValueTask<int> CreateAsync(User user, CancellationToken token = default);
    ValueTask<bool> EditAsync(User user, CancellationToken token = default);
    Task<User> GetAsync(int id, CancellationToken token = default);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default);
    ValueTask<bool> DeleteAsync(int id, CancellationToken token = default);
    Task<User> GetUserByEmailAsync(string email, CancellationToken token = default);
    Task<User> GetUserByEmailAndPasswordAsync(string email, string password, CancellationToken token = default);
}