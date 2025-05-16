using Core.Entities.Concrete.Auth;
using Core.Utilities.Results;
using System.Linq.Expressions;


namespace Business.Abstract.Auths
{
    public interface IUserService
    {
        Task<IResult> AddAsync(User user);
        Task<IResult> UpdateAsync(User user);
        Task<IResult> DeleteAsync(User user);
        Task<IResult> UpdateInfosAsync(User user);
        Task<List<OperationClaim>> GetClaimsAsync(User user);
        Task<IDataResult<User>> GetUserByEmailAsync(string email);
        Task<User> GetByMailAsync(string email);
        Task<IDataResult<User>> GetAsync(Expression<Func<User, bool>> filter);
        Task<bool> IsExistAsync(Expression<Func<User, bool>> filter);
    }
}
