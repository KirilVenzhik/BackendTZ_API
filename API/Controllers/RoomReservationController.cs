using API.Dto;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomReservationController : ControllerBase
    {
        //Info: "Приватні поля для репозиторію бронювань залів та AutoMapper"
        private readonly IRoomReservationRepository _roomReservationRepository;
        private readonly IMapper _mapper;

        //Info: "Конструктор, що ініціалізує залежності"
        public RoomReservationController(IRoomReservationRepository roomReservationRepository, IMapper mapper)
        {
            _roomReservationRepository = roomReservationRepository;
            _mapper = mapper;
        }

        //Info: "Метод для отримання всіх бронювань залів"
        [HttpGet("GetAllReservations")]
        [ProducesResponseType(200)]  // Успішний результат
        public async Task<IActionResult> GetAllReservations()
        {
            //Info: "Отримує всі бронювання через репозиторій"
            var reservations = await _roomReservationRepository.GetAllAsync();

            //Info: "Перевіряє, чи є бронювання в базі і чи валідний ModelState"
            if (reservations == null || !ModelState.IsValid)
                return NotFound();

            //Info: "Мапує отримані бронювання до DTO за допомогою AutoMapper"
            var conferenceRoomMap = _mapper.Map<IEnumerable<RoomReservationsDto>>(reservations);

            //Info: "Повертає результат у форматі Ok (статус 200)"
            return Ok(conferenceRoomMap);
        }

        //Info: "Метод для отримання конкретного бронювання за Id"
        [HttpGet("{reservationId}")]
        [ProducesResponseType(200)]  // Успішний результат
        [ProducesResponseType(400)]  // Некоректний запит
        [ProducesResponseType(404)]  // Не знайдено
        public async Task<IActionResult> GetReservationById(int reservationId)
        {
            //Info: "Перевіряє, чи валідний ModelState"
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Info: "Шукає бронювання за Id через репозиторій"
            var reservation = await _roomReservationRepository.GetByIdAsync(reservationId);

            //Info: "Перевіряє, чи знайдено бронювання"
            if (reservation == null)
                return NotFound();

            //Info: "Мапує отримане бронювання до DTO і повертає результат"
            var reservationMap = _mapper.Map<RoomReservationsDto>(reservation);

            return Ok(reservationMap);
        }

        //Info: "Метод для отримання вартості бронювання за Id"
        [HttpGet("GetReservationCost/{reservationId}")]
        [ProducesResponseType(200)]  // Успішний результат
        [ProducesResponseType(400)]  // Некоректний запит
        [ProducesResponseType(404)]  // Не знайдено
        public async Task<IActionResult> GetReservationCostById(int reservationId)
        {
            //Info: "Перевіряє, чи валідний ModelState"
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Info: "Шукає бронювання за Id через репозиторій"
            var reservation = await _roomReservationRepository.GetByIdAsync(reservationId);

            //Info: "Перевіряє, чи знайдено бронювання"
            if (reservation == null)
                return NotFound();

            //Info: "Розраховує вартість бронювання через репозиторій"
            var reservationCost = await _roomReservationRepository.CalculateReservationCost(
                reservation.ReservedRoom.Id, reservation.TimeReservation, reservation.EndTimeReservation, reservation.SelectedServices);

            return Ok(reservationCost);
        }

        //Info: "Метод для отримання доступних конференц-залів за вказаними параметрами (час і місткість)"
        [HttpGet("GetAvilableConferenceRooms")]
        [ProducesResponseType(200)]  // Успішний результат
        [ProducesResponseType(404)]  // Не знайдено
        public async Task<IActionResult> GetAvilableConferenceRooms(
            [FromQuery] DateTime startTime, [FromQuery] DateTime endTime, [FromQuery] int capacity)
        {
            //Info: "Перевіряє валідність введених даних"
            if (!ModelState.IsValid || startTime > endTime || capacity <= 0)
                return BadRequest(ModelState);

            //Info: "Отримує список доступних конференц-залів через репозиторій"
            var rooms = await _roomReservationRepository.GetAvailableRooms(startTime, endTime, capacity);

            //Info: "Перевіряє, чи знайдено доступні зали"
            if (!rooms.Any())
                return NotFound();

            return Ok(rooms);
        }

        //Info: "Метод для створення нового бронювання"
        [HttpPost]
        [ProducesResponseType(200)]  // Успішне створення
        [ProducesResponseType(400)]  // Некоректний запит
        [ProducesResponseType(500)]  // Помилка сервера
        public async Task<IActionResult> CreateReservation([FromBody] RoomReservationsDto reservationCreate,
            [FromQuery] int conferenceRoomId)
        {
            //Info: "Перевіряє валідність даних"
            if (reservationCreate == null || !ModelState.IsValid)
                return BadRequest();

            //Info: "Мапує DTO на сутність бронювання"
            var reservationMap = _mapper.Map<RoomReservations>(reservationCreate);

            //Info: "Об'єднує бронювання з конференц-залом через репозиторій"
            var resultMerge = await _roomReservationRepository.MergeReservationAndConferenceRoom(reservationMap, conferenceRoomId);

            //Info: "Перевіряє, чи успішно об'єднано бронювання з залом"
            if (!resultMerge.ResultBool)
                return BadRequest(ModelState);

            reservationMap = resultMerge.ResultReservation;

            //Info: "Додає нове бронювання до бази даних"
            await _roomReservationRepository.AddAsync(reservationMap);

            //Info: "Перевіряє, чи збережено зміни"
            if (!await _roomReservationRepository.Save())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);  // Повертає статус 500 при помилці
            }

            //Info: "Повертає вартість бронювання"
            return Ok(await _roomReservationRepository.CalculateReservationCost(
                reservationMap.ReservedRoom.Id, reservationMap.TimeReservation, reservationMap.EndTimeReservation, reservationMap.SelectedServices));
        }

        //Info: "Метод для видалення бронювання за Id"
        [HttpDelete("{reservationId}")]
        [ProducesResponseType(204)]  // Успішне видалення
        [ProducesResponseType(404)]  // Не знайдено
        [ProducesResponseType(500)]  // Помилка сервера
        public async Task<IActionResult> DeleteReservation(int reservationId)
        {
            //Info: "Перевіряє валідність даних"
            if (!ModelState.IsValid)
                return BadRequest();

            //Info: "Шукає бронювання за Id перед видаленням"
            if (await _roomReservationRepository.GetByIdAsync(reservationId) == null)
                return NotFound();

            //Info: "Видаляє бронювання"
            await _roomReservationRepository.DeleteAsync(reservationId);

            //Info: "Перевіряє, чи збережено зміни після видалення"
            if (!await _roomReservationRepository.Save())
            {
                ModelState.AddModelError("", "Сталася помилка під час збереження!");
                return StatusCode(500, ModelState);  // Повертає статус 500 при помилці
            }

            return NoContent();  // Повертає статус 204 (No Content) при успішному видаленні
        }
    }
}
