using API.Dto;
using API.Entityes;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConferenceRoomController : ControllerBase
    {
        //Info: "Приватні поля для репозиторію конференц-залів та AutoMapper"
        private readonly IConferenceRoomRepository _conferenceRoomRepository;
        private readonly IMapper _mapper;

        //Info: "Конструктор, що ініціалізує залежності"
        public ConferenceRoomController(IConferenceRoomRepository conferenceRoomRepository, IMapper mapper)
        {
            _conferenceRoomRepository = conferenceRoomRepository;
            _mapper = mapper;
        }

        //Info: "Метод для отримання всіх конференц-залів"
        [HttpGet]
        [ProducesResponseType(200)]  // Успішний результат
        public async Task<IActionResult> GetAllConferenceRooms()
        {
            //Info: "Отримує всі зали через репозиторій"
            var conferenceRoom = await _conferenceRoomRepository.GetAllAsync();

            //Info: "Перевіряє, чи є зали в базі і чи валідний ModelState"
            if (conferenceRoom == null || !ModelState.IsValid)
                return NotFound();

            //Info: "Мапує отримані зали до DTO за допомогою AutoMapper"
            var conferenceRoomMap = _mapper.Map<IEnumerable<ConferenceRoomDto>>(conferenceRoom);

            //Info: "Повертає результат у форматі Ok (статус 200)"
            return Ok(conferenceRoomMap);
        }

        //Info: "Метод для отримання конкретного конференц-залу за Id"
        [HttpGet("{conferenceRoomId}")]
        [ProducesResponseType(200)]  // Успішний результат
        [ProducesResponseType(400)]  // Некоректний запит
        [ProducesResponseType(404)]  // Не знайдено
        public async Task<IActionResult> GetConferenceRoomById(int conferenceRoomId)
        {
            //Info: "Перевіряє, чи валідний ModelState"
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Info: "Шукає конференц-зал за Id через репозиторій"
            var conferenceRoom = await _conferenceRoomRepository.GetByIdAsync(conferenceRoomId);

            //Info: "Перевіряє, чи знайдений зал"
            if (conferenceRoom == null)
                return NotFound();

            //Info: "Мапує отриманий зал до DTO і повертає результат"
            var conferenceRoomMap = _mapper.Map<ConferenceRoomDto>(conferenceRoom);

            return Ok(conferenceRoomMap);
        }

        //Info: "Метод для створення нового конференц-залу"
        [HttpPost]
        [ProducesResponseType(200)]  // Успішне створення
        [ProducesResponseType(400)]  // Некоректний запит
        [ProducesResponseType(500)]  // Помилка сервера
        public async Task<IActionResult> CreateConferenceRoom([FromBody] ConferenceRoomDto conferenceRoomCreate,
            [FromQuery] ICollection<int>? avilableAdditionalServiceIds)
        {
            //Info: "Перевіряє, чи валідний ModelState"
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Info: "Перевіряє, чи існує конференц-зал з такою ж назвою"
            if (await _conferenceRoomRepository.GetByNameAsync(conferenceRoomCreate.Name) != null)
            {
                ModelState.AddModelError("", "Conference room with the same name already created.");
                return BadRequest(ModelState);
            }

            //Info: "Мапує DTO на сутність та додає її через репозиторій"
            var conferenceRoomMap = _mapper.Map<ConferenceRooms>(conferenceRoomCreate);

            //Info: "Об'єднує конференц-зал з вибраними додатковими послугами"
            if (await _conferenceRoomRepository.MergeConferenceRoomAndAdditionalServies(conferenceRoomMap, avilableAdditionalServiceIds) == null)
                return BadRequest();

            //Info: "Додає конференц-зал до бази даних"
            await _conferenceRoomRepository.AddAsync(conferenceRoomMap);

            //Info: "Перевіряє, чи збережено зміни"
            if (!await _conferenceRoomRepository.Save())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);  // Повертає статус 500 при помилці
            }

            return Ok();
        }

        //Info: "Метод для оновлення існуючого конференц-залу"
        [HttpPut("{conferenceRoomId}")]
        [ProducesResponseType(200)]  // Успішне оновлення
        [ProducesResponseType(400)]  // Некоректний запит
        [ProducesResponseType(404)]  // Не знайдено
        [ProducesResponseType(500)]  // Помилка сервера
        public async Task<IActionResult> UpdateConferenceRoom(int conferenceRoomId,
            [FromBody] ConferenceRoomDto conferenceRoomUpdate,
            [FromQuery] ICollection<int>? additionalServiceIds)
        {
            //Info: "Перевіряє, чи валідний ModelState"
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Info: "Шукає конференц-зал для оновлення за Id"
            var conferenceRoomResult = await _conferenceRoomRepository.GetByIdAsync(conferenceRoomId);
            if (conferenceRoomResult == null)
                return NotFound();

            //Info: "Оновлює конференц-зал новими даними"
            conferenceRoomResult = _mapper.Map<ConferenceRooms>(conferenceRoomUpdate);

            //Info: "Об'єднує конференц-зал з вибраними додатковими послугами"
            if (await _conferenceRoomRepository.MergeConferenceRoomAndAdditionalServies(conferenceRoomResult, additionalServiceIds) == null)
                return BadRequest();

            //Info: "Оновлює конференц-зал у базі даних"
            await _conferenceRoomRepository.UpdateAsync(conferenceRoomResult);

            //Info: "Перевіряє, чи збережено зміни"
            if (!await _conferenceRoomRepository.Save())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        //Info: "Метод для видалення конференц-залу за Id"
        [HttpDelete("{conferenceRoomId}")]
        [ProducesResponseType(204)]  // Успішне видалення
        [ProducesResponseType(500)]  // Помилка сервера
        public async Task<IActionResult> DeleteConferenceRoom(int conferenceRoomId)
        {
            //Info: "Перевіряє, чи валідний ModelState"
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Info: "Шукає конференц-зал за Id перед видаленням"
            if (await _conferenceRoomRepository.GetByIdAsync(conferenceRoomId) == null)
                return NotFound();

            //Info: "Видаляє конференц-зал"
            await _conferenceRoomRepository.DeleteAsync(conferenceRoomId);

            //Info: "Перевіряє, чи збережено зміни після видалення"
            if (!await _conferenceRoomRepository.Save())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);
            }

            return NoContent();  // Повертає статус 204 (No Content) при успішному видаленні
        }
    }
}
