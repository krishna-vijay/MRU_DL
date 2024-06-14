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
    [Display("Serial Number from MES", Group: "RjioMRU.TestSteps.MESSteps", Description: "Insert a description here")]
    public class GetSerialNumber : TestStep
    {
        #region Settings
        ClsMES _MESSerialNumber = null;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public GetSerialNumber()
        {
            // ToDo: Set default values for properties / settings.
        }

        public ClsMES MESSerialNumber { get => _MESSerialNumber; set => _MESSerialNumber = value; }

        public override void Run()
        {
           var serialnumber= MESSerialNumber.GetSerialNumber();
            Log.Info(serialnumber.Result);
             // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
