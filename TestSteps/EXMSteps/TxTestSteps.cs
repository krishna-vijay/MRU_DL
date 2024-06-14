// Author: MyName
// Copyright:   Copyright 2022 Keysight Technologies
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
using RjioODSC.Instruments;
using RjioODSC.Reporting;

namespace RjioODSC
{
    [Display("Tx Radio Direction", Group: "RjioODSC", Description: "Insert a description here")]
    public class TxRadioDirection : TestStep
    {
        EXM_E6680A e6680_Inst;
        EXM_E6680A.Direction rDirection;
        #region Settings

        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680_Inst { get => e6680_Inst; set => e6680_Inst = value; }
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public TxRadioDirection()
        {
            // ToDo: Set default values for properties / settings.
        }

        [Display("Radio Direction", Description: "Select Radio Direction", Order: 2)]
        public EXM_E6680A.Direction RDirection { get => rDirection; set => rDirection = value; }

        public override void Run()
        {
            E6680_Inst.TxradioDirection(RDirection);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("Tx Ref Level", Group: "RjioODSC", Description: "Insert a description here")]
    public class TxRefLevel : TestStep
    {
        EXM_E6680A e6680_Inst;
        int refLevel;
        int windowSelect;
        #region Settings

        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680_Inst { get => e6680_Inst; set => e6680_Inst = value; }
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public TxRefLevel()
        {
            // ToDo: Set default values for properties / settings.
        }

        [Display("Ref Level", Description: "Enter Ref Level", Order: 2)]

        public int RefLevel { get => refLevel; set => refLevel = value; }
        [Display("Window number", Description: "Enter window number", Order: 3)]
        public int WindowSelect { get => windowSelect; set => windowSelect = value; }

        public override void Run()
        {
            E6680_Inst.TxRefLevel(WindowSelect, RefLevel);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("Tx Scale/Div", Group: "RjioODSC", Description: "Insert a description here")]
    public class TxScalePerDivision : TestStep
    {
        EXM_E6680A e6680_Inst;
        double scale_Div;
        int windowSelect;
        #region Settings

        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680_Inst { get => e6680_Inst; set => e6680_Inst = value; }
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public TxScalePerDivision()
        {
            // ToDo: Set default values for properties / settings.
        }

        [Display("Scalse/Div", Description: "Enter Ref Level", Order: 2)]

        public double Scale_Div { get => scale_Div; set => scale_Div = value; }

        [Display("Window number", Description: "Enter window number", Order: 3)]
        public int WindowSelect { get => windowSelect; set => windowSelect = value; }


