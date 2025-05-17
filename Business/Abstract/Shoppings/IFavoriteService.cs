using Core.Utilities.Paging;
using Core.Utilities.Results;
using Entities.Concrete.Shoppings;
using Entities.DTOs.Shoppings;
using Entities.DTOs.Shoppings.FavoriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract.Shoppings
{
    public interface IFavoriteService
    {
        Task<IResult> AddAsync(AddFavoriteDto addFavoriteDto);
        Task<IResult> UpdateAsync(Favorite favorite);
        Task<IResult> DeleteAsync(int id);
        Task<IDataResult<IPaginate<Favorite>>> GetAllAsync(int index, int size);
        Task<IDataResult<Favorite>> GetAsync(Expression<Func<Favorite, bool>> filter);
    }
}
