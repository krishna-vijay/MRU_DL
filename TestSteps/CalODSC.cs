// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
//#define NORMAL
using OpenTap;
using System;
using System.Diagnostics;

namespace RjioMRU.TestSteps
{
    [Display("CalMRU", Group: "RjioMRU", Description: "Insert a description here")]
    public class CalMRU : TestStep
    {
        #region Settings
        static bool RestoreSecreensDone = false;
        static bool ScreenCreationgSettings = false;

        static object InstrumentLock = new object();
        static object MeasurementLock = new object();
        RjioReportCls rjioReport;

        private string sendScript10 = "./RFbEepromTool updRD";
        private string sendScript9 = "./IbtbEepromTool updRD";
        private string validateScript8 = "L1 init execution status: PASS";
        private string sendScript8 = "./l1-init.sh";
        private string sendScript7 = "cd /custom-sw/MRU/scripts/";
        private string sendScript6 = "mkdir log-archives";
        private string sendScript5 = "cd /mnt/rw-area";
        private string sendScript4 = "./IbtbEepromTool A A A A A A A A A updRFV s060214t100921 1 A A A A 1";
        private string sendScript3 = "./RFbEepromTool -updFRQ_SET 1 -updDSA_SET_TX 0x24,0x21,0x23,0x1C -updDSA_SET_FB 0x0f,0x0f,0x0f,0x0f -updDAC_SET 1280,1700 -updS_PVLD";
        private string sendScript2 = "./RFbEepromTool A A updRFFWV 1.0 updRFHWV B A A A A A A A A A A A A A updS_PVLD";
        private string sendScript1 = "cd /custom-sw/MRU/bin";
        private string stateFilePath = @"D:/5G_NR_MRU.state";
        private string scpFilePath = @"D:/5G_NR_MRU.scp";
        private bool useSCP = false;

        private string hexValue1 = "24";
        private string hexValue2 = "21";
        private string hexValue3 = "23";
        private string hexValue4 = "1C";


        MRU_Rjio mRU;
        EXM_E6680A e6680E;

        double ch1CableLoss = -31.6;
        double ch2CableLoss = -32.2;
        double ch3CableLoss = -32.0;
        double ch4CableLoss = -31.9;
        double ch5CableLoss = -31.6;
        double ch6CableLoss = -32.2;
        double ch7CableLoss = -32.0;
        double ch8CableLoss = -31.9;

        double channelPowerLimit = 0.2;
        double channelPower = 38;
        double UpperChannelLimit;
        double LowerChannelLimit;

        int mRUSettlingTime = 5000;

        int startPort = 0;
        int endPort = 1;

        string dUTNumber = "DUT1";
        string strCh1Measurements = string.Empty;
        string strCh2Measurements = string.Empty;
        string strCh3Measurements = string.Empty;
        string strCh4Measurements = string.Empty;


        double seqPeakPower = 12;
        double seqExpectedPower = 8;

        Stopwatch stopwatch = new Stopwatch();


        // ToDo: Add property here for each parameter the end user should be able to change

        public MRU_Rjio MRU { get => mRU; set => mRU = value; }
        public EXM_E6680A E6680E { get => e6680E; set => e6680E = value; }

        [Display("Recall SCP file?", Group: "File Paths", Order: 4)]
        public bool UseSCP { get => useSCP; set => useSCP = value; }
        [Display("State file Path", Group: "File Paths", Order: 5)]
        public string StateFilePath { get => stateFilePath; set => stateFilePath = value; }
        [Display("SCP file Path", Group: "File Paths", Order: 5.1)]
        public string ScpFilePath { get => scpFilePath; set => scpFilePath = value; }

        [Display("Send Script1", Group: "Scripts", Order: 7)]
        public string SendScript1 { get => sendScript1; set => sendScript1 = value; }
        [Display("Send Script2", Group: "Scripts", Order: 9)]
        public string SendScript2 { get => sendScript2; set => sendScript2 = value; }
        [Display("Send Script3", Group: "Scripts", Order: 11)]
        public string SendScript3 { get => sendScript3; set => sendScript3 = value; }
        [Display("Send Script4", Group: "Scripts", Order: 13)]
        public string SendScript4 { get => sendScript4; set => sendScript4 = value; }
        [Display("Send Script5", Group: "Scripts", Order: 15)]
        public string SendScript5 { get => sendScript5; set => sendScript5 = value; }
        [Display("Send Script6", Group: "Scripts", Order: 17)]
        public string SendScript6 { get => sendScript6; set => sendScript6 = value; }
        [Display("Send Script7", Group: "Scripts", Order: 19)]
        public string SendScript7 { get => sendScript7; set => sendScript7 = value; }

