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
    [Display("Clear Reports", Group: "RjioMRU.TestSteps", Description: "Insert a description here")]
    public class ClearReports : TestStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public ClearReports()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            CalibrationStep_CH1.StrChannelMeasurementsCh1 = new string[16] { ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";" };
            CalibrationStep_CH2.StrChannelMeasurementsCh2 = new string[16] { ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";" };
            GeneralFunctions.ChainTemperatureValuesCh1 = new string[16] { ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";" };
            GeneralFunctions.StrChannelMeasurementsCh2 = new string[16] { ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";", ";" };


            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
