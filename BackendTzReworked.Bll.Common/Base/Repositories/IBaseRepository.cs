using BackendTzReworked.Bll.Common.Base.Models;

namespace BackendTzReworked.Bll.Common.Base.Repositories
{
    public interface IBaseRepository<TModel> where TModel : BaseModel 
    {
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel> GetByIdAsync(int id);
        Task AddAsync(TModel model);
        Task UpdateAsync(TModel model);
        Task DeleteAsync(int id);
        Task<bool> Save();
    }
}
