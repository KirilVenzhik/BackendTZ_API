using Microsoft.EntityFrameworkCore;
using BackendTzReworked.DAL.EntityFeamework;
using BackendTzReworked.Bll.Common.ConferenceRooms.Repositories;
using BackendTzReworked.Bll.Common.RoomReservation.Repositories;
using BackendTzReworked.Bll.Common.RoomSupplements.Repositories;
using BackendTzReworked.DAL.ConferenceRooms.Repositories;
using BackendTzReworked.DAL.RoomReservations.Repositories;
using BackendTzReworked.DAL.RoomSupplements.Repositories;
using BackendTzReworked.Bll.Common.ConferenceRooms.Managers;
using BackendTzReworked.Bll.ConferenceRooms.Managers;



var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();



builder.Services.AddTransient<Seed>();



builder.Services.AddScoped<ISupplementsRepository, SupplementsRepository>();
builder.Services.AddScoped<IRoomsRepository, RoomsRepository>();
builder.Services.AddScoped<IReservationsRepository, ReservationsRepository>();
builder.Services.AddScoped<IRoomsManager, RoomsManager>();



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders(); // Очистка стандартных провайдеров
builder.Logging.AddConsole();     // Добавление провайдера для консоли
builder.Logging.AddDebug();

builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



var app = builder.Build();



if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);
void SeedData(IHost app)
{
    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<Seed>();
    service.SeedDataContext();
}



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
