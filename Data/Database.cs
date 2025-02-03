using MauiToDo.Models;  // Imports the models used in the application
using SQLite;  // Imports SQLite for database interactions

namespace MauiToDo.Data
{
    // Internal class responsible for handling database operations
    internal class Database
    {
        // Private field to hold the SQLite connection
        private readonly SQLiteAsyncConnection _connection;

        // Constructor initializes the database connection
        public Database()
        {
            // Gets the application's data directory path
            var dataDir = FileSystem.AppDataDirectory;

            // Constructs the database file path
            var databasePath = Path.Combine(dataDir, "MauiTodo.db");

            // Retrieves the encryption key from secure storage (asynchronous call awaited synchronously)
            string _dbEncryptionKey = SecureStorage.GetAsync("dbKey").Result;

            // If no encryption key exists, generate a new GUID and store it securely
            if (string.IsNullOrEmpty(_dbEncryptionKey))
            {
                Guid g = new Guid();  // Creates a new GUID
                _dbEncryptionKey = g.ToString();  // Converts GUID to string
                SecureStorage.SetAsync("dbKey", _dbEncryptionKey);  // Stores encryption key securely
            }

            // Creates database connection options with encryption enabled
            var dbOptions = new SQLiteConnectionString(databasePath, true, key: _dbEncryptionKey);

            // Initializes the SQLite connection asynchronously
            _connection = new SQLiteAsyncConnection(dbOptions);

            // Calls the Initialise method asynchronously to create necessary tables
            _ = Initialise();
        }

        // Initializes the database by creating necessary tables
        private async Task Initialise()
        {
            await _connection.CreateTableAsync<TodoItem>();  // Creates the TodoItem table if it doesn't exist
        }

        // Retrieves all Todo items from the database
        public async Task<List<TodoItem>> GetTodos()
        {
            return await _connection.Table<TodoItem>().ToListAsync();  // Fetches all records as a list
        }

        // Retrieves a specific Todo item by ID
        public async Task<TodoItem> GetTodo(int id)
        {
            var query = _connection.Table<TodoItem>().Where(t => t.Id == id);  // Queries for a specific ID
            return await query.FirstOrDefaultAsync();  // Returns the first matching record or null if not found
        }

        // Adds a new Todo item to the database
        public async Task<int> AddTodo(TodoItem item)
        {
            return await _connection.InsertAsync(item);  // Inserts the item and returns the number of rows affected
        }

        // Deletes a Todo item from the database
        public async Task<int> DeleteTodo(TodoItem item)
        {
            return await _connection.DeleteAsync(item);  // Deletes the item and returns the number of rows affected
        }

        // Updates an existing Todo item in the database
        public async Task<int> UpdateTodo(TodoItem item)
        {
            return await _connection.UpdateAsync(item);  // Updates the item and returns the number of rows affected
        }
    }
}
