using BackendTzReworked.Bll.Common.RoomSupplements.Managers;
using BackendTzReworked.Bll.Common.RoomSupplements.Models;
using BackendTzReworked.Bll.Common.RoomSupplements.Repositories;

namespace BackendTzReworked.Bll.RoomSupplements.Managers
{
    public class SupplementsManager : ISupplementsManager
    {
        public SupplementsManager(ISupplementsRepository _repository)
        {
            this._repository = _repository;
        }

        private readonly ISupplementsRepository _repository;



        public async Task<IEnumerable<MSupplements>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<MSupplements> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<MSupplements> GetByNameAsync(string name)
            => await _repository.GetByNameAsync(name);

        public async Task<IEnumerable<MSupplements>> GetByCostAsync(double cost)
            => await _repository.GetByCostAsync(cost);

        public async Task CreateAsync(MSupplements model)
            => await _repository.AddAsync(model);

        public async Task UpdateAsync(MSupplements model)
            => await _repository.UpdateAsync(model);

        public async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);

        public async Task<bool> SaveAsync()
            => await _repository.Save();
    }
}
