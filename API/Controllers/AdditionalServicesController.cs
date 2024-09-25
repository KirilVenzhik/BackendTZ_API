using API.Dto;
using API.Entityes;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdditionalServicesController : ControllerBase
    {
        //Info: "Приватні поля для репозиторію додаткових послуг та AutoMapper"
        private readonly IAdditionalServicesRepository _additionalServiceRepository;
        private readonly IMapper _mapper;

        //Info: "Конструктор, що ініціалізує залежності"
        public AdditionalServicesController(IAdditionalServicesRepository serviceRepository, IMapper mapper)
        {
            _additionalServiceRepository = serviceRepository;
            _mapper = mapper;
        }

        //Info: "Метод для отримання всіх додаткових послуг"
        [HttpGet]
        [ProducesResponseType(200)]  // Успішний результат
        public async Task<IActionResult> GetAllAdditionalServices()
        {
            //Info: "Отримує всі послуги через репозиторій"
            var services = await _additionalServiceRepository.GetAllAsync();

            //Info: "Перевіряє, чи є послуги в базі і чи валідний ModelState"
            if (services == null || !ModelState.IsValid)
                return NotFound();

            //Info: "Мапує отримані послуги до DTO за допомогою AutoMapper"
            var serviceMap = _mapper.Map<IEnumerable<AdditionalServicesDto>>(services);

            //Info: "Повертає результат у форматі Ok (статус 200)"
            return Ok(serviceMap);
        }

        //Info: "Метод для отримання конкретної послуги за Id"
        [HttpGet("{additionalServiceId}")]
        [ProducesResponseType(200)]  // Успішний результат
        [ProducesResponseType(400)]  // Некоректний запит
        [ProducesResponseType(404)]  // Не знайдено
        public async Task<IActionResult> GetAdditionalServiceById(int additionalServiceId)
        {
            //Info: "Перевіряє, чи валідний ModelState"
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Info: "Шукає послугу за Id через репозиторій"
            var service = await _additionalServiceRepository.GetByIdAsync(additionalServiceId);

            //Info: "Перевіряє, чи знайдена послуга та чи валідний ModelState"
            if (service == null || !ModelState.IsValid)
                return NotFound();

            //Info: "Мапує отриману послугу до DTO і повертає результат"
            var serviceMap = _mapper.Map<AdditionalServicesDto>(service);

            return Ok(serviceMap);
        }

        //Info: "Метод для створення нової послуги"
        [HttpPost]
        [ProducesResponseType(200)]  // Успішне створення
        [ProducesResponseType(400)]  // Некоректний запит
        [ProducesResponseType(500)]  // Помилка сервера
        public async Task<IActionResult> CreateAdditionalService([FromBody] AdditionalServicesDto serviceToCreate)
        {
            //Info: "Перевіряє, чи передані дані не порожні та чи валідний ModelState"
            if (serviceToCreate == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            //Info: "Перевіряє, чи існує вже така послуга за назвою або Id"
            if (await _additionalServiceRepository.GetByNameAsync(serviceToCreate.Name) != null ||
                await _additionalServiceRepository.GetByIdAsync(serviceToCreate.Id) != null)
                return BadRequest(ModelState);

            //Info: "Мапує DTO на сутність та додає її через репозиторій"
            var serviceMap = _mapper.Map<AdditionalServices>(serviceToCreate);

            await _additionalServiceRepository.AddAsync(serviceMap);

            //Info: "Перевіряє, чи збережено зміни"
            if (!await _additionalServiceRepository.Save())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);  // Повертає статус 500 при помилці
            }

            return Ok();
        }

        //Info: "Метод для оновлення існуючої послуги"
        [HttpPut("{additionalServicesId}")]
        [ProducesResponseType(200)]  // Успішне оновлення
        [ProducesResponseType(400)]  // Некоректний запит
        [ProducesResponseType(404)]  // Не знайдено
        [ProducesResponseType(500)]  // Помилка сервера
        public async Task<IActionResult> UpdateAdditonalService(int additionalServicesId, [FromBody] AdditionalServicesDto serviceUpdate)
        {
            //Info: "Перевіряє, чи передані дані не порожні, чи валідний ModelState і чи співпадають Id"
            if (serviceUpdate == null || !ModelState.IsValid || additionalServicesId != serviceUpdate.Id)
                return BadRequest(ModelState);

            //Info: "Шукає послугу за Id для оновлення"
            var serviceToUpdate = await _additionalServiceRepository.GetByIdAsync(additionalServicesId);

            if (serviceToUpdate == null)
                return NotFound();

            //Info: "Оновлює властивості послуги"
            serviceToUpdate.Name = serviceUpdate.Name;
            serviceToUpdate.Cost = serviceUpdate.Cost;

            var serviceMap = _mapper.Map<AdditionalServices>(serviceToUpdate);

            await _additionalServiceRepository.UpdateAsync(serviceMap);

            //Info: "Перевіряє, чи збережено зміни"
            if (!await _additionalServiceRepository.Save())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        //Info: "Метод для видалення послуги за Id"
        [HttpDelete("{additionalServiceId}")]
        [ProducesResponseType(204)]  // Успішне видалення
        [ProducesResponseType(404)]  // Не знайдено
        [ProducesResponseType(500)]  // Помилка сервера
        public async Task<IActionResult> DeleteAdditionalService(int additionalServiceId)
        {
            //Info: "Перевіряє, чи валідний ModelState"
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Info: "Шукає послугу за Id перед видаленням"
            if (await _additionalServiceRepository.GetByIdAsync(additionalServiceId) == null)
                return NotFound();

            await _additionalServiceRepository.DeleteAsync(additionalServiceId);

            //Info: "Перевіряє, чи збережено зміни після видалення"
            if (!await _additionalServiceRepository.Save())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);
            }

            return NoContent();  // Повертає статус 204 (No Content) при успішному видаленні
        }
    }
}
