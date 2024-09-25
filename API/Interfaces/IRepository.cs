//Info: "Загальний інтерфейс репозиторію для основних операцій з даними"
namespace API.Interfaces
{
    public interface IRepository<T> where T : class
    {
        //Info: "Отримати всі сутності"
        Task<IEnumerable<T>> GetAllAsync();

        //Info: "Отримати сутність за ідентифікатором"
        Task<T> GetByIdAsync(int id);

        //Info: "Додати нову сутність"
        Task AddAsync(T entity);

        //Info: "Оновити сутність"
        Task UpdateAsync(T entity);

        //Info: "Видалити сутність за ідентифікатором"
        Task DeleteAsync(int id);

        //Info: "Зберегти зміни"
        Task<bool> Save();
    }
}