        [Display("Send Script8", Group: "Scripts", Order: 21)]
        public string SendScript8 { get => sendScript8; set => sendScript8 = value; }

        [Display("Validate Script8", Group: "Scripts", Order: 23)]
        public string ValidateScript8 { get => validateScript8; set => validateScript8 = value; }

        [Display("Send Script9", Group: "Scripts", Order: 24)]
        public string SendScript9 { get => sendScript9; set => sendScript9 = value; }

        [Display("Send Script10", Group: "Scripts", Order: 24.5)]
        public string SendScript10 { get => sendScript10; set => sendScript10 = value; }

        [Display("CH1 Cable loss", Group: "Cable loss", Order: 25, Description: "If start port and end port from 1 - 4")]

        public double Ch1CableLoss { get => ch1CableLoss; set => ch1CableLoss = value; }
        [Display("CH2 Cable loss", Group: "Cable loss", Order: 27, Description: "If start port and end port from 1 - 4")]
        public double Ch2CableLoss { get => ch2CableLoss; set => ch2CableLoss = value; }
        [Display("CH3 Cable loss", Group: "Cable loss", Order: 29, Description: "If start port and end port from 1 - 4")]
        public double Ch3CableLoss { get => ch3CableLoss; set => ch3CableLoss = value; }
        [Display("CH4 Cable loss", Group: "Cable loss", Order: 31, Description: "If start port and end port from 1 - 4")]
        public double Ch4CableLoss { get => ch4CableLoss; set => ch4CableLoss = value; }

        [Display("CH5 Cable loss", Group: "Cable loss", Order: 31.1, Description: "If start port and end port from 5 - 8")]
        public double Ch5CableLoss { get => ch5CableLoss; set => ch5CableLoss = value; }

        [Display("CH6 Cable loss", Group: "Cable loss", Order: 31.2, Description: "If start port and end port from 5 - 8")]
        public double Ch6CableLoss { get => ch6CableLoss; set => ch6CableLoss = value; }

        [Display("CH7 Cable loss", Group: "Cable loss", Order: 31.3, Description: "If start port and end port from 5 - 8")]
        public double Ch7CableLoss { get => ch7CableLoss; set => ch7CableLoss = value; }

        [Display("CH8 Cable loss", Group: "Cable loss", Order: 31.4, Description: "If start port and end port from 5 - 8")]
        public double Ch8CableLoss { get => ch8CableLoss; set => ch8CableLoss = value; }

        [Display("Hex Value 1", Group: "DSA Values", Order: 33)]
        public string HexValue1 { get => hexValue1; set => hexValue1 = value; }
        [Display("Hex Value 2", Group: "DSA Values", Order: 35)]
        public string HexValue2 { get => hexValue2; set => hexValue2 = value; }
        [Display("Hex Value 3", Group: "DSA Values", Order: 37)]
        public string HexValue3 { get => hexValue3; set => hexValue3 = value; }
        [Display("Hex Value 4", Group: "DSA Values", Order: 39)]
        public string HexValue4 { get => hexValue4; set => hexValue4 = value; }
        [Display("Channel Power ", Group: "Power & Limit settings", Order: 41)]
        public double ChannelPower { get => channelPower; set => channelPower = value; }
        [Display("Channel Power Limit", Group: "Power & Limit settings", Order: 43)]
        public double ChannelPowerLimit { get => channelPowerLimit; set => channelPowerLimit = value; }

        [Display("MRU Settling time in ms", Group: "DUT Settling Time", Order: 45)]
        public int MRUSettlingTime { get => mRUSettlingTime; set => mRUSettlingTime = value; }
        public RjioReportCls RjioReport { get => rjioReport; set => rjioReport = value; }
        [Display("Starting Port Number", Group: "Port settings", Order: 47)]
        public int StartPort { get => startPort; set => startPort = value; }
        [Display("Ending Port Number", Group: "Port settings", Order: 49)]
        public int EndPort { get => endPort; set => endPort = value; }

        [Display("Enter DUT number", Order: 51)]
        public string DUTNumber { get => dUTNumber; set => dUTNumber = value; }
        #endregion
        public CalMRU()
        {
            // RjioReport.ChannelReport = new CalEndMeasurements[4];
            // ToDo: Set default values for properties / settings.
        }


