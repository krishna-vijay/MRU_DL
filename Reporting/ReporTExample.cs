// Author: MyName
// Copyright:   Copyright 2022 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using RjioMRU.Reporting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RjioMRU
{
    [Display("ReporTExample", Group: "RjioMRU", Description: "Insert a description here")]
    public class ReporTExample : TestStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public ReporTExample()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {


            //CalEndMeasurements[] ChResults = new CalEndMeasurements[4];
            //ChResults[0] = new CalEndMeasurements() { ChannelPower = 38.18926872, EVM = 2.939949751, ACLR_NEG2 = -52.47422409, ACLR_NEG1 = -51.52714157, ACLR_POS1 = -57.32237244, ACLR_POS2 = -52.04647827, FrequencyError = 6.824662685, channelNumber = 1, frequency = 3.54999e9 };
            //ChResults[1] = new CalEndMeasurements() { ChannelPower = 38.03317814, EVM = 3.075749874, ACLR_NEG2 = -48.73224258, ACLR_NEG1 = -49.93373871, ACLR_POS1 = -56.68915558, ACLR_POS2 = -51.27249527, FrequencyError = 7.337563515, channelNumber = 2, frequency = 3.54999e9 };
            //ChResults[2] = new CalEndMeasurements() { ChannelPower = 38.21498688, EVM = 3.032058477, ACLR_NEG2 = -49.47291565, ACLR_NEG1 = -50.31784058, ACLR_POS1 = -57.43385315, ACLR_POS2 = -50.57795334, FrequencyError = 5.535184383, channelNumber = 3, frequency = 3.54999e9 };
            //ChResults[3] = new CalEndMeasurements() { ChannelPower = 38.16527329, EVM = 3.003912687, ACLR_NEG2 = -51.31388855, ACLR_NEG1 = -49.38345718, ACLR_POS1 = -57.56851959, ACLR_POS2 = -51.48935318, FrequencyError = 6.422584534, channelNumber = 4, frequency = 3.54999e9 };


            RjioReportCls tempRep = new RjioReportCls
            {
                BootIndex = "0",
                DSA1 = "0x2D",
                DSA2 = "0x27",
                DSA3 = "0x27",
                DSA4 = "0x25",
                EMPID = "1123",
                MACID1 = "A0:73:FC:00:11:BA",
                MACID2 = "A0:73:FC:00:11:BB",
                MACID3 = "A0:73:FC:00:11:BC",
                MRU = "MRU",
                PCBSRNUM = "JITSAODCBIIBB00006",
                PID = "PID",
                PSN = "PSN",
                RFBFWVer = "1.0",
                RFBHWVer = "B",
                RFBSLNUM = "JITSAODCDIRFB00012",
                SWVersion = "SWVersion",
                TestStartTime = "13:33:41",
                TestEndTime = "13:41:32",
                testStage = "TestStage",
                TotalTestTime = "00:07:51",
                Measurements = "38.18926872,2.939949751, -52.47422409, -51.52714157, -57.32237244, -52.04647827, 6.824662685, 1, 3.54999e9 "
                //Ch1Measurements = "38.18926872,2.939949751, -52.47422409, -51.52714157, -57.32237244, -52.04647827, 6.824662685, 1, 3.54999e9 ",
                //Ch2Measurements = "38.03317814, 3.075749874, -48.73224258, -49.93373871,  -56.68915558, -51.27249527, 7.337563515,  2, 3.54999e9 ",
                //Ch3Measurements = "38.21498688,3.032058477, -49.47291565, -50.31784058,  -57.43385315, -50.57795334, 5.535184383, 3,  3.54999e9 ",
                //Ch4Measurements = "38.16527329, 3.003912687, -51.31388855, -49.38345718, -57.56851959, -51.48935318, 6.422584534,  4, 3.54999e9 "
            };


            Results.Publish<RjioReportCls>("Report", tempRep);
            /*,
             


PASS
    	
*/

            // ToDo: Add test case code.

            RunChildSteps(); //If the step supports child steps.



            // Results.Publish(new TempReportCls {  columnName= "Values" , vaues = Dvalues });
            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}