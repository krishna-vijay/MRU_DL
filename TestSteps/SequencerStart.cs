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
    [Display("SequencerStart", Group: "RjioMRU", Description: "Insert a description here")]
    public class SequencerStart : TestStep
    {
        #region Settings
        EXM_E6680A e6680a;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SequencerStart()
        {
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM ",Order:1)]
        public EXM_E6680A E6680a { get => e6680a; set => e6680a = value; }
       

        public override void Run()
        {
            E6680a.SetupSequencer();
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("Expected Power", Group: "RjioMRU", Description: "Insert a description here")]
    public class ExpectedPower : TestStep
    {
        #region Settings
        EXM_E6680A e6680a;
        double expecetedPower = 38;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public ExpectedPower()
        {
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM ", Order: 1)]
        public EXM_E6680A E6680a { get => e6680a; set => e6680a = value; }
        [Display("Expeced power", Order: 5)]
        public double ExpecetedPower { get => expecetedPower; set => expecetedPower = value; }

        public override void Run()
        {
            E6680a.SequenceExpectedPower(ExpecetedPower);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Read Sequencer Power", Group: "RjioMRU", Description: "Insert a description here")]
    public class ReadSeqPower : TestStep
    {
        #region Settings
        EXM_E6680A e6680a;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public ReadSeqPower()
        {
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM ", Order: 1)]
        public EXM_E6680A E6680a { get => e6680a; set => e6680a = value; }
         

        public override void Run()
        {
            Log.Info("Read Sequencer Power :"+E6680a.ReadSequencerPower().ToString());
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("Sequencer Meas Setup", Group: "RjioMRU", Description: "Insert a description here")]
    public class SeqSetup: TestStep
    {
        #region Settings
        EXM_E6680A e6680a;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SeqSetup()
        {
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM ", Order: 1)]
        public EXM_E6680A E6680a { get => e6680a; set => e6680a = value; }


        public override void Run()
        {
            E6680a.RestoreScreenDefaults();
            E6680a.TechModeSelect(EXM_E6680A.ModeSelect.SEQAN);
            E6680a.renameScreen("SEQ");
            E6680a.CreateNewScreen();
            E6680a.TechModeSelect(EXM_E6680A.ModeSelect.NR5G);
            E6680a.renameScreen("EVM");
            E6680a.SelectMeasurements(EXM_E6680A.LTEAFDD_MEASUREMENT_Type.EVM);
            E6680a.SelectInstScreen("SEQ");
            E6680a.SetupSequencer();
          //  E6680a.SequenceExpectedPower(-10);


            Log.Info("Read Sequencer Poer :" + E6680a.ReadSequencerPower().ToString());
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
