//Info: "Інтерфейс репозиторію для додаткових послуг, що успадковує основні методи від IRepository"
using API.Entityes;

namespace API.Interfaces
{
    public interface IAdditionalServicesRepository : IRepository<AdditionalServices>
    {
        //Info: "Асинхронний метод для отримання послуги за назвою"
        Task<AdditionalServices> GetByNameAsync(string name);

        //Info: "Асинхронний метод для отримання послуг за вартістю"
        Task<IEnumerable<AdditionalServices>> GetByCostAsync(double cost);
    }
}