        public override void PrePlanRun()
        {
            MRU.bootMode = String.Empty;
            base.PrePlanRun();
        }
        public override void Run()
        {
            string testStartTime = DateTime.Now.ToLongTimeString();
            string testEndTime = DateTime.Now.ToLongTimeString();
            stopwatch.Reset();
            stopwatch.Start();
            UpperChannelLimit = channelPower + channelPowerLimit;
            LowerChannelLimit = channelPower - channelPowerLimit;
            bool dpdLogin = false;
            string command = string.Empty;

            int CH1HEX = int.Parse(HexValue1, System.Globalization.NumberStyles.HexNumber);
            int CH2HEX = int.Parse(HexValue2, System.Globalization.NumberStyles.HexNumber);
            int CH3HEX = int.Parse(HexValue3, System.Globalization.NumberStyles.HexNumber);
            int CH4HEX = int.Parse(HexValue4, System.Globalization.NumberStyles.HexNumber);

            string hexTempValue = "./RFbEepromTool -updFRQ_SET 1 -updDSA_SET_TX " + $"0x{CH1HEX:X}" + "," + $"0x{CH2HEX:X}" + "," + $"0x{CH3HEX:X}" + "," + $"0x{CH4HEX:X}" + " -updDSA_SET_FB 0x0f,0x0f,0x0f,0x0f -updDAC_SET 1280,1700 -updS_PVLD";
            //MRU.StartMeasurements = false;
            lock (InstrumentLock)
            {
                if (!RestoreSecreensDone)
                {
                    E6680E.RestoreScreenDefaults();
                    RestoreSecreensDone = true;
                }
            }
            MRU.loginL2();
            lock (InstrumentLock)
            {

                if (!ScreenCreationgSettings)
                {
                    ScreenCreationgSettings = true;

                    E6680E.TechModeSelect(EXM_E6680A.ModeSelect.SEQAN);
                    E6680E.renameScreen("SEQ");
                    E6680E.CreateNewScreen();
                    E6680E.TechModeSelect(EXM_E6680A.ModeSelect.NR5G);
                    E6680E.renameScreen("EVM");
                    E6680E.SelectMeasurements(EXM_E6680A.LTEAFDD_MEASUREMENT_Type.EVM);
                    E6680E.MeasureContinues(false);
                    if (UseSCP)
                    {
                        E6680E.RecallSCPFile(ScpFilePath);
                        E6680E.TrackEVM();
                    }
                    else
                    {
                        E6680E.RecallStateFile(stateFilePath: StateFilePath);
                    }
                    // E6680E.RecallStateFile_SCreen(stateFilePath: StateFilePath);
                    // E6680E.renameScreen("EVM");
                    if (useSCP)
                    {
                        //E6680E.SSBAutoDetect(true);
                        //E6680E.PDCCHAutoDetect(true);
                    }
                    else
                    {
                        E6680E.DecodePBCHBitsNone();
                        E6680E.DecodePDCCHBitsNone();
                        E6680E.DecodePDSCHBitsNone();
                    }
                    E6680E.SetFrequency(3.54999e9);
                    E6680E.ViewOnlySummary();
                    E6680E.SelectInstScreen("SEQ");
                    E6680E.SetupSequencer();
                    E6680E.seqSourceFrequency(3.54999e9);
                    E6680E.MeasureContinues(false);

                    E6680E.TriggerBurstSelect();
                    E6680E.TriggerLevel(0);
                    E6680E.SequencePeakPower(seqPeakPower);
                    E6680E.SequenceExpectedPower(seqExpectedPower);
                }
            }
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                TapThread.Sleep(10);
                if (MRU.bootMode.Trim() == "EMMC_MODE")
                {
                    break;
                }
                if (sw.ElapsedMilliseconds > 30000)
                {
                    return;
                }
            }
            MRU.executeScripts(SendScript1, String.Empty);
            string mac1 = string.Empty, mac2 = string.Empty, mac3 = string.Empty, bootIndex = String.Empty, RFSOCFWVER = String.Empty, PCBSLNum = String.Empty;
            string RFBSerialNumber = string.Empty, RFBFWVer = string.Empty, RFBHWVer = string.Empty;

