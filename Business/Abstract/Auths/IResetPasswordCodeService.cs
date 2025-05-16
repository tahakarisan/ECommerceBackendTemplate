using Core.Entities.Concrete.Auth;
using Core.Utilities.Results;
using Entities.DTOs.Users;
using System.Linq.Expressions;

namespace Business.Abstract.Auths
{
    public interface IResetPasswordCodeService
    {
        Task<IResult> AddAsync(ResetPasswordCode resetPasswordCode);
        Task<IResult> UpdateAsync(ResetPasswordCode resetPasswordCode);
        Task<IResult> DeleteAsync(ResetPasswordCode resetPasswordCode);
        Task<IDataResult<List<ResetPasswordCode>>> GetAllAsync();
        Task<IDataResult<ResetPasswordCode>> GetAsync(Expression<Func<ResetPasswordCode, bool>> filter);
        Task<IDataResult<ResetPasswordCode>> GetByCodeAsync(string resetCode);
        Task<IDataResult<ResetPasswordCode>> GetByUserIdAsync(int userId);
        Task<IResult> ConfirmResetCodeAsync(string code);
        Task<IResult> ConfirmResetCodeWithUserIdAsync(ConfirmPasswordResetDto confirmPasswordResetDto);
    }
}
