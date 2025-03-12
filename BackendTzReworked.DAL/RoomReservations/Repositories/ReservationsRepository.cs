using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BackendTzReworked.Bll.Common.ConferenceRooms.Models;
using BackendTzReworked.Bll.Common.RoomReservation.Models;
using BackendTzReworked.Bll.Common.RoomReservation.Repositories;
using BackendTzReworked.DAL.EntityFeamework;
using BackendTzReworked.DAL.Base.Repositories;
using BackendTzReworked.DAL.RoomReservations.Entityes;
using BackendTzReworked.DAL.RoomSupplements.Entityes;

namespace BackendTzReworked.DAL.RoomReservations.Repositories
{
    public class ReservationsRepository : BaseRepository, IReservationsRepository
    {
        public ReservationsRepository(Context _context, IMapper _mapper) : base(_context, _mapper) { }



        public async Task<IEnumerable<MReservations>> GetAllAsync()
            => _mapper.Map<List<MReservations>>(await _context.Reservation.ToListAsync());

        public async Task<MReservations> GetByIdAsync(int id)
            => _mapper.Map<MReservations>(await _context.Reservation.FindAsync(id));

        public async Task<IEnumerable<MReservations>> GetReservationsByClientPhone(string phoneNumber)
            => _mapper.Map<List<MReservations>>(await _context.Reservation
                .Where(r => r.ClientPhone == phoneNumber)
                .ToListAsync());

        public async Task<MReservations> GetReservationByTimeReservation(DateTime timeReservation)
            => _mapper.Map<MReservations>(await _context.Reservation
                .FirstOrDefaultAsync(r => r.TimeReservation == timeReservation));

        public async Task AddAsync(MReservations model)
            => await _context.Reservation.AddAsync(_mapper.Map<Reservations>(model));

        public async Task UpdateAsync(MReservations model)
            => _context.Reservation.Update(_mapper.Map<Reservations>(model));

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Reservation.FindAsync(id);
            if (entity != null)
                _context.Reservation.Remove(entity);
        }



        public async Task<IEnumerable<MRooms>> SearchAvailableRoomsAsync(DateTime startTime, DateTime endTime, int capacity)
            => _mapper.Map<List<MRooms>>(await _context.Room
                .Include(cr => cr.Reservations)
                .Where(cr => cr.Capacity >= capacity &&
                    !cr.Reservations.Any(r => r.TimeReservation < endTime && r.EndTimeReservation > startTime))
                .ToListAsync());



        public async Task<double> CalculateReservationCost(int roomId, DateTime reservationStartTime, DateTime reservationEndTime, IEnumerable<int> supplementsIds)
        {
            var conferenceRoom = await _context.Room.FindAsync(roomId);
            if (conferenceRoom == null)
                throw new ArgumentException("Conference room not found");

            double totalCost = 0;
            var duration = reservationEndTime.Hour - reservationStartTime.Hour; 

            for (var counter = duration; counter > 0; counter--)
                if (reservationStartTime.Hour + counter >= 12 && reservationStartTime.Hour + counter < 14)
                {
                    totalCost += conferenceRoom.CostPerHour * 1.15;
                }
                else if (reservationStartTime.Hour + counter >= 6 && reservationStartTime.Hour + counter < 9)
                {
                    totalCost += conferenceRoom.CostPerHour * 0.90;
                }
                else if (reservationStartTime.Hour + counter >= 18 && reservationStartTime.Hour + counter < 23)
                {
                    totalCost += conferenceRoom.CostPerHour * 0.80;
                }
                else
                {
                    totalCost += conferenceRoom.CostPerHour;
                }

            //Info: "Додає вартість додаткових послуг до загальної суми"
            var selectedSupplements = new List<Supplements>();
            foreach (var el in supplementsIds)
            {
                selectedSupplements.Add(await _context.Supplement.FirstOrDefaultAsync(a => a.Id == el));
            }
            if (selectedSupplements != null && selectedSupplements.Any() && selectedSupplements[0] != null)
                totalCost += selectedSupplements.Sum(service => service.Cost);

            return totalCost;
        }



        public async Task<(MReservations ResultReservation, bool ResultBool)> MergeReservationAndConferenceRoom(MReservations reservation, int conferenceRoomId)
        {
            var reservationEntity = _mapper.Map<Reservations>(reservation);

            var room = await _context.Room
                .Include(cr => cr.RoomsAndSupplementsList)
                .FirstOrDefaultAsync(cr => cr.Id == conferenceRoomId);

            if (room == null)
                return (reservation, false);

            var selectedSupplementsIds = reservationEntity.SelectedSupplements;

            //Info: "Перевіряє, чи були вибрані додаткові послуги"
            if (selectedSupplementsIds == null || !selectedSupplementsIds.Any())
                return (reservation, false);

            //Info: "Перевіряє, чи всі вибрані послуги доступні в даному конференц-залі"
            var availableSupplementsIds = room.RoomsAndSupplementsList
                .Select(cas => cas.SupplementId).ToList();

            if (availableSupplementsIds.Any())
                foreach (var additionalServiceId in availableSupplementsIds)
                    if (!room.RoomsAndSupplementsList.Any(cras => cras.SupplementId == additionalServiceId))
                        return (reservation, false);

            //Info: "Перевіряє, чи не перетинається бронювання з іншими бронюваннями для того ж залу"
            if (await CheckingIfReservationIsNotOverlapingOther(reservation, conferenceRoomId))
                return (reservation, false);

            //Info: "Оновлює бронювання: додає вибрані послуги і конференц-зал"
            reservationEntity.ReservedRoom = room;
            reservationEntity.SelectedSupplements = selectedSupplementsIds;

            return (_mapper.Map<MReservations>(reservationEntity), true);
        }

        public async Task<bool> CheckingIfReservationIsNotOverlapingOther(MReservations reservation, int reservedRoomId)
        {
            if (reservation == null)
                return false;

            return await _context.Reservation
                .AnyAsync(r => r.ReservedRoom.Id == reservedRoomId &&
                               r.TimeReservation < reservation.EndTimeReservation &&
                               r.EndTimeReservation > reservation.TimeReservation);
        }
    }
}
