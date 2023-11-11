
using System.Data.Common;
using Dapper;

namespace Movies.DataAccess;

public sealed class UserRepository : IUserRepository
{
     private readonly IDbConnectionFactory _db;
    
    public UserRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async ValueTask<int> CreateAsync(User user, CancellationToken token = default)
    {
       var conn  = await  _db.CreateConnectionAsync(token);

       return  1;
    }

    public ValueTask<bool> DeleteAsync(int id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> EditAsync(User user, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetAsync(int id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}