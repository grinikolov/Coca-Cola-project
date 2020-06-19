using BarCrawlers.Data;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Services
{
    public class BarUserCommentsService : IBarUserCommentsService
    {
        private readonly BCcontext _context;
        private readonly IBarUserCommentMapper _mapper;
        //private readonly IHttpClientFactory _clientFactory;

        public BarUserCommentsService(BCcontext context, IBarUserCommentMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            //this._clientFactory = httpClient ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<BarUserCommentDTO> CreateAsync(BarUserCommentDTO commentDTO)
        {
            try
            {
                if (await CombinationExistAsync(commentDTO.BarId, commentDTO.UserId))
                {
                    return new BarUserCommentDTO();
                }
                else
                {
                    var comment = _mapper.MapDTOToEntity(commentDTO);

                    _context.BarComments.Add(comment);

                    await _context.SaveChangesAsync();

                    return commentDTO;
                }
            }
            catch (Exception)
            {

                return new BarUserCommentDTO();
            }
        }

        private async Task<bool> CombinationExistAsync(Guid barId, Guid userId)
        {
            return await _context.BarComments.AnyAsync(c => c.BarId == barId && c.UserId == userId);
        }

        public async Task<bool> DeleteAsync(Guid barId, Guid userId)
        {
            try
            {
                var comment = await _context.BarComments.FirstOrDefaultAsync(c => c.BarId == barId && c.UserId == userId);

                _context.BarComments.Remove(comment);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<BarUserCommentDTO>> GetAllAsync(Guid barId, string page, string itemsOnPage)
        {
            try
            {
                var p = int.Parse(page);
                var item = int.Parse(itemsOnPage);
                var comments = await _context.BarComments
                                            .Include(c => c.Bar)
                                            .Include(c => c.User)
                                            .Where(c => c.BarId == barId)
                                            .Skip(p * item)
                                            .Take(item)
                                            .ToListAsync();

                var commentsDTO = comments.Select(c => _mapper.MapEntityToDTO(c));

                return commentsDTO;
            }
            catch (Exception)
            {
                return new List<BarUserCommentDTO>();
            }

        }

        public async Task<BarUserCommentDTO> GetAsync(Guid barId, Guid userId)
        {
            try
            {
                var comment = await _context.BarComments
                            .Include(c => c.Bar)
                            .Include(c => c.User)
                            .FirstOrDefaultAsync(c => c.BarId == barId && c.UserId == userId);

                var commentDTO = _mapper.MapEntityToDTO(comment);

                return commentDTO;

            }
            catch (Exception)
            {
                return new BarUserCommentDTO();
            }


        }

        public async Task<BarUserCommentDTO> UpdateAsync(BarUserCommentDTO commentDTO)
        {
            try
            {
                var comment = await _context.BarComments
                                            .Include(c => c.Bar)
                                            .Include(c => c.User)
                                            .FirstOrDefaultAsync(c => c.BarId == commentDTO.BarId && c.UserId == commentDTO.UserId);

                comment.Text = commentDTO.Text;
                comment.IsFlagged = commentDTO.IsFlagged;

                _context.Update(comment);

                await _context.SaveChangesAsync();

                return commentDTO;
            }
            catch (Exception)
            {
                return new BarUserCommentDTO();
            }
        }
    }
}
