﻿using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Services
{
    public class UserInteractionsService : IUserInteractionsService
    {

        private readonly BCcontext _context;
        private readonly IUserMapper _userMapper;
        private readonly ICocktailCommentMapper _cocktailCommentMapper;
        private readonly IUsersService _usersService;
        private readonly ICocktailsService _cocktailsService;
        private readonly ICocktailMapper _cocktailMapper;
        //private readonly IBarCommentMapper _barCommentMapper;
        //private readonly IBarMapper _barMapper;

        public UserInteractionsService(BCcontext context
            , IUserMapper userMapper
            , ICocktailCommentMapper cocktailCommentMapper
            , IUsersService usersService
            , ICocktailsService cocktailsService
            , ICocktailMapper cocktailMapper
            )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
            _cocktailCommentMapper = cocktailCommentMapper ?? throw new ArgumentNullException(nameof(cocktailCommentMapper));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _cocktailsService = cocktailsService ?? throw new ArgumentNullException(nameof(cocktailsService));
            _cocktailMapper = cocktailMapper ?? throw new ArgumentNullException(nameof(cocktailMapper));
            //this._barCommentMapper = barCommentMapper ?? throw new ArgumentNullException(nameof(barCommentMapper));
            //this._barMapper = barMapper ?? throw new ArgumentNullException(nameof(barMapper));

            //throw new NotImplementedException(" Service for Commenting / Rating Cocktails/Bars");
        }

        /// <summary>
        /// User rates a cocktail
        /// </summary>
        /// <param name="userId">User Id, Guid</param>
        /// <param name="cocktailId">Cocktail Id, Guid</param>
        /// <param name="theRating">Rating, 1 to 5 </param>
        /// <returns>The cocktail rated, DTO</returns>
        public async Task<CocktailDTO> RateCocktail(int theRating, Guid cocktailId, Guid userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
            var cocktail = await _context.Cocktails
                .FirstOrDefaultAsync(c => c.Id == cocktailId);

            var existingRating = await _context.CocktailRatings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.CocktailId == cocktailId);

            //bool isRated = this._context.CocktailRatings
            //    .Select(r => r.UserId == userId && r.CocktailId == cocktailId)
            //    .Count() == 1;

            if (existingRating != null)
            {
                existingRating.Rating = theRating;
                _context.CocktailRatings.Update(existingRating);
            }
            else
            {
                var theNewRating = new UserCocktailRating
                {
                    CocktailId = cocktailId,
                    Cocktail = cocktail,
                    UserId = userId,
                    User = user,
                    Rating = theRating,
                };
                cocktail.TimesRated += 1;
                _context.CocktailRatings.Add(theNewRating);
            }

            try
            {
                //Saving before recalculating for it to be correct.
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }

            cocktail.Rating = await RecalculateCocktailRatingAsync(cocktail.Id);

            try
            {
                _context.Cocktails.Update(cocktail);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }
            return _cocktailMapper.MapEntityToDTO(cocktail);
        }

        /// <summary>
        /// Recalculates the cocktail rating
        /// </summary>
        /// <param name="cocktailId">Cocktail Id, Guid</param>
        /// <returns>The new cocktail rating</returns>
        private async Task<double> RecalculateCocktailRatingAsync(Guid cocktailId)
        {
            var recalculatedRating = await _context.CocktailRatings
                .Where(r => r.CocktailId == cocktailId)
                .Select(r => r.Rating).AverageAsync();

            recalculatedRating = Math.Round(recalculatedRating, 2);
            return recalculatedRating;
        }

        /// <summary>
        /// User comments a cocktail
        /// </summary>
        /// <param name="commentDTO">Comment DTO </param>
        /// <param name="cocktailId">Cocktail Id that is commented</param>
        /// <param name="userId">User Id that leaves the comment</param>
        /// <returns>The comment, DTO</returns>
        public async Task<CocktailUserCommentDTO> AddCocktailComment(CocktailUserCommentDTO commentDTO, Guid cocktailId, Guid userId)
        {
            var cocktail = await _context.Cocktails
                .FirstOrDefaultAsync(x => x.Id == cocktailId);

            commentDTO.UserId = userId;
            var comment = _cocktailCommentMapper.MapDTOToEntity(commentDTO);

            await _context.CocktailComments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return _cocktailCommentMapper.MapEntityToDTO(comment);
        }


        /*
        /// <summary>
        /// User rates a bar
        /// </summary>
        /// <param name="userId">User Id that leaves the comment</param>
        /// <param name="barId">Bar Id that is being rated</param>
        /// <param name="theRating">The Rating, from 1s to 5</param>
        /// <returns>The Bar rated</returns>
        public async Task<BarDTO> RateBar(Guid userId, Guid barId, int theRating)
        {
            var user = await this._context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
            var bar = await this._context.Bars
                .FirstOrDefaultAsync(c => c.Id == barId);

            var theNewRating = new UserBarRating
            {
                BarId = barId,
                Bar = bar,
                UserId = userId,
                User = user,
                Rating = theRating,
            };
            this._context.BarRatings.Add(theNewRating);
            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }

            bar.Rating = await RecalculateBarRatingAsync(bar.Id);

            try
            {
                this._context.Bars.Update(bar);
                await this._context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }
            return this._barMapper.MapEntityToDTO(bar);
        }


        private async Task<double> RecalculateBarRatingAsync(Guid barId)
        {
            var recalculatedRating = await this._context.BarRatings
                .Where(r => r.BarId == barId)
                .Select(r => r.Rating).AverageAsync();
            return recalculatedRating;
        }

        public async Task<BarUserCommentDTO> AddBarComment(BarUserCommentDTO commentDTO, Guid barId, Guid userId)
        {
            throw new NotImplementedException(nameof(AddBarComment));
            //var bar = await this._context.Bars
            //    .FirstOrDefaultAsync(x => x.Id == barId);

            //commentDTO.UserId = userId;
            //var comment = this._barCommentMapper.MapDTOToEntity(commentDTO);

            //await this._context.BarComments.AddAsync(comment);
            //await this._context.SaveChangesAsync();

            //return this._barCommentMapper.MapEntityToDTO(comment);
        }
        */
    }
}
