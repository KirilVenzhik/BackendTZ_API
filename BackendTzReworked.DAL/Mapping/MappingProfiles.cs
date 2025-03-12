using AutoMapper;
using BackendTzReworked.Bll.Common.ConferenceRooms.Models;
using BackendTzReworked.Bll.Common.RoomReservation.Models;
using BackendTzReworked.Bll.Common.RoomsAndSupplementsMtoM.Models;
using BackendTzReworked.Bll.Common.RoomSupplements.Models;
using BackendTzReworked.DAL.ConferenceRooms.Entityes;
using BackendTzReworked.DAL.RoomReservations.Entityes;
using BackendTzReworked.DAL.RoomsAndSupplementsMtoM.Entityes;
using BackendTzReworked.DAL.RoomSupplements.Entityes;

namespace BackendTzReworked.DAL.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<MSupplements, Supplements>().ReverseMap();

            CreateMap<MRooms, Rooms>().ReverseMap();

            CreateMap<MReservations, Reservations>()
                .ForMember<List<int>>(dest => dest.SelectedSupplements, opt => opt.MapFrom(src => src.SelectedSupplements.ToList()))
                .ReverseMap()
                .ForMember<List<int>>(dest => dest.SelectedSupplements, opt => opt.MapFrom(src => src.SelectedSupplements.ToList()));

            CreateMap<MRoomsAndSupplements, RoomsAndSupplements>().ReverseMap();
        }
    }
}