using AutoMapper;
using BackendTzReworked.Bll.Common.ConferenceRooms.Managers;
using BackendTzReworked.Bll.Common.ConferenceRooms.Models;
using BackendTzReworked.DTOs.ConferenceRooms;
using Microsoft.AspNetCore.Mvc;

namespace BackendTzReworked.Controllers.ConferenceRooms
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : BaseController
    {
        public RoomsController(IRoomsManager _manager, IMapper _mapper, ILogger<RoomsController> _logger) : base(_mapper)
        {
            this._manager = _manager;
            this._logger = _logger;
        }
        private readonly IRoomsManager _manager;
        private readonly ILogger<RoomsController> _logger;


        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAllRooms()
        {
            _logger.LogInformation("Get request resieved.");
            var rooms = await _manager.GetAllAsync();

            if (rooms == null || !ModelState.IsValid)
                return NotFound();

            var roomDTOs = _mapper.Map<IEnumerable<RoomsDTO>>(rooms);

            return Ok(roomDTOs);
        }



        [HttpGet("{roomId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRoomById(int roomId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roomModel = await _manager.GetByIdAsync(roomId);

            if (roomModel == null)
                return NotFound();

            var roomDTO = _mapper.Map<RoomsDTO>(roomModel);

            return Ok(roomDTO);
        }



        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRoom([FromBody] RoomsDTO roomDTO,
            [FromQuery] ICollection<int>? avilableSupplementsIds)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _manager.GetByNameAsync(roomDTO.Name) != null)
            {
                ModelState.AddModelError("", "Room with the same name already created.");
                return BadRequest(ModelState);
            }

            var roomModel = _mapper.Map<MRooms>(roomDTO);

            if (await _manager.MergeRoomAndSupplementsAsync(roomModel, avilableSupplementsIds) == null)
                return BadRequest();

            await _manager.CreateAsync(roomModel);

            if (!await _manager.SaveAsync())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }



        [HttpPut("{roomId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateConferenceRoom(int roomId,
            [FromBody] RoomsDTO newRoomDataDTO,
            [FromQuery] ICollection<int>? supplementsIds)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roomToUpdateModel = await _manager.GetByIdAsync(roomId);
            if (roomToUpdateModel == null)
                return NotFound();

            roomToUpdateModel = _mapper.Map<MRooms>(newRoomDataDTO);

            if (await _manager.MergeRoomAndSupplementsAsync(roomToUpdateModel, supplementsIds) == null)
                return BadRequest();

            await _manager.UpdateAsync(roomToUpdateModel);

            if (!await _manager.SaveAsync())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }



        [HttpDelete("{roomId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteConferenceRoom(int roomId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _manager.GetByIdAsync(roomId) == null)
                return NotFound();

            await _manager.DeleteAsync(roomId);

            if (!await _manager.SaveAsync())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
