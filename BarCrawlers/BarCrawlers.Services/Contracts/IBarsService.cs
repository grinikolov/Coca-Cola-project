﻿using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarCrawlers.Services.Contracts
{
    public interface IBarsService
    {
        Task<IEnumerable<BarDTO>> GetAllAsync(string page, string itemsOnPage, string search, string order, bool access);
        Task<BarDTO> GetAsync(Guid id);
        Task<BarDTO> CreateAsync(BarDTO barDTO);
        Task<BarDTO> UpdateAsync(Guid id, BarDTO barDTO);
        Task<bool> DeleteAsync(Guid id);
        Task<BarDTO> RateBarAsync(Guid barId, Guid userId, int rating);
        Task<BarDTO> SetLocation(BarDTO dto);
        Task<IEnumerable<CocktailDTO>> GetCocktailsAsync(Guid id, string page, string itemsOnPage, string search, bool access);
        Task<IEnumerable<CocktailDTO>> GetCocktailsAsync(Guid id);
        Task<IEnumerable<BarDTO>> GetBestBarsAsync();
    }
}
