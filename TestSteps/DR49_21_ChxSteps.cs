// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
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
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
//rj-dsa-init 16 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 16 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 2 1200 1611
namespace RjioMRU.TestSteps
{

    /* if (WriteDSAToEEPROM)
            {
                MRU_DUT.Dr49_CH1_WriteDSAToEEPROM(HexValues);

                // IF Ch1 and Ch2, all measurement done properly, then only only write to EEPROM, otherwise NO.

            }*/


    [Display("DR49 Ch1 Write DSA", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR49_Ch1_DSA_Write : TestStep
    {
        #region Settings


        MRU_Rjio mruObj;
        private string[] hexValuesCh1 = new string[16] { "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F" };
        bool automaticDSAWriting = false;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DR49_Ch1_DSA_Write()
        {

            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }
        [Display("Select Automatic and Manual ", Order: 0, Description: "Enter Hex Values for DSA")]
        public bool AutomaticDSAWriting { get => automaticDSAWriting; set => automaticDSAWriting = value; }


        public override void Run()
        {
            DSACHexValues channelValues = new DSACHexValues();
            //foreach (var item in CalibrationStep_CH1.HexValues4DSAWriging)
            //{
            //    if (item > DsaHigherLimit.Value || item < DsaLowerLimit.Value)
            //    {
            //        var messageDecision = MessageBox.Show("DSA Value is out of range :Lower Limit =" + DsaLowerLimit.Value + " DSA Higher Limit :" + DsaHigherLimit.Value + " Measured DSA :" + item + ", Do you want to exit?", "DSA Out of Range", MessageBoxButtons.YesNo);
            //        if (messageDecision == DialogResult.Yes)
            //        {
            //            return;
            //        }
            //    }
            //}


            if (AutomaticDSAWriting)
            {
                for (int iteration = 0; iteration < CalibrationStep_CH1.HexValues4DSAWriging.Length; iteration++)
                {
                    hexValuesCh1[iteration] = $"0x{CalibrationStep_CH1.HexValues4DSAWriging[iteration]:X}";
                    //hexValuesCh1
                }
            }

            MruObj.Dr49_CH_WriteDSAToEEPROM(AutomaticDSAWriting ? hexValuesCh1 : channelValues.HexValuesCh1, MruObj.GetDR49Ch1ComObj());
            Log.Info("DSA Values has been update in EEPROM of Channel 1");
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("DR49 Ch2 Write DSA", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR49_Ch2_DSA_Write : TestStep
    {
        #region Settings


        MRU_Rjio mruObj;
        private string[] hexValuesCh2 = new string[16] { "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F" };
        bool automaticDSAWriting = false;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DR49_Ch2_DSA_Write()
        {

            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }
        [Display("Select Automatic and Manual ", Order: 0, Description: "Enter Hex Values for DSA")]
        public bool AutomaticDSAWriting { get => automaticDSAWriting; set => automaticDSAWriting = value; }


        public override void Run()
        {
            DSACHexValues channelValues = new DSACHexValues();
            //foreach (var item in CalibrationStep_CH1.HexValues4DSAWriging)
            //{
            //    if (item > DsaHigherLimit.Value || item < DsaLowerLimit.Value)
            //    {
            //        var messageDecision = MessageBox.Show("DSA Value is out of range :Lower Limit =" + DsaLowerLimit.Value + " DSA Higher Limit :" + DsaHigherLimit.Value + " Measured DSA :" + item + ", Do you want to exit?", "DSA Out of Range", MessageBoxButtons.YesNo);
            //        if (messageDecision == DialogResult.Yes)
            //        {
            //            return;
            //        }
            //    }
            //}


            if (AutomaticDSAWriting)
            {
                for (int iteration = 0; iteration < CalibrationStep_CH2.HexValues4DSAWriging.Length; iteration++)
                {
                    hexValuesCh2[iteration] = $"0x{CalibrationStep_CH2.HexValues4DSAWriging[iteration]:X}";
                    //hexValuesCh1
                }
            }

            MruObj.Dr49_CH_WriteDSAToEEPROM(AutomaticDSAWriting ? hexValuesCh2 : channelValues.HexValuesCh2, MruObj.GetDR49Ch2ComObj());
            Log.Info("DSA Values has been update in EEPROM of Channel 2");
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("DR49 Write PowerFactor", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR49_Write_PowerFactor : TestStep
    {
        #region Settings
        //Input<int> dsaHigherLimit ;
        //Input<int> dsaLowerLimit ;
        public enum Channels
        {
            Channel1 = 1,
            Channel2
        };
        Channels channelsSelection;
        MRU_Rjio mruObj;
        private string[] hexValuesCh1 = new string[16] { "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F" };
        bool automaticDSAWriting = false;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DR49_Write_PowerFactor()
        {
            //DsaHigherLimit = new Input<int>();
            //DsaLowerLimit = new Input<int>();
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }
        [Display("Select Automatic and Manual ", Order: 0, Description: "Enter Hex Values for DSA")]
        public bool AutomaticDSAWriting { get => automaticDSAWriting; set => automaticDSAWriting = value; }

        [Display("Select Channel", Order: 0, Description: "Select Channel")]
        public Channels ChannelsSelection { get => channelsSelection; set => channelsSelection = value; }

        //[Display("DSA Lower Limit", Order: 0, Description: "Enter DSA Lower Limit")]
        //public Input<int> DsaLowerLimit { get => dsaLowerLimit; set => dsaLowerLimit = value; }
        //[Display("DSA Higher Limit", Order: 0, Description: "Enter DSA Higher Limit")]
        //public Input<int> DsaHigherLimit { get => dsaHigherLimit; set => dsaHigherLimit = value; }

        public override void Run()
        {
            DSACHexValues channelValues = new DSACHexValues();
            //foreach (var item in (int)ChannelsSelection==1?CalibrationStep_CH1.HexValues4DSAWriging:CalibrationStep_CH2.HexValues4DSAWriging)
            //{
            //    //if (item>DsaHigherLimit.Value||item<DsaLowerLimit.Value)
            //    //{
            //    //   var messageDecision = MessageBox.Show("DSA Value is out of range :Lower Limit ="+DsaLowerLimit.Value +" DSA Higher Limit :" + DsaHigherLimit.Value +" Measured DSA :"+item +", Do you want to exit?","DSA Out of Range",MessageBoxButtons.YesNo);
            //    //    if (messageDecision == DialogResult.Yes)
            //    //    {
            //    //        return;
            //    //    }
            //    //}
            //}

            MruObj.Dr49_CH_WritePowerFactorToEEPROM(AutomaticDSAWriting ? (ChannelsSelection == Channels.Channel1 ? CalibrationStep_CH1.powerFactorValues : CalibrationStep_CH2.powerFactorValues) : (ChannelsSelection == Channels.Channel1 ? channelValues.PowerFactorHexValuesCh1 : channelValues.PowerFactorHexValuesCh2), ((channelsSelection == Channels.Channel1 ? MruObj.GetDR49Ch1ComObj() : MruObj.GetDR49Ch2ComObj())));
            Log.Info("DSA Values has been update in EEPROM of Channel 1");
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("DR49 Read Tempertaure", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR49_Read_Temperature : TestStep
    {
        #region Settings
        //Input<int> dsaHigherLimit ;
        //Input<int> dsaLowerLimit ;
        public enum Channels
        {
            Channel1 = 1,
            Channel2
        };
        Channels channelsSelection;
        MRU_Rjio mruObj;
        private string tobeParsed = "MRU DAC temp read tool:3.0\r\nChannel-0\r\nDAC:0    Temp:23.94 degC\r\nDAC:1    Temp:24.12 degC\r\nDAC:2    Temp:24.31 degC\r\nDAC:3    Temp:24.19 degC\r\nDAC:4    Temp:23.88 degC\r\nDAC:5    Temp:24.12 degC\r\nDAC:6    Temp:24.06 degC\r\nDAC:7    Temp:24.12 degC\r\nChannel-1\r\nDAC:0    Temp:23.31 degC\r\nDAC:1    Temp:24.25 degC\r\nDAC:2    Temp:23.94 degC\r\nDAC:3    Temp:24.06 degC\r\nDAC:4    Temp:23.50 degC\r\nDAC:5    Temp:23.94 degC\r\nDAC:6    Temp:24.25 degC\r\nDAC:7    Temp:23.94 degC\r\n";
        private string[] hexValuesCh1 = new string[16] { "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F" };

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DR49_Read_Temperature()
        {
            //DsaHigherLimit = new Input<int>();
            //DsaLowerLimit = new Input<int>();
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }

        [Display("Select Channel", Order: 0, Description: "Select Channel")]
        public Channels ChannelsSelection { get => channelsSelection; set => channelsSelection = value; }
        [Display("Chain Number", Order: 2, Description: "Enter Chain Number")]
        public int ChainNumber { get; set; }

        [Display("Temperature Read Script", Order: 6, Description: "Enter Temperature Read Script")]
        public string TemperatureReadScript { get; set; } = "rj-dac-tmp -mru_dac_num";
        //[Display("DSA Lower Limit", Order: 0, Description: "Enter DSA Lower Limit")]
        //public Input<int> DsaLowerLimit { get => dsaLowerLimit; set => dsaLowerLimit = value; }
        //[Display("DSA Higher Limit", Order: 0, Description: "Enter DSA Higher Limit")]
        //public Input<int> DsaHigherLimit { get => dsaHigherLimit; set => dsaHigherLimit = value; }

        public override void Run()
        {

            string temperaureValue = MruObj.Dr49_CH_ReadTemperature((channelsSelection == Channels.Channel1 ? MruObj.GetDR49Ch1ComObj() : MruObj.GetDR49Ch2ComObj()), ChainNumber, TemperatureReadScript);
            Log.Info("Temperature values measure for chain : {0} is {1}", ChainNumber, temperaureValue);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }



    [Display("DR49 Read RFB_SER_NUM", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR49_Read_RFB_SER_NUM : TestStep
    {
        /*
          rj-rfeeprom-updater -rd_rfb_info
MRU EEPROM tool version:4.1
RFB_SER_NUM:  JITSAMRUFKMRB00016
RFB_RF_FWVER:  1.0
RFB_RF_HWVER:  B
TX_DSA_VAL:  0x1a 0x19 0x1a 0x18 0x1f 0x18 0x1f 0x1b 0x1d 0x19 0x21 0x1a 0x1c 0x19 0x1a 0x13
FB_DSA_VAL:  0xf 0xf 0xf 0xf 0xf 0xf 0xf 0xf 0xf 0xf 0xf 0xf 0xf 0xf 0xf 0xf
RFB_DAC_VAL:  1150 1611
RFB_EPDATA_CRC 0x1654
RX_DSA_VAL:  0xff 0xff 0xff 0xff 0xff 0xff 0xff 0xff 0xff 0xff 0xff 0xff 0xff 0xff 0xff 0xff
RFB_RXDSA_CRC 0xffff
RFB_PROD_VAL 0xc3
TX_PWR_FACT:  0x155a 0x15b4 0x1543 0x157d 0x15bc 0x158b 0x1569 0x15c7 0x154d 0x15ad 0x1514 0x15b7 0x1592 0x15bd 0x15d5 0x15f2
TX_PWR_FACT_CRC 0x2fb4
TX_PWR_FACT_STATUS:  1
End of MRU-EEPROM Read and Write Utility

         */
        #region Settings
        //Input<int> dsaHigherLimit ;
        //Input<int> dsaLowerLimit ;
        public enum Channels
        {
            Channel1 = 1,
            Channel2
        };
        Channels channelsSelection;
        MRU_Rjio mruObj;


        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DR49_Read_RFB_SER_NUM()
        {
            //DsaHigherLimit = new Input<int>();
            //DsaLowerLimit = new Input<int>();
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }

        [Display("Select Channel", Order: 0, Description: "Select Channel")]
        public Channels ChannelsSelection { get => channelsSelection; set => channelsSelection = value; }


        [Display(" Read RFB Script", Order: 6, Description: "Enter Temperature Read Script")]
        public string RFBReadScript { get; set; } = "rj-rfeeprom-updater -rd_rfb_info";
        [Output]
        public string RFBReadValue { get; set; }
        //[Display("DSA Lower Limit", Order: 0, Description: "Enter DSA Lower Limit")]
        //public Input<int> DsaLowerLimit { get => dsaLowerLimit; set => dsaLowerLimit = value; }
        //[Display("DSA Higher Limit", Order: 0, Description: "Enter DSA Higher Limit")]
        //public Input<int> DsaHigherLimit { get => dsaHigherLimit; set => dsaHigherLimit = value; }

        public override void Run()
        {
            RFBReadValue = MruObj.Dr49_ReadRFBSerialNumber((channelsSelection == Channels.Channel1 ? MruObj.GetDR49Ch1ComObj() : MruObj.GetDR49Ch2ComObj()), RFBReadScript);

            Log.Info($"RFB Serial number for 49Dr Channel {ChannelsSelection} is " + RFBReadValue);


            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict == Verdict.Pass?"PASS":"FAIL", " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    //[Display("DR49 Ch2 Write DSA", Group: "RjioMRU", Description: "Insert a description here")]
    //public class DR49_Ch2_DSA_Write : TestStep
    //{
    //    #region Settings
    //    Input<int> dsaHigherLimit;
    //    Input<int> dsaLowerLimit;
    //    MRU_Rjio mruObj;
    //    private string[] hexValuesCh2 = new string[16] { "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F" };
    //    bool automaticDSAWriting = false;
    //    [Display("Select Automatic and Manual ", Order: 0, Description: "Enter Hex Values for DSA")]
    //    public bool AutomaticDSAWriting { get => automaticDSAWriting; set => automaticDSAWriting = value; }
    //    [Display("DSA Lower Limit", Order: 0, Description: "Enter DSA Lower Limit")]
    //    public Input<int> DsaLowerLimit { get => dsaLowerLimit; set => dsaLowerLimit = value; }
    //    [Display("DSA Higher Limit", Order: 0, Description: "Enter DSA Higher Limit")]
    //    public Input<int> DsaHigherLimit { get => dsaHigherLimit; set => dsaHigherLimit = value; }

    //    // ToDo: Add property here for each parameter the end user should be able to change


    //    #endregion

    //    public DR49_Ch2_DSA_Write()
    //    {
    //        DsaHigherLimit = new Input<int>();
    //        DsaLowerLimit = new Input<int>();

    //        // ToDo: Set default values for properties / settings.
    //    }

    //    public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }

    //    public override void Run()
    //    {
    //        DSACHexValues channelValues = new DSACHexValues();
    //        foreach (var item in CalibrationStep_CH2.HexValues4DSAWriging)
    //        {
    //            if (item > DsaHigherLimit.Value || item < DsaLowerLimit.Value)
    //            {
    //                var messageDecision = MessageBox.Show("DSA Value is out of range :Lower Limit =" + DsaLowerLimit.Value + " DSA Higher Limit :" + DsaHigherLimit.Value + " Measured DSA :" + item + ", Do you want to exit?", "DSA Out of Range", MessageBoxButtons.YesNo);
    //                if (messageDecision == DialogResult.Yes)
    //                {
    //                    return;
    //                }
    //            }
    //        }
    //        if (AutomaticDSAWriting)
    //        {
    //            for (int iteration = 0; iteration < CalibrationStep_CH2.HexValues4DSAWriging.Length; iteration++)
    //            {
    //                hexValuesCh2[iteration] = $"0x{CalibrationStep_CH2.HexValues4DSAWriging[iteration]:X}";
    //                //hexValuesCh1
    //            }
    //        }
    //        MruObj.Dr49_CH2_WriteDSAToEEPROM(AutomaticDSAWriting ? hexValuesCh2 : channelValues.HexValuesCh2);

    //        //MruObj.Dr49_CH2_WriteDSAToEEPROM(CalibrationStep_CH2.HexValues4DSAWriging);
    //        Log.Info("DSA Values has been update in EEPROM of Channel 2");

    //        // ToDo: Add test case code.
    //        RunChildSteps(); //If the step supports child steps.

    //        // If no verdict is used, the verdict will default to NotSet.
    //        // You can change the verdict using UpgradeVerdict() as shown below.
    //        // UpgradeVerdict(Verdict.Pass);
    //    }
    //}



    [Display("DR49 Ch1 DPD Pypass", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR49_Ch1Bypass : TestStep
    {
        #region Settings
        MRU_Rjio mruObj;
        string bypassScript = "dpd_bypass_en_dis.sh";
        string bypassValidataionScript = "Bypass is completed";
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DR49_Ch1Bypass()
        {
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }
        [Display("Bypass Script", Order: 0, Description: "Enter Pypass script")]
        public string BypassScript { get => bypassScript; set => bypassScript = value; }
        [Display("Bypass Validation Script", Order: 0, Description: "Enter Pypass Validation script")]
        public string BypassValidataionScript { get => bypassValidataionScript; set => bypassValidataionScript = value; }

        public override void Run()
        {
            var returnValue = MruObj.DR49CH1executeScripts(BypassScript, BypassValidataionScript);
            if (returnValue)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("DR49 Ch2 DPD Pypass", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR49_Ch2Bypass : TestStep
    {
        #region Settings
        MRU_Rjio mruObj;
        string bypassScript = "dpd_bypass_en_dis.sh";
        string bypassValidataionScript = "Bypass is completed";
        // ToDo: Add property here for each parameter the end user should be able to change
        [Display("Bypass Script", Order: 0, Description: "Enter Pypass script")]
        public string BypassScript { get => bypassScript; set => bypassScript = value; }
        [Display("Bypass Validation Script", Order: 0, Description: "Enter Pypass Validation script")]
        public string BypassValidataionScript { get => bypassValidataionScript; set => bypassValidataionScript = value; }
        #endregion

        public DR49_Ch2Bypass()
        {
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }

        public override void Run()
        {
            var returnValue = MruObj.DR49CH2executeScripts(BypassScript, BypassValidataionScript);
            if (returnValue)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("DR49 Ch1 Login", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR49_Ch1Login : TestStep
    {
        #region Settings
        MRU_Rjio mruObj;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DR49_Ch1Login()
        {
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }

        public override void Run()
        {
            var returnValue = MruObj.login49drCh1("root", "root");
            if (returnValue)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE","",  "EQ", "TRUE", "Bool");
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("DR49 Ch2 Login", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR49_Ch2Login : TestStep
    {
        #region Settings

        MRU_Rjio mruObj;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DR49_Ch2Login()
        {
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }

        public override void Run()
        {
            var returnValue = MruObj.login49drCh2("root", "root");
            if (returnValue)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }

    }


    [Display("DR21 Login", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR21Login : TestStep
    {
        #region Settings
        public static string TestPlanStartTime = string.Empty;
        public static DateTime testplanStartTime_dateTime = DateTime.MinValue;
        public static string TestPlanStopTime = string.Empty;
        public static string EmpID = string.Empty;
        public static string softwareVersion = string.Empty;
        public static string testStage = string.Empty;
        MRU_Rjio mruObj;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DR21Login()
        {
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }

        public override void Run()
        {
            var returnValue = MruObj.login21dr("root", "root");
            if (returnValue)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE","", "EQ", "TRUE", "Bool");

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }

        public override void PrePlanRun()
        {
            TestPlanStartTime = DateTime.Now.ToLongTimeString();
            testplanStartTime_dateTime = DateTime.Now;
            EmpID = "12345";
            softwareVersion = "1.1";
            testStage = "MRU EOL";

        }
        public override void PostPlanRun()
        {
            TestPlanStopTime = DateTime.Now.ToLongTimeString();
        }
    }

    [Display("DR21 PTP Sync Check", Group: "RjioMRU", Description: "Wait until PTP syncronisation completed")]
    public class DR21PTPSyncCheck : TestStep
    {
        MRU_Rjio mruObj;
        public DR21PTPSyncCheck()
        {

        }

        public override void Run()
        {
            mruObj.PTPSyncCheck();

        }
    }

    [Display("DR21 ap calib function", Group: "RjioMRU", Description: "ap_calib --set-rx and tx -cal-en 0 0")]
    public class DR21_AP_CALIB_Functions : TestStep
    {
        MRU_Rjio mruObj;
        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }
        public DR21_AP_CALIB_Functions()
        {

        }

        public override void Run()
        {
            MruObj.Dr21_ap_calib_Rx_Tx_Functions();
           UpgradeVerdict(Verdict.Pass);    
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), "", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");



        }
    }
    [Display("DR21 MRU Ipchange", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR21MRUIpChange : TestStep
    {
        #region Settings
        MRU_Rjio mruObj;
        string interfaceName = "eth2";
        string ipAddress = "192.168.1.21";

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DR21MRUIpChange()
        {
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }

        [Display("MRU Interface", Description: "Interface for which IP to be changed", Order: 1)]
        public string InterfaceName { get => interfaceName; set => interfaceName = value; }

        [Display("MRU Interface IP Address", Description: "Interface IP Address", Order: 10)]
        public string IpAddress { get => ipAddress; set => ipAddress = value; }

        public override void Run()
        {
            bool ipChangeResult = MruObj.Dr21MRUIpChange(InterfaceName, IpAddress);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            if (ipChangeResult)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("CCDU Ping Test ", Group: "RjioMRU", Description: "Pinging testing")]

    public class PingTestVerificaiton : TestStep
    {
        private string interfaceName = "ens1f1";
        private string ipAddress = "192.168.1.30";
        private int noOfPingsRequested = 2;
        private int noOfContinuesPingRequried = 1;

        MRU_Rjio mruObj;

        #region Settings

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }
        [Display("Interface Name", Order: 2, Description: "Interface Name")]
        public string InterfaceName { get => interfaceName; set => interfaceName = value; }
        [Display("Ip Address", Order: 5, Description: "IP address ")]
        public string IpAddress { get => ipAddress; set => ipAddress = value; }
        [Display("No of Ping Request", Order: 10, Description: "Number of pings requested")]
        public int NoOfPingsRequested { get => noOfPingsRequested; set => noOfPingsRequested = value; }

        [Display("Number of continuous Ping requried to Pass", Order: 12, Description: "Enter number of continuous required to pass, Take care of number ping requeseted ")]
        public int NoOfContinuesPingRequried { get => noOfContinuesPingRequried; set => noOfContinuesPingRequried = value; }



        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public PingTestVerificaiton()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            bool PingStatus = false;
            int PingCount = 0;
            for (int iteration = 0; iteration < NoOfPingsRequested; iteration++)
            {
                bool singlePingTestStatus = MruObj.Dr21PingTest(1, IpAddress, ", 0% packet loss");
                //if (!PingTestStatus)
                //{
                //    PingTestStatus = MruObj.Dr21PingTest(NoOfPingsRequested, IpAddress, ", 0% packet loss");
                //}
                if (singlePingTestStatus)
                {
                    PingCount++;
                    if (PingCount == NoOfContinuesPingRequried)
                    {
                        PingStatus = true;
                        break;
                    }
                }
                else
                {
                    PingCount = 0;
                    PingStatus = false;
                }
            }

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            if (PingStatus)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), PingStatus ? "TRUE" : "FALSE", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("PTP Sync Check ", Group: "RjioMRU", Description: "Pinging testing")]

    public class PtpSyncCheck : TestStep
    {
        private string interfaceName = "ens1f1";
        private string ipAddress = "192.168.1.30";
        private int noOfPingsRequested = 2;

        MRU_Rjio mruObj;

        #region Settings

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }


        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public PtpSyncCheck()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            var PingTestStatus = MruObj.Dr21PTPSyncStatusCheck();

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            if (PingTestStatus)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, this.Verdict== Verdict.Pass?"PASS":"FAIL", "", Verdict== Verdict.Pass?"TRUE":"FALSE","","EQ", "TRUE", "Bool");

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("DR21 ORAN Init Check", Group: "RjioMRU", Description: "Pinging testing")]

    public class DR21ORANInitCheck : TestStep
    {


        MRU_Rjio mruObj;

        #region Settings

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }




        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DR21ORANInitCheck()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            var PingTestStatus = MruObj.dr21oran_modem_initializationcheck(30, "Modem started");
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            if (PingTestStatus)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("DR21 PTP Sync Established Check", Group: "RjioMRU", Description: "Pinging testing")]

    public class DR21PTYSynEstablishedCheck : TestStep
    {

        string pTPRestart_Command = "ptp_init_21dr.sh --restart";
        MRU_Rjio mruObj;
        int waittime = 50;
        #region Settings

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }

        [Display("Wait time")]
        public int Waittime { get => waittime; set => waittime = value; }
        [Display("PTP Restart command", Description: "This command will execute after waittime delay")]
        public string PTPRestart_Command { get => pTPRestart_Command; set => pTPRestart_Command = value; }



        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DR21PTYSynEstablishedCheck()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            var PingTestStatus = MruObj.dr21PTPInitEstablishedncheck(Waittime, "PTP synchronization detected established!", PTPRestart_Command);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            if (PingTestStatus)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ",this.Verdict == Verdict.Pass ? "TRUE" : "FALSE","",  "EQ", "TRUE", "Bool");

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("DR49 Ch1 ORAN Init Check", Group: "RjioMRU", Description: "Pinging testing")]

    public class DR49Ch1ORANInitCheck : TestStep
    {


        MRU_Rjio mruObj;

        #region Settings

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }




        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DR49Ch1ORANInitCheck()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            var PingTestStatus = MruObj.dr49ch1oran_modem_initializationcheck(30, "Modem initialization is in process");
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            if (PingTestStatus)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("DR49 Ch2 ORAN Init Check", Group: "RjioMRU", Description: "Pinging testing")]

    public class DR49Ch2ORANInitCheck : TestStep
    {


        MRU_Rjio mruObj;

        #region Settings

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }




        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public DR49Ch2ORANInitCheck()
        {
            // ToDo: Set default values for properties / settings.
        }
        public override void Run()
        {
            var PingTestStatus = MruObj.dr49ch2oran_modem_initializationcheck(30, "Modem initialization is in process");
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            if (PingTestStatus)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
           // MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", PingTestStatus.ToString(), "TRUE", "EQ", "TRUE", " ");
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("DR49 Ch2 DSA Script", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR49_Ch2DSAScript : TestStep
    {
        #region Settings
        MRU_Rjio mruObj;
        string dsaScript = "rj-dsa-init 16 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 16 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 2 1200 1611";
        string dsaValidataionScriptCh2 = "rjInitialConfiguration Completed";
        // ToDo: Add property here for each parameter the end user should be able to change
        [Display("DSA Script", Order: 0, Description: "Enter Pypass script")]
        public string DsaScript { get => dsaScript; set => dsaScript = value; }
        [Display("DSA Validation Script", Order: 10, Description: "Enter Pypass Validation script")]
        public string DsaValidataionScriptCh2 { get => dsaValidataionScriptCh2; set => dsaValidataionScriptCh2 = value; }
        #endregion

        public DR49_Ch2DSAScript()
        {
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }

        public override void Run()
        {
            var returnValue = MruObj.DR49CH2executeScripts(DsaScript, DsaValidataionScriptCh2);
            if (!returnValue)
            {
                returnValue = MruObj.DR49CH2executeScripts(DsaScript, DsaValidataionScriptCh2);
            }
            if (returnValue)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("DR49 Ch2 DSA Disable Script", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR49_Ch2DSADisableScript : TestStep
    {
        #region Settings
        MRU_Rjio mruObj;
        string dsaScript = "rj-dsa-init 16 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 16 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 2 0 0";
        string dsaValidataionScriptCh2 = "rjInitialConfiguration Completed";
        // ToDo: Add property here for each parameter the end user should be able to change
        [Display("DSA Script", Order: 0, Description: "Enter Pypass script")]
        public string DsaScript { get => dsaScript; set => dsaScript = value; }
        [Display("DSA Validation Script", Order: 10, Description: "Enter Pypass Validation script")]
        public string DsaValidataionScriptCh2 { get => dsaValidataionScriptCh2; set => dsaValidataionScriptCh2 = value; }
        #endregion

        public DR49_Ch2DSADisableScript()
        {
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }

        public override void Run()
        {
            var returnValue = MruObj.DR49CH2executeScripts(DsaScript, DsaValidataionScriptCh2);
            if (!returnValue)
            {
                returnValue = MruObj.DR49CH2executeScripts(DsaScript, DsaValidataionScriptCh2);
            }
            if (returnValue)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("DR49 Ch1 DSA Script", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR49_Ch1DSAScript : TestStep
    {
        #region Settings
        MRU_Rjio mruObj;
        string dsaScript = "rj-dsa-init 16 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 16 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 2 1200 1611";
        string dsaValidataionScriptCh1 = "rjInitialConfiguration Completed";
        // ToDo: Add property here for each parameter the end user should be able to change
        [Display("DSA Script", Order: 0, Description: "Enter Pypass script")]
        public string DsaScript { get => dsaScript; set => dsaScript = value; }
        [Display("DSA DR49Ch1 Validation Script", Order: 10, Description: "Enter Pypass Validation script")]
        public string DsaValidataionScriptCh1 { get => dsaValidataionScriptCh1; set => dsaValidataionScriptCh1 = value; }
        #endregion

        public DR49_Ch1DSAScript()
        {
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }


        public override void Run()
        {
            var returnValue = MruObj.DR49CH1executeScripts(DsaScript, DsaValidataionScriptCh1);
            if (!returnValue)
            {
                returnValue = MruObj.DR49CH1executeScripts(DsaScript, DsaValidataionScriptCh1);
            }
            if (returnValue)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("DR49 Ch1 DSA Disable Script", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR49_Ch1DSADisableScript : TestStep
    {
        #region Settings
        MRU_Rjio mruObj;
        string dsaScript = "rj-dsa-init 16 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 0x40 16 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 2 0 0";
        string dsaValidataionScriptCh1 = "rjInitialConfiguration Completed";
        // ToDo: Add property here for each parameter the end user should be able to change
        [Display("DSA Script", Order: 0, Description: "Enter Pypass script")]
        public string DsaScript { get => dsaScript; set => dsaScript = value; }
        [Display("DSA DR49Ch1 Validation Script", Order: 10, Description: "Enter Pypass Validation script")]
        public string DsaValidataionScriptCh1 { get => dsaValidataionScriptCh1; set => dsaValidataionScriptCh1 = value; }
        #endregion

        public DR49_Ch1DSADisableScript()
        {
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }


        public override void Run()
        {
            var returnValue = MruObj.DR49CH1executeScripts(DsaScript, DsaValidataionScriptCh1);
            if (!returnValue)
            {
                returnValue = MruObj.DR49CH1executeScripts(DsaScript, DsaValidataionScriptCh1);
            }
            if (returnValue)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("DR21 ORAN Status", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR21_ORANStatus : TestStep
    {
        #region Settings
        MRU_Rjio mruObj;

        // ToDo: Add property here for each parameter the end user should be able to change

        #endregion

        public DR21_ORANStatus()
        {
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }


        public override void Run()
        {
            var returnValue = MruObj.dr21ORANStatusCheck();
            if (returnValue)
                UpgradeVerdict(Verdict.Pass);
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }

            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE","",  "EQ", "TRUE", "Bool");

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    //[Display("DR49 Ch1 DPD Measurements", Group: "RjioMRU", Description: "Insert a description here")]
    //public class DR49_CH1_DPDMeasurement: TestStep
    //{
    //    #region Settings
    //    MRU_Rjio mruObj;
    //    int channelNumber = 0;
    //    double txPowerValue = double.NaN;
    //    double rxPowerValue = double.NaN;
    //    // ToDo: Add property here for each parameter the end user should be able to change

    //    #endregion

    //    public DR49_CH1_DPDMeasurement()
    //    {
    //        // ToDo: Set default values for properties / settings.
    //    }

    //    public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }
    //    public int ChannelNumber { get => channelNumber; set => channelNumber = value; }

    //    public override void Run()
    //    {
    //        var returnValue = MruObj.Dr49_CH1_DPD_Measurement(ChannelNumber, out txPowerValue,out rxPowerValue) ;
    //        //if (returnValue)
    //        //    UpgradeVerdict(Verdict.Pass);
    //        //elsed
    //        //{
    //        //    UpgradeVerdict(Verdict.Fail);



    //        // ToDo: Add test case code.
    //        RunChildSteps(); //If the step supports child steps.

    //        // If no verdict is used, the verdict will default to NotSet.
    //        // You can change the verdict using UpgradeVerdict() as shown below.
    //        // UpgradeVerdict(Verdict.Pass);
    //    }
    //}

    //[Display("DR49 Ch2 DPD Measurements", Group: "RjioMRU", Description: "Insert a description here")]
    //public class DR49_CH2_DPDMeasurement: TestStep
    //{
    //    #region Settings
    //    MRU_Rjio mruObj;
    //    int channelNumber = 0;
    //    double txPowerValue = double.NaN;
    //    double rxPowerValue = double.NaN;
    //    // ToDo: Add property here for each parameter the end user should be able to change

    //    #endregion

    //    public DR49_CH2_DPDMeasurement()
    //    {
    //        // ToDo: Set default values for properties / settings.
    //    }

    //    public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }
    //    public int ChannelNumber { get => channelNumber; set => channelNumber = value; }

    //    public override void Run()
    //    {
    //        var returnValue = MruObj.Dr49_CH2_DPD_Measurement(ChannelNumber, out txPowerValue,out rxPowerValue) ;
    //        //if (returnValue)
    //        //    UpgradeVerdict(Verdict.Pass);
    //        //elsed
    //        //{
    //        //    UpgradeVerdict(Verdict.Fail);



    //        // ToDo: Add test case code.
    //        RunChildSteps(); //If the step supports child steps.

    //        // If no verdict is used, the verdict will default to NotSet.
    //        // You can change the verdict using UpgradeVerdict() as shown below.
    //        // UpgradeVerdict(Verdict.Pass);
    //    }
    //}

    [Display("DR21 Read info", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR21_ReadInfo : TestStep
    {
        public static string MAC1_ = string.Empty;
        public static string MAC2_ = string.Empty;
        public static string MAC3_ = string.Empty;
        public static string MAC4_ = string.Empty;
        public static string ProductSerialNumber_ = string.Empty;
        public static string PCBserialNumber_ = string.Empty;
        public static string ProdID_ = string.Empty;

        #region Settings
        MRU_Rjio mruObj;


        // ToDo: Add property here for each parameter the end user should be able to change

        [Display("SELECT MRU", Group: "DUT SELECTION ")]
        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }

        string mac2 = string.Empty;
        [Output]
        [Display("MAC ID", Order: 1)]
        public string MAC2 { get { return mac2; } set { mac2 = value; } }

        string mac1 = string.Empty;
        [Output]
        [Display("MAC1 ID", Order: 5)]
        public string MAC1
        {
            get { return mac1; }
            set { mac1 = value; }
        }

        string mac3 = string.Empty;
        [Output]
        [Display("MAC3 ID", Order: 10)]
        public string MAC3 { get { return mac3; } set { mac3 = value; } }
        string mac4 = string.Empty;
        [Output]
        [Display("MAC4 ID", Order: 15)]
        public string MAC4 { get { return mac4; } set { mac4 = value; } }
        string productSerialNumber = string.Empty;
        [Output]
        [Display("Product Serial Number", Order: 20)]
        public string ProductSerialNumber { get { return productSerialNumber; } set { productSerialNumber = value; } }
        string pcbSerialNumber = string.Empty;
        [Output]
        [Display("PCB Serial Number", Order: 25)]
        public string PCBserialNumber { get { return pcbSerialNumber; } set { pcbSerialNumber = value; } }
        string prodID = string.Empty;
        [Output]
        [Display("Prod ID", Order: 30)]
        public string ProdID { get { return prodID; } set { prodID = value; } }
        #endregion


        public DR21_ReadInfo()
        {

            // ToDo: Set default values for properties / settings.
        }



        public override void Run()
        {

            var returnValue = MruObj.Dr21GetEepromInfo(out mac1, out mac2, out mac3, out mac4, out productSerialNumber, out pcbSerialNumber, out prodID);
            MAC1_ = mac1;
            MAC2_ = mac2;
            MAC3_ = mac3;
            MAC4_ = mac4;
            ProductSerialNumber_ = productSerialNumber;
            PCBserialNumber_ = pcbSerialNumber;
            ProdID_ = prodID;

            if (returnValue)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }

            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    //[Display("DR21 Write info", Group: "RjioMRU", Description: "Insert a description here")]
    //public class DR21_WriteInfo : TestStep
    //{
    //    public static string MAC1 = string.Empty;
    //    public static string MAC2 = string.Empty;
    //    public static string MAC3 = string.Empty;
    //    public static string MAC4 = string.Empty;
    //    public static string ProductSerialNumber = string.Empty;
    //    public static string PCBserialNumber = string.Empty;
    //    public static string ProdID = string.Empty;
    //    #region Settings
    //    MRU_Rjio mruObj;

    //    // ToDo: Add property here for each parameter the end user should be able to change

    //    #endregion

    //    public DR21_WriteInfo()
    //    {
    //        // ToDo: Set default values for properties / settings.
    //    }

    //    public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }


    //    public override void Run()
    //    {

    //        var returnValue = MruObj.Dr21GetEepromInfo(out MAC1, out MAC2, out MAC3, out MAC4, out ProductSerialNumber, out PCBserialNumber, out ProdID);
    //        if (returnValue)
    //            UpgradeVerdict(Verdict.Pass);
    //        else
    //        {
    //            UpgradeVerdict(Verdict.Fail);
    //        }


    //        // ToDo: Add test case code.
    //        RunChildSteps(); //If the step supports child steps.

    //        // If no verdict is used, the verdict will default to NotSet.
    //        // You can change the verdict using UpgradeVerdict() as shown below.
    //        // UpgradeVerdict(Verdict.Pass);
    //    }
    //}


    [Display("49DR Ch1 DPD Reset", Group: "RjioMRU", Description: "Insert a description here")]
    public class DR49Ch1_DPDReset : TestStep
    {
        int channelNuber = 1;

        #region Settings
        MRU_Rjio mruObj;

        // ToDo: Add property here for each parameter the end user should be able to change

        #endregion

        public DR49Ch1_DPDReset()
        {
            // ToDo: Set default values for properties / settings.
        }

        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }
        [Display("Channel Number ")]
        public int ChannelNuber { get => channelNuber; set => channelNuber = value; }

        public override void Run()
        {

            var returnValue = MruObj.Dr49_CH1_DPD_TillDCLRun(ChannelNuber);
            if (returnValue)
                UpgradeVerdict(Verdict.Pass);
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }


            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }



    [Display("Check Yocta Version", Group: "RjioMRU", Description: "Insert a description here")]
    public class CheckYoctaVersion : TestStep
    {
        int channelNuber = 1;
        #region Settings
        MRU_Rjio mruObj;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public CheckYoctaVersion()
        {
            // ToDo: Set default values for properties / settings.
        }
        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }
        public override void Run()
        {
            string yoctaVersion21Dr = string.Empty;
            string yoctaVersion49DrCh1 = string.Empty;
            string yoctaVersion49DrCh2 = string.Empty;
            string command = "cat /build-summary.json | grep \"version\"";

            yoctaVersion21Dr = MruObj.Dr21_checkYoctoVersion(command);
            Log.Info("Yocta Version 21 Dr :" + yoctaVersion21Dr);
            yoctaVersion49DrCh1 = MruObj.Dr49_Ch1_checkYoctoVersion(command);
            Log.Info("Yocta Version 49DR Ch1 :" + yoctaVersion49DrCh1);
            yoctaVersion49DrCh2 = MruObj.Dr49_Ch2_checkYoctoVersion(command);
            Log.Info("Yocta Version 49DR Ch2 :" + yoctaVersion49DrCh2);
            if (string.IsNullOrEmpty(yoctaVersion21Dr) || string.IsNullOrEmpty(yoctaVersion49DrCh1) || string.IsNullOrEmpty(yoctaVersion49DrCh2))
            {
                UpgradeVerdict(Verdict.Fail);
                return;
            }
            int StringOk = string.Compare(yoctaVersion21Dr, yoctaVersion49DrCh1);
            int string2OK = string.Compare(yoctaVersion21Dr, yoctaVersion49DrCh2);
            if (StringOk != 0)
            {
                UpgradeVerdict(Verdict.Fail);
                return;
            }
            else
            {
                if (string2OK != 0)
                {
                    UpgradeVerdict(Verdict.Fail);
                    return;
                }
            }
            UpgradeVerdict(Verdict.Pass);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.
                             // If no verdict is used, the verdict will default to NotSet.
                             // You can change the verdict using UpgradeVerdict() as shown below.
                             // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("Check slot Version", Group: "RjioMRU", Description: "Insert a description here")]
    public class CheckSlotNumber : TestStep
    {
        int channelNuber = 1;
        #region Settings
        MRU_Rjio mruObj;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public CheckSlotNumber()
        {
            // ToDo: Set default values for properties / settings.
        }
        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }
        public override void Run()
        {
            string slot21dr = string.Empty;
            string slot49DrCh1 = string.Empty;
            string slot49DrCh2 = string.Empty;
            string command = "fw_printenv boot_source_info";

            slot21dr = MruObj.Dr21_checkSlot(command);
            Log.Info("Slot 21 Dr :" + slot21dr);
            slot49DrCh1 = MruObj.Dr49_Ch1_checkSlot(command);
            Log.Info("Slot 49DR Ch1 :" + slot49DrCh1);
            slot49DrCh2 = MruObj.Dr49_Ch2_checkSlot(command);
            Log.Info("Slot 49DR Ch2 :" + slot49DrCh2);

            if (string.IsNullOrEmpty(slot21dr) || string.IsNullOrEmpty(slot49DrCh1) || string.IsNullOrEmpty(slot49DrCh2))
            {
                UpgradeVerdict(Verdict.Fail);
                return;
            }


            int StringOk = string.Compare(slot21dr, slot49DrCh1);
            int string2OK = string.Compare(slot21dr, slot49DrCh2);

            if (StringOk != 0)
            {
                UpgradeVerdict(Verdict.Fail);
                return;
            }
            else
            {
                if (string2OK != 0)
                {
                    UpgradeVerdict(Verdict.Fail);
                    return;
                }
            }
            var stepStatus = StringOk == 0 & string2OK == 0;
            UpgradeVerdict(Verdict.Pass);
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", stepStatus?"TRUE":"FALSE","","EQ","TRUE" ,"Bool");

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.
                             // If no verdict is used, the verdict will default to NotSet.
                             // You can change the verdict using UpgradeVerdict() as shown below.
                             // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("Dr21 Execute Extract ", Group: "RjioMRU", Description: "Insert a description here")]
    public class ExecuteExtractScript : TestStep
    {
        string commandScripts = "cd /mnt/data\r\ncd /mnt/data/\r\ntar -xvzf fact_testing_config.tar.gz\r\ncd fact_testing_config_files/dl_testing/\r\nchmod 777 config_before_dl_testing.sh\r\n./config_before_dl_testing.sh";
        #region Settings
        MRU_Rjio mruObj;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public ExecuteExtractScript()
        {
            // ToDo: Set default values for properties / settings.
        }
        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }

        [Display("21Dr Extract scripts ", Order: 10, Description: "Enter all the extract scripts ")]
        public string CommandScripts { get => commandScripts; set => commandScripts = value; }

        public override void Run()
        {


            var returnValue = MruObj.Dr21RunExtractCommandScripts(CommandScripts);
            Log.Info("Extract Scripts :");
            foreach (var item in returnValue.Split('\n'))
            {
                Log.Info(item);
            }

            UpgradeVerdict(Verdict.Pass);
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.
                             // If no verdict is used, the verdict will default to NotSet.
                             // You can change the verdict using UpgradeVerdict() as shown below.
                             // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("Dr21 Reboot MRU All", Group: "RjioMRU", Description: "Insert a description here")]
    public class Dr21RebbotMRUAll : TestStep
    {

        #region Settings
        MRU_Rjio mruObj;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public Dr21RebbotMRUAll()
        {
            // ToDo: Set default values for properties / settings.
        }
        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }



        public override void Run()
        {


            MruObj.Dr21RebootMRUAll();


            UpgradeVerdict(Verdict.Pass);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.
                             // If no verdict is used, the verdict will default to NotSet.
                             // You can change the verdict using UpgradeVerdict() as shown below.
                             // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("21Dr rm -rf /root/.ssh/known_hosts", Group: "RjioMRU", Description: "Insert a description here")]
    public class Dr21Known_HostsCommand : TestStep
    {

        #region Settings
        MRU_Rjio mruObj;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public Dr21Known_HostsCommand()
        {
            // ToDo: Set default values for properties / settings.
        }
        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }



        public override void Run()
        {


            MruObj.Dr21known_hostsCommand();


            UpgradeVerdict(Verdict.Pass);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.
                             // If no verdict is used, the verdict will default to NotSet.
                             // You can change the verdict using UpgradeVerdict() as shown below.
                             // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("MAC Provisioning ", Group: "RjioMRU", Description: "Insert a description here")]
    public class Dr21_MACProvisioning : TestStep
    {
        #region Settings
        MRU_Rjio mruObj;
        ClsMES mesObj;
        //string serialNumber = string.Empty;
        //string productID = string.Empty;
        //string macID = string.Empty;
        //string ihstbID = string.Empty;
        //string rffeID = string.Empty;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public Dr21_MACProvisioning()
        {
            SerialNumber = new Input<string>();
            ProductID = new Input<string>();
            MacID = new Input<string>();
            IhstbID = new Input<string>();
            RffeID = new Input<string>();
        }
        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }
        // public ClsMES MesObj { get => mesObj; set => mesObj = value; }

        [Display("Serial Number", Order: 10, Description: "Read the serial number")]
        public Input<string> SerialNumber { get; set; }

        [Display("Product ID", Order: 11, Description: "Read the Product ID")]
        public Input<string> ProductID { get; set; }

        [Display("MAC ID", Order: 12, Description: "MAC ID")]
        public Input<string> MacID { get; set; }

        [Display("Ihstb ID", Order: 13, Description: "Read ihstb ID")]
        public Input<string> IhstbID { get; set; }

        [Display("Rffe ID", Order: 14, Description: "Rffe ID")]
        public Input<string> RffeID { get; set; }

        public override void Run()
        {
            // var prod_mac = MesObj.GetDataFromMac_ProductID(SerialNumber.Value);
            //if (MruObj != null)
            //{
            Log.Info("Serial number {0} , MAC ID {1} and Product id {2} from MES, Writing to EEPROM of MRU", SerialNumber.Value, MacID.Value, ProductID.Value);
            MruObj.Dr21MAC_SLNO_PRODID_Provisioning(MacID.Value, SerialNumber.Value, ProductID.Value);
            // If S.NO and MAC ID is already there in the MRU, NO need to overwrite
            // Check before writing: Read Info from MRU
            //}
            //else
            //{
            //    Log.Info("MRU object is null");
            //}
            UpgradeVerdict(Verdict.Pass);

            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");


            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.
                             // If no verdict is used, the verdict will default to NotSet.
                             // You can change the verdict using UpgradeVerdict() as shown below.
                             // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("21Dr Remove IP from Eth2", Group: "RjioMRU", Description: "Insert a description here")]
    public class Dr21RemoveIP4mETH : TestStep
    {

        #region Settings
        MRU_Rjio mruObj;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public Dr21RemoveIP4mETH()
        {
            // ToDo: Set default values for properties / settings.
        }
        public MRU_Rjio MruObj { get => mruObj; set => mruObj = value; }



        public override void Run()
        {
            MruObj.RemoveIP4mETH2();


            UpgradeVerdict(Verdict.Pass);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.
                             // If no verdict is used, the verdict will default to NotSet.
                             // You can change the verdict using UpgradeVerdict() as shown below.
                             // UpgradeVerdict(Verdict.Pass);
        }
    }

}
