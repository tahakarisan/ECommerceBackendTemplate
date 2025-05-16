using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete.Auth;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;

namespace DataAccess.Concrete.EntityFramework.Auths
{
    public class EfResetPasswordCodeDal : EfEntityRepositoryBase<ResetPasswordCode, ECommerceContext>, IResetPasswordCodeDal
    {
    }
}

