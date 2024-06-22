// Author: MyName
// Copyright:   Copyright 2024 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using RjioMRU.MES;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RjioMRU.TestSteps.MESSteps
{
    //[Display("Serial Number from MES", Group: "RjioMRU.TestSteps.MESSteps", Description: "Insert a description here")]
    //public class GetSerialNumber : TestStep
    //{
    //    #region Settings
    //    ClsMES _MESSerialNumber = null;
    //    string serialnumber = string.Empty; 
    //    // ToDo: Add property here for each parameter the end user should be able to change
    //    #endregion

    //    public GetSerialNumber()
    //    {
    //        // ToDo: Set default values for properties / settings.
    //    }

    //    public ClsMES MESSerialNumber { get => _MESSerialNumber; set => _MESSerialNumber = value; }

    //    [Display("Serial Number", Order: 2)]
    //    public string Serialnumber { get => serialnumber; set => serialnumber = value; }

    //    public override void Run()
    //    {
    //       var serialnumberValue= MESSerialNumber.GetDataFromMac_ProductID(Serialnumber);
    //       Log.Info("MAC Number -> "+serialnumberValue.Result.MacAddress + " " + serialnumberValue.Result.ProductCode);

    //        //If success : true from MES read response/ Continue the test, else Stop test Plan
    //        if (serialnumberValue.Result.ToString().Contains("success : true"))
    //        {
    //            //please decode the serialnumberValue and get the serial number

    //            UpgradeVerdict(Verdict.Pass);
    //            // procee to the test plan execution
    //        }
    //        else
    //        {
    //            UpgradeVerdict(Verdict.Fail);
    //            //abort the OPENTAP plan
    //            this.PlanRun.MainThread.Abort();
    //        }
    //        // ToDo: Add test case code.
    //        RunChildSteps(); //If the step supports child steps.

    //        // If no verdict is used, the verdict will default to NotSet.
    //        // You can change the verdict using UpgradeVerdict() as shown below.
    //        // UpgradeVerdict(Verdict.Pass);
    //    }
    //}
}
