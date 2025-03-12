using AutoMapper;
using BackendTzReworked.Bll.Common.ConferenceRooms.Models;
using BackendTzReworked.Bll.Common.ConferenceRooms.Repositories;
using BackendTzReworked.Bll.Common.RoomSupplements.Models;
using BackendTzReworked.DAL.Base.Repositories;
using BackendTzReworked.DAL.ConferenceRooms.Entityes;
using BackendTzReworked.DAL.EntityFeamework;
using BackendTzReworked.DAL.RoomsAndSupplementsMtoM.Entityes;
using Microsoft.EntityFrameworkCore;

namespace BackendTzReworked.DAL.ConferenceRooms.Repositories
{
    public class RoomsRepository : BaseRepository, IRoomsRepository
    {
        public RoomsRepository(Context _context, IMapper _mapper) : base(_context, _mapper) { }



        public async Task<IEnumerable<MRooms>> GetAllAsync()
            => _mapper.Map<List<MRooms>>(await _context.Room.ToListAsync());

        public async Task<MRooms> GetByIdAsync(int id)
            => _mapper.Map<MRooms>(await _context.Room.FindAsync(id));

        public async Task<MRooms> GetByNameAsync(string name)
            => _mapper.Map<MRooms>(await _context.Room.FirstOrDefaultAsync(cr => cr.Name == name));

        public async Task<List<MRooms>> GetByCapacityAsync(int capacity)
            => _mapper.Map<List<MRooms>>(await _context.Room.Where(cr => cr.Capacity >= capacity).ToListAsync());

        public async Task<IEnumerable<MRooms>> GetByCostPerHourAsync(double costPerHour)
            => _mapper.Map<List<MRooms>>(await _context.Room.Where(cr => cr.CostPerHour == costPerHour).ToListAsync());

        public async Task AddAsync(MRooms model)
            => await _context.Room.AddAsync(_mapper.Map<Rooms>(model));

        public async Task UpdateAsync(MRooms model)
            => _context.Room.Update(_mapper.Map<Rooms>(model));

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Room.FindAsync(id);
            if (entity != null)
                _context.Room.Remove(entity);
        }



        public async Task<IEnumerable<MSupplements>> GetAvilableSupplementsByIdAsync(int conferenceRoomId)
        {
            var conferenceRoom = await _context.Room
                .Include(cr => cr.RoomsAndSupplementsList)
                .ThenInclude(cas => cas.Supplement)
                .FirstOrDefaultAsync(cr => cr.Id == conferenceRoomId);

            return _mapper.Map<List<MSupplements>>(conferenceRoom?.RoomsAndSupplementsList.Select(cas => cas.Supplement).ToList());
        }



        public async Task<MRooms> MergeConferenceRoomAndAdditionalServiesAsync(MRooms roomModel, ICollection<int>? supplementsIds)
        {
            var roomEntity = _mapper.Map<Rooms>(roomModel);

            if (supplementsIds == null || !supplementsIds.Any())
                return _mapper.Map<MRooms>(roomModel);

            var supplementsEntity = await _context.Supplement
                                                   .Where(s => supplementsIds.Contains(s.Id))
                                                   .ToListAsync();

            //bll
            foreach (var supplementEntity in supplementsEntity)
            {
                if (!roomEntity.RoomsAndSupplementsList.Any(cas => cas.SupplementId == supplementEntity.Id))
                {
                    var conferenceRoomAdditionalService = new RoomsAndSupplements
                    {
                        Room = roomEntity,
                        Supplement = supplementEntity
                    };

                    roomEntity.RoomsAndSupplementsList.Add(conferenceRoomAdditionalService);
                }
            }

            return _mapper.Map<MRooms>(roomEntity);
        }
    }
}
