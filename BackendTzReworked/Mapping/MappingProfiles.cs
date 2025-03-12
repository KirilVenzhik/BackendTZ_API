using AutoMapper;
using BackendTzReworked.Bll.Common.ConferenceRooms.Models;
using BackendTzReworked.Bll.Common.RoomReservation.Models;
using BackendTzReworked.Bll.Common.RoomsAndSupplementsMtoM.Models;
using BackendTzReworked.Bll.Common.RoomSupplements.Models;
using BackendTzReworked.DTOs.ConferenceRooms;
using BackendTzReworked.DTOs.RoomReservations;
using BackendTzReworked.DTOs.RoomsAndSupplementsMtoM;
using BackendTzReworked.DTOs.RoomSupplements;

namespace BackendTzReworked.DAL.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<MSupplements, SupplementsDTO>().ReverseMap();

            CreateMap<MRooms, RoomsDTO>().ReverseMap();

            CreateMap<MReservations, ReservationsDTO>()
                .ForMember<List<int>>(dest => dest.SelectedSupplements, opt => opt.MapFrom(src => src.SelectedSupplements.ToList()))
                .ReverseMap()
                .ForMember<List<int>>(dest => dest.SelectedSupplements, opt => opt.MapFrom(src => src.SelectedSupplements.ToList()));

            CreateMap<MRoomsAndSupplements, RoomsAndSupplementsDTO>().ReverseMap();
        }
    }
}