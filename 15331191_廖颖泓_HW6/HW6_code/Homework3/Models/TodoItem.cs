using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Homework3.Models
{
    public class TodoItem
    {
        public string id;

        public string title { get; set; }

        public string description { get; set; }

        public bool? completed { get; set; }

        public DateTime date { get; set; }

        public Windows.UI.Xaml.Media.ImageSource image { get; set; }

        public TodoItem(string title, string description)
        {
            this.id = Guid.NewGuid().ToString();
            this.title = title;
            this.description = description;
            this.completed = false;
            this.date = DateTime.Today.Date;
        }

        public TodoItem(string id, string title, string detail, DateTime date)
        {
            this.id = id;
            this.title = title;
            this.description = detail;
            this.date = date;
            this.completed = false;
        }

        public TodoItem(string title, string description, ImageSource image, DateTime date)
        {
            this.id = Guid.NewGuid().ToString();
            this.title = title;
            this.description = description;
            this.completed = false;
            this.date = date;
            this.image = image;
        }
    }
}