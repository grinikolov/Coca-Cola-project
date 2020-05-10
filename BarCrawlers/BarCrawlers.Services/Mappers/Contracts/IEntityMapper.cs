using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Services.Mappers.Contracts
{
    public interface IEntityMapper< TEntity, TDTO>
    {
        TEntity MapDTOToEntity(TDTO dto);
    }
}
