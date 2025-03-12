using AutoMapper;
using BackendTzReworked.Bll.Common.RoomSupplements.Models;
using BackendTzReworked.Bll.Common.RoomSupplements.Repositories;
using BackendTzReworked.DAL.Base.Repositories;
using BackendTzReworked.DAL.EntityFeamework;
using BackendTzReworked.DAL.RoomSupplements.Entityes;
using Microsoft.EntityFrameworkCore;

namespace BackendTzReworked.DAL.RoomSupplements.Repositories
{
    public class SupplementsRepository : BaseRepository, ISupplementsRepository
    {
        public SupplementsRepository(Context _context, IMapper _mapper) : base(_context, _mapper) { }

        public async Task<IEnumerable<MSupplements>> GetAllAsync()
            => _mapper.Map<List<MSupplements>>(await _context.Supplement.ToListAsync());

        public async Task<MSupplements> GetByIdAsync(int id)
            => _mapper.Map<MSupplements>(await _context.Supplement.FindAsync(id));

        public async Task AddAsync(MSupplements model)
            => await _context.Supplement.AddAsync(_mapper.Map<Supplements>(model));

        public async Task UpdateAsync(MSupplements model)
            => _context.Supplement.Update(_mapper.Map<Supplements>(model));

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Supplement.FindAsync(id);
            if (entity != null)
                _context.Supplement.Remove(entity);
        }

        public async Task<MSupplements> GetByNameAsync(string name)
            => _mapper.Map<MSupplements>(await _context.Supplement.FirstOrDefaultAsync(a => a.Name == name));

        public async Task<IEnumerable<MSupplements>> GetByCostAsync(double cost)
            => _mapper.Map<List<MSupplements>>(await _context.Supplement.Where(a => a.Cost == cost).ToListAsync());

    }
}
