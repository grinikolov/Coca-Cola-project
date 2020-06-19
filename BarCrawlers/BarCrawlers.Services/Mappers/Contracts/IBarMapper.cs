using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.DTOs;

namespace BarCrawlers.Services.Mappers.Contracts
{
    public interface IBarMapper
    {
        Bar MapDTOToEntity(BarDTO dto);
        BarDTO MapEntityToDTO(Bar entity);
    }
}
