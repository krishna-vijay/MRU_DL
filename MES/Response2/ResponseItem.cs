namespace RjioMRU.MES.Response2
{
    public class ResponseItem
    {
        public int serial_key { get; set; }
        public string serial_number { get; set; }
        public int part_key { get; set; }
        public string parent_part_number { get; set; }
        public string component_id { get; set; }
        public object id { get; set; }
        public int component_part_key { get; set; }
        public string ref_designator { get; set; }
        public int removed { get; set; }
        public int level { get; set; }
        public string path { get; set; }
        public bool cycle { get; set; }
        public string component_part_number { get; set; }
        public object HSTB { get; set; }
        public object RFFE { get; set; }
    }
}
