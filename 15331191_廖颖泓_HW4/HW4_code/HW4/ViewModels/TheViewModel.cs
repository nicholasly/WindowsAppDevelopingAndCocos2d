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
    class TheViewModel
    {
        public string Field1 { get; set; }
        public string Field2 { get; set; }

        public DateTimeOffset DueDate { get; set; }
        #region Methods for handling the apps permanent data

        public TheViewModel()
        {
            Field1 = string.Empty;
            Field2 = string.Empty;
            DueDate = DateTime.Today.Date;
        }
        public void LoadData()
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("TheData"))
            {
                TheViewModel data = JsonConvert.DeserializeObject<TheViewModel>(
                    (string)ApplicationData.Current.RoamingSettings.Values["TheData"]);
                Field1 = data.Field1;
                Field2 = data.Field2;
                DueDate = data.DueDate;
            }
            else
            {
                Field1 = string.Empty;
                Field2 = string.Empty;
                DueDate = DateTime.Today.Date;
            }
        }

        public void SaveData()
        {
            TheViewModel data = new TheViewModel { Field1 = this.Field1, Field2 = this.Field2, DueDate = this.DueDate };
            ApplicationData.Current.RoamingSettings.Values["TheData"] =
                JsonConvert.SerializeObject(data);
        }
        #endregion

    }
}
