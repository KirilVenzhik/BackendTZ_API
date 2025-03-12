using AutoMapper;
using BackendTzReworked.Bll.Common.RoomSupplements.Managers;
using BackendTzReworked.Bll.Common.RoomSupplements.Models;
using BackendTzReworked.DTOs.RoomSupplements;
using Microsoft.AspNetCore.Mvc;

namespace BackendTzReworked.Controllers.RoomSupplements
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplementsController : BaseController
    {
        public SupplementsController(ISupplementsManager _manager, IMapper _mapper) : base(_mapper)
        {
            this._manager = _manager;
        }
        private readonly ISupplementsManager _manager;



        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAllSupplements()
        {
            var suplementsModel = await _manager.GetAllAsync();

            if (suplementsModel == null || !ModelState.IsValid)
                return NotFound();

            var supplementsDTOs = _mapper.Map<IEnumerable<SupplementsDTO>>(suplementsModel);

            return Ok(supplementsDTOs);
        }



        [HttpGet("{additionalServiceId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSupplementById(int supplementId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplementModel = await _manager.GetByIdAsync(supplementId);

            if (supplementModel == null || !ModelState.IsValid)
                return NotFound();

            var supplementDTO = _mapper.Map<SupplementsDTO>(supplementModel);

            return Ok(supplementDTO);
        }



        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateSupplement([FromBody] SupplementsDTO newSupplementDTO)
        {
            if (newSupplementDTO == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _manager.GetByNameAsync(newSupplementDTO.Name) != null ||
                await _manager.GetByIdAsync(newSupplementDTO.Id) != null)
                return BadRequest(ModelState);

            var supplementModel = _mapper.Map<MSupplements>(newSupplementDTO);

            await _manager.CreateAsync(supplementModel);

            if (!await _manager.SaveAsync())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
        


        [HttpPut("{supplementId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSupplement(int supplementId, [FromBody] SupplementsDTO newSupplementDataDTO)
        {
            if (newSupplementDataDTO == null || !ModelState.IsValid || supplementId != newSupplementDataDTO.Id)
                return BadRequest(ModelState);

            var supplementToUpdateModel = await _manager.GetByIdAsync(supplementId);

            if (supplementToUpdateModel == null)
                return NotFound();

            supplementToUpdateModel.Name = newSupplementDataDTO.Name;
            supplementToUpdateModel.Cost = newSupplementDataDTO.Cost;

            await _manager.UpdateAsync(supplementToUpdateModel);

            //Info: "Перевіряє, чи збережено зміни"
            if (!await _manager.SaveAsync())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }



        [HttpDelete("{supplementId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSupplement(int supplementId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _manager.GetByIdAsync(supplementId) == null)
                return NotFound();

            await _manager.DeleteAsync(supplementId);

            if (!await _manager.SaveAsync())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
