using BarCrawlers.Areas.Magician.Models.Contrtacts;
using BarCrawlers.Services.DTOs;
using System;

namespace BarCrawlers.Areas.Magician.Models
{
    public class UserViewMapper : IUserViewMapper
    {
        public UserViewModel MapDTOToView(UserDTO dto)
        {
            try
            {
                var view = new UserViewModel()
                {
                    Id = dto.Id,
                    ImageSrc = dto.ImageSrc,
                    UserName = dto.UserName,
                    Email = dto.UserName,
                    EmailConfirmed = dto.EmailConfirmed,
                    PhoneNumber = dto.PhoneNumber,
                    PhoneNumberConfirmed = dto.PhoneNumberConfirmed,
                    LockoutEnd = dto.LockoutEnd,
                    LockoutEnabled = dto.LockoutEnabled,
                };

                return view;
            }
            catch (Exception)
            {
                return new UserViewModel();
            }
        }

        public UserDTO MapViewToDTO(UserViewModel view)
        {
            throw new NotImplementedException();
        }
    }
}
