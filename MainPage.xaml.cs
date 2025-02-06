using MauiToDo.Data;
using MauiToDo.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiToDo
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<TodoItem> Todos { get; set; } = new();
        private readonly Database _database;

        public ICommand DeleteItemCommand { get; }

        public MainPage()
        {
            InitializeComponent();
            _database = new Database();
            DeleteItemCommand = new Command<TodoItem>(async (item) => await DeleteTodoItem(item));
            _ = Initialize();
        }

        private async Task Initialize()
        {
            var todos = await _database.GetTodos();
            foreach (var todo in todos)
                Todos.Add(todo);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var todo = new TodoItem
            {
                Due = DueDatePicker.Date,
                Title = TodoTitleEntry.Text
            };
            var inserted = await _database.AddTodo(todo);
            if (inserted != 0)
            {
                Todos.Add(todo);
                TodoTitleEntry.Text = string.Empty;
                DueDatePicker.Date = DateTime.Now;
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (TodosCollection.SelectedItem is TodoItem selectedTodo)
            {
                await DeleteTodoItem(selectedTodo);
            }
        }

        private async Task DeleteTodoItem(TodoItem item)
        {
            if (item != null)
            {
                var deleted = await _database.DeleteTodo(item);
                if (deleted != 0)
                {
                    Todos.Remove(item);
                }
            }
        }
    }
}
