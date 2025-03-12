using BackendTzReworked.Bll.Common.Base.Managers;
using BackendTzReworked.Bll.Common.RoomSupplements.Models;

namespace BackendTzReworked.Bll.Common.RoomSupplements.Managers
{
    public interface ISupplementsManager : IBaseManager<MSupplements>
    {
        Task<MSupplements> GetByNameAsync(string name);
        Task<IEnumerable<MSupplements>> GetByCostAsync(double cost);
    }
}
