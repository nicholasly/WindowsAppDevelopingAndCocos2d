using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Shapes;
using Windows.ApplicationModel.DataTransfer;
using Homework3.Models;
using Homework3.LiveTile;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.Data.Xml.Dom;
using System.Collections.ObjectModel;
using System.Text;
using SQLitePCL;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Homework3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ViewModels.TodoItemViewModel ViewModel { get; set; }

        private List<Grid> m_renderedGrids = new List<Grid>();
        public MainPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            this.ViewModel = new ViewModels.TodoItemViewModel();
            GenerateTiles();
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
        }

        private void GenerateTiles()
        {
            foreach (var item in ViewModel.AllItems)
            {
                LiveTile.LiveTile.SendTileNotification(item.title, item.description, item.date);
            }
        }

        private async void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            var deferral = args.Request.GetDeferral();

            try
            {
                request.Data.Properties.Title = ViewModel.SelectedItem.title;
                request.Data.Properties.Description = ViewModel.SelectedItem.description;
                Uri pictureUri = ((BitmapImage)ViewModel.SelectedItem.image).UriSource;
                var photoFile = await StorageFile.GetFileFromApplicationUriAsync(pictureUri);
                RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromFile(photoFile);
                request.Data.Properties.Thumbnail = imageStreamRef;
                request.Data.SetBitmap(imageStreamRef);

                var shareDate = ViewModel.SelectedItem.date;
                request.Data.SetText(ViewModel.SelectedItem.description + '\n' + shareDate.Year.ToString() + "/" +
                    shareDate.Month.ToString() + "/" + shareDate.Day.ToString() + "/");
            }
            finally
            {
                deferral.Complete();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel))
            {
                this.ViewModel = (ViewModels.TodoItemViewModel)(e.Parameter);
            }

            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
            if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel))
            {
                this.ViewModel = (ViewModels.TodoItemViewModel)(e.Parameter);
            }
        }
        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedItem = null;
            Frame.Navigate(typeof(NewPage), ViewModel);
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (Title.Text == "")
            {
                var i = new MessageDialog("Title is not allowed to be empty!").ShowAsync();
                return;
            }
            if (Details.Text == "")
            {
                var i = new MessageDialog("Detail is not allowed to be empty!").ShowAsync();
                return;
            }
            if (DueDate.Date < DateTime.Today.Date)
            {
                var i = new MessageDialog("Past date is not allowed to be selected!").ShowAsync();
                return;
            }
            if (createButton.Content.ToString() == "Create")
            {
                ViewModel.AddTodoItem(Title.Text, Details.Text, new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/background.jpg")), DueDate.Date.DateTime);
            }
            else
            {
                ViewModel.UpdateTodoItem(ViewModel.SelectedItem.id, Title.Text, Details.Text, image.Source, DueDate.Date.DateTime);
                createButton.Content = "Create";
                GenerateTiles();
            }
            Title.Text = "";
            Details.Text = "";
            DueDate.Date = DateTime.Today.Date;
        }

        private async void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (cancelButton.Content.ToString() == "Cancel")
            {
                Title.Text = "";
                Details.Text = "";
                DueDate.Date = DateTime.Today.Date;
                WriteableBitmap defaultimage = null;
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/background.jpg"));
                if (file != null)
                {
                    using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {
                        BitmapDecoder bitmapDecoder = await BitmapDecoder.CreateAsync(fileStream);

                        BitmapTransform dummyTransform = new BitmapTransform();
                        PixelDataProvider pixelDataProvider =
                           await bitmapDecoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8,
                           BitmapAlphaMode.Premultiplied, dummyTransform,
                           ExifOrientationMode.RespectExifOrientation,
                           ColorManagementMode.ColorManageToSRgb);
                        byte[] pixelData = pixelDataProvider.DetachPixelData();

                        defaultimage = new WriteableBitmap(
                           (int)bitmapDecoder.OrientedPixelWidth,
                           (int)bitmapDecoder.OrientedPixelHeight);
                        using (Stream pixelStream = defaultimage.PixelBuffer.AsStream())
                        {
                            await pixelStream.WriteAsync(pixelData, 0, pixelData.Length);
                        }
                    }
                }
                image.Source = defaultimage;
                ViewModel.SelectedItem = null;
                createButton.Content = "Create";
                cancelButton.Content = "Cancel";
            }
            else
            {
                ViewModel.RemoveTodoItem(ViewModel.SelectedItem.id);
                Title.Text = "";
                Details.Text = "";
                DueDate.Date = DateTime.Today.Date;
                createButton.Content = "Create";
                cancelButton.Content = "Cancel";
            }

        }

        private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (Models.TodoItem)(e.ClickedItem);
            Grid currentGrid = (Grid)GetChildren(sender as ListView).First(x => x.Name == "ItemGrid");
            Line myLine = (Line)GetChildren(currentGrid).First(x => x.Name == "LINE");
            CheckBox myCheckBox = (CheckBox)GetChildren(currentGrid).First(x => x.Name == "checkbox");
            if (myCheckBox.IsChecked == true)
            {
                ViewModel.SelectedItem.completed = true;
            }
            else
            {
                ViewModel.SelectedItem.completed = false;
            }
            if (All.ActualWidth < 800.0)
            {
                Frame.Navigate(typeof(NewPage), ViewModel);
            }
            else
            {
                Title.Text = ViewModel.SelectedItem.title;
                Details.Text = ViewModel.SelectedItem.description;
                DueDate.Date = ViewModel.SelectedItem.date;
                createButton.Content = "Update";
                cancelButton.Content = "Delete";
            }
        }

        private List<FrameworkElement> GetChildren(DependencyObject parent)
        {
            List<FrameworkElement> controls = new List<FrameworkElement>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); ++i)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is FrameworkElement)
                {
                    controls.Add(child as FrameworkElement);
                }
                controls.AddRange(GetChildren(child));
            }
            return controls;
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        private void setSelectItem(object sender, RoutedEventArgs e)
        {
            dynamic item = e.OriginalSource;

            ViewModel.SelectedItem = (TodoItem)item.DataContext;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<TodoItem> resultItems = new ObservableCollection<TodoItem>();
            string keyWords = searchBox.Text;
            StringBuilder sb = new StringBuilder();
            string sql = @"SELECT title, description, date
                           FROM TodoItem
                           WHERE title LIKE ('%' || ? || '%') OR description LIKE ('%' || ? || '%') OR date LIKE ('%' || ? || '%')
                           ORDER BY date";
            using (var statement = Service.SQLiteService.SQLiteService.conn.Prepare(sql))
            {
                statement.Bind(1, keyWords);
                statement.Bind(2, keyWords);
                statement.Bind(3, keyWords);
                while (statement.Step() == SQLiteResult.ROW)
                {
                    sb.AppendFormat("{0}: {1} {2}: {3}  {4}: {5}", "Title", (string)statement[0], "Description", (string)statement[1], "Time", DateTime.Parse((string)statement[2]));
                    sb.AppendLine();
                }
            }
            string str = sb.ToString();

            var i = new MessageDialog(str).ShowAsync();
        }
    }
}
