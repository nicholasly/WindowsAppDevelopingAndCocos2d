using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Todos.Models;
using System.Xml.Linq;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System;

namespace Homework3.LiveTile
{
    public class LiveTile
    {
        public static void SendTileNotification(string title, string description, DateTime date)
        {
            XDocument xdoc = XDocument.Load("Tile.xml");
            string temp = xdoc.ToString();
            string destXml = temp.Replace("title", title);
            destXml = destXml.Replace("description", description);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(destXml);
            TileNotification notification = new TileNotification(xml);
            notification.Tag = title;
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }
    }
}
