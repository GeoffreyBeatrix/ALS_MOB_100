using Quick.Xamarin.BLE;
using Quick.Xamarin.BLE.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ALS_MOB_100
{
    public partial class Search : ContentPage
    {

        public static AdapterConnectStatus BleStatus;
        public static IBle ble;
        public static IDev ConnectDevice = null;
        ObservableCollection<BleList> blelist = new ObservableCollection<BleList>();
        public static List<IDev> ScanDevices = new List<IDev>();
        bool isConnected = false;
        private string DevName = "AMLOS";
        const string Pos_color = "#57B743";
        const string Neg_color = "#808080";
        const string Uuid_color = "#B5B5B5";
        const string Name_color = "#13518F";
        const string Chev_color = "#273389";
        const string Des_color = "#D6D6D6";
        private int Indic_Cycl = 0;
        public Search()
        {
            InitializeComponent();

            ble = CrossBle.Createble();
            //when search devices
            ble.OnScanDevicesIn += Ble_OnScanDevicesIn;

            BleStatus = ble.AdapterConnectStatus;
            listView.ItemsSource = blelist;


        }

        private void Ble_OnScanDevicesIn(object sender, IDev e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Indic_Cycl++;
                if (Indic_Cycl > 1000)
                {
                    Indic_Cycl = 0;
                    for (int i = 0; i < blelist.Count; i++)
                    {
                        blelist[i].Rssi = 0;
                        blelist[i].Bar1 = Neg_color;
                        blelist[i].Bar2 = Neg_color;
                        blelist[i].Bar3 = Neg_color;
                        blelist[i].Bar4 = Neg_color;
                        blelist[i].Status = false;
                        blelist[i].Chev_col = Des_color;
                        blelist[i].Name_col = Des_color;
                        blelist[i].Uuid_col = Des_color;
                    }
                }
                try
                {
                    if (e.Name != null)
                    {

                        if (e.Name.Contains(DevName))
                        {
                            var n = ScanDevices.Find(x => x.Uuid == e.Uuid);
                            if (n == null)
                            {

                                string BLEName_trunc = e.Name.Substring(5);
                                string Bar1_val, Bar2_val, Bar3_val, Bar4_val;
                                ScanDevices.Add(e);
                                Bar1_val = (e.Rssi + 100 > 0) ? Pos_color : Neg_color;
                                Bar2_val = (e.Rssi + 100 > 25) ? Pos_color : Neg_color;
                                Bar3_val = (e.Rssi + 100 > 50) ? Pos_color : Neg_color;
                                Bar4_val = (e.Rssi + 100 > 75) ? Pos_color : Neg_color;
                                blelist.Add(new BleList(BLEName_trunc, e.Uuid, e.Rssi, Bar1_val, Bar2_val, Bar3_val, Bar4_val, true, Uuid_color, Name_color, Chev_color));
                            }
                            else
                            {
                                for (int i = 0; i < blelist.Count; i++)
                                {
                                    if (e.Uuid == blelist[i].Uuid)
                                    {
                                        string BLEName_trunc = e.Name.Substring(5);
                                        string Bar1_val, Bar2_val, Bar3_val, Bar4_val;
                                        Bar1_val = (e.Rssi + 100 > 0) ? Pos_color : Neg_color;
                                        Bar2_val = (e.Rssi + 100 > 25) ? Pos_color : Neg_color;
                                        Bar3_val = (e.Rssi + 100 > 50) ? Pos_color : Neg_color;
                                        Bar4_val = (e.Rssi + 100 > 75) ? Pos_color : Neg_color;

                                        blelist[i].Name = BLEName_trunc;
                                        blelist[i].Rssi = e.Rssi;
                                        blelist[i].Bar1 = Bar1_val;
                                        blelist[i].Bar2 = Bar2_val;
                                        blelist[i].Bar3 = Bar3_val;
                                        blelist[i].Bar4 = Bar4_val;
                                        blelist[i].Status = true;
                                        blelist[i].Chev_col = Chev_color;
                                        blelist[i].Name_col = Name_color;
                                        blelist[i].Uuid_col = Uuid_color;
                                    }
                                }
                            }

                        }
                    }
                }
                catch { }

            });
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var n = (BleList)e.Item;
            foreach (var dev in ScanDevices)
            {
                if (n.Uuid == dev.Uuid)
                {
                    ConnectDevice = dev;
                    ConnectDevice.ConnectToDevice();
                    isConnected = true;
                    ble.StopScanningForDevices();
                    await Navigation.PushAsync(new ControlBLEV(), false);

                }
            }
        }
        protected override void OnAppearing()
        {

            list_layout.IsVisible = false;
            ScanDevices.Clear();
            blelist.Clear();

            if (isConnected == true)
            {
                ConnectDevice.DisconnectDevice();
                isConnected = false;
            }
            list_layout.IsVisible = true;
            ble.StartScanningForDevices();
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            ble.StopScanningForDevices();
            base.OnDisappearing();
        }
        private void ListView_Refreshing(object sender, EventArgs e)
        {

            ble.StopScanningForDevices();
            ScanDevices.Clear();
            blelist.Clear();
            ble.StartScanningForDevices();
            listView.EndRefresh();

        }
        private async void LogFileViewer_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListLogViewer(), false);
        }

    }

}
