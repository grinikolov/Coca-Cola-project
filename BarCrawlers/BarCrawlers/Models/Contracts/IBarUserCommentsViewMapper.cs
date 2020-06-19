using BarCrawlers.Services.DTOs;

namespace BarCrawlers.Models.Contracts
{
    public interface IBarUserCommentViewMapper
    {
        public BarUserCommentView MapDTOToView(BarUserCommentDTO dto);

        public BarUserCommentDTO MapViewToDTO(BarUserCommentView view);
    }
}
