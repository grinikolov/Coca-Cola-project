using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BarCrawlers.Services
{
    public class CocktailCommentsService : ICocktailCommentsService
    {
        private readonly BCcontext _context;
        private readonly ICocktailCommentMapper _mapper;
        //private readonly IHttpClientFactory _clientFactory;

        public CocktailCommentsService(BCcontext context, ICocktailCommentMapper mapper)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CocktailUserCommentDTO> CreateAsync(CocktailUserCommentDTO commentDTO)
        {
            try
            {
                if (await CombinationExistAsync(commentDTO.CocktailId, commentDTO.UserId))
                {
                    return new CocktailUserCommentDTO();
                }
                else
                {
                    var comment = this._mapper.MapDTOToEntity(commentDTO);

                    this._context.CocktailComments.Add(comment);

                    await _context.SaveChangesAsync();

                    return commentDTO;
                }
            }
            catch (Exception)
            {

                return new CocktailUserCommentDTO();
            }
        }

        private async Task<bool> CombinationExistAsync(Guid cocktailId, Guid userId)
        {
            return await _context.CocktailComments.AnyAsync(c => c.CocktailId == cocktailId && c.UserId == userId);
        }

        public async Task<bool> DeleteAsync(Guid cocktailId, Guid userId)
        {
            try
            {
                var comment = await this._context.CocktailComments
                    .FirstOrDefaultAsync(c => c.CocktailId == cocktailId && c.UserId == userId);

                this._context.CocktailComments.Remove(comment);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<CocktailUserCommentDTO>> GetAllAsync(Guid cocktailId, string page, string itemsOnPage)
        {
            try
            {
                var p = int.Parse(page);
                var item = int.Parse(itemsOnPage);
                var comments = await this._context.CocktailComments
                                            .Include(c => c.Cocktail)
                                            .Include(c => c.User)
                                            .Where(c => c.CocktailId == cocktailId)
                                            .Skip(p * item)
                                            .Take(item)
                                            .ToListAsync();

                var commentsDTO = comments.Select(c => _mapper.MapEntityToDTO(c));

                return commentsDTO;
            }
            catch (Exception)
            {
                return new List<CocktailUserCommentDTO>();
            }

        }

        public async Task<CocktailUserCommentDTO> GetAsync(Guid cocktailId, Guid userId)
        {
            try
            {
                var comment = await this._context.CocktailComments
                            .Include(c => c.Cocktail)
                            .Include(c => c.User)
                            .FirstOrDefaultAsync(c => c.CocktailId == cocktailId && c.UserId == userId);

                var commentDTO = _mapper.MapEntityToDTO(comment);

                return commentDTO;

            }
            catch (Exception)
            {
                return new CocktailUserCommentDTO();
            }


        }

        public async Task<CocktailUserCommentDTO> UpdateAsync(CocktailUserCommentDTO commentDTO)
        {
            try
            {
                var comment = await this._context.CocktailComments
                                            .Include(c => c.Cocktail)
                                            .Include(c => c.User)
                                            .FirstOrDefaultAsync(c => c.CocktailId == commentDTO.CocktailId && c.UserId == commentDTO.UserId);

                comment.Text = commentDTO.Text;
                comment.IsFlagged = commentDTO.IsFlagged;

                _context.Update(comment);

                await _context.SaveChangesAsync();

                return commentDTO;
            }
            catch (Exception)
            {
                return new CocktailUserCommentDTO();
            }
        }
    }
}
