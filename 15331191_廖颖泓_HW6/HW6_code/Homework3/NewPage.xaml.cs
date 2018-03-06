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
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Homework3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewPage : Page
    {
        public NewPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            createButton.Content = "Create";
            cancelButton.Content = "Cancel";
        }

        private ViewModels.TodoItemViewModel ViewModel;
        private WriteableBitmap writeableBitmap;
        private StorageFile imageFile;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

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
            ViewModel = ((ViewModels.TodoItemViewModel)e.Parameter);
            if (ViewModel.SelectedItem == null)
            {
                createButton.Content = "Create";
                var i = new MessageDialog("Welcome!").ShowAsync();
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
                ViewModel.AddTodoItem(Title.Text, Details.Text, image.Source, DueDate.Date.DateTime);
                LiveTile.LiveTile.SendTileNotification(Title.Text, Details.Text, DueDate.Date.DateTime);
            }
            else
            {
                ViewModel.UpdateTodoItem(ViewModel.SelectedItem.id, Title.Text, Details.Text, image.Source, DueDate.Date.DateTime);
            }
            Frame.Navigate(typeof(MainPage), ViewModel);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (cancelButton.Content.ToString() == "Cancel")
            {
                ViewModel.SelectedItem = null;
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
            else
            {
                ViewModel.RemoveTodoItem(ViewModel.SelectedItem.id);
            }
            Frame.Navigate(typeof(MainPage), ViewModel);
        }

        private async void SelectPictureButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker open = new FileOpenPicker();
            open.ViewMode = PickerViewMode.Thumbnail;
            open.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            open.FileTypeFilter.Add(".jpg");
            open.FileTypeFilter.Add(".png");
            StorageFile file = await open.PickSingleFileAsync();
            if (file != null)
            {
                imageFile = file;
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

                    writeableBitmap = new WriteableBitmap(
                       (int)bitmapDecoder.OrientedPixelWidth,
                       (int)bitmapDecoder.OrientedPixelHeight);
                    using (Stream pixelStream = writeableBitmap.PixelBuffer.AsStream())
                    {
                        await pixelStream.WriteAsync(pixelData, 0, pixelData.Length);
                    }
                }
            }
            image.Source = writeableBitmap;
        }
    }
}
