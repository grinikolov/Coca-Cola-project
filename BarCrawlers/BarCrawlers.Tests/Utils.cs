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
            return Guid.Parse("a17960f4-d779-4b1c-ace2-056ad2dbb7d4");
        }
        public static Guid MySampleGuid2()
        {
            return Guid.Parse("b27960f4-d779-4b1c-ace2-056ad2dbb7d4");
        }
        public static Guid MySampleGuid3()
        {
            return Guid.Parse("c37960f4-d779-4b1c-ace2-056ad2dbb7d4");
        }
    }
}
