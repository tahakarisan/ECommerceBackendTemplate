using Business.Abstract.Auths;
using Business.Constants;
using Core.Entities.Concrete.Auth;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.DTOs.Users;
using System.Linq.Expressions;

namespace Business.Concrete.Auths
{
    public class ResetPasswordCodeManager : IResetPasswordCodeService
    {
        private IResetPasswordCodeDal _resetPasswordCodeDal;
        public ResetPasswordCodeManager(IResetPasswordCodeDal resetPasswordCodeDal)
        {
            _resetPasswordCodeDal = resetPasswordCodeDal;
        }
        public async Task<IResult> AddAsync(ResetPasswordCode resetPasswordCode)
        {
            int result = await _resetPasswordCodeDal.AddAsync(resetPasswordCode);
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        public async Task<IResult> ConfirmResetCodeAsync(string code)
        {
            ResetPasswordCode result = await _resetPasswordCodeDal.GetAsync(p => p.Code == code);
            if (result.IsActive == false) { return new ErrorResult("link geçersizdir"); }

            bool IsAvailable;
            if (result.EndDate <= DateTime.Now) { IsAvailable = true; }
            else { IsAvailable = false; }
            if (IsAvailable == false) { return new ErrorResult("Linkin Süresi Geçmiştir."); }
            return new SuccessResult();
        }
        public async Task<IResult> ConfirmResetCodeWithUserIdAsync(ConfirmPasswordResetDto confirmPasswordResetDto)
        {
            ResetPasswordCode result = await _resetPasswordCodeDal.GetAsync(p => p.Code == confirmPasswordResetDto.Code);

            if (result.UserId != confirmPasswordResetDto.UserId) { return new ErrorResult("link geçersizdir"); }

            if (result.IsActive == false) { return new ErrorResult("link geçersizdir"); }

            bool IsAvailable;
            if (result.EndDate > DateTime.Now) { IsAvailable = true; }
            else { IsAvailable = false; }

            if (IsAvailable == false) { return new ErrorResult("Linkin Süresi Geçmiştir."); }
            return new SuccessResult();
        }
        public async Task<IResult> DeleteAsync(ResetPasswordCode resetPasswordCode)
        {
            bool result = await _resetPasswordCodeDal.DeleteAsync(resetPasswordCode);
            return result ? new SuccessResult(Messages.Deleted) : new ErrorResult(Messages.NotDeleted);
        }
        public async Task<IDataResult<List<ResetPasswordCode>>> GetAllAsync()
        {
            List<ResetPasswordCode>? result = await _resetPasswordCodeDal.GetAllAsync();
            return result != null ? new SuccessDataResult<List<ResetPasswordCode>>(result, Messages.Listed) : new ErrorDataResult<List<ResetPasswordCode>>(Messages.NotListed);
        }
        public async Task<IDataResult<ResetPasswordCode>> GetByCodeAsync(string resetCode)
        {
            ResetPasswordCode result = await _resetPasswordCodeDal.GetAsync(p => p.Code == resetCode);
            return result != null ? new SuccessDataResult<ResetPasswordCode>(result, Messages.Found) : new ErrorDataResult<ResetPasswordCode>(Messages.NotFound);

        }
        public async Task<IDataResult<ResetPasswordCode>> GetByIdAsync(int resetCodeID)
        {
            ResetPasswordCode result = await _resetPasswordCodeDal.GetAsync(p => p.Id == resetCodeID);
            return result != null ? new SuccessDataResult<ResetPasswordCode>(result, Messages.Found) : new ErrorDataResult<ResetPasswordCode>(Messages.NotFound);
        }
        public async Task<IDataResult<ResetPasswordCode>> GetByUserIdAsync(int userId)
        {
            ResetPasswordCode result = await _resetPasswordCodeDal.GetAsync(p => p.UserId == userId);
            return result != null ? new SuccessDataResult<ResetPasswordCode>(result, Messages.Found) : new ErrorDataResult<ResetPasswordCode>(Messages.NotFound);
        }
        public async Task<IDataResult<ResetPasswordCode>> GetAsync(Expression<Func<ResetPasswordCode, bool>> filter)
        {
            ResetPasswordCode result = await _resetPasswordCodeDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<ResetPasswordCode>(result, Messages.Found) : new ErrorDataResult<ResetPasswordCode>(Messages.NotFound);
        }
        public async Task<IResult> UpdateAsync(ResetPasswordCode resetPasswordCode)
        {
            bool result = await _resetPasswordCodeDal.UpdateAsync(resetPasswordCode);
            return result ? new SuccessResult(Messages.Updated) : new ErrorResult(Messages.NotUpdated);
        }

    }
}
