# "BarCrawlers" 
### a Coca-Cola partner project
### by Grigor Nikolov & Ivan Ivanov 
### -- A18 Team Carlsberg --

### Access the [Trello board here](https://trello.com/b/7qJ5V83k/coca-cola-project "Trello board").


#Data
    DBModels - The folder contains the classes representing the database entities.
        Bar - All properties are public. 
            Id - Represents the unique database asigned index of the entity.
            Name - Represents the name of the bar.
            Rating - Represent the average value of user asigned ratings.
            TimesRated - Represents the number of users who have asigned this bar a rating.
            ImageSrc - Represents an url source for bar image.
            IsDeleted - Represents the entity status of soft deletion.
            Cocktails - Represents a collection of cocktail entities offerd in this bar.
            Comments - Represents a collection of comments made by users for this bar.
            BarRatings - Represents a collection of ratings made by users for this bar.
            Phone - Represents a contact phone information.
            Email - Represents a contact e-mail information.
            Address - Represents the address of this bar.
            Districr - Represents the district part of address.
            Town - Represents the town part of address.
            Country - Represents the country part of address.
            LocationId - Represents the unique database asigned index of the address.
            Location - Represents the location entity of this bar's address. 
        BarUserComments - All properties are public. Joining entity between bar and user for comment.
            BarId - Represents the unique database asigned index of the bar entity.
            Bar - Represents the bar entity of the comment.
            UserId - Represents the unique database asigned index of the user entity.
            User - Represents the user entity of the comment.
            Text - Represent the text input of the comment.
            IsFlagged - Represents whether the comment has been flagged.
        Cocktail - All properties are public.
            Id - Represents the unique database asigned index of the entity.
            Name - Represents the name of the cocktail.
            Rating - Represent the average value of user asigned ratings.
            TimesRated - Represents the number of users who have asigned this cocktail a rating.
            ImageSrc - Represents an url source for cocktail image.
            IsDeleted - Represents the entity status of soft deletion.
            IsAlcoholic - Represents whether the cocktail contains alcohol.
            Ingredients - Represents a collection of ingredient entities used in the.
            Bars -Represents a collection of bar entities that offer the cocktail.
            Comments - Represents a collection of comments made by users for this cocktail.
            CocktailRatings - Represents a collection of ratings made by users for this cocktail.
            Instructions - Represents the recipe for making this cocktail.
        CocktailBar - All properties are public. Joining entity between bar and cocktail.
            BarId - Represents the unique database asigned index of the bar entity.
            Bar - Represents the bar entity.
            CocktailId - Represents the unique database asigned index of the cocktail entity.
            Cocktail - Represents the cocktail entity.
        CocktailIngredient - All properties are public. Joining entity between ingredient and cocktail.
            CocktailId - Represents the unique database asigned index of the cocktail entity.
            Cocktail - Represents the cocktail entity.
            IngredientId - Represents the unique database asigned index of the ingredient entity.
            Ingredient - Represents the ingredient entity.
            Parts - Represents the relative ratio of the ingredient in the cocktail recipe.
        CocktailUserComment - All properties are public. Joining entity between cocktail and user for comment.
            CocktailId - Represents the unique database asigned index of the cocktail entity.
            Cocktail - Represents the cocktail entity of the comment.
            UserId - Represents the unique database asigned index of the user entity.
            User - Represents the user entity of the comment.
            Text - Represent the text input of the comment.
            IsFlagged - Represents whether the comment has been flagged.
        Ingredient - All properties are public.
            Id - Represents the unique database asigned index of the entity.
            Name - Represents the name of the ingredient.
            IsAlcoholic - Represents whether the ingredient contains alcohol.
            Ingredients - Represents a collection of ingredient entities used in the.
            Cocktails - Represents a collection of cocktail entities using this ingredient.
        Location - All properties are public.
            Id - Represents the unique database asigned index of the entity.
            Lattitude - Represents the lattitude coordinate of this location.
            Longtitude - Represents the longtitude coordinate of this location.
        Role - Inherits ASP.NET Core IdentityRole.
        User - Inherits ASP.NET Core IdentityUser , properties building over identity are public.
            ImageSrc - Represents an url source for user image.
            BarComments - Represents a collection of comments made by this user for bars.
            CocktailComments - Represents a collection of comments made by this user for cocktails.
            BarRatings - Represents a collection of ratings made by this user for bars.
            CocktailRatings - Represents a collection of ratings made by this user for cocktails.
        UserBarRating - All properties are public. Joining entity between bar and user for rating.
            BarId - Represents the unique database asigned index of the bar entity.
            Bar - Represents the bar entity of the rating.
            UserId - Represents the unique database asigned index of the user entity.
            User - Represents the user entity of the rating.
            Rating - Represent the value of the rating.
        UserCocktailRating - All properties are public. Joining entity between cocktail and user for rating.
            CocktailId - Represents the unique database asigned index of the cocktail entity.
            Cocktail - Represents the cocktail entity of the rating.
            UserId - Represents the unique database asigned index of the user entity.
            User - Represents the user entity of the rating.
            Rating - Represent the value of the rating.
    Migrartions - Folder containing the made database migration and their snapshots.
    ModelSettings - Folder containing the entitity builder options for the database.
    Seeder - Folder containing the class filling the database with dummy data.
    BCcontext - Class containing the database context methods, properties and options. Inherits ASP.NET Core IdentityDbContext. Each set represents a model entity.
