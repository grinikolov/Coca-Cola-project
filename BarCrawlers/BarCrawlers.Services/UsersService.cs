using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using BarCrawlers.Services.Contracts;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers.Contracts;
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

        public UsersService(BCcontext context,
            IUserMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync(string page, string itemsOnPage)
        {
            try
            {
                var p = int.Parse(page);
                var item = int.Parse(itemsOnPage);
                var users = await _context.Users
                .Include(u => u.BarRatings)
                .Include(u => u.CocktailRatings)
                .Include(u => u.BarComments)
                .Include(u => u.CocktailComments)
                .Skip(p*item)
                .Take(item)
                .ToListAsync();

                var usersDTO = users.Select(u => _mapper.MapEntityToDTO(u));

                return usersDTO;
            }
            catch (Exception)
            {
                return new List<UserDTO>();
            }

                            
        }

        public Task<UserDTO> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> UpdateAsync(Guid id, UserDTO userDTO)
        {
            throw new NotImplementedException();
        }
    }
}
