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
    public interface IFavoriteItemService
    {
        Task<IResult> AddAsync(AddFavoriteItemDto favoriteItemDto);
        Task<IResult> UpdateAsync(FavoriteItem favoriteItem);
        Task<IResult> DeleteAsync(int id);
        Task<IDataResult<IPaginate<FavoriteItem>>> GetAllAsync(int index, int size);
        Task<IDataResult<FavoriteItem>> GetAsync(Expression<Func<FavoriteItem, bool>> filter);
        Task<IDataResult<List<FavoriteItem>>> GetFavoriteItemsByIdUserIdAsync(int userId);
    }
}
