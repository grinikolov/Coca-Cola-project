using BarCrawlers.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCrawlers.Models.Contracts
{
    public interface IBarViewMapper
    {
        public BarViewModel MapDTOToView(BarDTO dto);

        public BarDTO MapViewToDTO(BarViewModel view);
    }
}
