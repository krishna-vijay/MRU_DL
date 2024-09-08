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
using System.Windows.Forms;

namespace RjioMRU.TestSteps
{
    [Display("Serial Number Input", Group: "RjioMRU.TestSteps", Description: "Insert a description here")]
    public class SerialNumberInput : TestStep
    {
        #region Settings
        string serialNumber = "JITSAF1LIMRU00006";
        [Output]
        [Display("Serial Number", Order: 1)]
        public string SerialNumber { get => serialNumber; set => serialNumber = value; }
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SerialNumberInput()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            InputBox inputBox = new InputBox("Barcore Reader", "Please SCAN the QR/ Barcode");
            if (inputBox.ShowDialog() == DialogResult.OK)
            {
                string inputValue = inputBox.InputValue;
                SerialNumber = inputValue;
            }
            else
            {
                this.PlanRun.MainThread.Abort();
                Log.Info("User has cancelled the Barcode SCAN's operation");
            }
            MES_CSV.MRU_Serial_number = SerialNumber;

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
