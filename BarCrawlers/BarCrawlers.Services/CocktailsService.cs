using BarCrawlers.Data;
using BarCrawlers.Services.DTOs;
using BarCrawlers.Services.Mappers;
using BarCrawlers.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Services
{
    public class CocktailsService
    {
        private readonly BCcontext _context;
        private readonly ICocktailMapper _mapper;

        public CocktailsService(BCcontext context,
            ICocktailMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

    }
}
