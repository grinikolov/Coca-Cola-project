using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Areas.Magician.Models.Contrtacts
{
    public interface IUserViewMapper
    {
        public UserViewModel MapDTOToView(UserDTO dto);

        public UserDTO MapViewToDTO(UserViewModel view);
    }
}
