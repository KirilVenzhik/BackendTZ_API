# BackendTZ_API

## Опис
Проєкт надає в оренду конференц-зали для бізнесу. Розроблено просте 
API, яке дозволяє клієнтам шукати доступні зали, бронювати їх, а також 
розраховувати вартість оренди залежно від часу та обраних послуг.

## Вимоги
- .NET 6.0 або новіша версія
- SQL Server
- AutoMapper
- EntityFramework
- Swagger

## Встановлення

1. Клонуйте репозиторій:
    git clone https://github.com/KirilVenzhik/BackendTZ_API.git
   
2. Змінити у файлі appsettings.json: DefaultConnection на посилання вашої бази даних.
3. Ввести в Package Manager Console
   a. Add-Migration InitialCreate
   b. Update-Database
4. Ввести в Developer PowerShell
   a. dotnet run seeddata //Для базового заповнення таблиці.

## Запуск
Запускати проєкт через VisualStudio.

## Автор
Венжик Кирило Євгенович - https://github.com/KirilVenzhik
