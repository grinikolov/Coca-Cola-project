using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCrawlers.Services.Contracts
{
    public interface IUsersService
    {
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<UserDTO>> GetAllAsync(string page, string itemsOnPage);
        Task<UserDTO> GetAsync(Guid id);
        Task<UserDTO> UpdateAsync(Guid id, UserDTO userDTO);
    }
}
