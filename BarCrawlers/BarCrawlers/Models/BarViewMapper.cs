using BarCrawlers.Models.Contracts;
using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Models
{
    public class BarViewMapper : IBarViewMapper
    {
        public BarViewModel MapDTOToView(BarDTO dto)
        {
            try
            {
                return new BarViewModel
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Rating = dto.Rating,
                    TimesRated = dto.TimesRated,
                    ImageSrc = dto.ImageSrc,
                    IsDeleted = dto.IsDeleted,
                    Address = dto.Address,
                    Country = dto.Country,
                    District = dto.District,
                    Email = dto.Email,
                    LocationId = dto.LocationId,
                    Phone = dto.Phone,
                    Town = dto.Town,

                };
            }
            catch (Exception)
            {
                return new BarViewModel();
            }
        }

        public BarDTO MapViewToDTO(BarViewModel view)
        {
            try
            {
                return new BarDTO
                {
                    Id = view.Id,
                    Name = view.Name,
                    Rating = view.Rating,
                    TimesRated = view.TimesRated,
                    ImageSrc = view.ImageSrc,
                    IsDeleted = view.IsDeleted,
                    Address = view.Address,
                    Country = view.Country,
                    District = view.District,
                    Email = view.Email,
                    LocationId = view.LocationId,
                    Phone = view.Phone,
                    Town = view.Town,
                    Cocktails = view.Cocktails.Select(c => new CocktailBarDTO()
                    {
                        BarId = c.BarId,
                        BarName = c.BarName,
                        CocktailId = c.CocktailId,
                        CocktailName = c.CocktailName
                    }).ToList()
                };
            }
            catch (Exception)
            {
                return new BarDTO();
            }
        }
    }
}
