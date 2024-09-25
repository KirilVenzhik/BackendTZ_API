using API.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Info: "���� ���������� ��� ������� ������"
builder.Services.AddControllers();

//Info: "������ ���� Seed ��� ���������� ���� �����"
builder.Services.AddTransient<Seed>();

//Info: "������ ��������� ��� ���������� ������, ���������-���� � ���������"
builder.Services.AddScoped<IAdditionalServicesRepository, AdditionalServicesRepository>();
builder.Services.AddScoped<IConferenceRoomRepository, ConferenceRoomRepository>();
builder.Services.AddScoped<IRoomReservationRepository, RoomReservationRepository>();

//Info: "���� AutoMapper ��� ������������� ��������� ��'����"
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Info: "���� ����������� ��� �������������� API ����� Swagger"
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Info: "������������ ���������� �� ���� ����� ����� SQL Server"
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

//Info: "�������� ��������� ���������� �����, �� ��������� ������������ ��� (seed)"
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

//Info: "����� ��� ������� Seed ��� ���������� ���� �����"
void SeedData(IHost app)
{
    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<Seed>();
    service.SeedDataContext();
}

//Info: "������������ ��� �������������� ����������"
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

//Info: "���� �������� ��� HTTPS"
app.UseHttpsRedirection();

//Info: "��������� �����������"
app.UseAuthorization();

//Info: "������������� ������ �� ����������"
app.MapControllers();

//Info: "������ ��������"
app.Run();
