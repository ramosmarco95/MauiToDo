using MauiToDo.Data;
using MauiToDo.Models;

namespace MauiToDo
{
    public partial class MainPage : ContentPage
    {
        string _todoListData = string.Empty;
        readonly Database _database;
        public MainPage()
        {
            InitializeComponent();
            _database = new Database();
            _ = Initialize();
        }

       /* private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }*/

        private async Task Initialize()
        {
            var todos = await _database.GetTodos();
            foreach (var todo in todos)
                _todoListData += $"Title: {todo.Title}\tDue Date: {todo.Due:f} {Environment.NewLine}";
            TodosLabel.Text = _todoListData;
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
                _todoListData += $"{todo.Title} - {todo.Due:f} {Environment.NewLine}";
                TodosLabel.Text = _todoListData;
                TodoTitleEntry.Text = string.Empty;
                DueDatePicker.Date = DateTime.Now;
            }
        }
    }

}
