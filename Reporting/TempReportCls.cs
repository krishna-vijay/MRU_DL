using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTap;

namespace RjioMRU.Reporting
{


    [Display("TestReport")]
    class TejasReport
    {
        [Display("Measure name")]
        public string MeasureName { get; set; }
        public static int serialNumberGlob = 1;
        [Display("Date")]
        public DateTime ReportDate { get; set; }

        //[Display("SerialNumber")]
        //public Int32 SerialNumber { get; set; }

        //[Display("Test Case Name")]
        //public string TestCaseName { get; set; }

        [Display("Antenna")]
        public string AntennaNumber { get; set; }

        [Display("Frequency")]
        public string Frequency { get; set; }


        [Display("TxPower")]
        public string TxPower { get; set; }

        [Display("EVM")]
        public string EVM { get; set; }

        [Display("FreqError")]
        public string FreqError { get; set; }

        [Display("ACLR")]
        public string ACLR { get; set; }


        [Display("Rx Sensitivity 0 PRB_SetPRB")]
        public string RxSensitivity0PRB_SetPRB { get; set; }

        [Display("Rx Sensitivity 0 PRB_Antenna")]
        public string RxSensitivity0PRB_Antenna { get; set; }


        [Display("Rx Sensitivity 0 PRB_PRB")]
        public string RxSensitivity0PRB_PRB { get; set; }

        [Display("Rx Sensitivity 0 PRB_MCS")]
        public string RxSensitivity0PRB_MCS { get; set; }

        [Display("Rx Sensitivity 0 PRB_RSSI")]
        public string RxSensitivity0PRB_RSSI { get; set; }


        [Display("Rx Sensitivity 0 PRB_SNR")]
        public string RxSensitivity0PRB_SNR { get; set; }

        [Display("Rx Sensitivity 0 PRB_TIMING")]
        public string RxSensitivity0PRB_TIMING { get; set; }

        [Display("Rx Sensitivity 0 PRB_BLER")]
        public string RxSensitivity0PRB_BLER { get; set; }




        public TejasReport()
        {
            ReportDate = DateTime.Today;
        }
    }




    [Display("Test Report Header")]
    public class TejasReportHeader
    {
        public string SwoNumber { get; set; }
        [Display("Test Location")]
        public string TestLocation { get; set; }
        [Display("Test Station Name")]
        public string TestStationName { get; set; }

        [Display("Tejas RRH Part Number")]
        public string TejasPartNumberRRH { get; set; }
        [Display("Tejas RRH Card Number")]
        public string TejasCARDNumberRRH { get; set; }

        [Display("Tejas RFPC Part Number")]
        public string TejasPartNuberRFPC { get; set; }

        [Display("Tejas RFPC card Number")]
        public string TejasCardNuberRFPC { get; set; }

        [Display("Tejas PAM1 Part Number")]
        public string TejasPartNumberPAM1 { get; set; }

        [Display("Tejas PAM1 Card Number")]
        public string TejasCardNumberPAM1 { get; set; }



        [Display("Tejas  PAM2 Pard Number")]
        public string TejasPartNumberPAM2 { get; set; }

        [Display("Tejas  PAM2 Card Number")]
        public string TejasCardNumberPAM2 { get; set; }

        [Display("Tejas  PAM3 Part Number")]
        public string TejasPartNumberPAM3 { get; set; }

        [Display("Tejas  PAM3 Card Number")]
        public string TejasCardNumberPAM3 { get; set; }


        [Display("Tejas  PAM4 Part Number")]
        public string TejasPartNumberPAM4 { get; set; }

        [Display("Tejas  PAM4 Card Number")]
        public string TejasCardNumberPAM4 { get; set; }

        [Display("Tejas LNA Part Number")]
        public string TejasPartNumberLNA { get; set; }

        [Display("Tejas LNA Card Number")]
        public string TejasCardNumberLNA { get; set; }

        [Display("Software Version")]
        public string SoftwaareVersion { get; set; }
        [Display("Operator Name")]
        public string OperatorName { get; set; }

        [Display("Shipment Approver")]
        public string ShipmentApprover { get; set; }

        [Display("Tested Date")]
        public string TestedDate { get; set; }

        [Display("Approved Date")]
        public string ApprovedDate { get; set; }

        public TejasReportHeader()
        {

        }
    }
}
