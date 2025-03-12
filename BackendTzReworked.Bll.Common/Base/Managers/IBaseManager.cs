using BackendTzReworked.Bll.Common.Base.Models;

namespace BackendTzReworked.Bll.Common.Base.Managers
{
    public interface IBaseManager<TModel> where TModel : BaseModel
    {
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel> GetByIdAsync(int id);
        Task CreateAsync(TModel model);
        Task UpdateAsync(TModel model);
        Task DeleteAsync(int id);
        Task<bool> SaveAsync();
    }
}
