// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
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
    [Display("DummyTEsting", Group: "RjioMRU.TestSteps", Description: "Insert a description here")]
    public class DummyTEsting : TestStep
    {
        #region Settings
        EXM_E6680A exm;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DummyTEsting()
        {
            // ToDo: Set default values for properties / settings.
        }

        public EXM_E6680A Exm { get => exm; set => exm = value; }

        public override void Run()
        {
           Log.Info( Exm.trialGetSeqPower());
           Log.Info( Exm.TrailmeasureModulation());
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
