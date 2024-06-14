// Author: MyName
// Copyright:   Copyright 2022 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using RjioODSC.Instruments;
using RjioODSC.Reporting;

namespace RjioODSC.TestSteps.RxSteps
{
    [Display("Rx Sensitivity", Group: "RjioODSC", Description: "Tracking till get \"Antenna,PRB,MCS,RSSI,SNR,TimingOffset,CRCFail\" next line expected to be the rx sensitivity values")]
    public class RxSensitivityCls : TestStep
    {
        #region Settings
        RxSensitivityPC rxSensitivity;
        private string rACCARDIP = "165";
        private int sectorID = 2;
        private int pRBNumber = 0;
        private int portID = 0;
        private double setPower = -55;
        private string logName_SerialNumber = "TejasSerialNumber";

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RxSensitivityCls()
        {
            //SerialNumber = new Input<string>();
            //PRBNumberSEt = new Input<int>();
            // ToDo: Set default values for properties / settings.
        }
        [Display("Rx Sensitivity", "PC Rx sensitivity", Order: 1)]
        public RxSensitivityPC RxSensitivity { get => rxSensitivity; set => rxSensitivity = value; }
        [Display("RAC CARD IP", "Enter RAC CARD IP address", Order: 5)]
        public string RACCARDIP { get => rACCARDIP; set => rACCARDIP = value; }

        [Display("Sector ID", "Enter Sector ID", Order: 10)]
        public int SectorID { get => sectorID; set => sectorID = value; }
        [Display("PRB Number", "Enter PRB Number", Order: 15)]
        public int PRBNumber { get => pRBNumber; set => pRBNumber = value; }

        [Display("Port ID Number", "Enter Port ID Number", Order: 20)]
        public int PortID { get => portID; set => portID = value; }
        [Display("Power set for Rx signal", "Enter Set Power at port", Order: 25)]
        public double SetPower { get => setPower; set => setPower = value; }


        //[Display("Serial Number ", "Enter Serial number of RRH", Order: 30)]

        // public Input<string> SerialNumber { get; set; }

        //[Display("PRB Number ", "Enter PRB number of waveform", Order: 30)]

        //public Input<int> PRBNumberSEt { get; set; }


        //string serialNumber = "";
        public override void Run()
        {
            try
            {
                // RxSensitivity.Open();
            }
            catch (Exception)
            {

                throw;
            }

            //string logTxtName = SerialNumber.Value + DateTime.Now.ToShortTimeString().Replace("/","_");
            string logTxtName = "12345_"+ DateTime.Now.ToShortTimeString().Replace(":", "_");
            string returnValue = RxSensitivity.GetRxSensitivity(RACCARDIP, SectorID, PRBNumber, PortID, SetPower, logTxtName);
            Log.Info("Rx Sensitivity data " + returnValue);
            //Antenna,PRB,MCS,RSSI,SNR,TimingOffset,CRCFail
            // 0,25,5,-122,-3,Invalid,100.00
            var rxSplitVal = returnValue.Split(',');
            if (rxSplitVal.Length < 7)
            {
                Log.Info("Invalid Values");
                UpgradeVerdict(Verdict.Fail);
                return;
            }
            else
            {
                TejasReport RxMeasureData = new TejasReport
                {
                    Frequency = "".ToString(),
                    MeasureName = "Rx",
                    ReportDate = DateTime.Today,
                    ACLR = "".ToString(),
                    AntennaNumber = "".ToString(),
                    EVM = "",
                    FreqError = "",
                    RxSensitivity0PRB_SetPRB = "RxSensitivity0PRB",
                    RxSensitivity0PRB_Antenna = rxSplitVal[0].ToString(),
                    RxSensitivity0PRB_PRB = rxSplitVal[1].ToString(),
                    RxSensitivity0PRB_MCS = rxSplitVal[2].ToString(),
                    RxSensitivity0PRB_RSSI = rxSplitVal[3].ToString(),
                    RxSensitivity0PRB_SNR = rxSplitVal[4].ToString(),
                    RxSensitivity0PRB_TIMING = rxSplitVal[5].ToString(),
                    RxSensitivity0PRB_BLER = rxSplitVal[6].ToString(),
                    TxPower = "TxPower"
                };

                Results.Publish(RxMeasureData);
            }
       

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
