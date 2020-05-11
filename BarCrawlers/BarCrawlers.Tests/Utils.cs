using BarCrawlers.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarCrawlers.Tests
{
    class Utils
    {
        public static DbContextOptions<BCcontext> GetOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<BCcontext>()
                .UseInMemoryDatabase(databaseName).Options;
        }

        public static Guid MySampleGuid()
        {
            return Guid.Parse("f57960f4-d779-4b1c-ace2-056ad2dbb7d4");
        }

    }
}
