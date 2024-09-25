//Info: "Базова реалізація загального репозиторію, що працює з сутностями типу T"
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

public class Repository<T> : IRepository<T> where T : class
{
    //Info: "Поле контексту бази даних, через який виконуються операції"
    protected readonly Context _context;

    //Info: "Конструктор, що приймає контекст і ініціалізує його для використання в методах"
    public Repository(Context context)
    {
        _context = context;
    }

    //Info: "Отримує всі записи сутності типу T з бази даних"
    //Info: "Звертається до відповідної таблиці бази даних через Set<T> і виконує запит ToListAsync для отримання всіх записів"
    public async Task<IEnumerable<T>> GetAllAsync()
        => await _context.Set<T>().ToListAsync();

    //Info: "Шукає запис за ідентифікатором (Id)"
    //Info: "Використовує метод FindAsync для пошуку запису в таблиці"
    public async Task<T> GetByIdAsync(int id)
        => await _context.Set<T>().FindAsync(id);

    //Info: "Додає новий запис сутності типу T до таблиці"
    //Info: "Використовує метод AddAsync для асинхронного додавання нової сутності до таблиці"
    public async Task AddAsync(T entity)
        => await _context.Set<T>().AddAsync(entity);

    //Info: "Оновлює існуючий запис в таблиці"
    //Info: "Використовує метод Update для позначення зміненої сутності як зміненої"
    public async Task UpdateAsync(T entity)
        => _context.Set<T>().Update(entity);

    //Info: "Видаляє запис з таблиці за ідентифікатором (Id)"
    //Info: "Шукає запис за Id, якщо такий існує — видаляє його з таблиці"
    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
            _context.Set<T>().Remove(entity);
    }

    //Info: "Зберігає зміни в базі даних після операцій додавання, оновлення або видалення"
    //Info: "Використовує SaveChangesAsync для асинхронного збереження змін і повертає true, якщо зміни успішно застосовані"
    public async Task<bool> Save()
        => await _context.SaveChangesAsync() > 0;
}
