namespace ALS_MOB_100
{
    public class CharacteristicsList
    {

        public string Name { get; set; }
        public string Uuid { get; set; }
        public bool Isread { get; set; }
        public bool Iswrite { get; set; }
        public bool Isnotify { get; set; }
        public CharacteristicsList(string name, string uuid, bool isread, bool iswrite, bool isnotify)
        {
            Isread = isread;
            Iswrite = iswrite;
            Isnotify = isnotify;
            Uuid = uuid;
            Name = name;
        }
        //public string Uuid { get; set; }
        //public bool Isread { get; set; }
        //public bool Iswrite { get; set; }
        //public bool Isnotify { get; set; }
        //public CharacteristicsList(string uuid, bool isread, bool iswrite, bool isnotify)
        //{
        //    Isread = isread;
        //    Iswrite = iswrite;
        //    Isnotify = isnotify;
        //    Uuid = uuid;
        //}

    }
}
