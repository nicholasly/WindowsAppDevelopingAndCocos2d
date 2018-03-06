using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace HW4.ViewModels
{
    class LineModel
    {
        public bool? complete1 { set; get; }

        public bool? complete2 { set; get; }

        public double visible1 { set; get; }

        public double visible2 { set; get; }

        #region Methods for handling the apps permanent data

        public LineModel()
        {
            complete1 = false;
            complete2 = false;
            visible1 = 0;
            visible2 = 0;
        }
        public void LoadData()
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("Data"))
            {
                LineModel data = JsonConvert.DeserializeObject<LineModel>(
                    (string)ApplicationData.Current.RoamingSettings.Values["Data"]);
                complete1 = data.complete1;
                complete2 = data.complete1;
                visible1 = data.visible1;
                visible2 = data.visible2;
            }
            else
            {
                complete1 = false;
                complete2 = false;
                visible1 = 0;
                visible2 = 0;
            }
        }

        public void SaveData()
        {
            LineModel data = new LineModel { complete1 = this.complete1, complete2 = this.complete2, visible1 = this.visible1, visible2 = this.visible2 };
            ApplicationData.Current.RoamingSettings.Values["Data"] =
                JsonConvert.SerializeObject(data);
        }
        #endregion
    }
}
