using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SQLitePCL;
using System.Text;
using System.Threading.Tasks;
using Homework3.Models;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;

namespace Homework3.ViewModels
{
    class TodoItemViewModel
    {
        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return this.allItems; } }

        private Models.TodoItem selectedItem = default(Models.TodoItem);
        public Models.TodoItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }

        public TodoItemViewModel()
        {
            // this.allItems.Add(new Models.TodoItem("123", "123"));
            // this.allItems.Add(new Models.TodoItem("456", "456"));
            string sql = "SELECT id, title, description, date FROM TodoItem ORDER BY date";
            using (var statement = Service.SQLiteService.SQLiteService.conn.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    TodoItem todoItem = new TodoItem((string)statement[0],
                           (string)statement[1],
                           (string)statement[2],
                           DateTime.Parse((string)statement[3]));
                    this.allItems.Add(todoItem);
                }
            }
        }

        public void AddTodoItem(string title, string description, ImageSource image, DateTime date)
        {
            this.allItems.Add(new Models.TodoItem(title, description, image, date));
            string sql = "INSERT INTO TodoItem (id, title, description, date) VALUES (?, ?, ?, ?)";
            using (var statement = Service.SQLiteService.SQLiteService.conn.Prepare(sql))
            {
                statement.Bind(1, this.allItems.Last().id);
                statement.Bind(2, this.allItems.Last().title);
                statement.Bind(3, this.allItems.Last().description);
                statement.Bind(4, this.allItems.Last().date.ToString());
                statement.Step();
            }

            // set selectedItem to null after add
            this.selectedItem = null;
        }

        public void RemoveTodoItem(string id)
        {
            string sql = "DELETE FROM ToDoItem WHERE id = ? ";
            using (var statement = Service.SQLiteService.SQLiteService.conn.Prepare(sql))
            {
                statement.Bind(1, id);
                statement.Step();
            }

            this.selectedItem = this.allItems.SingleOrDefault(i => i.id == id);
            this.allItems.Remove(this.selectedItem);
            // set selectedItem to null after remove
            this.selectedItem = null;
        }

        public void UpdateTodoItem(string id, string title, string description, ImageSource img, DateTime date)
        {
            this.allItems.SingleOrDefault(i => i.id == id).title = title;
            this.allItems.SingleOrDefault(i => i.id == id).description = description;
            this.allItems.SingleOrDefault(i => i.id == id).image = img;
            this.allItems.SingleOrDefault(i => i.id == id).date = date.Date;

            string sql = "UPDATE TodoItem SET id = ?, title = ?, description = ?, date = ? WHERE id = ?";
            using (var statement = Service.SQLiteService.SQLiteService.conn.Prepare(sql))
            {
                statement.Bind(1, id);
                statement.Bind(2, title);
                statement.Bind(3, description);
                statement.Bind(4, date.ToString());
                statement.Bind(5, id);
                statement.Step();
            }

            // set selectedItem to null after update
            this.selectedItem = null;
        }

    }
}
