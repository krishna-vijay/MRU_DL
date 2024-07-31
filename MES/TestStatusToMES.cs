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

namespace RjioMRU.MES
{
    [Display("Test Status To MES", Group: "RjioMRU.MES", Description: "Executes at the end of the test plan")]
    public class TestStatusToMES : TestStep
    {
        #region Settings
        ClsMES mesResource = null;
        public ClsMES MesResource { get => mesResource; set => mesResource = value; }
        [Display("Server URL", Order: 1)]
        public Input<string> ServerURL { get; set; } //= "http://42qconduituat2.42-q.com:18003/conduit";
        Input<string> serialNumber;
        public Input<string> SerialNumber { get => serialNumber; set => serialNumber = value; }
      //  public Input<string> ClientID { get; set; }// = "p5547dc2_uat";
      //  public Input<string> EmployeeID { get; set; }// = "62153666";
       // public Input<string> stageID { get; set; }
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public TestStatusToMES()
        {
            SerialNumber = new Input<string>();
            ServerURL = new Input<string>();
           // ClientID = new Input<string>();
           // EmployeeID = new Input<string>();
          //  stageID = new Input<string>();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
        public override void PostPlanRun()
        {
            if (PlanRun.Verdict == Verdict.Pass)
            {
                var value1 = MesResource.SingleSerialPass(ServerURL.Value, MesResource.ClientID, MesResource.Employee, SerialNumber.Value,MesResource.Station);
                Log.Info("Single Serial Pass:  " + value1.Result.ToString());
            }
            else
            {
                var value1 = MesResource.SingleSerialFail(ServerURL.Value, MesResource.ClientID,MesResource.Employee, SerialNumber.Value,MesResource.Station);
                Log.Info("Single Serial Fail:  " + value1.Result.ToString());
            }
        }
    }
}
