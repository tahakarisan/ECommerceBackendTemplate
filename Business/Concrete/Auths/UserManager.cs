using Business.Abstract.Auths;
using Business.Constants;
using Core.Entities.Concrete.Auth;
using Core.Utilities.Results;
using DataAccess.Abstract;
using System.Linq.Expressions;

namespace Business.Concrete.Auths
{
    public class UserManager : IUserService
    {
        private IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }
        public async Task<IResult> AddAsync(User user)
        {
            int result = await _userDal.AddAsync(user);
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        public async Task<IResult> UpdateAsync(User user)
        {
            bool result = await _userDal.UpdateAsync(user);
            return result ? new SuccessResult(Messages.Updated) : new ErrorResult(Messages.NotUpdated);
        }
        public async Task<IResult> DeleteAsync(User user)
        {
            bool result = await _userDal.DeleteAsync(user);
            return result ? new SuccessResult(Messages.Deleted) : new ErrorResult(Messages.NotDeleted);
        }
        public async Task<IResult> UpdateInfosAsync(User user)
        {
            User userToUpdate = await _userDal.GetAsync(q => q.Id == user.Id);
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Email = user.Email;
            bool result = await _userDal.UpdateAsync(userToUpdate);
            return result ? new SuccessResult(Messages.Updated) : new ErrorResult(Messages.NotUpdated);
        }
        public async Task<List<OperationClaim>> GetClaimsAsync(User user)
        {
            return await _userDal.GetClaimsAsync(user);
        }
        public async Task<User> GetByMailAsync(string email)
        {
            return await _userDal.GetAsync(u => u.Email == email);
        }
        public async Task<IDataResult<User>> GetUserByEmailAsync(string email)
        {
            User result = await _userDal.GetAsync(u => u.Email == email);
            return result != null ? new SuccessDataResult<User>(result, Messages.Found) : new ErrorDataResult<User>(Messages.NotFound);
        }
        public async Task<IDataResult<User>> GetAsync(Expression<Func<User, bool>> filter)
        {
            User result = await _userDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<User>(result, Messages.Found) : new ErrorDataResult<User>(Messages.NotFound);
        }
        public async Task<bool> IsExistAsync(Expression<Func<User, bool>> filter)
        {
            return await _userDal.IsExistAsync(filter);
        }
    }
}
