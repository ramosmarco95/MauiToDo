using MauiToDo.Models;
using SQLite;
using System.Threading.Tasks;

namespace MauiToDo.Data
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _connection;

        public Database()
        {
            var dataDir = FileSystem.AppDataDirectory;
            var databasePath = Path.Combine(dataDir, "MauiTodo.db");

            string? _dbEncryptionKey = SecureStorage.GetAsync("dbKey").Result;
            if (string.IsNullOrEmpty(_dbEncryptionKey))
            {
                _dbEncryptionKey = Guid.NewGuid().ToString();
                SecureStorage.SetAsync("dbKey", _dbEncryptionKey);
            }

            var dbOptions = new SQLiteConnectionString(databasePath, true, key: _dbEncryptionKey);

            // ✅ Assign within the constructor
            _connection = new SQLiteAsyncConnection(dbOptions);

            _ = Initialise(); // Initialize DB
        }


        private async Task Initialise()
        {
            try
            {
                await _connection.CreateTableAsync<TodoItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization failed: {ex.Message}");
            }
        }

        public async Task<List<TodoItem>> GetTodos()
        {
            try
            {
                return await _connection.Table<TodoItem>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving todos: {ex.Message}");
                return new List<TodoItem>();
            }
        }

        public async Task<TodoItem?> GetTodo(int id)
        {
            try
            {
                return await _connection.Table<TodoItem>().Where(t => t.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving todo with ID {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<int> AddTodo(TodoItem item)
        {
            try
            {
                return await _connection.InsertAsync(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding todo: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> DeleteTodo(TodoItem item)
        {
            try
            {
                return await _connection.DeleteAsync(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting todo: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> UpdateTodo(TodoItem item)
        {
            try
            {
                return await _connection.UpdateAsync(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating todo: {ex.Message}");
                return 0;
            }
        }
    }
}
