using Gunner.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gunner.Data
{
    class UserRecipes
    {
        public const string namedb = "Recipesss.db3";
        SQLiteAsyncConnection _database;
        async Task Init()
        {
            if (_database != null)
                return;
            _database = new SQLiteAsyncConnection(ConstantsRecipes.DatabasePath, ConstantsRecipes.sQLiteOpenFlags);

            await _database.CreateTableAsync<Models.Recipes>();

        }
        public async Task<List<Models.Recipes>> GetRecipesAsync()
        {
            await Init();
            return await _database.Table<Models.Recipes>().ToListAsync();

        }
        public async Task<Models.Recipes> GetRecipesAsync(int id)
        {
            await Init();
            return await _database.Table<Models.Recipes>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<int> SaveRecipesAsync(Models.Recipes Recipes)
        {
            await Init();
            if (Recipes.Id != 0)
            {
                return await _database.UpdateAsync(Recipes);
            }
            else
            {
                return await _database.InsertAsync(Recipes);
            }
        }
        public async Task<int> DeleteRecipeByNameAsync(string name)
        {
            await Init();  // Ensure the database is initialized
            var recipesToDelete = await _database.Table<Models.Recipes>().Where(x => x.Name == name).ToListAsync();
            int count = 0;

            foreach (var recipe in recipesToDelete)
            {
                count += await _database.DeleteAsync(recipe);
            }

            return count; // Returns the number of records deleted
        }

        public async Task<List<Models.Recipes>> findusername(string username)
        {
            await Init();
            return await _database.Table<Models.Recipes>()
                   .Where(i => i.Username == username)
                   .ToListAsync();
        }

        public async Task<Recipes> GetRecipeByNameAsync(string name)
        {
            await Init(); // Make sure the database connection is initialized
            return await _database.Table<Recipes>().Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task UpdateRecipeAsync(Models.Recipes recipe)
        {
            await Init();
            await _database.UpdateAsync(recipe);
        }





    }

}
