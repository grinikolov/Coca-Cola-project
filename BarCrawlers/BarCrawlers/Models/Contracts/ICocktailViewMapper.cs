using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Models.Contracts
{
    public interface ICocktailViewMapper
    {
        public CocktailViewModel MapDTOToView(CocktailDTO dto);

        public CocktailDTO MapViewToDTO(CocktailViewModel view); 
        public CocktailDTO MapViewToDTO(CocktailCreateViewModel view);
    }
}
