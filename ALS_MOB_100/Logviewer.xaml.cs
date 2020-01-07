using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ALS_MOB_100
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Logviewer : ContentPage
    {
        private string fileName = ListLogViewer.SelectedFile;
        ObservableCollection<HistoricalRes> HistoricalRes = new ObservableCollection<HistoricalRes>();

        const string Neg_color = "#E2423D";
        const string Pos_color = "#57B743";
        const string Nc_color = "#F7AB14";
        const string Verd_Char = "f4f501";
        const string Val_Char = "f4f500";
        const string Start_Char = "f4f503";
        const string Temp_Char = "f4f502";
        const string Pass_Str = "00000000";
        const string Neg_Str = "01000000";
        const string Verd_Pos = " P ";
        const string Verd_Neg = " N ";
        const string Verd_NC = " NC ";


        public Logviewer()
        {
            InitializeComponent();
            Title = ListLogViewer.SelectedFileName;
            LogList.ItemsSource = HistoricalRes;
            LoadLog();
        }
        void LoadLog()
        {

            Result[] results_acq;
            var json = File.ReadAllText(fileName);
            var rootobject = JsonConvert.DeserializeObject<Rootobject>(json);
            results_acq = rootobject.results_acq;
            int compteur;
            for (compteur = 0; compteur < results_acq.Length; compteur++)
            {
                string Color_Str;
                if (results_acq[compteur].Verdict == Verd_Pos)
                {
                    Color_Str = Pos_color;
                }
                else if (results_acq[compteur].Verdict == Verd_Neg)
                {
                    Color_Str = Neg_color;
                }
                else
                {
                    Color_Str = Nc_color;
                }
                HistoricalRes.Add(new HistoricalRes(results_acq[compteur].equipment, results_acq[compteur].Acq_Date, results_acq[compteur].Value, results_acq[compteur].Verdict, results_acq[compteur].Id, Color_Str, results_acq[compteur].Temperature, results_acq[compteur].SerialNumber, true));
            }

        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var n = (HistoricalRes)e.Item;
            info_acq.Text = n.Id.ToString();
            info_val.Text = n.Value;
            if (n.Verdict == Verd_Neg)
            {
                info_verd.Text = "Negative";
                info_verd_NC.IsVisible = false;

            }
            else if (n.Verdict == Verd_Pos)
            {
                info_verd.Text = "Positive";
                info_verd_NC.IsVisible = false;
            }
            else if (n.Verdict == Verd_NC)
            {
                info_verd.Text = "Not Conclusive";
                info_verd_NC.IsVisible = true;
            }
            info_date.Text = n.Date;
            info_time.Text = n.Time;
            info_verd.BackgroundColor = Color.FromHex(n.BgColor);
            info_temp.Text = n.Temperature + "°C";
            info_sn.Text = n.SerialNumber;
            background.IsVisible = true;
            info.IsVisible = true;

        }

        private void background_click(object sender, EventArgs e)
        {
            background.IsVisible = false;
            info.IsVisible = false;
        }
    }
}