        public override void Run()
        {
            E6680_Inst.TxScalePerDivision(WindowSelect, Scale_Div);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Trigger Type", Group: "RjioODSC", Description: "Insert a description here")]
    public class TriggerType : TestStep
    {
        #region Settings
        EXM_E6680A e6680A;
        EXM_E6680A.TriggerTypeEnum triggerTypeEnum;

        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }

        [Display("Trigger Type", Description: "Select trigger type from options", Order: 2)]
        public EXM_E6680A.TriggerTypeEnum TriggerTypeEnum { get => triggerTypeEnum; set => triggerTypeEnum = value; }


        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public TriggerType()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.SetTriggerType(TriggerTypeEnum);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("ARB State", Group: "RjioODSC", Description: "Use to switch On or Off of ARB State")]
    public class ARBState : TestStep
    {
        #region Settings
        bool putOn;
        EXM_E6680A e6680A;
        [Display("EXM", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }

        [Display("ARB state On?", Order: 2, Description: "Check the box for ARB state ON")]
        public bool PutOn { get => putOn; set => putOn = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public ARBState()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.ARBState(PutOn);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Create Window", Group: "RjioODSC", Description: "Use to create new measurement window")]
    public class CreateWindow : TestStep
    {
        #region Settings
        EXM_E6680A e6680A { get; set; }


        [Display("EXM", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public CreateWindow()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.CreateNewScreen();
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Freq Ref Input", Group: "RjioODSC", Description: "Use to select freq referance input")]
    public class FreqRefInpuf : TestStep
    {
        #region Settings
        EXM_E6680A e6680A;
        EXM_E6680A.FreqRefInputEnum freqRefInput;

        [Display("EXM", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }

        [Display("Freq Ref", Description: "Select freq Ref type from options", Order: 2)]
        public EXM_E6680A.FreqRefInputEnum FreqRefInput { get => freqRefInput; set => freqRefInput = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public FreqRefInpuf()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.FreqRefInput(FreqRefInput);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Global Center Freq", Group: "RjioODSC", Description: "Insert a description here")]
    public class GlobalCenterFreq : TestStep
    {
        #region Settings
        EXM_E6680A e6680AInst;
        bool globalCentFreq;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public GlobalCenterFreq()
        {
            E6680AInst = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        [Display("EXM", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680AInst { get => e6680AInst; set => e6680AInst = value; }


        [Display("Global Cent freq", Description: "Check box for On", Order: 2)]
        public bool GlobalCentFreq { get => globalCentFreq; set => globalCentFreq = value; }

        public override void Run()
        {
            E6680AInst.GlobalCenterFrequency(GlobalCentFreq);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("InitiateMeasurements", Group: "RjioODSC", Description: "Insert a description here")]
    public class InitiateMeasurements : TestStep
    {
        #region Settings
        EXM_E6680A e6680A;
        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public InitiateMeasurements()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.InitiateMeasurement();
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Measure ACP", Group: "RjioODSC", Description: "Insert a description here")]
    public class MeasureACP : TestStep
    {
        #region Settings
        EXM_E6680A e6680A;

        [Display("Input frequency Value")]
        // Properties defined using the Input generic class will accept value from other 
        // test steps with properties that have been marked with the Output attribute.
        public Input<double> InputValue { get; set; }


        [Display("Input port Value")]
        // Properties defined using the Input generic class will accept value from other 
        // test steps with properties that have been marked with the Output attribute.
        public Input<int> InputValuePort { get; set; }


        [Display("EXM", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public MeasureACP()
        {

            InputValue = new Input<double>();
            InputValuePort = new Input<int>();

            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            if (InputValue == null) throw new ArgumentException();
            if (InputValuePort == null) throw new ArgumentException();
            var ACPResult = E6680A.measureACP();
            //foreach (var item in ACPResult)
            //{
            //    Log.Info(item);
            //}

            TejasReport acpMeasureData = new TejasReport
            {
                Frequency = InputValue.ToString(),
                MeasureName = "ACP",
                ReportDate = DateTime.Today,
                ACLR = ACPResult[4].ToString(),
                AntennaNumber = InputValuePort.ToString(),
                EVM = "EVM",
                FreqError = "FreqError",
                RxSensitivity0PRB_SetPRB = "RxSensitivity0PRB",
                RxSensitivity0PRB_Antenna = "",
                RxSensitivity0PRB_PRB="",
                RxSensitivity0PRB_MCS = "",
                RxSensitivity0PRB_RSSI="",
                RxSensitivity0PRB_SNR="",
                RxSensitivity0PRB_TIMING = "",
                RxSensitivity0PRB_BLER = "",
                TxPower = "TxPower"
            };

            Results.Publish(acpMeasureData);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Measure Continues", Group: "RjioODSC", Description: "Insert a description here")]
    public class MeasureContinues : TestStep
    {
        #region Settings
        bool isContinues;
        EXM_E6680A e6680A;
        [Display("EXM", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }

        [Display("Measure Continues?", Order: 2, Description: "Check the box for continues measurements")]
        public bool IsContinues { get => isContinues; set => isContinues = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public MeasureContinues()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.MeasureContinues(IsContinues);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Measure Modulation", Group: "RjioODSC", Description: "Insert a description here")]
    public class MeasureModulation : TestStep
    {
        #region Settings
        EXM_E6680A e6680A;
        [Display("Input frequency Value")]
        // Properties defined using the Input generic class will accept value from other 
        // test steps with properties that have been marked with the Output attribute.
        public Input<double> InputValue { get; set; }


        [Display("Input port Value")]
        // Properties defined using the Input generic class will accept value from other 
        // test steps with properties that have been marked with the Output attribute.
        public Input<int> InputValuePort { get; set; }

        [Display("EXM", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public MeasureModulation()
        {


            InputValue = new Input<double>();
            InputValuePort = new Input<int>();

            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            if (InputValue == null) throw new ArgumentException();
            if (InputValuePort == null) throw new ArgumentException();
            var ModResult = E6680A.measureModulation();

            TejasReport MOdulationMeasureData = new TejasReport
            {
                Frequency = InputValue.ToString(),
                MeasureName = "EVM",
                ReportDate = DateTime.Today,
                ACLR = "ACLR",
                AntennaNumber = InputValuePort.ToString(),
                EVM = ModResult[0].ToString(),
                FreqError = ModResult[12].ToString(),
                RxSensitivity0PRB_SetPRB = "RxSensitivity0PRB",
                
                TxPower = ModResult[29].ToString()

            };
            Results.Publish(MOdulationMeasureData);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Pulse RF Blank", Group: "RjioODSC", Description: "Use to change Pulse/RF Blank value")]
    public class PulseRFBlank : TestStep
    {
        #region Settings
        EXM_E6680A.PulseRFBlankEnum pulseRFBlank;
        EXM_E6680A e6680A;

        [Display("EXM", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }
        [Display("Pulse/RFBlank", Description: "Select from dropdown", Order: 2)]
        public EXM_E6680A.PulseRFBlankEnum PulseRFBlankEunm { get => pulseRFBlank; set => pulseRFBlank = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public PulseRFBlank()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.SetPulseRFBlank(PulseRFBlankEunm);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("RB Auto Detect", Group: "RjioODSC", Description: "Insert a description here")]
    public class RBAutoDetect : TestStep
    {
        #region Settings
        EXM_E6680A e6680AInst;
        bool rbAutoDetect;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RBAutoDetect()
        {
            E6680AInst = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680AInst { get => e6680AInst; set => e6680AInst = value; }

        [Display("RB Auto Detect?", Description: "Check box for On", Order: 2)]
        public bool RbAutoDetect { get => rbAutoDetect; set => rbAutoDetect = value; }

        public override void Run()
        {
            E6680AInst.RBAutoDetect(RbAutoDetect, 0);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Rename Screen", Group: "RjioODSC", Description: "Use to change the screen names")]
    public class RenameScreen : TestStep
    {
        #region Settings
        string screenName;

        EXM_E6680A e6680A;
        [Display("EXM", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }
        [Display("Screen Name", Order: 2, Description: "Enter Screen Name")]
        public string ScreenName { get => screenName; set => screenName = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RenameScreen()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.renameScreen(ScreenName);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Restore Default Screens", Group: "RjioODSC", Description: "Use to restore the default screens")]
    public class RestoreDefaultScreens : TestStep
    {
        #region Settings
        EXM_E6680A e6680A { get; set; }

        [Display("EXM", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RestoreDefaultScreens()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.RestoreScreenDefaults();
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Select Instrument Screen", Group: "RjioODSC", Description: "Use to select the opened other instrument screens")]
    public class SelectInstrumentScreen : TestStep
    {
        #region Settings
        EXM_E6680A e6680A;

        [Display("EXM", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }

        [Display("Screen Name", Description: "Enter the screen name to select", Order: 2)]
        public string ScreenName { get => screenName; set => screenName = value; }

        string screenName;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SelectInstrumentScreen()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.SelectInstScreen(screenName);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("LTE Mode", Group: "RjioODSC", Description: "Use to select LTE mode")]
    public class SelectLTE : TestStep
    {
        #region Settings
        EXM_E6680A e6680A;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SelectLTE()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }

        public override void Run()
        {
            E6680A.TechModeSelect(EXM_E6680A.ModeSelect.LTEAFDD);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("Measurement select", Group: "RjioODSC", Description: "Use to select the measurement type")]
    public class SelectMeasurement : TestStep
    {
        #region Settings
        EXM_E6680A e6680A;
        EXM_E6680A.LTEAFDD_MEASUREMENT_Type lTEAFDD_MEASUREMENT_Type;

        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }

        [Display("LET MEASUREMENT TYPE", Order: 2, Group: "MEASUREMENT CONFIGURATION")]
        public EXM_E6680A.LTEAFDD_MEASUREMENT_Type LTEAFDD_MEASUREMENT_Type { get => lTEAFDD_MEASUREMENT_Type; set => lTEAFDD_MEASUREMENT_Type = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SelectMeasurement()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.SelectMeasurements(LTEAFDD_MEASUREMENT_Type);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Band width", Group: "RjioODSC", Description: "Use to select the RF band width")]
    public class SetBandwidth : TestStep
    {
        #region Settings
        EXM_E6680A.LTE_BANDWIDTH lteBandwidth;
        // ToDo: Add property here for each parameter the end user should be able to change
        EXM_E6680A e6680A { get; set; }

        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }
        [Display("LTE BANDWIDTH", Description: "Select LTE Bandwidth", Order: 2)]
        public EXM_E6680A.LTE_BANDWIDTH LteBandwidth { get => lteBandwidth; set => lteBandwidth = value; }
        #endregion

        public SetBandwidth()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.SetBandwidth(LteBandwidth);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("External Power Loss", Group: "RjioODSC", Description: "Use to change the external power loss in dB")]
    public class SetExternalPowerLoss : TestStep
    {
        #region Settings
        double externalPowerLoss = double.NaN;
        EXM_E6680A e6680A;
        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }

        [Display("External Power Loss", Description: "Enter external power loss in dB", Order: 2)]
        public double ExternalPowerLoss { get => externalPowerLoss; set => externalPowerLoss = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SetExternalPowerLoss()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.SetExternalPowerLoss(ExternalPowerLoss);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("Center Frequency", Group: "RjioODSC", Description: "Use to change the center frequency")]
    public class SetFrequency : TestStep
    {
        #region Settings
        EXM_E6680A e6680A;
        double frequency = double.NaN;

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SetFrequency()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        //[Output]
        //[Display("Output Value")]
        //public double OutputValue { get; private set; }


        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }

        [Output]
        [Display("Carrier Frequency", Order: 2, Description: "Enter the center frequency")]
        [Unit("Hz", UseEngineeringPrefix: true)]

        public double Frequency { get => frequency; set => frequency = value; }

        public override void Run()
        {
            //  OutputValue = 0.1;
            E6680A.SetFrequency(Frequency);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("Gate On Off", Group: "RjioODSC", Description: "Select Gate On Off.")]
    public class SetGateOnOff : TestStep
    {
        #region Settings
        bool putOnGate;
        EXM_E6680A e6680A;

        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }
        [Display("Gate On Off", Description: "Check the box to On the gate", Order: 2)]
        public bool PutOnGate { get => putOnGate; set => putOnGate = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SetGateOnOff()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.SetGeteOffOn(PutOnGate);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("RF Input Port", Group: "RjioODSC", Description: "Use to select RF Input Port")]
    public class SetRFInputPort : TestStep
    {
        #region Settings
        int portNumber;
        EXM_E6680A e6680A;

        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }

        [Output]
        [Display("RF Port", Description: "Select RF Port Number", Order: 2)]
        public int PortNumber { get => portNumber; set => portNumber = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SetRFInputPort()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.SetRFInputPort(PortNumber);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("RF Power Range", Group: "RjioODSC", Description: "Use to select RF Power Range")]
    public class SetRFPowerRange : TestStep
    {
        #region Settings
        double range;
        EXM_E6680A e6680A;

        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }
        [Display("RF Power Range", Description: "Enter RF Power Range", Order: 2)]
        public double Range { get => range; set => range = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SetRFPowerRange()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.SetRFPowerRange(Range);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Trigger Source", Group: "RjioODSC", Description: "Use to select the trigger source.")]
    public class TriggerSource : TestStep
    {
        #region Settings
        EXM_E6680A e6680A;
        EXM_E6680A.TriggerSourceEnum triggerSource;

        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }
        [Display("Trigger Source", Description: "Select trigger source", Order: 2)]
        public EXM_E6680A.TriggerSourceEnum TriggerSourceValue { get => triggerSource; set => triggerSource = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public TriggerSource()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.TriggerSource(TriggerSourceValue);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

}
