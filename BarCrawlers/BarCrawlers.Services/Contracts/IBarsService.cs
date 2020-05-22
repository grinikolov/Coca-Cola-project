using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCrawlers.Services.Contracts
{
    public interface IBarsService
    {
        Task<IEnumerable<BarDTO>> GetAllAsync(string page, string itemsOnPage, string search);
        Task<BarDTO> GetAsync(Guid id);
        Task<BarDTO> CreateAsync(BarDTO cocktailDTO);
        Task<BarDTO> UpdateAsync(Guid id, BarDTO cocktailDTO);
        Task<bool> DeleteAsync(Guid id);
    }
}
