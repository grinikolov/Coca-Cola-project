using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCrawlers.Services.Contracts
{
    public interface IUsersService
    {
        //Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<UserDTO>> GetAllAsync(string page, string itemsOnPage, string search);
        Task<UserDTO> GetAsync(Guid id);
        Task<UserDTO> UpdateAsync(Guid id, UserDTO userDTO, UserManager<User> userManager);

        Task<UserDTO> UnbanAsync(Guid id, UserManager<User> userManager);
    }
}
