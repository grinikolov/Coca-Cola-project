using BarCrawlers.Services.DTOs;

namespace BarCrawlers.Models.Contracts
{
    public interface IBarViewMapper
    {
        public BarViewModel MapDTOToView(BarDTO dto);

        public BarDTO MapViewToDTO(BarViewModel view);
    }
}
