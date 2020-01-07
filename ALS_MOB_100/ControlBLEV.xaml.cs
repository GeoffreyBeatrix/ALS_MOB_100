using Newtonsoft.Json;
using Quick.Xamarin.BLE.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;


namespace ALS_MOB_100
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ControlBLEV : ContentPage
    {
        public static AdapterConnectStatus BleStatus;
        List<IGattCharacteristic> AllCharacteristics = new List<IGattCharacteristic>();
        //IGattCharacteristic SelectCharacteristic = null;
        ObservableCollection<HistoricalRes> HistoricalRes = new ObservableCollection<HistoricalRes>();
        ObservableCollection<CharacteristicsList> CharacteristicsList = new ObservableCollection<CharacteristicsList>();

        //bool isnotify = false;
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
        private string BLE_dev_SN = Search.ConnectDevice.Name.Substring(6);
        private string fileName;
        private bool doesExist;
        ImageSource PlayLogo = ImageSource.FromFile("Image/PlayN40.png");



        public ControlBLEV()
        {
            
            InitializeComponent();
            Strt_btn.ImageSource = PlayLogo;
            Search.ble.AdapterStatusChange += Ble_AdapterStatusChange;
            Search.ble.ServerCallBackEvent += Ble_ServerCallBackEvent;
            Hist_Res.ItemsSource = HistoricalRes;


        }
        private void Ble_ServerCallBackEvent(string uuid, byte[] value)
        {
            Device.BeginInvokeOnMainThread(() =>
           {
               if (uuid.Contains(Val_Char))
               {
                   double dbl = BitConverter.ToDouble(value, 0);
                   info_Val2.Text = dbl.ToString("0.000");
                   foreach (var c in AllCharacteristics)
                   {
                       if (c.Uuid.Contains(Verd_Char))
                       {
                           c.ReadCallBack();
                       }
                   }
               }
               else if (uuid.Contains(Verd_Char))
               {
                   StringBuilder hex = new StringBuilder(value.Length * 2);
                   foreach (byte b in value)
                   {
                       hex.AppendFormat("{0:X2}", b);
                   }
                   string temp_hex_str = hex.ToString();
                   string Verdict_Str;
                   string Color_Str;
                   if (temp_hex_str == Pass_Str)
                   {
                       Verdict_Str = Verd_Pos;
                       Color_Str = Pos_color;
                   }
                   else if (temp_hex_str == Neg_Str)
                   {
                       Verdict_Str = Verd_Neg;
                       Color_Str = Neg_color;
                   }
                   else
                   {
                       Verdict_Str = Verd_NC;
                       Color_Str = Nc_color;
                   }
                   info_Verd2.Text = Verdict_Str;
                   info_Verd2.BackgroundColor = Color.FromHex(Color_Str);

                   foreach (var c in AllCharacteristics)
                   {
                       if (c.Uuid.Contains(Temp_Char))
                       {
                           c.ReadCallBack();
                       }
                   }
               }
               else if (uuid.Contains(Temp_Char))
               {

                   double dbl = BitConverter.ToDouble(value, 0);
                   string Temp_Val = dbl.ToString("0.00");
                   string Color_Str = info_Verd2.BackgroundColor.ToHex();
                   string acq_date = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                   string acq_time = DateTime.Now.ToString("HH:mm:ss");
                   string acq_day = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                   string nb_acq = Convert.ToString(HistoricalRes.Count);
                   HistoricalRes.Add(new HistoricalRes()
                   {
                       Device_SN = BLE_dev_SN,
                       Acq_Date = acq_date,
                       Date = acq_day,
                       Time = acq_time,
                       Value = info_Val2.Text,
                       Verdict = info_Verd2.Text,
                       Id = nb_acq,
                       BgColor = Color_Str,
                       Temperature = Temp_Val,
                       SerialNumber = null,
                       Saved = false,
                   });

                   Save_2_file.IsEnabled = true;
                   Clear_tb.IsEnabled = true;
               }

           });

        }

        private void Ble_AdapterStatusChange(object sender, AdapterConnectStatus e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                Search.BleStatus = e;
                if (Search.BleStatus == AdapterConnectStatus.Connected)
                {
                    Load_file.IsEnabled = false;
                    Delete_tb.IsEnabled = false;
                    Clear_tb.IsEnabled = false;
                    Delete_tb.IsEnabled = false;
                    Save_2_file.IsEnabled = false;
                    msg_txt.Text = "Connecting....";
                    Title = "Connecting....";
                    await Task.Delay(1000);
                    msg_txt.Text = "Success";
                    await Task.Delay(1000);
                    Title = "Connected to " + BLE_dev_SN;
                    msg_layout_bg.IsVisible = false;
                    Status_layout.IsVisible = true;
                    msg_layout.IsVisible = false;
                    test_layout.IsVisible = false;
                    Hist_Res.IsVisible = true;
                    Hist_Res_Lab.IsVisible = true;
                    ReadCharacteristics();
                    fileName = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "AMLOS_" + BLE_dev_SN + ".json");
                    doesExist = File.Exists(fileName);
                    var files = System.IO.Directory.GetFiles(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)));


                    if (doesExist)
                    {
                        Load_file.IsEnabled = true;
                        Delete_tb.IsEnabled = true;
                    }
                    else
                    {
                        Load_file.IsEnabled = false;
                        Delete_tb.IsEnabled = false;
                    }
                }
                if (Search.BleStatus == AdapterConnectStatus.None)
                {
                    await DisplayAlert("Alert", "Connection to  " + BLE_dev_SN + " lost", "OK");
                    await Navigation.PopToRootAsync(true);
                }
            });
        }
        void ReadCharacteristics()
        {

            Search.ConnectDevice.CharacteristicsDiscovered(cha =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (cha.Uuid.Contains(Val_Char) || cha.Uuid.Contains(Verd_Char) || cha.Uuid.Contains(Start_Char) || cha.Uuid.Contains(Temp_Char))
                    {
                        string Name;
                        if (cha.Uuid.Contains(Val_Char))
                        {
                            cha.NotifyEvent += SelectCharacteristic_NotifyEvent;
                            cha.Notify();
                            Name = "Result";
                        }
                        else if (cha.Uuid.Contains(Verd_Char))
                        {
                            Name = "Verdict";
                        }
                        else if (cha.Uuid.Contains(Start_Char))
                        {
                            Name = "Start Acquisition";
                        }
                        else if (cha.Uuid.Contains(Temp_Char))
                        {
                            Name = "Temperature";
                        }
                        else
                        {
                            Name = "";
                        }
                        AllCharacteristics.Add(cha);
                        CharacteristicsList.Add(new CharacteristicsList(Name, cha.Uuid, cha.CanRead(), cha.CanWrite(), cha.CanNotify()));

                    }

                });
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Search.ble.AdapterStatusChange -= Ble_AdapterStatusChange;
            Search.ble.ServerCallBackEvent -= Ble_ServerCallBackEvent;
            if (Search.ConnectDevice != null) Search.ConnectDevice.DisconnectDevice();
        }
        private void strt_Clicked(object sender, EventArgs e)
        {
            foreach (var c in AllCharacteristics)
            {

                if (c.Uuid.Contains(Start_Char))
                {
                    var bytearray = StringToByteArray("01");
                    c.Write(bytearray);
                }
            }
        }
        private void getlast_Clicked(object sender, EventArgs e)
        {
            foreach (var c in AllCharacteristics)
            {
                if (c.Uuid.Contains(Val_Char))
                {
                    c.ReadCallBack();
                }
            }

        }
        public static byte[] StringToByteArray(string hex)
        {
            try
            {
                return Enumerable.Range(0, hex.Length)
                                 .Where(x => x % 2 == 0)
                                 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                                 .ToArray();
            }
            catch { return null; }
        }
        private void SelectCharacteristic_NotifyEvent(object sender, byte[] value)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                double dbl = BitConverter.ToDouble(value, 0);
                info_Val2.Text = dbl.ToString("0.000");
                foreach (var c in AllCharacteristics)
                {
                    if (c.Uuid.Contains(Verd_Char))
                    {
                        c.ReadCallBack();
                    }

                }
            });
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
            RefreshListview();
        }
        private void RefreshListview()
        {
            Hist_Res.ItemsSource = null;
            Hist_Res.ItemsSource = HistoricalRes;
        }
        private void OpenScanner(object sender, EventArgs e)
        {
            Scanner();
        }
        public async void Scanner()
        {

            var ScannerPage = new ZXingScannerPage();

            ScannerPage.OnScanResult += (result) =>
            {
                ScannerPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                    info_sn.Text = result.Text;
                    HistoricalRes[Convert.ToInt32(info_acq.Text)].SerialNumber = result.Text;
                });
            };


            await Navigation.PushAsync(ScannerPage);

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Result[] results_acq = Json_Deserializing(fileName);
            for (int compteur = 0; compteur < results_acq.Length; compteur++)
            {
                bool isExisting = false;
                for (int i = 0; i < HistoricalRes.Count; i++)
                {
                    if (HistoricalRes[i].Acq_Date == results_acq[compteur].Acq_Date) { isExisting = true; }
                }
                if (!isExisting)
                {
                    string Color_Str;
                    Color_Str = (results_acq[compteur].Verdict == Verd_Pos) ? Pos_color : Neg_color;
                    Color_Str = (results_acq[compteur].Verdict == Verd_Neg) ? Neg_color : Nc_color;
                    //HistoricalRes.Add(new HistoricalRes(results_acq[compteur].equipment, results_acq[compteur].Acq_Date, results_acq[compteur].Value, results_acq[compteur].Verdict, results_acq[compteur].Id, Color_Str, results_acq[compteur].Temperature, results_acq[compteur].SerialNumber,true));
                    HistoricalRes.Add(new HistoricalRes()
                    {
                        Device_SN = results_acq[compteur].equipment,
                        Acq_Date = results_acq[compteur].Acq_Date,
                        Date = results_acq[compteur].Acq_Date.Substring(0, 10),
                        Time = results_acq[compteur].Acq_Date.Substring(results_acq[compteur].Acq_Date.Length - 8),
                        Value = results_acq[compteur].Value,
                        Verdict = results_acq[compteur].Verdict,
                        Id = results_acq[compteur].Id,
                        BgColor = Color_Str,
                        Temperature = results_acq[compteur].Temperature,
                        SerialNumber = results_acq[compteur].SerialNumber,
                        Saved = true,
                    });
                }
            }
            Clear_tb.IsEnabled = true;

        }

        private void SaveLog_Clicked(object sender, EventArgs e)
        {
            string output = "";
            Result[] results_acq;
            if (doesExist)
            {
                results_acq = Json_Deserializing(fileName);
                for (int i = 0; i < results_acq.Length; i++)
                {
                    output += Json_Serialization(results_acq[i].equipment, results_acq[i].Acq_Date, results_acq[i].SerialNumber, results_acq[i].Id, results_acq[i].Value, results_acq[i].Verdict, results_acq[i].Temperature);
                }
            }
            for (int i = 0; i < HistoricalRes.Count; i++)
            {
                if (HistoricalRes[i].Saved == false)
                {

                    output += Json_Serialization(HistoricalRes[i].Device_SN, HistoricalRes[i].Acq_Date, HistoricalRes[i].SerialNumber, HistoricalRes[i].Id, HistoricalRes[i].Value, HistoricalRes[i].Verdict, HistoricalRes[i].Temperature);

                }
            }
            output = Json_StringFormat(output);
            File.WriteAllText(fileName, output);
            Load_file.IsEnabled = true;
            Delete_tb.IsEnabled = true;
            Clear_tb.IsEnabled = true;
            doesExist = true;
        }
        private Result[] Json_Deserializing(String file)
        {
            Result[] results_acq;
            var json = File.ReadAllText(file);
            var rootobject = JsonConvert.DeserializeObject<Rootobject>(json);
            results_acq = rootobject.results_acq;
            return results_acq;
        }
        private string Json_Serialization(string equipement, string Acq_Date, string SerialNumber, string Id, string Value, string Verdict, string Temperature)
        {

            var result = new Result()
            {
                equipment = equipement,
                Acq_Date = Acq_Date,
                SerialNumber = SerialNumber,
                Id = Id,
                Value = Value,
                Verdict = Verdict,
                Temperature = Temperature,
            };
            var json = JsonConvert.SerializeObject(result, Formatting.Indented);
            string output = json + ",";
            return output;
        }
        private string Json_StringFormat(string In_Str)
        {
            string Out_Str = "{\"results_acq\": [" + In_Str + "]}";
            return Out_Str;
        }
        private void Clear_Clicked(object sender, EventArgs e)
        {
            HistoricalRes.Clear();
            Save_2_file.IsEnabled = false;
            Clear_tb.IsEnabled = false;
        }
        private void Delete_Clicked(object sender, EventArgs e)
        {
            File.Delete(fileName);
            doesExist = false;
            Load_file.IsEnabled = false;
            Delete_tb.IsEnabled = false;

        }



    }
}