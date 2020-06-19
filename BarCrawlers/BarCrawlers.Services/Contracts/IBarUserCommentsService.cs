using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarCrawlers.Services.Contracts
{
    public interface IBarUserCommentsService
    {
        Task<IEnumerable<BarUserCommentDTO>> GetAllAsync(Guid barId, string page, string itemsOnPage);
        Task<BarUserCommentDTO> GetAsync(Guid barId, Guid userId);
        Task<BarUserCommentDTO> CreateAsync(BarUserCommentDTO commentDTO);
        Task<BarUserCommentDTO> UpdateAsync(BarUserCommentDTO commentDTO);
        Task<bool> DeleteAsync(Guid barId, Guid userId);
    }
}
