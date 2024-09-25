using API.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Info: "Додає контролери для обробки запитів"
builder.Services.AddControllers();

//Info: "Реєструє клас Seed для заповнення бази даних"
builder.Services.AddTransient<Seed>();

//Info: "Реєструє репозиторії для додаткових послуг, конференц-залів і бронювань"
builder.Services.AddScoped<IAdditionalServicesRepository, AdditionalServicesRepository>();
builder.Services.AddScoped<IConferenceRoomRepository, ConferenceRoomRepository>();
builder.Services.AddScoped<IRoomReservationRepository, RoomReservationRepository>();

//Info: "Додає AutoMapper для автоматичного мапування об'єктів"
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Info: "Додає інструменти для документування API через Swagger"
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Info: "Налаштування підключення до бази даних через SQL Server"
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

//Info: "Перевіряє аргументи командного рядка, чи необхідно ініціалізувати дані (seed)"
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

//Info: "Метод для запуску Seed для заповнення бази даних"
void SeedData(IHost app)
{
    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<Seed>();
    service.SeedDataContext();
}

//Info: "Налаштування для розробницького середовища"
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

//Info: "Додає підтримку для HTTPS"
app.UseHttpsRedirection();

//Info: "Увімкнення авторизації"
app.UseAuthorization();

//Info: "Маршрутизація запитів до контролерів"
app.MapControllers();

//Info: "Запуск програми"
app.Run();
