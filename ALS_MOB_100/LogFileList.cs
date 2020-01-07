namespace ALS_MOB_100
{
    class LogFileList
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public LogFileList(string path, string filename)
        {
            FileName = filename;
            Path = path;
        }
    }
}
