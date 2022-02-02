using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using ArchiveReader.Models;

namespace ArchiveReader.Data
{
    public class LibraryDatabase
    {
        private static SQLiteAsyncConnection _database;
        public EventHandler<Work> WorkAddedEvent;

        public static readonly AsyncLazy<LibraryDatabase> Instance = new AsyncLazy<LibraryDatabase>(async () =>
        {
            var instance = new LibraryDatabase();
            CreateTableResult result = await _database.CreateTableAsync<Work>();
            return instance;
        });

        public LibraryDatabase()
        {
            _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<Work>> GetWorksAsync()
        {
            return _database.Table<Work>().ToListAsync();
        }

        // Not 100% sure about this SQL query
        public Task<List<Work>> GetWorksNotDoneAsync()
        {
            return _database.QueryAsync<Work>("SELECT * FROM [Work]");
        }

        public Task<Work> GetWorkAsync(string id)
        {
            return _database.Table<Work>().Where(i => i.id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveWorkAsync(Work work)
        {
            Task<int> outValue;

            if (work.dbID != 0)
            {
                outValue = _database.UpdateAsync(work);
            }
            else
            {
                outValue =  _database.InsertAsync(work);
            }

            WorkAddedEvent?.Invoke(this, work);
            return outValue;
        }

        public Task<int> DeleteWorkAsync(Work work)
        {
            return _database.DeleteAsync(work);
        }
    }
}
