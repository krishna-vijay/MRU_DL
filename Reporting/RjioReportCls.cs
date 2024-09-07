using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTap;

namespace RjioMRU
{
    [Display("RjioMRUReport")]
    public class RjioReportCls
    {
        public static bool reportGenerated = false;
        [Display("SWVersion")]
        public string SWVersion { get; set; }

        [Display("EMPID")]
        public string EMPID { get; set; }

        [Display("testStage")]
        public string testStage { get; set; }

        [Display("MRU")]
        public string MRU { get; set; }

        [Display("TestStartTime")]
        public string TestStartTime { get; set; }

        [Display("TestEndTime")]
        public string TestEndTime { get; set; }

        [Display("TotalTestTime")]
        public string TotalTestTime { get; set; }

        [Display("MACID1")]
        public string MACID1 { get; set; }

        [Display("MACID2")]
        public string MACID2 { get; set; }

        [Display("MACID3")]
        public string MACID3 { get; set; }
        [Display("MACID4")]
        public string MACID4 { get; set; }

        [Display("ProductSerialNumber")]
        public string ProductSerialNumber { get; set; }

        [Display("PCB Seril Number")]
        public string PcbSerialNumber { get; set; }

        [Display("Product ID")]
        public string ProdID{ get; set; }

        [Display("Measurements")]
        public string Measurements { get; set; }
        
        [Display("DUT Number")]
        public string DUTN { get; set; }
        [Display("Test Result")]
        public bool testResult { get; set; }

    }
    //[Display("Channel Cal Reports")]
    //public class CalEndMeasurements
    //{
    //    [Display("frequency")]
    //    public double frequency { get; set; }

    //    [Display("channelNumber")]
    //    public int channelNumber { get; set; }

    //    [Display("EVM")]
    //    public Double EVM { get; set; }

    //    [Display("ACLR_POS1")]
    //    public Double ACLR_POS1 { get; set; }

    //    [Display("ACLR_POS2")]
    //    public Double ACLR_POS2 { get; set; }

    //    [Display("ACLR_NEG1")]
    //    public double ACLR_NEG1 { get; set; }

    //    [Display("ACLR_NEG2")]
    //    public double ACLR_NEG2 { get; set; }

    //    [Display("FrequencyError")]
    //    public double FrequencyError { get; set; }

    //    [Display("ChannelPower")]
    //    public double ChannelPower { get; set; }
    //}
}
