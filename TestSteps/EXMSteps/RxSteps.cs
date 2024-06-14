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

namespace RjioODSC
{
    [Display("Rx Continuous Trigger", Group: "RjioODSC", Description: "Insert a description here")]
    public class RxContinuousTrigger : TestStep
    {
        #region Settings
        EXM_E6680A e6680_Inst;
        EXM_E6680A.ContinuesTrigger continuestrigger;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RxContinuousTrigger()
        {
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680_Inst { get => e6680_Inst; set => e6680_Inst = value; }

        [Display("Continuous Trigger", Description: "Select continuous trigger type", Order: 2)]
        public EXM_E6680A.ContinuesTrigger Continuestrigger { get => continuestrigger; set => continuestrigger = value; }

        public override void Run()
        {
            E6680_Inst.RxContinuesTrigger(Continuestrigger);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("Rx ARB Trigger source", Group: "RjioODSC", Description: "Insert a description here")]
    public class RxARBTriggerSource : TestStep
    {
        #region Settings
        EXM_E6680A e6680_Inst;
        EXM_E6680A.ArbTriggerSource arbtriggersource;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RxARBTriggerSource()
        {
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680_Inst { get => e6680_Inst; set => e6680_Inst = value; }

        [Display("Arb Trigger source", Description: "Select trigger type source", Order: 2)]
        public EXM_E6680A.ArbTriggerSource Arbtriggersource { get => arbtriggersource; set => arbtriggersource = value; }

        public override void Run()
        {
            E6680_Inst.RxTriggerSource(Arbtriggersource);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("Rx Sync to Trigger source", Group: "RjioODSC", Description: "Insert a description here")]
    public class RxARBSyncToTriggerSource : TestStep
    {
        #region Settings
        EXM_E6680A e6680_Inst;
        bool sysnctoSource;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RxARBSyncToTriggerSource()
        {
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680_Inst { get => e6680_Inst; set => e6680_Inst = value; }

        [Display("Arb sysnc to source", Description: "Check for sysn to source on ", Order: 2)]

        public bool SysnctoSource { get => sysnctoSource; set => sysnctoSource = value; }

        public override void Run()
        {
            E6680_Inst.RxSyncToTriggerSource(SysnctoSource);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("Rx Trig External Delay", Group: "RjioODSC", Description: "Insert a description here")]
    public class RxARBTrigExtDelay : TestStep
    {
        #region Settings
        EXM_E6680A e6680_Inst;
        double extDelay;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RxARBTrigExtDelay()
        {
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680_Inst { get => e6680_Inst; set => e6680_Inst = value; }

        [Display("Arb Trig Ext Delay", Description: "Enter the external delay value", Order: 2)]


        public double ExtDelay { get => extDelay; set => extDelay = value; }

        public override void Run()
        {
            E6680_Inst.RxARBTrigExtDelay(ExtDelay);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Rx Trig polority", Group: "RjioODSC", Description: "Insert a description here")]
    public class RxATrigPolority : TestStep
    {
        #region Settings
        EXM_E6680A e6680_Inst;
        EXM_E6680A.polority trigPolority;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RxATrigPolority()
        {
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680_Inst { get => e6680_Inst; set => e6680_Inst = value; }

        [Display("Arb Trig Polority", Description: "Select Trigger Polority", Order: 2)]



        public EXM_E6680A.polority TrigPolority { get => trigPolority; set => trigPolority = value; }

        public override void Run()
        {
            E6680_Inst.RxExtTrigPolarity(TrigPolority);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Rx ARB RMS", Group: "RjioODSC", Description: "Insert a description here")]
    public class RxARBRMS : TestStep
    {
        #region Settings
        EXM_E6680A e6680_Inst;
        double arbRms;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RxARBRMS()
        {
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680_Inst { get => e6680_Inst; set => e6680_Inst = value; }

        [Display("Arb RMS", Description: "Enter RMS Value", Order: 2)]
        public double ArbRms { get => arbRms; set => arbRms = value; }

        public override void Run()
        {
            E6680_Inst.RxARBRms(ArbRms);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Rx Output Port", Group: "RjioODSC", Description: "Insert a description here")]
    public class RxOutputPort : TestStep
    {
        #region Settings
        EXM_E6680A e6680_Inst;
        EXM_E6680A.RfOutputPorts outputPorts;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RxOutputPort()
        {
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680_Inst { get => e6680_Inst; set => e6680_Inst = value; }

        [Display("RF OUTPUT PORT", Description: "Select the RF output ports", Order: 2)]

        public EXM_E6680A.RfOutputPorts OutputPorts { get => outputPorts; set => outputPorts = value; }

        public override void Run()
        {
            E6680_Inst.RxOutputPort(OutputPorts);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("Rx Output OnOff", Group: "RjioODSC", Description: "Insert a description here")]
    public class RxOutputOnOff : TestStep
    {
        #region Settings
        EXM_E6680A e6680_Inst;
        bool outputPortsOnOff;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RxOutputOnOff()
        {
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680_Inst { get => e6680_Inst; set => e6680_Inst = value; }

        [Display("RF O/P On Off", Description: "Check for RF output on ", Order: 2)]


        public bool OutputPortsOnOff { get => outputPortsOnOff; set => outputPortsOnOff = value; }

        public override void Run()
        {
            E6680_Inst.RxOutputOnOFf(OutputPortsOnOff);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Rx RF O/P Power", Group: "RjioODSC", Description: "Insert a description here")]
    public class RxOutputPower : TestStep
    {
        #region Settings
        EXM_E6680A e6680_Inst;
        double outputPortsPower;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RxOutputPower()
        {
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680_Inst { get => e6680_Inst; set => e6680_Inst = value; }

        [Display("RF O/P Power", Description: "Enter output power value", Order: 2)]



        public double OutputPortsPower { get => outputPortsPower; set => outputPortsPower = value; }

        public override void Run()
        {
            E6680_Inst.RxSourcePower(OutputPortsPower);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("Rx RF Frequency", Group: "RjioODSC", Description: "Insert a description here")]
    public class RxFrequency : TestStep
    {
        #region Settings
        EXM_E6680A e6680_Inst;
        double rfFrequency;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RxFrequency()
        {
            // ToDo: Set default values for properties / settings.
        }
        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680_Inst { get => e6680_Inst; set => e6680_Inst = value; }

        [Display("RF Frequency", Description: "Enter RF Frequency", Order: 2)]

        public double RfFrequency { get => rfFrequency; set => rfFrequency = value; }

        public override void Run()
        {
            E6680_Inst.RxFrequency(RfFrequency);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Select Waveform file", Group: "RjioODSC", Description: "Use to select the waveform")]
    public class SelectWaveform : TestStep
    {
        #region Settings
        EXM_E6680A e6680A;
         
        // List<string> waveformList;
        private string waveform;

        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }
        [Display("Waveform file", Description: "Select waveform file", Order: 2)]

        public string Waveform { get => waveform; set => waveform = value; }

       



        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SelectWaveform()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.SelectWaveform(Waveform);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Select Waveform RB", Group: "RjioODSC", Description: "Use to select the waveform")]
    public class SelectWaveformRB : TestStep
    {
        #region Settings
        EXM_E6680A e6680A;
        int prbOffset = 0;
        int bandwidth = 5;
        // List<string> waveformList;
        private string waveform;

        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }
        [Display("Waveform file", Description: "Select waveform file", Order: 2)]

        public string Waveform { get => waveform; set => waveform = value; }



        [Output]
        [Display("RB Offset", Description: "Enter waveform RB", Order: 3)]
        public int RbOffset { get => prbOffset; set => prbOffset = value; }

        [Display("Waveform BW",Description:"Enter Waveform Bandwidth",Order: 4)]
        public int Bandwidth { get => bandwidth; set => bandwidth = value; }



        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SelectWaveformRB()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {

            Waveform = "FDD_1.95GHz_bw_" + Bandwidth + "MHz_FRC_A-1-3_RNTI61_000000_RBOffset_"+((RbOffset==0)?"00":(RbOffset==25)?"25":"00");
            E6680A.SelectWaveform(Waveform);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("ARB Sample Rate", Group: "RjioODSC", Description: "Use to change ARB sample rate")]
    public class SetARBSampleRate : TestStep
    {
        #region Settings
        double aRBSampleRate;
        EXM_E6680A e6680A;
        [Display("EXM ", Order: 1, Description: "Select the instrument address")]
        public EXM_E6680A E6680A { get => e6680A; set => e6680A = value; }

        [Display("ARB Sample Rate", Order: 2, Description: "Enter ARB Sample Rate")]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double ARBSampleRate { get => aRBSampleRate; set => aRBSampleRate = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SetARBSampleRate()
        {
            E6680A = new EXM_E6680A();
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            E6680A.SetARBSampleRate(ARBSampleRate);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


}
