using BackendTzReworked.Bll.Common.Base.Repositories;
using BackendTzReworked.Bll.Common.RoomSupplements.Models;

namespace BackendTzReworked.Bll.Common.RoomSupplements.Repositories
{
    public interface ISupplementsRepository : IBaseRepository<MSupplements>
    {
        Task<MSupplements> GetByNameAsync(string name);
        Task<IEnumerable<MSupplements>> GetByCostAsync(double cost);
    }
}
