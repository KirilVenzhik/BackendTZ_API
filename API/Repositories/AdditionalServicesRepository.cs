//Info: "Реалізація репозиторію для додаткових послуг"
using API.Entityes;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

public class AdditionalServicesRepository : Repository<AdditionalServices>, IAdditionalServicesRepository
{
    //Info: "Конструктор ініціалізує базовий клас з контекстом бази даних"
    public AdditionalServicesRepository(Context context) : base(context) { }

    //Info: "Асинхронний метод для отримання додаткової послуги за назвою"
    //Info: "Шукає перший запис у таблиці AdditionalService, де Name дорівнює переданій назві"
    public async Task<AdditionalServices> GetByNameAsync(string name)
        => await _context.AdditionalService.FirstOrDefaultAsync(a => a.Name == name);

    //Info: "Асинхронний метод для отримання списку послуг, вартість яких дорівнює заданій"
    //Info: "Фільтрує таблицю AdditionalService за полем Cost і повертає результат як список"
    public async Task<IEnumerable<AdditionalServices>> GetByCostAsync(double cost)
        => await _context.AdditionalService.Where(a => a.Cost == cost).ToListAsync();
}
