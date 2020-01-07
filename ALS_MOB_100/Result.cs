namespace ALS_MOB_100
{
    public class Result
    {
        public string equipment { get; set; }
        public string Acq_Date { get; set; }
        public string SerialNumber { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public string Verdict { get; set; }
        public string Temperature { get; set; }

        public string Data
        {
            get { return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", equipment, Acq_Date, SerialNumber, Id, Value, Verdict, Temperature); }
        }

        public override string ToString()
        {
            return string.Format("Date: {0}, Acquisition: {1}", Acq_Date.Substring(0, 10), Id);
        }
        public Result() { }
    }
    public class Rootobject
    {
        public Result[] results_acq { get; set; }
    }
}
