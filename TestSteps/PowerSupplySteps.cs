// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using RjioMRU.Instruments;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RjioMRU.TestSteps
{
    [Display("Power supply On Off", Group: "RjioMRU.PowerSupply", Description: "Insert a description here")]
    public class PowerSupplySteps : TestStep
    {
        #region Settings
        Agilent_N5767A_PS agilent_N5767A_powerSupply;
        bool PowerOn = false;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public PowerSupplySteps()
        {
            // ToDo: Set default values for properties / settings.
        }

        public Agilent_N5767A_PS Agilent_N5767A_powerSupply { get => agilent_N5767A_powerSupply; set => agilent_N5767A_powerSupply = value; }

        [Display("Switch on Power supply?")]
        public bool PowerOn1 { get => PowerOn; set => PowerOn = value; }

        public override void Run()
        {
            Agilent_N5767A_powerSupply.OnPowerSupply(PowerOn1);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.
            UpgradeVerdict(Verdict.Pass);

            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
