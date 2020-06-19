using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using System;

namespace BarCrawlers.Services.Mappers
{
    public class UserMapper : IUserMapper
    {
        public User MapDTOToEntity(UserDTO dto)
        {
            throw new NotImplementedException();
        }

        public UserDTO MapEntityToDTO(User entity)
        {
            try
            {
                var dto = new UserDTO()
                {
                    Id = entity.Id,
                    ImageSrc = entity.ImageSrc,
                    UserName = entity.UserName,
                    Email = entity.UserName,
                    EmailConfirmed = entity.EmailConfirmed,
                    PhoneNumber = entity.PhoneNumber,
                    PhoneNumberConfirmed = entity.PhoneNumberConfirmed,
                    LockoutEnd = entity.LockoutEnd,
                    LockoutEnabled = entity.LockoutEnabled,
                };

                return dto;
            }
            catch (Exception)
            {
                return new UserDTO();
            }

        }
    }
}
