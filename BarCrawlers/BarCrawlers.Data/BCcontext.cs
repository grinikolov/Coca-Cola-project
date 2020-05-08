using BarCrawlers.Data.DBModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BarCrawlers.Data
{
    public class BCcontext : IdentityDbContext<User, Role, Guid>
    {
        public BCcontext(DbContextOptions<BCcontext> options)
    :       base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
