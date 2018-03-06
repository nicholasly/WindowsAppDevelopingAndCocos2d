using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Homework3.Models;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Homework3.ViewModels
{
    class TodoItemViewModel
    {
        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return this.allItems; } }

        private Models.TodoItem selectedItem = default(Models.TodoItem);
        public Models.TodoItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; }  }

        public TodoItemViewModel()
        {
            this.allItems.Add(new Models.TodoItem("123", "123"));
            this.allItems.Add(new Models.TodoItem("456", "456"));
        }

        public void AddTodoItem(string title, string description)
        {
            this.allItems.Add(new Models.TodoItem(title, description));
        }

        public void RemoveTodoItem(string id)
        {
            this.selectedItem = this.allItems.SingleOrDefault(i => i.id == id);
            this.allItems.Remove(this.selectedItem);
            // set selectedItem to null after remove
            this.selectedItem = null;
        }

        public void UpdateTodoItem(string id, string title, string description, DateTime date)
        {
            this.allItems.SingleOrDefault(i => i.id == id).title = title;
            this.allItems.SingleOrDefault(i => i.id == id).description = description;
            this.allItems.SingleOrDefault(i => i.id == id).date = date.Date;
            // set selectedItem to null after update
            this.selectedItem = null;
        }

    }
}
