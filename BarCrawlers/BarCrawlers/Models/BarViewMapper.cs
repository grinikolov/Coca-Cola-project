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
                var vm = new BarViewModel
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

                if (dto.Location != null)
                {
                    vm.Location = new LocationView()
                    {
                        Lat = dto.Location.Lat,
                        Lon = dto.Location.Lon,
                        Src = "https://www.bing.com/maps/embed?h=280&w=325&cp=" + dto.Location.Lat + "~" + dto.Location.Lon + "&lvl=16&typ=s&sty=r&src=SHELL&FORM=MBEDV8",
                        //"https://www.bing.com/maps/embed?h=280&w=325&cp={LAT~LON}&lvl=16&typ=s&sty=r&src=SHELL&FORM=MBEDV8"
                        LargeMapLink = "https://www.bing.com/maps?cp=" + dto.Location.Lat + "~" + dto.Location.Lon + "&amp;sty=r&amp;lvl=16&amp;FORM=MBEDLD",
                        //"https://www.bing.com/maps?cp={LAT~LON}&amp;sty=r&amp;lvl=16&amp;FORM=MBEDLD"
                        DirMapLink = "https://www.bing.com/maps/directions?cp=" + dto.Location.Lat + "~" + dto.Location.Lon + "&amp;sty=r&amp;lvl=16&amp;rtp=~pos." + dto.Location.Lat + "~" + dto.Location.Lon + "____&amp;FORM=MBEDLD",
                        //https://www.bing.com/maps/directions?cp={LAT~LON}&amp;sty=r&amp;lvl=16&amp;rtp=~pos.{LAT~LON}____&amp;FORM=MBEDLD"
                    };
                }
                return vm;
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
                        CocktailName = c.CocktailName,
                        Remove = c.Remove
                        
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
