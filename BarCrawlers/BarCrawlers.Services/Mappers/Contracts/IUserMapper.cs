using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Services.Mappers.Contracts
{
    public interface IUserMapper
    {
        public User MapDTOToEntity(UserDTO dto);
        public UserDTO MapEntityToDTO(User entity);
    }
}
