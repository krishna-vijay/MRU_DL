// Author: MyName
// Copyright:   Copyright 2024 Keysight Technologies
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

namespace RjioMRU.TestSteps
{
    [Display("CrossCheckParameters", Group: "RjioMRU.TestSteps", Description: "Insert a description here")]
    public class CrossCheckParameters : TestStep
    {
        #region Settings

        [Display("Product Serial Number", Order: 10, Group: "DR21 inputs", Description: "Match Product serial number from DR21")]
        public Input<string> SerialNumber { get; set; }
        [Display("PCB Serial Number", Order: 11, Group: "DR21 inputs", Description: "Match PCB serial number from DR21")]
        public Input<string> PCBSerialNumber { get; set; }

        [Display("MAC ID", Order: 12, Group: "DR21 inputs", Description: "Match MAC ID from DR21")]
        public Input<string> MacID { get; set; }

        [Display("Product ID", Order: 11, Group: "DR21 inputs", Description: "Match Product ID from Dr21")]
        public Input<string> ProductID { get; set; }
        [Display("RFB Serial Number 49DR Ch1", Order: 12, Group: "49DR CH1 inputs", Description: "Match RFB Serial Number from 49DR CH1")]
        public Input<string> RFB_SeriaNumber49DRCh1 { get; set; }

        [Display("RFB Serial Number 49DR Ch2", Order: 12, Group: "49DR CH2 inputs", Description: "Match RFB Serial Number from 49DR CH2")]
        public Input<string> RFB_SeriaNumber49DRCh2 { get; set; }

        [Display("Serial Number From MES", Group: "MES inputs", Order: 0)]
        public Input<string> Product_SerialnumberMES { get; set; }



        [Display("MAC ID From MES", Group: "MES inputs", Order: 2)]
        public Input<string> MacID_MES { get; set; }


        [Display("Product id From MES", Group: "MES inputs", Order: 4)]
        public Input<string> ProductID_MES { get; set; }


        [Display("PCB Serial Number(HSTB) From MES", Group: "MES inputs", Order: 6)]
        public Input<string> PCBSerialNumber_HSTB_MES { get; set; }

        [Display("RFB Serial Number From MES", Group: "MES inputs", Order: 10)]
        public Input<string> RFBSerialNumber_MES { get; set; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public CrossCheckParameters()
        {
            SerialNumber = new Input<string>();
            PCBSerialNumber = new Input<string>();
            ProductID = new Input<string>();
            MacID = new Input<string>();
            RFB_SeriaNumber49DRCh1 = new Input<string>();
            RFB_SeriaNumber49DRCh2 = new Input<string>();

            Product_SerialnumberMES = new Input<string>();
            MacID_MES = new Input<string>();
            ProductID_MES = new Input<string>();
            PCBSerialNumber_HSTB_MES = new Input<string>();
            RFBSerialNumber_MES = new Input<string>();
        }

                // ToDo: Set default values for properties / settings.


        public override void Run()
        {
            if (SerialNumber.Value.Trim()==Product_SerialnumberMES.Value.Trim()&&PCBSerialNumber.Value.Trim()==PCBSerialNumber_HSTB_MES.Value.Trim() &&ProductID.Value.Trim() ==ProductID_MES.Value.Trim()&&MacID.Value.Trim().ToUpper() ==MacID_MES.Value.Trim().ToUpper()&& RFBSerialNumber_MES.Value.Trim() == RFB_SeriaNumber49DRCh1.Value.Trim()&& RFBSerialNumber_MES.Value.Trim() == RFB_SeriaNumber49DRCh2.Value.Trim())
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
