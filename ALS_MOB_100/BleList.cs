using System.ComponentModel;

namespace ALS_MOB_100
{
    public class BleList : INotifyPropertyChanged
    {
        private string names;
        public string Name
        {
            get { return names; }
            set
            {
                names = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }
        public string Uuid { get; set; }

        private int rssis;
        public int Rssi
        {
            get { return rssis; }
            set
            {
                rssis = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Rssi"));
            }
        }
        private string bar1s;
        public string Bar1
        {
            get { return bar1s; }
            set
            {
                bar1s = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Bar1"));
            }
        }

        private string bar2s;
        public string Bar2
        {
            get { return bar2s; }
            set
            {
                bar2s = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Bar2"));
            }
        }

        private string bar3s;
        public string Bar3
        {
            get { return bar3s; }
            set
            {
                bar3s = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Bar3"));
            }
        }

        private string bar4s;
        public string Bar4
        {
            get { return bar4s; }
            set
            {
                bar4s = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Bar4"));
            }
        }

        private bool statuss;
        public bool Status
        {
            get { return statuss; }
            set
            {
                statuss = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Status"));
            }
        }

        private string uuid_cols;
        public string Uuid_col
        {
            get { return uuid_cols; }
            set
            {
                uuid_cols = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Uuid_col"));
            }
        }

        private string name_cols;
        public string Name_col
        {
            get { return name_cols; }
            set
            {
                name_cols = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Name_col"));
            }
        }

        private string chev_cols;
        public string Chev_col
        {
            get { return chev_cols; }
            set
            {
                chev_cols = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Chev_col"));
            }
        }


        public BleList(string name, string uuid, int rssi, string bar1, string bar2, string bar3, string bar4, bool status, string uuid_col, string name_col, string chev_col)
        {

            Uuid = uuid;
            Name = name;
            Rssi = rssi;
            Bar1 = bar1;
            Bar2 = bar2;
            Bar3 = bar3;
            Bar4 = bar4;
            Status = status;
            Uuid_col = uuid_col;
            Name_col = name_col;
            Chev_col = chev_col;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
