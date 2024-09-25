//Info: "DTO клас для додаткових послуг з назвою та вартістю"
namespace API.Dto
{
    public class AdditionalServicesDto : BaseEntityDto
    {
        public string Name { get; set; }
        public double Cost { get; set; }
    }
}
