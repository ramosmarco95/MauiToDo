using SQLite;

namespace MauiToDo.Models
{
    public class TodoItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty; // Ensures a default non-null value

        public DateTime Due { get; set; } = DateTime.UtcNow; // Default to current date

        public bool Done { get; set; } = false;
    }
}