            MRU.executeScriptsReadBackIBTB(SendScript9, out mac1, out mac2, out mac3, out bootIndex, out RFSOCFWVER, out PCBSLNum);
            MRU.executeScriptsReadBackRFB(SendScript10, out RFBSerialNumber, out RFBFWVer, out RFBHWVer);
            MRU.executeScriptsReadBackRFB(SendScript10, out RFBSerialNumber, out RFBFWVer, out RFBHWVer);
            //Log.Info(DUTNumber + " MAC1          : " + mac1);
            //Log.Info(DUTNumber + " MAC2          : " + mac2);
            //Log.Info(DUTNumber + " MAC3          : " + mac3);
            //Log.Info(DUTNumber + " BootIndex     : " + bootIndex);
            //Log.Info(DUTNumber + " RF SOC FV VER : " + RFSOCFWVER);
            //Log.Info(DUTNumber + " PCB SL Num    : " + PCBSLNum);
            //Log.Info(DUTNumber + " RFB Serial Num: " + RFBSerialNumber);
            //Log.Info(DUTNumber + " RFB FW Ver    : " + RFBFWVer);
            //Log.Info(DUTNumber + " RFB HW Ver    : " + RFBHWVer);
            MRU.executeScripts(SendScript2, String.Empty);
            MRU.executeScripts(SendScript3, string.Empty);
            MRU.executeScripts(SendScript4, String.Empty);
            MRU.executeScripts(SendScript5, String.Empty);
            MRU.executeScripts(SendScript6, String.Empty);
            MRU.executeScripts(SendScript7, String.Empty);

            MRU.executeScripts(SendScript8, ValidateScript8);


            E6680E.SequenceAcquireSetup("636666", seqPeakPower, 1, 0);
            //E6680E.RecallRegistry(4);

