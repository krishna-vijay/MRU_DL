// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;

namespace RjioMRU
{
    [Display("Restore Default Screens", Group: "RjioMRU.E6680A", Description: "Insert a description here")]
    public class E6680A_DefaultScreens : TestStep
    {
        #region Settings
        EXM_E6680A e6680Ainstrumet;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public E6680A_DefaultScreens()
        {
            // ToDo: Set default values for properties / settings.
        }

        public EXM_E6680A E6680Ainstrumet { get => e6680Ainstrumet; set => e6680Ainstrumet = value; }

        public override void Run()
        {
            E6680Ainstrumet.RestoreScreenDefaults();
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Seq and EVM Screens", Group: "RjioMRU.E6680A", Description: "Insert a description here")]
    public class E6680A_Seq_EVMScreens : TestStep
    {
        #region Settings
        EXM_E6680A e6680Ainstrumet;
        private string scpFilePath = @"D:\MRU DL Test\TM3.1a_pdcchRemoved.scp";
        private double frequency = 3.55e9;
        double seqPeakPower = 13;
        double seqExpectedPower = 8;
        double inputTriggerLevel = 0;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion
        [Display("Recall SCP File", Order: 1)]
        public string ScpFilePath { get => scpFilePath; set => scpFilePath = value; }
        public E6680A_Seq_EVMScreens()
        {
            // ToDo: Set default values for properties / settings.
        }

        public EXM_E6680A E6680Ainstrumet { get => e6680Ainstrumet; set => e6680Ainstrumet = value; }
        [Display("Frequency ", Order: 4)]
        [Unit("Hz", true)]
        public double Frequency { get => frequency; set => frequency = value; }
        [Display("Expected Power", Order: 5, Description: "Enter nominal expeceted power value")]
        public double SeqExpectedPower { get => seqExpectedPower; set => seqExpectedPower = value; }

        [Display("Peak Power ", Order: 10, Description: "Enter the Peak Power value")]
        public double SeqPeakPower { get => seqPeakPower; set => seqPeakPower = value; }

        [Display("Input Trigger Value", Order: 15, Description: "Enter trigger level value ")]
        public double InputTriggerLevel { get => inputTriggerLevel; set => inputTriggerLevel = value; }
        public override void Run()
        {
            E6680Ainstrumet.TechModeSelect(EXM_E6680A.ModeSelect.SEQAN);
            E6680Ainstrumet.renameScreen("SEQ");
            E6680Ainstrumet.CreateNewScreen();
            E6680Ainstrumet.TechModeSelect(EXM_E6680A.ModeSelect.NR5G);
            E6680Ainstrumet.renameScreen("EVM");
            E6680Ainstrumet.SelectMeasurements(EXM_E6680A.LTEAFDD_MEASUREMENT_Type.EVM);
            E6680Ainstrumet.MeasureContinues(false);
          

            E6680Ainstrumet.RecallSCPFile(ScpFilePath);
            E6680Ainstrumet.TrackEVM();
            E6680Ainstrumet.SetFrequency(Frequency);
            E6680Ainstrumet.CreateNewScreen();
            E6680Ainstrumet.TechModeSelect(EXM_E6680A.ModeSelect.NR5G);
            E6680Ainstrumet.renameScreen("ACP");
            E6680Ainstrumet.SelectMeasurements(EXM_E6680A.LTEAFDD_MEASUREMENT_Type.ACP);
            E6680Ainstrumet.MeasureContinues(true);
            E6680Ainstrumet.ACPTriggerSource(EXM_E6680A.TriggerSourceEnum.IMMediate);

            E6680Ainstrumet.SetACPNoiseCorrection(true);
            E6680Ainstrumet.SetGateOffOn(true);
            E6680Ainstrumet.SetGateSource("RFB");
            E6680Ainstrumet.SetGateDelay(0);
            E6680Ainstrumet.SetGateLength(2e-3);
            E6680Ainstrumet.ACPRefValue(45);
            E6680Ainstrumet.ACP_GATESource_ABs_Trig_Level(5);
            // E6680E.RecallStateFile_SCreen(stateFilePath: StateFilePath);
            // E6680E.renameScreen("EVM");

            E6680Ainstrumet.SetFrequency(Frequency);
            E6680Ainstrumet.ViewOnlySummary();
            E6680Ainstrumet.SelectInstScreen("SEQ");
            E6680Ainstrumet.SetupSequencer();

            E6680Ainstrumet.MeasureContinues(false);

            E6680Ainstrumet.TriggerBurstSelect();
            E6680Ainstrumet.TriggerLevel(InputTriggerLevel);
            E6680Ainstrumet.SequencePeakPower(SeqPeakPower);
            E6680Ainstrumet.SequenceExpectedPower(SeqExpectedPower);
            E6680Ainstrumet.SequenceAcquireSetup("636666", SeqPeakPower, 1, InputTriggerLevel);
            E6680Ainstrumet.seqSourceFrequency(Frequency);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("Select Screen", Group: "RjioMRU.E6680A", Description: "Insert a description here")]
    public class E6680A_SelectScreen : TestStep
    {
        #region Settings
        EXM_E6680A e6680Ainstrumet;
        private string screenName = "EVM";

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion
        [Display("Recall SCP File", Order: 1)]
        public string ScreenName { get => screenName; set => screenName = value; }
        public E6680A_SelectScreen()
        {
            // ToDo: Set default values for properties / settings.
        }

        public EXM_E6680A E6680Ainstrumet { get => e6680Ainstrumet; set => e6680Ainstrumet = value; }


        public override void Run()
        {

            E6680Ainstrumet.SelectInstScreen(ScreenName);

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }




}
