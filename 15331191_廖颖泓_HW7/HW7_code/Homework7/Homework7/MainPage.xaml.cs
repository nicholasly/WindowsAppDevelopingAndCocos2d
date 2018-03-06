using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Data.Xml.Dom;
using Windows.UI.Popups;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Homework7
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gender.Text = "";
            location.Text = "";
            queryAsync1(id.Text);
        }

        async void queryAsync1(string id)
        {
            string address = "http://api.k780.com:88/?app=idcard.get&idcard=" + id + "&appkey=10003&sign=b59bc3ef6191eb9f747dd4e83c99f2a4&format=xml";
            Uri uri = new Uri(address);
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(uri);
            XmlDocument document = new XmlDocument();
            document.LoadXml(result);
            XmlNodeList list = document.GetElementsByTagName("sex");
            IXmlNode node = list.Item(0);
            gender.Text = node.InnerText;
            list = document.GetElementsByTagName("style_citynm");
            node = list.Item(0);
            location.Text = node.InnerText;
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            ip.Text = "";
            queryAsync2(ip.Text);
        }

        async void queryAsync2(string ip)
        {
            string address = " http://api.k780.com:88/?app=ip.get&ip=" + ip + "&appkey=10003&sign=b59bc3ef6191eb9f747dd4e83c99f2a4&format=json";
            Uri uri = new Uri(address);
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(uri);
            JObject jo = (JObject)JsonConvert.DeserializeObject(result);
            country.Text = jo["result"]["area_style_simcall"].ToString();
        }
    }
}