            while (true)
            {
                TapThread.Sleep(10);
                if (MRU.StartMeasurements)
                {
                    Log.Info(DUTNumber + " ------------------------START MEASUREMNTS___________________________________________");
                    break;
                }
            }
            for (int ChannelNumber = StartPort; ChannelNumber <= EndPort; ChannelNumber++)
            {
                switch (ChannelNumber)
                {
                    case 1:
                    case 5:
                        command = "/custom-sw/MRU/bin/ibtb_init_allv6 5 " + $"0x{CH1HEX:X} 0x7f 0x7f 0x7f" + " 5 0x0f 0x0f 0x0f 0x0f 5 1280 1700 0";
                        break;
                    case 2:
                    case 6:
                        command = "/custom-sw/MRU/bin/ibtb_init_allv6 5 0x7f " + $"0x{CH2HEX:X}" + " 0x7f 0x7f" + " 5 0x0f 0x0f 0x0f 0x0f 5 1280 1700 0";
                        break;
                    case 3:
                    case 7:
                        command = "/custom-sw/MRU/bin/ibtb_init_allv6 5 0x7f 0x7f " + $"0x{CH3HEX:X}" + " 0x7f 5 0x0f 0x0f 0x0f 0x0f 5 1280 1700 0";
                        break;
                    case 4:
                    case 8:
                        command = "/custom-sw/MRU/bin/ibtb_init_allv6 5 " + "0x7f 0x7f 0x7f " + $"0x{CH4HEX:X}" + " 5 0x0f 0x0f 0x0f 0x0f 5 1280 1700 0";
                        break;
                    default:
                        break;
                }

                MRU.CaLexecuteScripts(command, String.Empty);
                Log.Info(DUTNumber + " " + ChannelNumber + " Command Sent : " + command);
                double MeasuredPowerValue = double.NaN;
                TapThread.Sleep(MRUSettlingTime);

                string[] resultStrings = new string[6];
                lock (MeasurementLock)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        do
                        {
                            E6680E.SetRFInputPort(ChannelNumber);
                            resultStrings = E6680E.ReadSequencerPower();
                            MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);

                        } while (MeasuredPowerValue < 0);
                    }  // tempvalue2 += MeasuredPowerValue;
                }
                // MeasuredPowerValue = tempvalue2 / 2;
                MeasuredPowerValue += Math.Abs(ChannelNumber == 1 ? Ch1CableLoss : (ChannelNumber == 2) ? Ch2CableLoss : (ChannelNumber == 3) ? Ch3CableLoss : (ChannelNumber == 4) ? Ch4CableLoss : (ChannelNumber == 5 ? Ch5CableLoss : (ChannelNumber == 6) ? Ch6CableLoss : (ChannelNumber == 7) ? Ch7CableLoss : Ch8CableLoss));

                Log.Info(DUTNumber + " " + ChannelNumber + " ------------------------Loop Start-----------------");
                var LoopStartTime = stopwatch.Elapsed;
                Log.Info(DUTNumber + " CH" + (ChannelNumber).ToString() + " Channel Power :" + MeasuredPowerValue + "dBm");
                double powerDifferance = 0;
                while (true)
                {
                    powerDifferance = Math.Abs(MeasuredPowerValue - ChannelPower);
                    if (MeasuredPowerValue <= LowerChannelLimit)
                    {
                        switch (ChannelNumber)
                        {
                            case 1:
                            case 5:
                                if (powerDifferance < 0.15)
                                {
                                    CH1HEX -= 1;
                                }
                                else if (powerDifferance <= 0.5)
                                {
                                    CH1HEX -= 2;
                                }
                                else if (powerDifferance > 0.5)
                                {
                                    int changeValue = (int)Math.Floor((powerDifferance / 0.25));
                                    CH1HEX -= changeValue;
                                    // CH1HEX -= (Math.Abs((changeValue * 0.25) - channelPower) > 0.15) ? 1 : 0;
                                }

                                break;
                            case 2:
                            case 6:

                                if (powerDifferance < 0.15)
                                {
                                    CH2HEX -= 1;
                                }
                                else if (powerDifferance < 0.5)
                                {
                                    CH2HEX -= 2;
                                }
                                else if (powerDifferance > 0.5)
                                {
                                    int changeValue = (int)Math.Floor((powerDifferance / 0.25));
                                    CH2HEX -= changeValue;
                                    // CH2HEX -= (Math.Abs((changeValue * 0.25) - channelPower) > 0.15) ? 1 : 0;
                                }
                                break;
                            case 3:
                            case 7:

                                if (powerDifferance < 0.15)
                                {
                                    CH3HEX -= 1;
                                }
                                else if (powerDifferance < 0.5)
                                {
                                    CH3HEX -= 2;
                                }
                                else if (powerDifferance > 0.5)
                                {
                                    int changeValue = (int)Math.Floor((powerDifferance / 0.25));
                                    CH3HEX -= changeValue;
                                    // CH3HEX -= (Math.Abs((changeValue * 0.25) - channelPower) > 0.15) ? 1 : 0;
                                }
                                break;
                            case 4:
                            case 8:
                                if (powerDifferance < 0.15)
                                {
                                    CH4HEX -= 1;
                                }
                                else if (powerDifferance < 0.5)
                                {
                                    CH4HEX -= 2;
                                }
                                else if (powerDifferance > 0.5)
                                {
                                    int changeValue = (int)Math.Floor((powerDifferance / 0.25));
                                    CH4HEX -= changeValue;
                                    //CH4HEX -= (Math.Abs((changeValue * 0.25) - channelPower) > 0.15) ? 1 : 0;
                                }
                                break;
                            default:
                                break;
                        }

                        // MRU.executeScripts("./RFbEepromTool -updFRQ_SET 1 -updDSA_SET_TX " + $"0x{CH1HEX:X}" + "," + $"0x{CH2HEX:X}" + "," + $"0x{CH3HEX:X}" + "," + $"0x{CH4HEX:X}" + " -updDSA_SET_FB 0x0f,0x0f,0x0f,0x0f -updDAC_SET 1280,1700 -updS_PVLD", string.Empty);
                    }
                    else if (MeasuredPowerValue >= UpperChannelLimit)
                    {
                        switch (ChannelNumber)
                        {
                            case 1:
                            case 5:
                                if (powerDifferance < 0.15)
                                {
                                    CH1HEX += 1;
                                }
                                else if (powerDifferance < 0.5)
                                {
                                    CH1HEX += 2;
                                }
                                if (powerDifferance > 0.5)
                                {
                                    int changeValue = (int)Math.Ceiling((powerDifferance / 0.25));
                                    CH1HEX += changeValue;
                                    // CH1HEX += (Math.Abs((changeValue * 0.25) - channelPower) > 0.15) ? 1 : 0;
                                }
                                break;
                            case 2:
                            case 6:
                                if (powerDifferance < 0.15)
                                {
                                    CH2HEX += 1;
                                }
                                else if (powerDifferance < 0.5)
                                {
                                    CH2HEX += 2;
                                }
                                if (powerDifferance > 0.5)
                                {
                                    int changeValue = (int)Math.Ceiling((powerDifferance / 0.25));
                                    CH2HEX += changeValue;
                                    // CH2HEX += (Math.Abs((changeValue * 0.25) - channelPower) > 0.15) ? 1 : 0;
                                }
                                break;
                            case 3:
                            case 7:
                                if (powerDifferance < 0.15)
                                {
                                    CH3HEX += 1;
                                }
                                else if (powerDifferance < 0.5)
                                {
                                    CH3HEX += 2;
                                }
                                if (powerDifferance > 0.5)
                                {
                                    int changeValue = (int)Math.Ceiling((powerDifferance / 0.25));
                                    CH3HEX += changeValue;
                                    // CH3HEX += (Math.Abs((changeValue * 0.25) - channelPower) > 0.15) ? 1 : 0;
                                }
                                break;
                            case 4:
                            case 8:
                                if (powerDifferance < 0.15)
                                {
                                    CH4HEX += 1;
                                }
                                else if (powerDifferance < 0.5)
                                {
                                    CH4HEX += 2;
                                }
                                if (powerDifferance > 0.5)
                                {
                                    int changeValue = (int)Math.Ceiling((powerDifferance / 0.25));
                                    CH4HEX += changeValue;
                                    //  CH4HEX += (Math.Abs((changeValue * 0.25) - channelPower) > 0.15) ? 1 : 0;
                                }
                                break;
                            default:
                                break;
                        }

                        // MRU.executeScripts("./RFbEepromTool -updFRQ_SET 1 -updDSA_SET_TX " + $"0x{CH1HEX:X}" + "," + $"0x{CH2HEX:X}" + "," + $"0x{CH3HEX:X}" + "," + $"0x{CH4HEX:X}" + " -updDSA_SET_FB 0x0f,0x0f,0x0f,0x0f -updDAC_SET 1280,1700 -updS_PVLD", string.Empty);
                    }
                    else
                    {
                        break;
                    }
                    switch (ChannelNumber)
                    {
                        case 1:
                        case 5:
                            command = "/custom-sw/MRU/bin/ibtb_init_allv6 5 " + $"0x{CH1HEX:X} 0x7f 0x7f 0x7f" + " 5 0x0f 0x0f 0x0f 0x0f 5 1280 1700 0";

                            break;
                        case 2:
                        case 6:
                            command = "/custom-sw/MRU/bin/ibtb_init_allv6 5 0x7f " + $"0x{CH2HEX:X}" + " 0x7f 0x7f" + " 5 0x0f 0x0f 0x0f 0x0f 5 1280 1700 0";

                            break;
                        case 3:
                        case 7:
                            command = "/custom-sw/MRU/bin/ibtb_init_allv6 5 0x7f 0x7f " + $"0x{CH3HEX:X}" + " 0x7f 5 0x0f 0x0f 0x0f 0x0f 5 1280 1700 0";

                            break;
                        case 4:
                        case 8:
                            command = "/custom-sw/MRU/bin/ibtb_init_allv6 5 " + "0x7f 0x7f 0x7f " + $"0x{CH4HEX:X}" + " 5 0x0f 0x0f 0x0f 0x0f 5 1280 1700 0";

                            break;
                        default:
                            break;
                    }
                    MRU.executeScripts(command, String.Empty);
                    Log.Info(DUTNumber + " " + ChannelNumber + " Command Sent : " + command);

                    lock (MeasurementLock)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            do
                            {
                                TapThread.Sleep(200);
                                E6680E.SetRFInputPort(ChannelNumber);
                                resultStrings = E6680E.ReadSequencerPower();
                                MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                            } while (MeasuredPowerValue < 0);
                            // tempValue1 += MeasuredPowerValue;
                        }
                    }
                    // MeasuredPowerValue = tempValue1 / 2;
                    MeasuredPowerValue += Math.Abs(ChannelNumber == 1 ? Ch1CableLoss : (ChannelNumber == 2) ? Ch2CableLoss : (ChannelNumber == 3) ? Ch3CableLoss : (ChannelNumber == 4) ? Ch4CableLoss : (ChannelNumber == 5 ? Ch5CableLoss : (ChannelNumber == 6) ? Ch6CableLoss : (ChannelNumber == 7) ? Ch7CableLoss : Ch8CableLoss));
                    // MeasuredPowerValue = E6680E.ReadSequencerPower();
                    Log.Info(DUTNumber + " CH" + (ChannelNumber).ToString() + " Channel Power : " + MeasuredPowerValue + "dBm");
                }
                Log.Info(DUTNumber + " " + ChannelNumber + " ------------------------Loop end ---------------------------");
                var loopEndtime = stopwatch.Elapsed;
                Log.Info(DUTNumber + " Channel :" + ChannelNumber + " Cal time :" + (loopEndtime - LoopStartTime).TotalMilliseconds);
                MRU.dPdExit((ChannelNumber - 1) % 4);
                if (!dpdLogin)
                {
                    var dpdLoginStarttime = stopwatch.Elapsed;
                    dpdLogin = MRU.loginDPD();
                    var dpdloginStopTime = stopwatch.Elapsed;
                    Log.Info(DUTNumber + " Channel :" + ChannelNumber + " Dpd Login time : " + (dpdloginStopTime - dpdLoginStarttime).Milliseconds);
                }
                if (dpdLogin)
                {
                    var dpdResetStart = stopwatch.Elapsed;
                    MRU.ResetCoefficientsdone = false;
                    MRU.dPdReset((ChannelNumber - 1) % 4);
                    var dpdResetStopTime = stopwatch.Elapsed;
                    Log.Info(DUTNumber + " Channel No:" + ChannelNumber + " Dpd Reset Time: " + (dpdResetStopTime - dpdResetStopTime).Milliseconds);
                }
                else
                {
                    // return;
                }

                // for (int k = 0; k < 8; k++)
                var varificationStartTime = stopwatch.Elapsed;
                TapThread.Sleep(2000);
                double[] ACPValues = new double[4];
                string[] modMeasurements = new string[40];
                lock (MeasurementLock)
                {
                    E6680E.SetRFInputPort(ChannelNumber);
                    for (int i = 0; i < 4; i++)
                    {
                        TapThread.Sleep(100);
                        resultStrings = E6680E.ReadSequencerPower();
                        MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                        MeasuredPowerValue += Math.Abs(ChannelNumber == 1 ? Ch1CableLoss : (ChannelNumber == 2) ? Ch2CableLoss : (ChannelNumber == 3) ? Ch3CableLoss : (ChannelNumber == 4) ? Ch4CableLoss : (ChannelNumber == 5 ? Ch5CableLoss : (ChannelNumber == 6) ? Ch6CableLoss : (ChannelNumber == 7) ? Ch7CableLoss : Ch8CableLoss));

                        if (MeasuredPowerValue > 37)
                        {
                            break;
                        }
                    }

                    ACPValues = new double[4] { Convert.ToDouble(resultStrings[67]), Convert.ToDouble(resultStrings[69]), Convert.ToDouble(resultStrings[71]), Convert.ToDouble(resultStrings[73]) };
                    Log.Info(DUTNumber + " channel" + ChannelNumber + "  ACLR Valuesfor : " + " : Lower A" + ACPValues[0] + " , Upper A " + ACPValues[1] + ", Lower B " + ACPValues[2] + " ,Upper B " + ACPValues[3]);
                    E6680E.SelectInstScreen("EVM");
                    E6680E.SetExternalPowerLoss(ChannelNumber == 1 ? Ch1CableLoss : (ChannelNumber == 2) ? Ch2CableLoss : (ChannelNumber == 3) ? Ch3CableLoss : (ChannelNumber == 4) ? Ch4CableLoss : (ChannelNumber == 5 ? Ch5CableLoss : (ChannelNumber == 6) ? Ch6CableLoss : (ChannelNumber == 7) ? Ch7CableLoss : Ch8CableLoss));
                    modMeasurements = E6680E.measureModulationRead();
                    var aa = Math.Abs(38 - MeasuredPowerValue);
                    var bb = Math.Abs(38 - Convert.ToDouble(modMeasurements[22]));
                    Log.Info(DUTNumber + " channel" + ChannelNumber + " Modulation measurements  : " + ",Channel Power : " + (aa < bb ? MeasuredPowerValue : Convert.ToDouble(modMeasurements[22]))/*Convert.ToDouble(modMeasurements[22])*/ + "dBm ,EVM : " + Convert.ToDouble(modMeasurements[1]) + "% ,Frequency Error : " + Convert.ToDouble(modMeasurements[3]) + "Hz;");
                    E6680E.SelectInstScreen("SEQ");
                    Log.Info("Vj" + Convert.ToDouble(modMeasurements[22]).ToString());
                }
                var varificationStopTime = stopwatch.Elapsed;
                Log.Info(DUTNumber + " Channel NO: " + ChannelNumber + " Varification time " + (varificationStopTime - varificationStartTime).TotalMilliseconds);
                switch (ChannelNumber)
                {
                    case 1:
                    case 5:
                        strCh1Measurements = MeasuredPowerValue + "," + Convert.ToDouble(modMeasurements[1]) + "," + ACPValues[0] + "," + +ACPValues[1] + "," + +ACPValues[2] + "," + ACPValues[3] + "," + Convert.ToDouble(modMeasurements[3]) + "," + ChannelNumber.ToString() + ", 3.54999e9";
                        break;
                    case 2:
                    case 6:
                        strCh2Measurements = MeasuredPowerValue + "," + Convert.ToDouble(modMeasurements[1]) + "," + ACPValues[0] + "," + +ACPValues[1] + "," + +ACPValues[2] + "," + ACPValues[3] + "," + Convert.ToDouble(modMeasurements[3]) + "," + ChannelNumber.ToString() + ", 3.54999e9";
                        break;
                    case 3:
                    case 7:
                        strCh3Measurements = MeasuredPowerValue + "," + Convert.ToDouble(modMeasurements[1]) + "," + ACPValues[0] + "," + +ACPValues[1] + "," + +ACPValues[2] + "," + ACPValues[3] + "," + Convert.ToDouble(modMeasurements[3]) + "," + ChannelNumber.ToString() + ", 3.54999e9";
                        break;
                    case 4:
                    case 8:
                        strCh4Measurements = MeasuredPowerValue + "," + Convert.ToDouble(modMeasurements[1]) + "," + ACPValues[0] + "," + +ACPValues[1] + "," + +ACPValues[2] + "," + ACPValues[3] + "," + Convert.ToDouble(modMeasurements[3]) + "," + ChannelNumber.ToString() + ", 3.54999e9";
                        break;
                    default:
                        break;
                }
                //    ChannelPower= MeasuredPowerValue , FrequencyError= Convert.ToDouble(modMeasurements[3]) , ACLR_NEG2 = Convert.ToDouble(resultStrings[67]),
                //    ACLR_NEG1 = Convert.ToDouble(resultStrings[69]), ACLR_POS1= Convert.ToDouble(resultStrings[71]), ACLR_POS2= Convert.ToDouble(resultStrings[73]) };
            }
            //RjioReport.MACID1 = mac1;
            //RjioReport.MACID2 = mac2;
            //RjioReport.MACID3 = mac3;
            //RjioReport.BootIndex= bootIndex;
            //RjioReport.PCBSRNUM = PCBSLNum;
            //RjioReport.RFBSLNUM = RFBSerialNumber;
            //RjioReport.RFBFWVer = RFBFWVer;
            //RjioReport.RFBHWVer = RFBHWVer;
            //RjioReport.DSA1 = $"0x{CH1HEX:X} ";
            //RjioReport.DSA2 = $"0x{CH2HEX:X} ";
            //RjioReport.DSA3 = $"0x{CH3HEX:X} ";
            //RjioReport.DSA4 = $"0x{CH4HEX:X} ";
            //RjioReport.PSN = "";

            Log.Info(DUTNumber + " MAC1          : " + mac1);
            Log.Info(DUTNumber + " MAC2          : " + mac2);
            Log.Info(DUTNumber + " MAC3          : " + mac3);
            Log.Info(DUTNumber + " BootIndex     : " + bootIndex);
            Log.Info(DUTNumber + " RF SOC FV VER : " + RFSOCFWVER);
            Log.Info(DUTNumber + " PCB SL Num    : " + PCBSLNum);
            Log.Info(DUTNumber + " RFB Serial Num: " + RFBSerialNumber);
            Log.Info(DUTNumber + " RFB FW Ver    : " + RFBFWVer);
            Log.Info(DUTNumber + " RFB HW Ver    : " + RFBHWVer);
            command = "/custom-sw/MRU/bin/ibtb_init_allv6 5 " + $"0x{CH1HEX:X} " + $"0x{CH2HEX:X} " + $"0x{CH3HEX:X} " + $"0x{CH4HEX:X} 5 0x0f 0x0f 0x0f 0x0f 5 1280 1700 0";
            Log.Info(DUTNumber + " Final script after calibration : " + command);
            MRU.executeScripts(command, string.Empty);
            RjioReportCls tempRep = new RjioReportCls
            {
                BootIndex = bootIndex,
                DSA1 = $"0x{CH1HEX:X} ",
                DSA2 = $"0x{CH2HEX:X} ",
                DSA3 = $"0x{CH3HEX:X} ",
                DSA4 = $"0x{CH4HEX:X} ",
                EMPID = "1123",
                MACID1 = mac1,
                MACID2 = mac2,
                MACID3 = mac3,
                MRU = "MRU",
                PCBSRNUM = PCBSLNum,
                PID = "PID",
                PSN = "PSN",
                RFBFWVer = RFBFWVer,
                RFBHWVer = RFBHWVer,
                RFBSLNUM = RFBSerialNumber,
                SWVersion = "SWVersion",
                TestStartTime = testStartTime,
                TestEndTime = DateTime.Now.ToLongTimeString(),
                testStage = "TestStage",
                TotalTestTime = "",
                Ch1Measurements = strCh1Measurements,
                Ch2Measurements = strCh2Measurements,
                Ch3Measurements = strCh3Measurements,
                Ch4Measurements = strCh4Measurements,
                DUTN = DUTNumber
            };
            Results.Publish<RjioReportCls>(DUTNumber, tempRep);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
