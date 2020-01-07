using System.ComponentModel;

namespace ALS_MOB_100
{
    class HistoricalRes : INotifyPropertyChanged
    {
        public string Value { get; set; }
        public string Verdict { get; set; }
        public string Id { get; set; }
        public string BgColor { get; set; }
        public string Temperature { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Altitude { get; set; }

        private string SN;
        public string SerialNumber
        {
            get { return SN; }

            set
            {
                SN = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SerialNumber"));
            }
        }

        public string Device_SN { get; set; }
        public string Acq_Date { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public bool Saved { get; set; }
        public HistoricalRes(string device_SN, string acq_date, string result, string value, string id, string bgcolor, string temperature, string serialnumber, bool saved)
        {
            Value = result;
            Verdict = value;
            Id = id;
            BgColor = bgcolor;
            Temperature = temperature;
            SerialNumber = serialnumber;
            Device_SN = device_SN;
            Acq_Date = acq_date;
            Date = Acq_Date.Substring(0, 10);
            Time = Acq_Date.Substring(Acq_Date.Length - 8);
            Saved = saved;


        }

        public HistoricalRes() { }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

    }
}
