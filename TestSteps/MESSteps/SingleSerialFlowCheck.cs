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

namespace RjioMRU.TestSteps.MESSteps
{
    [Display("SingleSerialFlowCheck", Group: "RjioMRU.TestSteps.MESSteps", Description: "Insert a description here")]
    public class SingleSerialFlowCheck : TestStep
    {
        #region Settings
        ClsMES mesResource = null;
        string serialNumber = string.Empty;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SingleSerialFlowCheck()
        {
            // ToDo: Set default values for properties / settings.
        }

        public ClsMES MesResource { get => mesResource; set => mesResource = value; }
        [Display("Serial Number", Order: 1)]
        public string SerialNumber { get => serialNumber; set => serialNumber = value; }

        public override void Run()
        {
          var value1 =  MesResource.SingleSerialFlowCheck(SerialNumber);
            Log.Info("Single Serial Flow Check:  " + value1.Result.ToString());
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
