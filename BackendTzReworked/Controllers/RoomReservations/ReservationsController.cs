using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BackendTzReworked.Bll.Common.RoomReservation.Managers;
using BackendTzReworked.Bll.Common.RoomReservation.Models;
using BackendTzReworked.DTOs.RoomReservations;
using BackendTzReworked.DTOs.ConferenceRooms;

namespace BackendTzReworked.Controllers.RoomReservations
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : BaseController
    {
        public ReservationsController(IReservationsManager _manager, IMapper _mapper) : base(_mapper)
        {
            this._manager = _manager;
        }
        private readonly IReservationsManager _manager;



        [HttpGet("GetAllReservations")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservationsModel = await _manager.GetAllAsync();

            if (reservationsModel == null || !ModelState.IsValid)
                return NotFound();

            var reservationsDTOs = _mapper.Map<IEnumerable<ReservationsDTO>>(reservationsModel);

            return Ok(reservationsDTOs);
        }



        [HttpGet("{reservationId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetReservationById(int reservationId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reservationModel = await _manager.GetByIdAsync(reservationId);

            //Info: "Перевіряє, чи знайдено бронювання"
            if (reservationModel == null)
                return NotFound();

            //Info: "Мапує отримане бронювання до DTO і повертає результат"
            var reservationDTO = _mapper.Map<MReservations>(reservationModel);

            return Ok(reservationDTO);
        }



        [HttpGet("GetReservationCost/{reservationId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetReservationCostById(int reservationId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reservationModel = await _manager.GetByIdAsync(reservationId);

            if (reservationModel == null)
                return NotFound();

            var reservationCost = await _manager.CalculateReservationCostAsync(reservationModel.ReservedRoom.Id, 
                reservationModel.TimeReservation, 
                reservationModel.EndTimeReservation, 
                reservationModel.SelectedSupplements);

            return Ok(reservationCost);
        }



        [HttpGet("GetAvilableRooms")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAvilableRooms(
            [FromQuery] DateTime startTime, [FromQuery] DateTime endTime, [FromQuery] int capacity)
        {
            if (!ModelState.IsValid || startTime > endTime || capacity <= 0)
                return BadRequest(ModelState);

            var roomsModel = await _manager.SearchAvailableRoomsAsync(startTime, endTime, capacity);

            if (!roomsModel.Any())
                return NotFound();

            var roomsDTOs = _mapper.Map<IEnumerable<RoomsDTO>>(roomsModel);

            return Ok(roomsDTOs);
        }



        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationsDTO newReservationDTO,
            [FromQuery] int roomId)
        {
            if (newReservationDTO == null || !ModelState.IsValid)
                return BadRequest();

            var reservationModel = _mapper.Map<MReservations>(newReservationDTO);

            var resultMerge = await _manager.MergeReservationAndConferenceRoomAsync(reservationModel, roomId);

            if (!resultMerge.ResultBool)
                return BadRequest(ModelState);

            reservationModel = resultMerge.ResultReservation;

            await _manager.CreateAsync(reservationModel);

            if (!await _manager.SaveAsync())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);
            }

            return Ok(await _manager.CalculateReservationCostAsync(reservationModel.ReservedRoom.Id,
                reservationModel.TimeReservation, 
                reservationModel.EndTimeReservation,
                reservationModel.SelectedSupplements));
        }



        [HttpDelete("{reservationId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteReservation(int reservationId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (await _manager.GetByIdAsync(reservationId) == null)
                return NotFound();

            await _manager.DeleteAsync(reservationId);

            if (!await _manager.SaveAsync())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
