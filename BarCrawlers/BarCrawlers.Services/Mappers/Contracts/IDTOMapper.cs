using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Services.Mappers.Contracts
{
    public interface IDTOMapper< TDTO, TEntity>
    {
        TDTO MapEntityToDTO(TEntity entity);
    }
}
