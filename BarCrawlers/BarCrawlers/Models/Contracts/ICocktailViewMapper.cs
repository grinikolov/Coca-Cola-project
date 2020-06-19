using BarCrawlers.Services.DTOs;

namespace BarCrawlers.Models.Contracts
{
    public interface ICocktailViewMapper
    {
        public CocktailViewModel MapDTOToView(CocktailDTO dto);

        public CocktailDTO MapViewToDTO(CocktailViewModel view);
        public CocktailDTO MapViewToDTO(CocktailCreateViewModel view);
    }
}
