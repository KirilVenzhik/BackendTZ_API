//Info: "Клас профілів AutoMapper для картографування між сутностями та їх DTO"
using API.Dto;
using API.Entityes;
using AutoMapper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        //Info: "Картографування між ConferenceRooms та ConferenceRoomDto"
        CreateMap<ConferenceRooms, ConferenceRoomDto>().ReverseMap();

        //Info: "Картографування між AdditionalServices та AdditionalServicesDto"
        CreateMap<AdditionalServices, AdditionalServicesDto>().ReverseMap();

        //Info: "Картографування між RoomReservations та RoomReservationsDto, обробка SelectedServices"
        CreateMap<RoomReservations, RoomReservationsDto>()
            .ForMember(dest => dest.SelectedServices, opt => opt.MapFrom(src => src.SelectedServices.ToList()))
            .ReverseMap()
            .ForMember(dest => dest.SelectedServices, opt => opt.MapFrom(src => src.SelectedServices.ToList()));

        //Info: "Картографування між ConferenceRoomAdditionalService та ConferenceRoomAdditionalServiceDto"
        CreateMap<ConferenceRoomAdditionalService, ConferenceRoomAdditionalServiceDto>().ReverseMap();
    }
}
