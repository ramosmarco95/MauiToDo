using MauiToDo.Data;
using MauiToDo.Models;
using System.Collections.ObjectModel;

namespace MauiToDo
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<TodoItem> Todos { get; set; } = new();
         
        //string _todoListData = string.Empty;
        readonly Database _database;
        public MainPage()
        {
            InitializeComponent();
            _database = new Database();
            _ = Initialize();
            TodosCollection.ItemsSource = Todos;
        }

        private async Task Initialize()
        {
            var todos = await _database.GetTodos();
            foreach (var todo in todos)
                Todos.Add(todo); // Add to ObservableCollection
                //_todoListData += $"Title: {todo.Title}\tDue Date: {todo.Due:f} {Environment.NewLine}";
            //TodosLabel.Text = _todoListData;
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
                Todos.Add(todo); //add new item to ObservableCollection
                //_todoListData += $"{todo.Title} - {todo.Due:f} {Environment.NewLine}";
                //TodosLabel.Text = _todoListData;
                TodoTitleEntry.Text = string.Empty;
                DueDatePicker.Date = DateTime.Now;
            }
        }
    }

}
