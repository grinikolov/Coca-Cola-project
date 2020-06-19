using BarCrawlers.Services.DTOs;

namespace BarCrawlers.Areas.Magician.Models.Contrtacts
{
    public interface IUserViewMapper
    {
        public UserViewModel MapDTOToView(UserDTO dto);

        public UserDTO MapViewToDTO(UserViewModel view);
    }
}
