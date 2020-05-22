using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCrawlers.Services
{
    public class UsersService : IUsersService
    {
        private readonly BCcontext _context;
        private readonly IUserMapper _mapper;
        private readonly ICocktailsService _cocktailsService;
        private readonly ICocktailCommentMapper _cocktailCommentMapper;

        public UsersService(BCcontext context
            ,IUserMapper mapper
            ,ICocktailCommentMapper cocktailCommentMapper)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._cocktailCommentMapper= cocktailCommentMapper ?? throw new ArgumentNullException(nameof(cocktailCommentMapper));
        }

        /// <summary>
        /// Gets all users from the database.
        /// </summary>
        /// <returns>List of users, DTOs</returns>
        public async Task<IEnumerable<UserDTO>> GetAllAsync(string page, string itemsOnPage, string search)
        {
            try
            {
                var p = int.Parse(page);
                var item = int.Parse(itemsOnPage);
                var users =  _context.Users
                                    .Include(u => u.BarRatings)
                                    .Include(u => u.CocktailRatings)
                                    .Include(u => u.BarComments)
                                    .Include(u => u.CocktailComments).AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    users = users.Where(u => u.UserName.Contains(search));
                }

                users = users.Skip(p * item)
                                .Take(item);

                var result = await users.ToListAsync();

                var usersDTO = result.Select(u => _mapper.MapEntityToDTO(u));

                return usersDTO;
            }
            catch (Exception)
            {
                return new List<UserDTO>();
            }

                            
        }

        /// <summary>
        /// Gets the user by ID
        /// </summary>
        /// <param name="id">User ID, Guid</param>
        /// <returns>The user, DTO</returns>
        public async Task<UserDTO> GetAsync(Guid id)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.BarRatings)
                    .Include(u => u.CocktailRatings)
                    .Include(u => u.BarComments)
                    .Include(u => u.CocktailComments)
                    .FirstOrDefaultAsync(u => u.Id == id);

                var userDTO =  _mapper.MapEntityToDTO(user);

                return userDTO;
            }
            catch (Exception)
            {
                return new UserDTO();
            }
        }

        public async Task<UserDTO> UpdateAsync(Guid id, UserDTO userDTO, UserManager<User> userManager)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.BarRatings)
                    .Include(u => u.CocktailRatings)
                    .Include(u => u.BarComments)
                    .Include(u => u.CocktailComments)
                    .FirstOrDefaultAsync(u => u.Id == id);

                await userManager.SetLockoutEnabledAsync(user,true);
                await userManager.SetLockoutEndDateAsync(user, userDTO.LockoutEnd);
                var status = userManager.GetLockoutEndDateAsync(user);
                await _context.SaveChangesAsync();

                return userDTO;
            }
            catch (Exception)
            {
                return new UserDTO();
            }
        }

        public async Task<UserDTO> UnbanAsync(Guid id, UserManager<User> userManager)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.BarRatings)
                    .Include(u => u.CocktailRatings)
                    .Include(u => u.BarComments)
                    .Include(u => u.CocktailComments)
                    .FirstOrDefaultAsync(u => u.Id == id);

                await userManager.SetLockoutEndDateAsync(user, null);
                var status = userManager.GetLockoutEndDateAsync(user);
                await _context.SaveChangesAsync();

                return _mapper.MapEntityToDTO(user);
            }
            catch (Exception)
            {
                return new UserDTO();
            }
        }

        public async Task<CocktailUserCommentDTO> AddCocktailReview(CocktailUserCommentDTO commentDTO, Guid cocktailId, Guid userId)
        {
            var cocktail = await this._context.Cocktails.FirstOrDefaultAsync(x => x.Id == cocktailId);

            var comment = this._cocktailCommentMapper.MapDTOToEntity(commentDTO);
            
            await this._context.CocktailComments.AddAsync(comment);
            await this._context.SaveChangesAsync();

            return this._cocktailCommentMapper.MapEntityToDTO(comment);
        }
    }
}
