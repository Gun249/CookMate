using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gunner.Models;
using SQLite;

namespace Gunner.Data
{
    class DataLogin
    {
        public const string namedb = "Loginnn.db3";
        SQLiteAsyncConnection _database;
        async Task Init()
        {
            if (_database != null)
                return;
            _database = new SQLiteAsyncConnection(ConstantsLogin.DatabasePath, ConstantsLogin.sQLiteOpenFlags);

            await _database.CreateTableAsync<Models.Login>();

        }
        public async Task<List<Models.Login>> GetLoginAsync()
        {
            await Init();
            return await _database.Table<Models.Login>().ToListAsync();

        }
        public async Task<Models.Login> GetLoginAsync(int id)
        {
            await Init();
            return await _database.Table<Models.Login>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<int> SaveLoginAsync(Models.Login Login)
        {
            await Init();
            if (Login.Id != 0)
            {
                return await _database.UpdateAsync(Login);
            }
            else
            {
                return await _database.InsertAsync(Login);
            }
        }
        public async Task<int> DeleteLoginAsync(Models.Login Login)
        {
            await Init();
            return await _database.DeleteAsync(Login);
        }

        public async Task<Models.Login> findusername(string username)
        {
            await Init();
            return await _database.Table<Models.Login>()
            .Where(i => i.Username == username)
            .FirstOrDefaultAsync();

        }

        public async Task UpdatePasswordAsync(string username, string newPassword)
        {
            await Init();
            var user = await _database.Table<Models.Login>()
                .Where(i => i.Username == username)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                user.Password = newPassword;
                await _database.UpdateAsync(user);
            }
        }



    }
}

