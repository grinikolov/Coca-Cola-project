using BarCrawlers.Data;
using BarCrawlers.Data.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCrawlers.Data.Seeder
{
    public class BCSeeder
    {
        //private readonly UserManager<User> _userManager;

        //private BCSeeder(UserManager<User> _userManager)
        //{
        //    this._userManager = _userManager;
        //}

        public static async Task InitAsync(UserManager<User> userManager, RoleManager<Role> roleManager)//BCcontext context)
        {
            await SeedRolesAsync(roleManager);
            //await SeedCountriesAsync(context);
            //await SeedStylesAsync(context);
            await SeedFirstAdmin(userManager);
            //await SeedBreweriesAsync(context);
            //await SeedBeersAsync(context);

        }
        private static async Task SeedRolesAsync(RoleManager<Role> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Magician"))
            {
                Role role = new Role();
                role.Name = "Magician";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!await roleManager.RoleExistsAsync("Crawler"))
            {
                Role role = new Role();
                role.Name = "Crawler";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            //if (context.Roles.Any())
            //    return;

            //var roleNames = new[] { "Magician", "Crawler" };
            //await context.Roles.AddRangeAsync(
            //    roleNames.Select(name => new Role()
            //    {
            //        Name = name,
            //        NormalizedName = name.ToUpper(),

            //    })
            //);
            //await context.SaveChangesAsync();
        }

        //private static async Task SeedStylesAsync(BOContext context)
        //{
        //    if (context.BeerStyles.Any())
        //        return;

        //    await context.BeerStyles.AddAsync(new BeerStyle()
        //    {
        //        Name = "Lager",
        //        Description = "Pale lagers are the standard international beer style, as personified by products from Miller to Heineken. This style is the generic spin-off of the pilsner style. Pale lagers are generally light- to medium-bodied with a light-to-medium hop impression and a clean, crisp malt character.",
        //        CreatedOn = DateTime.UtcNow,

        //    });
        //    await context.SaveChangesAsync();
        //}

        //private static async Task SeedCountriesAsync(BOContext context)
        //{
        //    if (context.Countries.Any())
        //        return;

        //    var countryNames = new[] { "Bulgaria", "Germany", "Chech Republic" };
        //    await context.Countries.AddRangeAsync(
        //        countryNames.Select(name => new Country()
        //        {
        //            Name = name,
        //            CreatedOn = DateTime.UtcNow,
        //        })
        //    );
        //    await context.SaveChangesAsync();
        //}

        private static async Task SeedFirstAdmin(UserManager<User> usermanager)//BCcontext context)
        {
            if (await usermanager.FindByNameAsync("MasterMagician") == null)
            {
                User user = new User();
                user.UserName = "MasterMagician";
                user.Email = "some@e.mail";
                var result = await usermanager.CreateAsync(user, "MasterOfCrawlers");
                if (result.Succeeded)
                {
                    await usermanager.AddToRoleAsync(user, "Magician");
                }
            }
            if (await usermanager.FindByNameAsync("TestUser1") == null)
            {
                User user = new User();
                user.UserName = "TestUser1";
                user.Email = "Test@User.One";
                var result = await usermanager.CreateAsync(user, "TestUser1");
                if (result.Succeeded)
                {
                    await usermanager.AddToRoleAsync(user, "Crawler");
                }
            }
            if (await usermanager.FindByNameAsync("TestUser2") == null)
            {
                User user = new User();
                user.UserName = "TestUser2";
                user.Email = "Test@User.Two";
                var result = await usermanager.CreateAsync(user, "TestUser2");
                if (result.Succeeded)
                {
                    await usermanager.AddToRoleAsync(user, "Crawler");
                }
            }
            if (await usermanager.FindByNameAsync("TestUser3") == null)
            {
                User user = new User();
                user.UserName = "TestUser3";
                user.Email = "Test@User.Three";
                var result = await usermanager.CreateAsync(user, "TestUser3");
                if (result.Succeeded)
                {
                    await usermanager.AddToRoleAsync(user, "Crawler");
                }
            }
            if (await usermanager.FindByNameAsync("TestUser4") == null)
            {
                User user = new User();
                user.UserName = "TestUser4";
                user.Email = "Test@User.Four";
                var result = await usermanager.CreateAsync(user, "TestUser4");
                if (result.Succeeded)
                {
                    await usermanager.AddToRoleAsync(user, "Crawler");
                }
            }
            if (await usermanager.FindByNameAsync("TestUser5") == null)
            {
                User user = new User();
                user.UserName = "TestUser5";
                user.Email = "Test@User.Five";
                var result = await usermanager.CreateAsync(user, "TestUser5");
                if (result.Succeeded)
                {
                    await usermanager.AddToRoleAsync(user, "Crawler");
                }
            }
            if (await usermanager.FindByNameAsync("TestUser6") == null)
            {
                User user = new User();
                user.UserName = "TestUser6";
                user.Email = "Test@User.Six";
                var result = await usermanager.CreateAsync(user, "TestUser6");
                if (result.Succeeded)
                {
                    await usermanager.AddToRoleAsync(user, "Crawler");
                }
            }
            if (await usermanager.FindByNameAsync("TestUser7") == null)
            {
                User user = new User();
                user.UserName = "TestUser7";
                user.Email = "Test@User.Seven";
                var result = await usermanager.CreateAsync(user, "TestUser7");
                if (result.Succeeded)
                {
                    await usermanager.AddToRoleAsync(user, "Crawler");
                }
            }
            if (await usermanager.FindByNameAsync("TestUser8") == null)
            {
                User user = new User();
                user.UserName = "TestUser8";
                user.Email = "Test@User.Eight";
                var result = await usermanager.CreateAsync(user, "TestUser8");
                if (result.Succeeded)
                {
                    await usermanager.AddToRoleAsync(user, "Crawler");
                }
            }
            if (await usermanager.FindByNameAsync("TestUser9") == null)
            {
                User user = new User();
                user.UserName = "TestUser9";
                user.Email = "Test@User.Nine";
                var result = await usermanager.CreateAsync(user, "TestUser9");
                if (result.Succeeded)
                {
                    await usermanager.AddToRoleAsync(user, "Crawler");
                }
            }
            if (await usermanager.FindByNameAsync("TestUser10") == null)
            {
                User user = new User();
                user.UserName = "TestUser10";
                user.Email = "Test@User.Ten";
                var result = await usermanager.CreateAsync(user, "TestUser10");
                if (result.Succeeded)
                {
                    await usermanager.AddToRoleAsync(user, "Crawler");
                }
            }
            if (await usermanager.FindByNameAsync("TestUser11") == null)
            {
                User user = new User();
                user.UserName = "TestUser11";
                user.Email = "Test@User.Eleven";
                var result = await usermanager.CreateAsync(user, "TestUser11");
                if (result.Succeeded)
                {
                    await usermanager.AddToRoleAsync(user, "Crawler");
                }
            }
            if (await usermanager.FindByNameAsync("TestUser12") == null)
            {
                User user = new User();
                user.UserName = "TestUser12";
                user.Email = "Test@User.Twelve";
                var result = await usermanager.CreateAsync(user, "TestUser12");
                if (result.Succeeded)
                {
                    await usermanager.AddToRoleAsync(user, "Crawler");
                }
            }
            //if (context.Users.Any())
            //{
            //    return;
            //}

            //var user = new User { UserName = "MasterMagician", Email = "some@e.mail" };
            //var result = await _userManager.CreateAsync(user, "MasterOfCrawlers");
            //await _userManager.AddToRoleAsync(user, "Magician");

            //await context.Users.AddAsync(user);
            //await context.SaveChangesAsync();
        }

        //private static async Task SeedBreweriesAsync(BOContext context)
        //{
        //    if (context.Breweries.Any())
        //        return;

        //    await context.Breweries.AddAsync(
        //        new Brewery()
        //        {
        //            Name = "Carlsberg",
        //            CountryID = context.Countries.FirstOrDefault(c => c.Name == "Bulgaria").ID,
        //            Country = context.Countries.FirstOrDefault(c => c.Name == "Bulgaria"),
        //            CreatedOn = DateTime.UtcNow
        //        });
        //    await context.SaveChangesAsync();
        //}

        //private static async Task SeedBeersAsync(BOContext context)
        //{
        //    if (context.Beers.Any())
        //        return;

        //    var beerNames = new[] { "Carlsberg", "Shumensko", "Pirinsko" };
        //    foreach (var beer in beerNames)
        //    {
        //        var b = new Beer() 
        //        {
        //            Name = beer,
        //            Country = await context.Countries.FirstOrDefaultAsync(c => c.Name == "Bulgaria"),
        //            Brewery = await context.Breweries.FirstOrDefaultAsync(b => b.Name == "Carlsberg"),
        //            ABV = 4,
        //            Style = await context.BeerStyles.FindAsync(1),
        //            CreatedOn = DateTime.UtcNow
        //        };
        //        b.CountryID = b.Country.ID;
        //        b.BreweryID = b.Brewery.ID;
        //        b.StyleID = b.Style.ID;
        //        await context.Beers.AddAsync(b);
        //    }
        //    await context.SaveChangesAsync();
        //}
    }
}
