// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using Microsoft.Office.Interop.Excel;
using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static RjioMRU.EXM_E6680A;

namespace RjioMRU.TestSteps
{

    [Display("CalibrationStep Ch1", Group: "RjioMRU.Calibration", Description: "Insert a description here")]
    public class CalibrationStep_CH1 : TestStep
    {
        GeneralFunctions dsaConstruction = new GeneralFunctions();
        //tempevary variables.
        double measuredPowerValueBeforeDPD = double.NaN;
        bool ChannelPowerOk = false, EVMOK = false, ACLR_L1OK = false, ACLR_L2OK = false, ACLR_R1OK = false, ACLR_R2OK = false, FREQERROK = false;
        double eVMLimit = 3.5, aCLR_L1Limit = -45, aCLR_L2Limit = -45, aCLR_R1Limit = -45, aCLR_R2Limit = -45, fREQErrorLimit = 350;
        int AttemptNumber = 1;

        public static string[] StrChannelMeasurementsCh1 = new string[16] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        #region Settings
        int calStartPort = 0;
        int calEndPort = 1;
        Stopwatch stopwathCh1 = new Stopwatch();
        bool stepPassFlag = true;
        int DSACalCycles = 0;

        MRU_Rjio mRU_DUT;
        EXM_E6680A e6680InsturmentTrx1;
        EXM_E6680A e6680InsturmentTrx2;
        //EXM_E6680A e6680InsturmentTrx3;
        //EXM_E6680A e6680InsturmentTrx4;
        string[] strHexValues = new string[16];
        public int[] HexValues = new int[16];
        public static int[] HexValues4DSAWriging = new int[16];
        double[] CableLosses = new double[16];
        private string dSA_CableLossFile = "DSA_CABLELOSS_Ch1.csv";

        //private string[] hexValue =new string[16];
        //private double[] cableLoss = new double[16];

        double channelPowerLimit = 0.25;
        double channelPower = 38;
        double UpperChannelLimit;
        double LowerChannelLimit;
        int postDpdDelay = 1000;

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public CalibrationStep_CH1()
        {
            RjioReportCls.reportGenerated = false;

            // ToDo: Set default values for properties / settings.
        }
        [Display("E6680A TRX1", Group: "E6680A TRX", Order: 0)]
        public EXM_E6680A E6680InsturmentTrx1 { get => e6680InsturmentTrx1; set => e6680InsturmentTrx1 = value; }
        [Display("E6680A TRX2", Group: "E6680A TRX", Order: 1)]
        public EXM_E6680A E6680InsturmentTrx2 { get => e6680InsturmentTrx2; set => e6680InsturmentTrx2 = value; }
        //[Display("E6680A TRX3", Group: "E6680A TRX", Order: 2)]
        //public EXM_E6680A E6680InsturmentTrx3 { get => e6680InsturmentTrx3; set => e6680InsturmentTrx3 = value; }
        //[Display("E6680A TRX4", Group: "E6680A TRX", Order: 3)]
        //public EXM_E6680A E6680InsturmentTrx4 { get => e6680InsturmentTrx4; set => e6680InsturmentTrx4 = value; }

        [Display("DSA_Cable loss file", Order: 2)]
        public string DSA_CableLossFile { get => dSA_CableLossFile; set => dSA_CableLossFile = value; }

        [Display("Channel Power", Order: 3)]
        public double ChannelPower { get => channelPower; set => channelPower = value; }

        [Display("Channel Power Limit", Order: 4)]
        public double ChannelPowerLimit { get => channelPowerLimit; set => channelPowerLimit = value; }
        public MRU_Rjio MRU_DUT { get => mRU_DUT; set => mRU_DUT = value; }


        [Display("Calibration Ports(Start)", Group: "Cal Ports", Order: 0)]
        public int CalStartPort { get => calStartPort; set => calStartPort = value; }
        [Display("Calibration Ports(End)", Group: "Cal Ports", Order: 1)]
        public int CalEndPort { get => calEndPort; set => calEndPort = value; }
        [Display("Post DPD Delay in ms", Order: 10)]
        public int PostDpdDelay { get => postDpdDelay; set => postDpdDelay = value; }
        [Display("EVM Max Limit", Order: 15, Group: "Measurement Limits", Description: "Enter the maximum limit of the EVM")]
        public double EVMLimit { get => eVMLimit; set => eVMLimit = value; }
        [Display("ACLR Imm Next Left Channel Min Limit", Order: 17, Group: "Measurement Limits", Description: "Enter the minimum first left channel ACLR  limit ")]
        public double ACLR_L1_Limit { get => aCLR_L1Limit; set => aCLR_L1Limit = value; }
        [Display("ACLR Second Left Channel Min Limit", Order: 19, Group: "Measurement Limits", Description: "Enter the minimum Second left channel ACLR  limit ")]
        public double ACLR_L2_Limit { get => aCLR_L2Limit; set => aCLR_L2Limit = value; }
        [Display("ACLR Imm Right Channel Min Limit", Order: 21, Group: "Measurement Limits", Description: "Enter the minimum first right channel ACLR  limit ")]
        public double ACLR_R1_Limit { get => aCLR_R1Limit; set => aCLR_R1Limit = value; }

        [Display("ACLR Second Right Channel Min Limit", Order: 23, Group: "Measurement Limits", Description: "Enter the minimum second right channel ACLR  limit ")]

        public double ACLR_R2_Limit { get => aCLR_R2Limit; set => aCLR_R2Limit = value; }

        [Display("Frquency Error Limit", Order: 25, Group: "Measurement Limits", Description: "Enter the frequency error limit")]

        public double FREQError_Limit { get => fREQErrorLimit; set => fREQErrorLimit = value; }

        [Display("DSA setting number of trials ")]
        public int DSACalCycles1 { get => DSACalCycles; set => DSACalCycles = value; }

        private int hexValueHigherLmit = 0x7F;
        private int hexValueLowerLimit = 0x90;
        [Display("Hex Value Higher Limit", Order: 26, Group: "Measurement Limits", Description: "Enter the maximum hex value limit ")]
        public int HexValueHigherLmit { get => hexValueHigherLmit; set => hexValueHigherLmit = value; }
        [Display("Hex Value Lower Limit", Order: 27, Group: "Measurement Limits", Description: "Enter the minimum hex value limit ")]
        public int HexValueLowerLimit { get => hexValueLowerLimit; set => hexValueLowerLimit = value; }


        public override void Run()
        {
            int hexDecimalValueHigherLimit = Convert.ToInt32(HexValueHigherLmit);
            int hexDecimalValueLowerLimit = Convert.ToInt32(HexValueLowerLimit);

            int DSATrailsCount = 0;
            stopwathCh1.Restart();
            string DSACommand = string.Empty;

            //MRU_DUT.startReceiveEvent();                                         c
            MRU_DUT.Dr49_CH1_ControlC();
            E6680InsturmentTrx1.SelectInstScreen("SEQ");
            E6680InsturmentTrx2.SelectInstScreen("SEQ");

            E6680InsturmentTrx1.MeasureContinues(false);
            if (calEndPort > 7)
            {
                E6680InsturmentTrx2.MeasureContinues(false);
            }
            // E6680Insturment.SelectInstScreen("EVM");
            UpperChannelLimit = channelPower + channelPowerLimit;
            LowerChannelLimit = channelPower - channelPowerLimit;
            string[] resultStrings = new string[6];
            readDSA_CableLossFile(DSA_CableLossFile, out strHexValues, out CableLosses);
            double[] ACPValues = new double[4];

            for (int iteration = 0; iteration < 16; iteration++)
            {
                HexValues[iteration] = int.Parse(strHexValues[iteration], System.Globalization.NumberStyles.HexNumber);
            }


            try
            {
                for (int iteration = calStartPort; iteration <= CalEndPort; iteration++)
                {
                    DSATrailsCount = 0;
                    EVMOK = false;
                    ACLR_L1OK = false;
                    ACLR_L2OK = false;
                    ACLR_R1OK = false;
                    ACLR_R2OK = false;
                    FREQERROK = false;
                    ChannelPowerOk = false;
                    AttemptNumber = 1;
                    if (iteration <= 7)
                        E6680InsturmentTrx1.SetRFInputPort((iteration % 8) + 1);
                    else
                        E6680InsturmentTrx2.SetRFInputPort((iteration % 8) + 1);

                    DSACommand = dsaConstruction.GenerateCommand(iteration, HexValues[iteration]);
                    Log.Info("Initialization Command for Ch" + iteration + " " + DSACommand);
                    MRU_DUT.DR49CH1executeCALDSAScripts(DSACommand, "rjInitialConfiguration Completed");
                    TapThread.Sleep(2000);
                    double MeasuredPowerValue = double.NaN;
                    if (iteration == 1)
                    {
                        Thread.Sleep(1000);
                    }
                    for (int l = 0; l < 5; l++)
                    {
                        try
                        {
                            resultStrings = (iteration <= 7) ? E6680InsturmentTrx1.ReadSequencerPower() : E6680InsturmentTrx2.ReadSequencerPower();

                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                            break;
                        }
                        // var resutlStrings = E6680Insturment.measureModulationRead();
                        if (resultStrings.Length < 5)
                        {
                            continue;
                        }
                        else
                        {
                            MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                            if (l > 0)
                            {
                                break;
                            }

                        }
                    }
                    if (resultStrings.Length < 5 || MeasuredPowerValue < 0)
                    {
                        StrChannelMeasurementsCh1[iteration] = iteration + "," + $" 0x{HexValues[iteration]:X}" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "" + "," + "";
                        continue;
                    }
                    ACPValues = new double[4] { Convert.ToDouble(resultStrings[67]), Convert.ToDouble(resultStrings[69]), Convert.ToDouble(resultStrings[71]), Convert.ToDouble(resultStrings[73]) };

                    MeasuredPowerValue += (CableLosses[iteration] * -1);
                    measuredPowerValueBeforeDPD = MeasuredPowerValue;
                    double powerDifferance = 0;
                    Log.Info("Initial Power before cal Ch" + iteration + " Is :" + MeasuredPowerValue.ToString());
                    while (true)
                    {
                        DSATrailsCount++;
                        if (DSATrailsCount >= DSACalCycles1)
                        {
                            break;
                        }
                        powerDifferance = Math.Abs(MeasuredPowerValue - ChannelPower);
                        if (MeasuredPowerValue <= LowerChannelLimit)
                        {
                            if (powerDifferance > 0.5)
                            {
                                int changeValue = (int)Math.Floor((powerDifferance / 0.25));
                                HexValues[iteration] -= changeValue;
                            }
                            else
                            {
                                HexValues[iteration] -= 1;
                            }
                        }
                        else if (MeasuredPowerValue >= UpperChannelLimit)
                        {
                            if (powerDifferance > 0.5)
                            {
                                int changeValue = (int)Math.Ceiling((powerDifferance / 0.25));
                                HexValues[iteration] += changeValue;
                            }
                            else
                            {
                                HexValues[iteration] += 1;
                            }
                        }
                        else
                        {
                            #region existing

                            string[] ACP5GValues;
                            if (iteration <= 7)
                            {
                                E6680InsturmentTrx1.SelectInstScreen("ACP");
                                E6680InsturmentTrx1.SetExternalPowerLoss(CableLosses[iteration]);
                                ACP5GValues = E6680InsturmentTrx1.measureACP();
                            }
                            else
                            {
                                E6680InsturmentTrx2.SelectInstScreen("ACP");
                                E6680InsturmentTrx2.SetExternalPowerLoss(CableLosses[iteration]);
                                ACP5GValues = E6680InsturmentTrx2.measureACP();
                            }

                            //E6680InsturmentTrx1.SetRFInputPort((iteration % 8) + 1);
                            //    TapThread.Sleep(1000);
                            //    resultStrings = ((iteration <= 7) ? E6680InsturmentTrx1.ReadSequencerPower() : E6680InsturmentTrx2.ReadSequencerPower());
                            //    MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                            //    ACPValues = new double[4] { Convert.ToDouble(resultStrings[67]), Convert.ToDouble(resultStrings[69]), Convert.ToDouble(resultStrings[71]), Convert.ToDouble(resultStrings[73]) };
                            //    MeasuredPowerValue += (CableLosses[iteration] * -1);
                            if (Convert.ToDouble(ACP5GValues[4]) > -45)
                            {
                                var DpdStartTime = stopwathCh1.ElapsedMilliseconds;
                                MRU_DUT.DR49CH1Jjio_DPD_InitRun(iteration);
                                var DpdStopTime = stopwathCh1.ElapsedMilliseconds;
                                Log.Info("DPD init Run TIme for Ch1 and chain {0} is {1}", iteration, (DpdStopTime - DpdStartTime) / 1000);
                                TapThread.Sleep(PostDpdDelay);
                            }
                            if (iteration <= 7)
                            {
                                ACP5GValues = E6680InsturmentTrx1.measureACP();
                            }
                            else
                            {
                                ACP5GValues = E6680InsturmentTrx2.measureACP();



                            }
                            var dpdMEasurementStartTime = stopwathCh1.ElapsedMilliseconds;
                            TapThread.Sleep(1000);
                            //MRU_DUT.Dr49_CH1_DPD_Measurement(iteration, out var txvalue, out var rxvalue);
                            var dpdMeasurementStopTime = stopwathCh1.ElapsedMilliseconds;
                            Log.Info("DPD Measurement time for Ch1 and chain {0} is {1} ", iteration, (dpdMeasurementStopTime - dpdMEasurementStartTime) / 1000);
                            if (iteration <= 7)
                            {
                                E6680InsturmentTrx1.SelectInstScreen("SEQ");
                            }
                            else
                            {
                                E6680InsturmentTrx2.SelectInstScreen("SEQ");
                            }
                            for (int j = 0; j < 5; j++)
                            {
                                resultStrings = ((iteration <= 7) ? E6680InsturmentTrx1.ReadSequencerPower() : E6680InsturmentTrx2.ReadSequencerPower());
                                if (resultStrings.Length < 5)
                                {
                                    continue;
                                }
                                else
                                {

                                    if (j > 0)
                                    {
                                        break;
                                    }

                                }

                            }
                            if (resultStrings.Length < 5)
                            {
                                continue;
                            }
                            MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                            //ACP Values L2 L1 H1 H2
                            ACPValues = new double[4] { Convert.ToDouble(ACP5GValues[8]), Convert.ToDouble(ACP5GValues[4]), Convert.ToDouble(ACP5GValues[6]), Convert.ToDouble(ACP5GValues[10]) };
                            MeasuredPowerValue += (CableLosses[iteration] * -1);

                            Log.Info("CH1 DSA Command Used: " + DSACommand);
                            Log.Info("CH1 Chain NO:" + iteration + " Channel Power : " + MeasuredPowerValue + "dBm ACP1: " + ACPValues[0] + " ACP2 : " + ACPValues[1] + " ACP3 : " + ACPValues[2] + " ACP4 : " + ACPValues[3]);
                            //Log.Info("CH1 Chain NO:" + iteration + " DPD TxValue :" + txvalue + " RxValue :" + rxvalue);
                            //StrChannelMeasurementsCh1[iteration] = iteration + "," + $" 0x{HexValues[iteration]:X}" + "," + MeasuredPowerValue + "," + ACPValues[0] + "," + ACPValues[1] + "," + ACPValues[2] + "," + ACPValues[3] + "," + txvalue + "," + rxvalue;

                            // Log.Info("CH1 Chain NO:" + iteration + " DPD TxValue :" + txvalue + " RxValue :" + rxvalue);
                            StrChannelMeasurementsCh1[iteration] = iteration + "," + $" 0x{HexValues[iteration]:X}" + "," + MeasuredPowerValue + "," + ACPValues[0] + "," + ACPValues[1] + "," + ACPValues[2] + "," + ACPValues[3] + "," + "" + "," + "";
                            ACLR_L1OK = ACLR_L1_Limit >= ACPValues[0];
                            ACLR_L2OK = ACLR_L2_Limit >= ACPValues[1];
                            ACLR_R1OK = ACLR_R1_Limit >= ACPValues[2];
                            ACLR_R2OK = ACLR_R2_Limit >= ACPValues[3];
                            /////////////////Modulation Measuremnts /////////////////////
                            var varificationStartTime = stopwathCh1.ElapsedMilliseconds;
                            if (iteration <= 7)
                            {
                                E6680InsturmentTrx1.SelectInstScreen("EVM");
                                E6680InsturmentTrx1.SetExternalPowerLoss(CableLosses[iteration]);
                                E6680InsturmentTrx1.SetRFInputPort((iteration % 8) + 1);

                            }
                            else
                            {
                                E6680InsturmentTrx2.SelectInstScreen("EVM");
                                E6680InsturmentTrx2.SetExternalPowerLoss(CableLosses[iteration]);
                                E6680InsturmentTrx2.SetRFInputPort((iteration % 8) + 1);
                            }
                            resultStrings = ((iteration <= 7) ? E6680InsturmentTrx1.measureModulationRead() : E6680InsturmentTrx2.measureModulationRead());
                            var varificationStopTIme = stopwathCh1.ElapsedMilliseconds;
                            Log.Info("Varification time for Ch1 chaing {0} is {1}", iteration, (varificationStopTIme - varificationStartTime) / 1000);

                            Log.Info(" CH1 ChainNo" + iteration + " Modulation measurements  :Channel Power : " + resultStrings[22] + "dBm ,EVM : " + Convert.ToDouble(resultStrings[1]) + "% ,Frequency Error : " + Convert.ToDouble(resultStrings[3]) + "Hz;");
                            if (resultStrings.Length < 5)
                            {
                                Log.Error("Skipping Chain No " + iteration + " because of result length " + resultStrings.Length.ToString());
                                continue;
                            }
                            else
                            {
                                MeasuredPowerValue = (Math.Abs((ChannelPower - MeasuredPowerValue)) > (Math.Abs(channelPower - Convert.ToDouble(resultStrings[22])))) ? Convert.ToDouble(resultStrings[22]) : MeasuredPowerValue;
                                StrChannelMeasurementsCh1[iteration] += "," + resultStrings[1] + "," + resultStrings[3] + "," + measuredPowerValueBeforeDPD;
                            }
                            if (iteration <= 7)
                            {
                                E6680InsturmentTrx1.SelectInstScreen("SEQ");
                            }
                            else
                            {
                                E6680InsturmentTrx2.SelectInstScreen("SEQ");
                            }
                            ChannelPowerOk = ChannelPowerLimit > Math.Abs((ChannelPower - MeasuredPowerValue));
                            EVMOK = EVMLimit >= Convert.ToDouble(resultStrings[1]);
                            FREQERROK = fREQErrorLimit >= Math.Abs(Convert.ToDouble(resultStrings[3]));
                            AttemptNumber++;
                            /////////////////////////////////////////////////////////////
                            if ((ChannelPowerOk && ACLR_R1OK && ACLR_L2OK && ACLR_R1OK && ACLR_R2OK && FREQERROK && EVMOK) || AttemptNumber > 2)
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                            #endregion existing

                        }



                        if (hexDecimalValueHigherLimit < HexValues[iteration] || hexDecimalValueLowerLimit > HexValues[iteration])
                        {
                            break;
                        }
                        ///Calibraiton logic starts........................................................................................
                        DSACommand = dsaConstruction.GenerateCommand(iteration, HexValues[iteration]);
                        MRU_DUT.DR49CH1executeCALDSAScripts(DSACommand, "rjInitialConfiguration Completed");
                        TapThread.Sleep(2000);
                        //TapThread.Sleep(10000);
                        for (int j = 0; j < 5; j++)
                        {
                            //do
                            //{
                            TapThread.Sleep(1000);
                            //  E6680Insturment.SetRFInputPort((iteration+1)%9);
                            try
                            {


                                resultStrings = ((iteration <= 7) ? E6680InsturmentTrx1.ReadSequencerPower() : E6680InsturmentTrx2.ReadSequencerPower());
                            }
                            catch (Exception ex)
                            {
                                Log.Info("Exception/ CH-1 at CAL-DSA Script: {0}", ex);
                                break;
                            }
                            /// resultStrings = E6680Insturment.measureModulationRead();
                            MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                            //  MeasuredPowerValue = Convert.ToDouble(resultStrings[22]);
                            if (resultStrings.Length < 5)
                            {
                                continue;
                            }
                            else
                            {
                                MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                                if (j > 0)
                                {
                                    break;
                                }

                            }
                            // } while (MeasuredPowerValue < -5 || TapThread.Current.AbortToken.IsCancellationRequested);
                        }
                        if (resultStrings.Length < 5 || MeasuredPowerValue < 0)
                        {
                            StrChannelMeasurementsCh1[iteration] = iteration + "," + $" 0x{HexValues[iteration]:X}" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "" + "," + "";
                            continue;
                        }
                        else
                        {
                            HexValues4DSAWriging[iteration] = HexValues[iteration];
                            ACPValues = new double[4] { Convert.ToDouble(resultStrings[67]), Convert.ToDouble(resultStrings[69]), Convert.ToDouble(resultStrings[71]), Convert.ToDouble(resultStrings[73]) };
                            MeasuredPowerValue += (CableLosses[iteration] * -1);
                            measuredPowerValueBeforeDPD = MeasuredPowerValue;
                        }
                        Log.Info("CH1 during Chain : " + iteration + " Cal Measured power value :" + MeasuredPowerValue);
                    }

                    // ToDo: Add test case code.
                    RunChildSteps(); //If the step supports child steps.

                    // If no verdict is used, the verdict will default to NotSet.
                    // You can change the verdict using UpgradeVerdict() as shown below.
                    // UpgradeVerdict(Verdict.Pass);
                    var totalCh1CalTime = stopwathCh1.Elapsed;
                    Log.Info("Total Ch1 Cal Time : " + (totalCh1CalTime.TotalMilliseconds / 1000).ToString());
                    stepPassFlag &= ChannelPowerOk && ACLR_R1OK && ACLR_L2OK && ACLR_R2OK && ACLR_L2OK && FREQERROK && EVMOK;

                }
            }
            catch (Exception ex)
            {
                CCDUServer.loopBreak = true;
                Log.Info("Exception/ CH-1: {0}", ex);
            }

            // MRU_DUT.stopReceiveEvent();
            if (stepPassFlag)
            {
                UpgradeVerdict(Verdict.Pass);

            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
        }
        /*
        public override void Run()
        {
            EXM_E6680A E6680InsturmentComman = new EXM_E6680A(); ;
            int DSATrailsCount = 0;
            stopwathCh1.Restart();
            string DSACommand = string.Empty;

            //MRU_DUT.startReceiveEvent();                                         c
            MRU_DUT.Dr49_CH1_ControlC();
            E6680InsturmentTrx1.SelectInstScreen("SEQ");
            E6680InsturmentTrx2.SelectInstScreen("SEQ");

            E6680InsturmentTrx1.MeasureContinues(false);
            if (calEndPort > 7)
            {
                E6680InsturmentTrx2.MeasureContinues(false);
            }
            // E6680Insturment.SelectInstScreen("EVM");
            UpperChannelLimit = channelPower + channelPowerLimit;
            LowerChannelLimit = channelPower - channelPowerLimit;
            string[] resultStrings = new string[6];
            readDSA_CableLossFile(DSA_CableLossFile, out strHexValues, out CableLosses);
            double[] ACPValues = new double[4];

            for (int iteration = 0; iteration < 16; iteration++)
            {
                HexValues[iteration] = int.Parse(strHexValues[iteration], System.Globalization.NumberStyles.HexNumber);
            }


            try
            {
                for (int iteration = calStartPort; iteration <= CalEndPort; iteration++)
                {
                    DSATrailsCount = 0;
                    EVMOK = false;
                    ACLR_L1OK = false;
                    ACLR_L2OK = false;
                    ACLR_R1OK = false;
                    ACLR_R2OK = false;
                    FREQERROK = false;
                    ChannelPowerOk = false;
                    AttemptNumber = 1;
                    if (iteration <= 7)
                        E6680InsturmentTrx1.SetRFInputPort((iteration % 8) + 1);
                    else
                        E6680InsturmentTrx2.SetRFInputPort((iteration % 8) + 1);

                    DSACommand = dsaConstruction.GenerateCommand(iteration, HexValues[iteration]);
                    Log.Info("Initialization Command for Ch" + iteration + " " + DSACommand);
                    MRU_DUT.DR49CH1executeCALDSAScripts(DSACommand, "rjInitialConfiguration Completed");
                    TapThread.Sleep(2000);
                    double MeasuredPowerValue = double.NaN;
                    if(iteration == 1)
                    {
                        Thread.Sleep(1000);
                    }
                    for (int l = 0; l < 2; l++)
                    {
                        try
                        {
                        resultStrings = (iteration <= 7) ? E6680InsturmentTrx1.ReadSequencerPower() : E6680InsturmentTrx2.ReadSequencerPower();

                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                            break;
                        }
                        // var resutlStrings = E6680Insturment.measureModulationRead();
                        if (resultStrings.Length < 5)
                        {
                            continue;
                        }
                        MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                    }
                    if (resultStrings.Length < 5 || MeasuredPowerValue < 0)
                    {
                        StrChannelMeasurementsCh1[iteration] = iteration + "," + $" 0x{HexValues[iteration]:X}" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "" + "," + "";
                        continue;
                    }
                    ACPValues = new double[4] { Convert.ToDouble(resultStrings[67]), Convert.ToDouble(resultStrings[69]), Convert.ToDouble(resultStrings[71]), Convert.ToDouble(resultStrings[73]) };

                    MeasuredPowerValue += (CableLosses[iteration] * -1);
                    measuredPowerValueBeforeDPD = MeasuredPowerValue;
                    double powerDifferance = 0;
                    Log.Info("Initial Power before cal Ch" + iteration + " Is :" + MeasuredPowerValue.ToString());
                    while (true)
                    {
                        DSATrailsCount++;
                        if (DSATrailsCount >= DSACalCycles1)
                        {
                            break;
                        }
                        powerDifferance = Math.Abs(MeasuredPowerValue - ChannelPower);
                        if (MeasuredPowerValue <= LowerChannelLimit)
                        {
                            if (powerDifferance > 0.5)
                            {
                                int changeValue = (int)Math.Floor((powerDifferance / 0.25));
                                HexValues[iteration] -= changeValue;
                            }
                            else
                            {
                                HexValues[iteration] -= 1;
                            }
                        }
                        else if (MeasuredPowerValue >= UpperChannelLimit)
                        {
                            if (powerDifferance > 0.5)
                            {
                                int changeValue = (int)Math.Ceiling((powerDifferance / 0.25));
                                HexValues[iteration] += changeValue;
                            }
                            else
                            {
                                HexValues[iteration] += 1;
                            }
                        }
                        else
                        {
                            #region existing

                            string[] ACP5GValues;
                            if (iteration <= 7)
                            {
                                E6680InsturmentTrx1.SelectInstScreen("ACP");
                                E6680InsturmentTrx1.SetExternalPowerLoss(CableLosses[iteration]);
                                ACP5GValues = E6680InsturmentTrx1.measureACP();
                            }
                            else
                            {
                                E6680InsturmentTrx2.SelectInstScreen("ACP");
                                E6680InsturmentTrx2.SetExternalPowerLoss(CableLosses[iteration]);
                                ACP5GValues = E6680InsturmentTrx2.measureACP();
                            }

                            //E6680InsturmentTrx1.SetRFInputPort((iteration % 8) + 1);
                            //    TapThread.Sleep(1000);
                            //    resultStrings = ((iteration <= 7) ? E6680InsturmentTrx1.ReadSequencerPower() : E6680InsturmentTrx2.ReadSequencerPower());
                            //    MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                            //    ACPValues = new double[4] { Convert.ToDouble(resultStrings[67]), Convert.ToDouble(resultStrings[69]), Convert.ToDouble(resultStrings[71]), Convert.ToDouble(resultStrings[73]) };
                            //    MeasuredPowerValue += (CableLosses[iteration] * -1);
                            if (Convert.ToDouble(ACP5GValues[4]) > -45)
                            {
                                var DpdStartTime = stopwathCh1.ElapsedMilliseconds;
                                MRU_DUT.DR49CH1Jjio_DPD_InitRun(iteration);
                                var DpdStopTime = stopwathCh1.ElapsedMilliseconds;
                                Log.Info("DPD init Run TIme for Ch1 and chain {0} is {1}", iteration, (DpdStopTime - DpdStartTime) / 1000);
                                TapThread.Sleep(PostDpdDelay);
                                if (iteration <= 7)
                                {
                                    ACP5GValues = E6680InsturmentTrx1.measureACP();
                                }
                                else
                                {
                                    ACP5GValues = E6680InsturmentTrx2.measureACP();

                                }

                            }
                            var dpdMEasurementStartTime = stopwathCh1.ElapsedMilliseconds;
                            TapThread.Sleep(1000);
                            //MRU_DUT.Dr49_CH1_DPD_Measurement(iteration, out var txvalue, out var rxvalue);
                            var dpdMeasurementStopTime = stopwathCh1.ElapsedMilliseconds;
                            Log.Info("DPD Measurement time for Ch1 and chain {0} is {1} ", iteration, (dpdMeasurementStopTime - dpdMEasurementStartTime) / 1000);
                            if (iteration <= 7)
                            {
                                E6680InsturmentTrx1.SelectInstScreen("SEQ");
                            }
                            else
                            {
                                E6680InsturmentTrx2.SelectInstScreen("SEQ");
                            }

                            resultStrings = ((iteration <= 7) ? E6680InsturmentTrx1.ReadSequencerPower() : E6680InsturmentTrx2.ReadSequencerPower());
                            MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                            //ACP Values L2 L1 H1 H2
                            ACPValues = new double[4] { Convert.ToDouble(ACP5GValues[8]), Convert.ToDouble(ACP5GValues[4]), Convert.ToDouble(ACP5GValues[6]), Convert.ToDouble(ACP5GValues[10]) };
                            MeasuredPowerValue += (CableLosses[iteration] * -1);

                            Log.Info("CH1 DSA Command Used: " + DSACommand);
                            Log.Info("CH1 Chain NO:" + iteration + " Channel Power : " + MeasuredPowerValue + "dBm ACP1: " + ACPValues[0] + " ACP2 : " + ACPValues[1] + " ACP3 : " + ACPValues[2] + " ACP4 : " + ACPValues[3]);
                            //Log.Info("CH1 Chain NO:" + iteration + " DPD TxValue :" + txvalue + " RxValue :" + rxvalue);
                            //StrChannelMeasurementsCh1[iteration] = iteration + "," + $" 0x{HexValues[iteration]:X}" + "," + MeasuredPowerValue + "," + ACPValues[0] + "," + ACPValues[1] + "," + ACPValues[2] + "," + ACPValues[3] + "," + txvalue + "," + rxvalue;

                            // Log.Info("CH1 Chain NO:" + iteration + " DPD TxValue :" + txvalue + " RxValue :" + rxvalue);
                            StrChannelMeasurementsCh1[iteration] = iteration + "," + $" 0x{HexValues[iteration]:X}" + "," + MeasuredPowerValue + "," + ACPValues[0] + "," + ACPValues[1] + "," + ACPValues[2] + "," + ACPValues[3] + "," + "" + "," + "";
                            ACLR_L1OK = ACLR_L1_Limit >= ACPValues[0];
                            ACLR_L2OK = ACLR_L2_Limit >= ACPValues[1];
                            ACLR_R1OK = ACLR_R1_Limit >= ACPValues[2];
                            ACLR_R2OK = ACLR_R2_Limit >= ACPValues[3];
                            /////////////////Modulation Measuremnts /////////////////////
                            var varificationStartTime = stopwathCh1.ElapsedMilliseconds;
                            if (iteration <= 7)
                            {
                                E6680InsturmentTrx1.SelectInstScreen("EVM");
                                E6680InsturmentTrx1.SetExternalPowerLoss(CableLosses[iteration]);
                                E6680InsturmentTrx1.SetRFInputPort((iteration % 8) + 1);

                            }
                            else
                            {
                                E6680InsturmentTrx2.SelectInstScreen("EVM");
                                E6680InsturmentTrx2.SetExternalPowerLoss(CableLosses[iteration]);
                                E6680InsturmentTrx2.SetRFInputPort((iteration % 8) + 1);
                            }
                            resultStrings = ((iteration <= 7) ? E6680InsturmentTrx1.measureModulationRead() : E6680InsturmentTrx2.measureModulationRead());
                            var varificationStopTIme = stopwathCh1.ElapsedMilliseconds;
                            Log.Info("Varification time for Ch1 chaing {0} is {1}", iteration, (varificationStopTIme - varificationStartTime) / 1000);

                            Log.Info(" CH1 ChainNo" + iteration + " Modulation measurements  :Channel Power : " + resultStrings[22] + "dBm ,EVM : " + Convert.ToDouble(resultStrings[1]) + "% ,Frequency Error : " + Convert.ToDouble(resultStrings[3]) + "Hz;");
                            MeasuredPowerValue = (Math.Abs((ChannelPower - MeasuredPowerValue)) > (Math.Abs(channelPower - Convert.ToDouble(resultStrings[22])))) ? Convert.ToDouble(resultStrings[22]) : MeasuredPowerValue;
                            StrChannelMeasurementsCh1[iteration] += "," + resultStrings[1] + "," + resultStrings[3] + "," + measuredPowerValueBeforeDPD;
                            if (iteration <= 7)
                            {
                                E6680InsturmentTrx1.SelectInstScreen("SEQ");
                            }
                            else
                            {
                                E6680InsturmentTrx2.SelectInstScreen("SEQ");
                            }
                            ChannelPowerOk = ChannelPowerLimit > Math.Abs((ChannelPower - MeasuredPowerValue));
                            EVMOK = EVMLimit >= Convert.ToDouble(resultStrings[1]);
                            FREQERROK = fREQErrorLimit >= Math.Abs(Convert.ToDouble(resultStrings[3]));
                            AttemptNumber++;
                            /////////////////////////////////////////////////////////////
                            if ((ChannelPowerOk && ACLR_R1OK && ACLR_L2OK && ACLR_R1OK && ACLR_R2OK && FREQERROK && EVMOK) || AttemptNumber > 2)
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                            #endregion existing

                        }

                        ///Calibraiton logic starts........................................................................................
                        DSACommand = dsaConstruction.GenerateCommand(iteration, HexValues[iteration]);
                        MRU_DUT.DR49CH1executeCALDSAScripts(DSACommand, "rjInitialConfiguration Completed");
                        for (int j = 0; j < 2; j++)
                        {
                            //do
                            //{
                            TapThread.Sleep(300);
                            //  E6680Insturment.SetRFInputPort((iteration+1)%9);
                            try
                            {


                            resultStrings = ((iteration <= 7) ? E6680InsturmentTrx1.ReadSequencerPower() : E6680InsturmentTrx2.ReadSequencerPower());
                            }
                            catch (Exception ex)
                            {
                                Log.Info("Exception/ CH-1 at CAL-DSA Script: {0}", ex);
                                break;
                            }
                            /// resultStrings = E6680Insturment.measureModulationRead();
                            MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                            //  MeasuredPowerValue = Convert.ToDouble(resultStrings[22]);
                            if (resultStrings.Length < 5)
                            {
                                continue;
                            }
                            // } while (MeasuredPowerValue < -5 || TapThread.Current.AbortToken.IsCancellationRequested);
                        }
                        if (resultStrings.Length < 5 || MeasuredPowerValue < 0)
                        {
                            StrChannelMeasurementsCh1[iteration] = iteration + "," + $" 0x{HexValues[iteration]:X}" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "" + "," + "";
                           continue;
                        }
                        ACPValues = new double[4] { Convert.ToDouble(resultStrings[67]), Convert.ToDouble(resultStrings[69]), Convert.ToDouble(resultStrings[71]), Convert.ToDouble(resultStrings[73]) };
                        MeasuredPowerValue += (CableLosses[iteration] * -1);
                        measuredPowerValueBeforeDPD = MeasuredPowerValue;
                        Log.Info("CH1 during Chain : " + iteration + " Cal Measured power value :" + MeasuredPowerValue);
                    }

                    // ToDo: Add test case code.
                    RunChildSteps(); //If the step supports child steps.

                    // If no verdict is used, the verdict will default to NotSet.
                    // You can change the verdict using UpgradeVerdict() as shown below.
                    // UpgradeVerdict(Verdict.Pass);
                    var totalCh1CalTime = stopwathCh1.Elapsed;
                    Log.Info("Total Ch1 Cal Time : " + (totalCh1CalTime.TotalMilliseconds / 1000).ToString());
                }
            }
            catch (Exception ex)
            {
                CCDUServer.loopBreak = true;
                Log.Info("Exception/ CH-1: {0}", ex);
            }
            if (WriteDSAToEEPROM)
            {
                MRU_DUT.Dr49_CH1_WriteDSAToEEPROM(HexValues);

                // IF Ch1 and Ch2, all measurement done properly, then only only write to EEPROM, otherwise NO.

            }
            // MRU_DUT.stopReceiveEvent();
            UpgradeVerdict(Verdict.Pass);
        }
        */
        //private string GenerateCommand(int portNumber, int HexNumber)
        //{
        //    if (HexNumber<0)
        //    {
        //        HexNumber = 0;  
        //    }
        //    string DSAValues = string.Empty;
        //    for (int iteration = 0; iteration < 16; iteration++)
        //    {
        //        if (iteration == portNumber)
        //        {
        //            DSAValues += $" 0x{HexNumber:X}";
        //        }
        //        else
        //        {
        //            DSAValues += " 0x7F";
        //        }
        //    }
        //    return "rj-dsa-init 16" + DSAValues + " 16 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 2 1150 1611";
        //}
        private static void readDSA_CableLossFile(string dSA_CableLossFile, out string[] hexValues, out double[] cableLosses)
        {
            hexValues = new string[16];
            cableLosses = new double[16];
            try
            {
                var strValues = File.ReadAllLines(dSA_CableLossFile);
                if (strValues.Count() != 16)
                {
                    throw new Exception("Invalid numbers of dsavalues on file.");
                }
                for (int iteration = 0; iteration < strValues.Length; iteration++)
                {
                    hexValues[iteration] = strValues[iteration].Split(',')[0];
                    cableLosses[iteration] = double.Parse(strValues[iteration].Split(',')[1]);
                }
            }
            catch (Exception ex)
            {
                Log.Error("DSA_ Attenuation File reading issue :" + ex.Message);
                throw;
            }
        }

        public override void PrePlanRun()
        {

            RjioReportCls.reportGenerated = false;
        }
        public override void PostPlanRun()
        {
            if (!RjioReportCls.reportGenerated)
            {
                RjioReportCls.reportGenerated = true;

                RjioReportCls MRURjioReportCls = new RjioReportCls();
                MRURjioReportCls.SWVersion = DR21Login.softwareVersion;
                MRURjioReportCls.EMPID = DR21Login.EmpID;
                MRURjioReportCls.TestStartTime = DR21Login.TestPlanStartTime;
                MRURjioReportCls.TestEndTime = DateTime.Now.ToLongTimeString();
                MRURjioReportCls.testStage = DR21Login.testStage;
                MRURjioReportCls.TotalTestTime = (DateTime.Now - DR21Login.testplanStartTime_dateTime).TotalMinutes.ToString();

                MRURjioReportCls.ProdID = DR21_ReadInfo.ProdID;
                MRURjioReportCls.MACID1 = DR21_ReadInfo.MAC1;
                MRURjioReportCls.MACID2 = DR21_ReadInfo.MAC2;
                MRURjioReportCls.MACID3 = DR21_ReadInfo.MAC3;
                MRURjioReportCls.MACID4 = DR21_ReadInfo.MAC4;
                MRURjioReportCls.PcbSerialNumber = DR21_ReadInfo.PCBserialNumber;
                MRURjioReportCls.ProductSerialNumber = DR21_ReadInfo.ProductSerialNumber;
                foreach (var item in RjioMRU.TestSteps.CalibrationStep_CH1.StrChannelMeasurementsCh1)
                {
                    MRURjioReportCls.Measurements += item + ";";
                }
                MRURjioReportCls.Measurements += ";";
                foreach (var item in CalibrationStep_CH2.StrChannelMeasurementsCh2)
                {
                    MRURjioReportCls.Measurements += item + ";";
                }
                Results.Publish<RjioReportCls>("Report", MRURjioReportCls);
                Log.Info("Measurements : " + MRURjioReportCls.Measurements);
            }
        }
    }


    [Display("CalibrationStep Ch2", Group: "RjioMRU.Calibration", Description: "Insert a description here")]
    public class CalibrationStep_CH2 : TestStep
    {
        GeneralFunctions dsaConstruction = new GeneralFunctions();
        //tempavary veriables
        double measuredPowerBeforeDPD = double.NaN;

        bool ChannelPowerOk = false, EVMOK = false, ACLR_L1OK = false, ACLR_L2OK = false, ACLR_R1OK = false, ACLR_R2OK = false, FREQERROK = false;
        double eVMLimit = 3.5, aCLR_L1Limit = -45, aCLR_L2Limit = -45, aCLR_R1Limit = -45, aCLR_R2Limit = -45, fREQErrorLimit = 350;
        int AttemptNumber = 1;

        public static string[] StrChannelMeasurementsCh2 = new string[16] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        #region Settings
        int calStartPort = 0;
        int calEndPort = 1;
        Stopwatch stopwathCh2 = new Stopwatch();
        int DSACalCycles = 10;
        MRU_Rjio mRU_DUT;
        EXM_E6680A e6680InsturmentTrx3;
        EXM_E6680A e6680InsturmentTrx4;
        //EXM_E6680A e6680InsturmentTrx3;
        //EXM_E6680A e6680InsturmentTrx4;
        string[] strHexValues = new string[16];
        public int[] HexValues = new int[16];
        public static int[] HexValues4DSAWriging = new int[16];
        double[] CableLosses = new double[16];
        private string dSA_CableLossFile = "DSA_CABLELOSS_Ch2.csv";
        bool stepPassFlag = true;
        //private string[] hexValue =new string[16];
        //private double[] cableLoss = new double[16];

        double channelPowerLimit = 0.25;
        double channelPower = 38;
        double UpperChannelLimit;
        double LowerChannelLimit;

        int postDPDDelay = 1000;

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public CalibrationStep_CH2()
        {
            RjioReportCls.reportGenerated = false;
            // ToDo: Set default values for properties / settings.
        }
        [Display("E6680A TRX3", Group: "E6680A TRX", Order: 0)]
        public EXM_E6680A E6680InsturmentTrx3 { get => e6680InsturmentTrx3; set => e6680InsturmentTrx3 = value; }
        [Display("E6680A TRX4", Group: "E6680A TRX", Order: 1)]
        public EXM_E6680A E6680InsturmentTrx4 { get => e6680InsturmentTrx4; set => e6680InsturmentTrx4 = value; }
        //[Display("E6680A TRX3", Group: "E6680A TRX", Order: 2)]
        //public EXM_E6680A E6680InsturmentTrx3 { get => e6680InsturmentTrx3; set => e6680InsturmentTrx3 = value; }
        //[Display("E6680A TRX4", Group: "E6680A TRX", Order: 3)]
        //public EXM_E6680A E6680InsturmentTrx4 { get => e6680InsturmentTrx4; set => e6680InsturmentTrx4 = value; }

        [Display("DSA_Cable loss file", Order: 2)]
        public string DSA_CableLossFile { get => dSA_CableLossFile; set => dSA_CableLossFile = value; }

        [Display("Channel Power", Order: 3)]
        public double ChannelPower { get => channelPower; set => channelPower = value; }

        [Display("Channel Power Limit", Order: 4)]
        public double ChannelPowerLimit { get => channelPowerLimit; set => channelPowerLimit = value; }
        public MRU_Rjio MRU_DUT { get => mRU_DUT; set => mRU_DUT = value; }


        [Display("Calibration Ports(Start)", Group: "Cal Ports", Order: 0)]
        public int CalStartPort { get => calStartPort; set => calStartPort = value; }
        [Display("Calibration Ports(End)", Group: "Cal Ports", Order: 1)]
        public int CalEndPort { get => calEndPort; set => calEndPort = value; }

        [Display("Post DPD Delay in ms", Order: 10)]
        public int PostDPDDelay { get => postDPDDelay; set => postDPDDelay = value; }
        [Display("EVM Max Limit", Order: 15, Group: "Measurement Limits", Description: "Enter the maximum limit of the EVM")]
        public double EVMLimit { get => eVMLimit; set => eVMLimit = value; }
        [Display("ACLR Imm Next Left Channel Min Limit", Order: 17, Group: "Measurement Limits", Description: "Enter the minimum first left channel ACLR  limit ")]
        public double ACLR_L1_Limit { get => aCLR_L1Limit; set => aCLR_L1Limit = value; }
        [Display("ACLR Second Left Channel Min Limit", Order: 19, Group: "Measurement Limits", Description: "Enter the minimum Second left channel ACLR  limit ")]
        public double ACLR_L2_Limit { get => aCLR_L2Limit; set => aCLR_L2Limit = value; }
        [Display("ACLR Imm Right Channel Min Limit", Order: 21, Group: "Measurement Limits", Description: "Enter the minimum first right channel ACLR  limit ")]
        public double ACLR_R1_Limit { get => aCLR_R1Limit; set => aCLR_R1Limit = value; }

        [Display("ACLR Second Right Channel Min Limit", Order: 23, Group: "Measurement Limits", Description: "Enter the minimum second right channel ACLR  limit ")]

        public double ACLR_R2_Limit { get => aCLR_R2Limit; set => aCLR_R2Limit = value; }

        [Display("Frquency Error Limit", Order: 25, Group: "Measurement Limits", Description: "Enter the frequency error limit")]

        public double FREQError_Limit { get => fREQErrorLimit; set => fREQErrorLimit = value; }

        [Display("DSA setting number of trials ")]
        public int DSACalCycles1 { get => DSACalCycles; set => DSACalCycles = value; }

        [Display("Write DSA values to EEPROM", Order: 100)]

        public override void Run()
        {
            EXM_E6680A E6680InsturmentComman = new EXM_E6680A(); ;
            int DSATrailsCount = 0;
            stopwathCh2.Restart();

            string DSACommand = string.Empty;
            //MRU_DUT.startReceiveEvent();
            MRU_DUT.Dr49_CH2_ControlC();

            E6680InsturmentTrx3.SelectInstScreen("SEQ");
            E6680InsturmentTrx4.SelectInstScreen("SEQ");
            E6680InsturmentTrx3.MeasureContinues(false);
            if (calEndPort > 7)
            {
                E6680InsturmentTrx4.MeasureContinues(false);
            }
            // E6680Insturment.SelectInstScreen("EVM");
            UpperChannelLimit = channelPower + channelPowerLimit;
            LowerChannelLimit = channelPower - channelPowerLimit;
            string[] resultStrings = new string[6];
            readDSA_CableLossFile(DSA_CableLossFile, out strHexValues, out CableLosses);
            double[] ACPValues = new double[4];


            for (int iteration = 0; iteration < 16; iteration++)
            {
                HexValues[iteration] = int.Parse(strHexValues[iteration], System.Globalization.NumberStyles.HexNumber);
            }


            try
            {
                for (int iteration = calStartPort; iteration <= CalEndPort; iteration++)
                {
                    #region InitialMeasurement
                    DSATrailsCount = 0;
                    EVMOK = false;
                    ACLR_L1OK = false;
                    ACLR_L2OK = false;
                    ACLR_R1OK = false;
                    ACLR_R2OK = false;
                    FREQERROK = false;
                    ChannelPowerOk = false;
                    AttemptNumber = 1;
                    if (iteration <= 7)
                        E6680InsturmentTrx3.SetRFInputPort((iteration % 8) + 1);
                    else
                        E6680InsturmentTrx4.SetRFInputPort((iteration % 8) + 1);

                    DSACommand = dsaConstruction.GenerateCommand(iteration, HexValues[iteration]);
                    Log.Info("Initialization Command for Ch" + iteration + " " + DSACommand);
                    MRU_DUT.DR49CH2executeCALDSAScripts(DSACommand, "rjInitialConfiguration Completed");
                    TapThread.Sleep(2000);

                    double MeasuredPowerValue = double.NaN;
                    for (int l = 0; l < 5; l++)
                    {
                        try
                        {
                            resultStrings = (iteration <= 7) ? E6680InsturmentTrx3.ReadSequencerPower() : E6680InsturmentTrx4.ReadSequencerPower();

                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                            break;
                        }
                        // var resutlStrings = E6680Insturment.measureModulationRead();
                        if (resultStrings.Length < 5)
                        {
                            continue;
                        }
                        else
                        {
                            MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                            if (l > 0)
                            {
                                break;
                            }

                        }
                    }

                    if (resultStrings.Length < 5 || MeasuredPowerValue < 0)
                    {
                        StrChannelMeasurementsCh2[iteration] = iteration + "," + $" 0x{HexValues[iteration]:X}" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "" + "," + "";
                        continue;
                    }

                    ACPValues = new double[4] { Convert.ToDouble(resultStrings[67]), Convert.ToDouble(resultStrings[69]), Convert.ToDouble(resultStrings[71]), Convert.ToDouble(resultStrings[73]) };

                    MeasuredPowerValue += (CableLosses[iteration] * -1);
                    measuredPowerBeforeDPD = MeasuredPowerValue;
                    double powerDifferance = 0;
                    Log.Info("Initial Power before cal Ch" + iteration + " Is :" + MeasuredPowerValue.ToString());

                    #endregion InitialMeasurement

                    while (true)
                    {
                        DSATrailsCount++;
                        if (DSATrailsCount >= DSACalCycles1)
                        {
                            break;
                        }
                        powerDifferance = Math.Abs(MeasuredPowerValue - ChannelPower);
                        if (MeasuredPowerValue <= LowerChannelLimit)
                        {
                            if (powerDifferance > 0.5)
                            {
                                int changeValue = (int)Math.Floor((powerDifferance / 0.25));
                                HexValues[iteration] -= changeValue;
                            }
                            else
                            {
                                HexValues[iteration] -= 1;
                            }
                        }
                        else if (MeasuredPowerValue >= UpperChannelLimit)
                        {
                            if (powerDifferance > 0.5)
                            {
                                int changeValue = (int)Math.Ceiling((powerDifferance / 0.25));
                                HexValues[iteration] += changeValue;
                            }
                            else
                            {
                                HexValues[iteration] += 1;
                            }
                        }
                        else
                        {
                            string[] ACP5GValues;
                            if (iteration <= 7)
                            {
                                E6680InsturmentTrx3.SelectInstScreen("ACP");
                                E6680InsturmentTrx3.SetExternalPowerLoss(CableLosses[iteration]);
                                ACP5GValues = E6680InsturmentTrx3.measureACP();
                            }
                            else
                            {
                                E6680InsturmentTrx4.SelectInstScreen("ACP");
                                E6680InsturmentTrx4.SetExternalPowerLoss(CableLosses[iteration]);
                                ACP5GValues = E6680InsturmentTrx4.measureACP();
                            }
                            if (Convert.ToDouble(ACP5GValues[4]) > -45)
                            {
                                var DPDStartTime = stopwathCh2.ElapsedMilliseconds;
                                MRU_DUT.DR49CH2Jjio_DPD_InitRun(iteration);
                                var DPDStopTIme = stopwathCh2.ElapsedMilliseconds;
                                Log.Info("CH2 DPD inti time for chain " + iteration + " is :" + (DPDStopTIme - DPDStartTime) / 1000);
                                TapThread.Sleep(PostDPDDelay);
                            }

                            if (iteration <= 7)
                            {
                                ACP5GValues = E6680InsturmentTrx3.measureACP();
                            }
                            else
                            {
                                ACP5GValues = E6680InsturmentTrx4.measureACP();

                            }
                            var DPDMeasureStartTIme = stopwathCh2.ElapsedMilliseconds;
                            TapThread.Sleep(1000);
                            //MRU_DUT.Dr49_CH2_DPD_Measurement(iteration, out var txvalue, out var rxvalue);
                            var DPDMeasureEndTime = stopwathCh2.ElapsedMilliseconds;
                            Log.Info("CH2 DPD Measuret time for Chain " + iteration + " is : " + (DPDMeasureEndTime - DPDMeasureStartTIme) / 1000);
                            if (iteration <= 7)
                            {
                                E6680InsturmentTrx3.SelectInstScreen("SEQ");
                            }
                            else
                            {
                                E6680InsturmentTrx4.SelectInstScreen("SEQ");
                            }
                            for (int j = 0; j < 5; j++)
                            {
                                resultStrings = ((iteration <= 7) ? E6680InsturmentTrx3.ReadSequencerPower() : E6680InsturmentTrx4.ReadSequencerPower());
                                if (resultStrings.Length < 5)
                                {
                                    continue;
                                }
                                else
                                {

                                    if (j > 0)
                                    {
                                        break;
                                    }

                                }

                            }
                            if (resultStrings.Length < 5)
                            {
                                continue;
                            }

                            MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                            ACPValues = new double[4] { Convert.ToDouble(ACP5GValues[8]), Convert.ToDouble(ACP5GValues[4]), Convert.ToDouble(ACP5GValues[6]), Convert.ToDouble(ACP5GValues[10]) };
                            MeasuredPowerValue += (CableLosses[iteration] * -1);



                            Log.Info("CH2 DSA Command Used: " + DSACommand);
                            Log.Info("CH2 Chain NO:" + iteration + " Channel Power : " + MeasuredPowerValue + "dBm ACP1: " + ACPValues[0] + " ACP2 : " + ACPValues[1] + " ACP3 : " + ACPValues[2] + " ACP4 : " + ACPValues[3]);
                            //Log.Info("CH2 Chain NO:" + iteration + " DPD TxValue :" + txvalue + " RxValue :" + rxvalue);
                            //StrChannelMeasurementsCh2[iteration] = iteration + "," + $" 0x{HexValues[iteration]:X}" + "," + MeasuredPowerValue + "," + ACPValues[0] + "," + ACPValues[1] + "," + ACPValues[2] + "," + ACPValues[3] + "," + txvalue + "," + rxvalue;

                            // Log.Info("CH1 Chain NO:" + iteration + " DPD TxValue :" + txvalue + " RxValue :" + rxvalue);
                            StrChannelMeasurementsCh2[iteration] = iteration + "," + $" 0x{HexValues[iteration]:X}" + "," + MeasuredPowerValue + "," + ACPValues[0] + "," + ACPValues[1] + "," + ACPValues[2] + "," + ACPValues[3] + "," + "" + "," + "";

                            ACLR_L1OK = ACLR_L1_Limit >= ACPValues[0];
                            ACLR_L2OK = ACLR_L2_Limit >= ACPValues[1];
                            ACLR_R1OK = ACLR_R1_Limit >= ACPValues[2];
                            ACLR_R2OK = ACLR_R2_Limit >= ACPValues[3];
                            /////////////////Modulation Measuremnts /////////////////////

                            if (iteration <= 7)
                            {
                                E6680InsturmentTrx3.SelectInstScreen("EVM");
                                E6680InsturmentTrx3.SetExternalPowerLoss(CableLosses[iteration]);
                                E6680InsturmentTrx3.SetRFInputPort((iteration % 8) + 1);

                            }
                            else
                            {
                                E6680InsturmentTrx4.SelectInstScreen("EVM");
                                E6680InsturmentTrx4.SetExternalPowerLoss(CableLosses[iteration]);
                                E6680InsturmentTrx4.SetRFInputPort((iteration % 8) + 1);
                            }

                            resultStrings = ((iteration <= 7) ? E6680InsturmentTrx3.measureModulationRead() : E6680InsturmentTrx4.measureModulationRead());
                            Log.Info(" CH1 ChainNo" + iteration + " Modulation measurements  :Channel Power : " + resultStrings[22] + "dBm ,EVM : " + Convert.ToDouble(resultStrings[1]) + "% ,Frequency Error : " + Convert.ToDouble(resultStrings[3]) + "Hz;");

                            if (resultStrings.Length < 5)
                            {
                                Log.Error("Skipping Chain No " + iteration + " because of result length " + resultStrings.Length.ToString());
                                continue;
                            }
                            else
                            {

                                MeasuredPowerValue = (Math.Abs((ChannelPower - MeasuredPowerValue)) > (Math.Abs(channelPower - Convert.ToDouble(resultStrings[22])))) ? Convert.ToDouble(resultStrings[22]) : MeasuredPowerValue;
                                StrChannelMeasurementsCh2[iteration] += "," + resultStrings[1] + "," + resultStrings[3] + "," + measuredPowerBeforeDPD;
                            }
                            if (iteration <= 7)
                            {
                                E6680InsturmentTrx3.SelectInstScreen("SEQ");
                            }
                            else
                            {
                                E6680InsturmentTrx4.SelectInstScreen("SEQ");
                            }

                            ChannelPowerOk = ChannelPowerLimit > Math.Abs((ChannelPower - MeasuredPowerValue));
                            EVMOK = EVMLimit >= Convert.ToDouble(resultStrings[1]);
                            FREQERROK = fREQErrorLimit >= Math.Abs(Convert.ToDouble(resultStrings[3]));
                            AttemptNumber++;
                            /////////////////////////////////////////////////////////////
                            // MRURjioReportCls.Measurements += StrChannelMeasurements[iteration] + "," + resultStrings[1] + "," + resultStrings[3] + ";";
                            if ((ChannelPowerOk && ACLR_R1OK && ACLR_L2OK && ACLR_R1OK && ACLR_R2OK && FREQERROK && EVMOK) || AttemptNumber > 2)
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        DSACommand = dsaConstruction.GenerateCommand(iteration, HexValues[iteration]);
                        MRU_DUT.DR49CH2executeCALDSAScripts(DSACommand, "rjInitialConfiguration Completed");
                        TapThread.Sleep(2000);
                        //TapThread.Sleep(10000);
                        for (int j = 0; j < 5; j++)
                        {

                            TapThread.Sleep(1000);
                            //  E6680Insturment.SetRFInputPort((iteration+1)%9);
                            try
                            {
                                resultStrings = ((iteration <= 7) ? E6680InsturmentTrx3.ReadSequencerPower() : E6680InsturmentTrx4.ReadSequencerPower());
                            }
                            catch (Exception ex)
                            {
                                Log.Info("Exception/ CH-2 at CAL-DSA Script: {0}", ex);
                                break;
                            }

                            /// resultStrings = E6680Insturment.measureModulationRead();
                            if (resultStrings.Length < 5)
                            {
                                continue;
                            }
                            else
                            {
                                MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                                if (j > 0)
                                {
                                    break;
                                }

                            }
                            //  MeasuredPowerValue = Convert.ToDouble(resultStrings[22]);
                        }

                        if (resultStrings.Length < 5 || MeasuredPowerValue < 0)
                        {
                            StrChannelMeasurementsCh2[iteration] = iteration + "," + $" 0x{HexValues[iteration]:X}" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "-999" + "," + "" + "," + "";
                            continue;
                        }
                        else
                        {
                            HexValues4DSAWriging[iteration] = HexValues[iteration];

                            ACPValues = new double[4] { Convert.ToDouble(resultStrings[67]), Convert.ToDouble(resultStrings[69]), Convert.ToDouble(resultStrings[71]), Convert.ToDouble(resultStrings[73]) };
                            MeasuredPowerValue += (CableLosses[iteration] * -1);
                            measuredPowerBeforeDPD = MeasuredPowerValue;
                        }

                        Log.Info("CH2 during Chain : " + iteration + " Cal Measured power value :" + MeasuredPowerValue);
                    }
                    // ToDo: Add test case code.
                    RunChildSteps(); //If the step supports child steps.

                    // If no verdict is used, the verdict will default to NotSet.
                    // You can change the verdict using UpgradeVerdict() as shown below.
                    // UpgradeVerdict(Verdict.Pass);
                    stepPassFlag &= ChannelPowerOk && ACLR_R1OK && ACLR_L2OK && ACLR_R2OK && ACLR_L2OK && FREQERROK && EVMOK;

                }
            }
            catch (Exception ex)
            {
                CCDUServer.loopBreak = true;
                Log.Info("Exception/ CH-2: {0}", ex);
            }

            Log.Info("CH2 Total Cal time : " + stopwathCh2.ElapsedMilliseconds / 1000 + " Seconds.");

            // MRU_DUT.stopReceiveEvent();
            if (stepPassFlag)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }

        }

        //private string GenerateCommand(int portNumber, int HexNumber)
        //{
        //    string DSAValues = string.Empty;
        //    for (int iteration = 0; iteration < 16; iteration++)
        //    {
        //        if (iteration == portNumber)
        //        {
        //            DSAValues += $" 0x{HexNumber:X}";
        //        }
        //        else
        //        {
        //            DSAValues += " 0x7F";
        //        }
        //    }
        //    return "rj-dsa-init 16" + DSAValues + " 16 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 2 1200 1611";
        //}
        private static void readDSA_CableLossFile(string dSA_CableLossFile, out string[] hexValues, out double[] cableLosses)
        {
            hexValues = new string[16];
            cableLosses = new double[16];
            try
            {
                var strValues = File.ReadAllLines(dSA_CableLossFile);
                if (strValues.Count() != 16)
                {
                    throw new Exception("Invalid numbers of dsavalues on file.");
                }
                for (int iteration = 0; iteration < strValues.Length; iteration++)
                {
                    hexValues[iteration] = strValues[iteration].Split(',')[0];
                    cableLosses[iteration] = double.Parse(strValues[iteration].Split(',')[1]);
                }
            }
            catch (Exception ex)
            {
                Log.Error("DSA_ Attenuation File reading issue :" + ex.Message);
                throw;
            }
        }


        public override void PostPlanRun()
        {
            if (!RjioReportCls.reportGenerated)
            {
                RjioReportCls.reportGenerated = true;
                RjioReportCls MRURjioReportCls = new RjioReportCls();
                MRURjioReportCls.ProdID = DR21_ReadInfo.ProdID;
                MRURjioReportCls.MACID1 = DR21_ReadInfo.MAC1;
                MRURjioReportCls.MACID2 = DR21_ReadInfo.MAC2;
                MRURjioReportCls.MACID3 = DR21_ReadInfo.MAC3;
                MRURjioReportCls.MACID4 = DR21_ReadInfo.MAC4;
                MRURjioReportCls.PcbSerialNumber = DR21_ReadInfo.PCBserialNumber;
                MRURjioReportCls.ProductSerialNumber = DR21_ReadInfo.ProductSerialNumber;
                MRURjioReportCls.TestStartTime = DR21Login.TestPlanStartTime;
                MRURjioReportCls.TestEndTime = DateTime.Now.ToLongTimeString();
                MRURjioReportCls.TotalTestTime = (DateTime.Now - DR21Login.testplanStartTime_dateTime).TotalMinutes.ToString();
                MRURjioReportCls.SWVersion = DR21Login.softwareVersion;
                MRURjioReportCls.testStage = DR21Login.testStage;
                MRURjioReportCls.EMPID = DR21Login.EmpID;

                foreach (var item in RjioMRU.TestSteps.CalibrationStep_CH1.StrChannelMeasurementsCh1)
                {
                    MRURjioReportCls.Measurements += item + ";";
                }
                MRURjioReportCls.Measurements += ";";
                foreach (var item in CalibrationStep_CH2.StrChannelMeasurementsCh2)
                {
                    MRURjioReportCls.Measurements += item + ";";
                }
                Results.Publish<RjioReportCls>("Report", MRURjioReportCls);
                Log.Info("Measurements : " + MRURjioReportCls.Measurements);

            }
        }
        public override void PrePlanRun()
        {
            RjioReportCls.reportGenerated = false;
        }
    }
    /* public class CalibrationStep_CH2 : TestStep
     {
         GeneralFunctions dsaConstruction=new GeneralFunctions();
         //tempavary veriables
         double measuredPowerBeforeDPD = double.NaN;

         bool ChannelPowerOk=false, EVMOK = false, ACLR_L1OK = false, ACLR_L2OK = false, ACLR_R1OK = false, ACLR_R2OK = false, FREQERROK = false;
         double eVMLimit = 3.5, aCLR_L1Limit = -45, aCLR_L2Limit = -45, aCLR_R1Limit = -45, aCLR_R2Limit = -45, fREQErrorLimit = 350;
         int AttemptNumber = 1;

         public static string[] StrChannelMeasurementsCh2 = new string[16] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
         #region Settings
         int calStartPort = 0;
         int calEndPort = 1;
         Stopwatch stopwathCh2 = new Stopwatch();
         MRU_Rjio mRU_DUT;
         EXM_E6680A e6680InsturmentTrx3;
         EXM_E6680A e6680InsturmentTrx4;
         //EXM_E6680A e6680InsturmentTrx3;
         //EXM_E6680A e6680InsturmentTrx4;
         string[] strHexValues = new string[16];
         int[] HexValues = new int[16];
         double[] CableLosses = new double[16];
         private string dSA_CableLossFile = "DSA_CABLELOSS_Ch2.csv";

         //private string[] hexValue =new string[16];
         //private double[] cableLoss = new double[16];

         double channelPowerLimit = 0.25;
         double channelPower = 38;
         double UpperChannelLimit;
         double LowerChannelLimit;

         int postDPDDelay = 1000;

         // ToDo: Add property here for each parameter the end user should be able to change
         #endregion

         public CalibrationStep_CH2()
         {
             RjioReportCls.reportGenerated = false;
             // ToDo: Set default values for properties / settings.
         }
         [Display("E6680A TRX3", Group: "E6680A TRX", Order: 0)]
         public EXM_E6680A E6680InsturmentTrx3 { get => e6680InsturmentTrx3; set => e6680InsturmentTrx3 = value; }
         [Display("E6680A TRX4", Group: "E6680A TRX", Order: 1)]
         public EXM_E6680A E6680InsturmentTrx4 { get => e6680InsturmentTrx4; set => e6680InsturmentTrx4 = value; }
         //[Display("E6680A TRX3", Group: "E6680A TRX", Order: 2)]
         //public EXM_E6680A E6680InsturmentTrx3 { get => e6680InsturmentTrx3; set => e6680InsturmentTrx3 = value; }
         //[Display("E6680A TRX4", Group: "E6680A TRX", Order: 3)]
         //public EXM_E6680A E6680InsturmentTrx4 { get => e6680InsturmentTrx4; set => e6680InsturmentTrx4 = value; }

         [Display("DSA_Cable loss file", Order: 2)]
         public string DSA_CableLossFile { get => dSA_CableLossFile; set => dSA_CableLossFile = value; }

         [Display("Channel Power", Order: 3)]
         public double ChannelPower { get => channelPower; set => channelPower = value; }

         [Display("Channel Power Limit", Order: 4)]
         public double ChannelPowerLimit { get => channelPowerLimit; set => channelPowerLimit = value; }
         public MRU_Rjio MRU_DUT { get => mRU_DUT; set => mRU_DUT = value; }


         [Display("Calibration Ports(Start)", Group: "Cal Ports", Order: 0)]
         public int CalStartPort { get => calStartPort; set => calStartPort = value; }
         [Display("Calibration Ports(End)", Group: "Cal Ports", Order: 1)]
         public int CalEndPort { get => calEndPort; set => calEndPort = value; }

         [Display("Post DPD Delay in ms", Order: 10)]
         public int PostDPDDelay { get => postDPDDelay; set => postDPDDelay = value; }
         [Display("EVM Max Limit",Order:15,Group:"Measurement Limits",Description:"Enter the maximum limit of the EVM")]
         public double EVMLimit { get => eVMLimit; set => eVMLimit = value; }
         [Display("ACLR Imm Next Left Channel Min Limit", Order: 17, Group: "Measurement Limits", Description: "Enter the minimum first left channel ACLR  limit ")]
         public double ACLR_L1_Limit { get => aCLR_L1Limit; set => aCLR_L1Limit = value; }
         [Display("ACLR Second Left Channel Min Limit", Order: 19, Group: "Measurement Limits", Description: "Enter the minimum Second left channel ACLR  limit ")]
         public double ACLR_L2_Limit { get => aCLR_L2Limit; set => aCLR_L2Limit = value; }
         [Display("ACLR Imm Right Channel Min Limit", Order: 21, Group: "Measurement Limits", Description: "Enter the minimum first right channel ACLR  limit ")]
         public double ACLR_R1_Limit { get => aCLR_R1Limit; set => aCLR_R1Limit = value; }

         [Display("ACLR Second Right Channel Min Limit", Order: 23, Group: "Measurement Limits", Description: "Enter the minimum second right channel ACLR  limit ")]

         public double ACLR_R2_Limit { get => aCLR_R2Limit; set => aCLR_R2Limit = value; }

         [Display("Frquency Error Limit", Order: 25, Group: "Measurement Limits", Description: "Enter the frequency error limit")]

         public double FREQError_Limit { get => fREQErrorLimit; set => fREQErrorLimit = value; }

         public override void Run()
         {
             stopwathCh2.Restart();

             string DSACommand = string.Empty;
             //MRU_DUT.startReceiveEvent();
             MRU_DUT.Dr49_CH2_ControlC();

             E6680InsturmentTrx3.SelectInstScreen("SEQ");
             E6680InsturmentTrx4.SelectInstScreen("SEQ");
             E6680InsturmentTrx3.MeasureContinues(false);
             if (calEndPort > 7)
             {
                 E6680InsturmentTrx4.MeasureContinues(false);
             }
             // E6680Insturment.SelectInstScreen("EVM");
             UpperChannelLimit = channelPower + channelPowerLimit;
             LowerChannelLimit = channelPower - channelPowerLimit;
             string[] resultStrings = new string[6];
             readDSA_CableLossFile(DSA_CableLossFile, out strHexValues, out CableLosses);
             double[] ACPValues = new double[4];


             for (int iteration = 0; iteration < 16; iteration++)
             {
                 HexValues[iteration] = int.Parse(strHexValues[iteration], System.Globalization.NumberStyles.HexNumber);
             }


             try
             {
                 for (int iteration = calStartPort; iteration <= CalEndPort; iteration++)
                 {
                     #region InitialMeasurement
                     EVMOK = false;
                     ACLR_L1OK = false;
                     ACLR_L2OK = false;
                     ACLR_R1OK = false;
                     ACLR_R2OK = false;
                     FREQERROK = false;
                     ChannelPowerOk = false;
                     AttemptNumber = 1;
                     if (iteration <= 7)
                         E6680InsturmentTrx3.SetRFInputPort((iteration % 8) + 1);
                     else
                         E6680InsturmentTrx4.SetRFInputPort((iteration % 8) + 1);

                     DSACommand = dsaConstruction.GenerateCommand(iteration, HexValues[iteration]);
                     Log.Info("Initialization Command for Ch" + iteration + " " + DSACommand);
                     MRU_DUT.DR49CH2executeCALDSAScripts(DSACommand, "rjInitialConfiguration Completed");
                     TapThread.Sleep(2000);

                     double MeasuredPowerValue = double.NaN;
                     for (int l = 0; l < 2; l++)
                     {
                         do
                         {
                             resultStrings = (iteration <= 7) ? E6680InsturmentTrx3.ReadSequencerPower() : E6680InsturmentTrx4.ReadSequencerPower();
                             MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                         } while (MeasuredPowerValue < -5);
                     }
                     ACPValues = new double[4] { Convert.ToDouble(resultStrings[67]), Convert.ToDouble(resultStrings[69]), Convert.ToDouble(resultStrings[71]), Convert.ToDouble(resultStrings[73]) };

                     MeasuredPowerValue += (CableLosses[iteration] * -1);
                     measuredPowerBeforeDPD = MeasuredPowerValue;
                     double powerDifferance = 0;
                     Log.Info("Initial Power before cal Ch" + iteration + " Is :" + MeasuredPowerValue.ToString());

                     #endregion InitialMeasurement

                     while (true)
                     {

                         powerDifferance = Math.Abs(MeasuredPowerValue - ChannelPower);
                         if (MeasuredPowerValue <= LowerChannelLimit)
                         {
                             if (powerDifferance > 0.5)
                             {
                                 int changeValue = (int)Math.Floor((powerDifferance / 0.25));
                                 HexValues[iteration] -= changeValue;
                             }
                             else
                             {
                                 HexValues[iteration] -= 1;
                             }
                         }
                         else if (MeasuredPowerValue >= UpperChannelLimit)
                         {
                             if (powerDifferance > 0.5)
                             {
                                 int changeValue = (int)Math.Ceiling((powerDifferance / 0.25));
                                 HexValues[iteration] += changeValue;
                             }
                             else
                             {
                                 HexValues[iteration] += 1;
                             }
                         }
                         else
                         {
                             if (ACPValues[0] > -45)
                             {
                                 var DPDStartTime = stopwathCh2.ElapsedMilliseconds;
                                 MRU_DUT.DR49CH2Jjio_DPD_InitRun(iteration);
                                 var DPDStopTIme = stopwathCh2.ElapsedMilliseconds;
                                 Log.Info("CH2 DPD inti time for chain " + iteration + " is :" + (DPDStopTIme - DPDStartTime) / 1000);
                                 TapThread.Sleep(PostDPDDelay);
                             }
                             var DPDMeasureStartTIme = stopwathCh2.ElapsedMilliseconds;
                             TapThread.Sleep(1000);
                             MRU_DUT.Dr49_CH2_DPD_Measurement(iteration, out var txvalue, out var rxvalue);
                             var DPDMeasureEndTime = stopwathCh2.ElapsedMilliseconds;
                             Log.Info("CH2 DPD Measuret time for Chain " + iteration + " is : " + (DPDMeasureEndTime - DPDMeasureStartTIme) / 1000);

                             resultStrings = ((iteration <= 7) ? E6680InsturmentTrx3.ReadSequencerPower() : E6680InsturmentTrx4.ReadSequencerPower());
                             MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                             ACPValues = new double[4] { Convert.ToDouble(resultStrings[67]), Convert.ToDouble(resultStrings[69]), Convert.ToDouble(resultStrings[71]), Convert.ToDouble(resultStrings[73]) };
                             MeasuredPowerValue += (CableLosses[iteration] * -1);

                             Log.Info("CH2 DSA Command Used: " + DSACommand);
                             Log.Info("CH2 Chain NO:" + iteration + " Channel Power : " + MeasuredPowerValue + "dBm ACP1: " + ACPValues[0] + " ACP2 : " + ACPValues[1] + " ACP3 : " + ACPValues[2] + " ACP4 : " + ACPValues[3]);
                             Log.Info("CH2 Chain NO:" + iteration + " DPD TxValue :" + txvalue + " RxValue :" + rxvalue);
                             StrChannelMeasurementsCh2[iteration] = iteration + "," + $" 0x{HexValues[iteration]:X}" + "," + MeasuredPowerValue + "," + ACPValues[0] + "," + ACPValues[1] + "," + ACPValues[2] + "," + ACPValues[3] + "," + txvalue + "," + rxvalue;

                             ACLR_L1OK = ACLR_L1_Limit >= ACPValues[0];
                             ACLR_L2OK = ACLR_L2_Limit >= ACPValues[1];
                             ACLR_R1OK = ACLR_R1_Limit >= ACPValues[2];
                             ACLR_R2OK = ACLR_R2_Limit >= ACPValues[3];
                             /////////////////Modulation Measuremnts /////////////////////

                             if (iteration <= 7)
                             {
                                 E6680InsturmentTrx3.SelectInstScreen("EVM");
                                 E6680InsturmentTrx3.SetExternalPowerLoss(CableLosses[iteration]);
                                 E6680InsturmentTrx3.SetRFInputPort((iteration % 8) + 1);

                             }
                             else
                             {
                                 E6680InsturmentTrx4.SelectInstScreen("EVM");
                                 E6680InsturmentTrx4.SetExternalPowerLoss(CableLosses[iteration]);
                                 E6680InsturmentTrx4.SetRFInputPort((iteration % 8) + 1);
                             }
                             resultStrings = ((iteration <= 7) ? E6680InsturmentTrx3.measureModulationRead() : E6680InsturmentTrx4.measureModulationRead());
                             Log.Info(" CH1 ChainNo" + iteration + " Modulation measurements  :Channel Power : " + resultStrings[22] + "dBm ,EVM : " + Convert.ToDouble(resultStrings[1]) + "% ,Frequency Error : " + Convert.ToDouble(resultStrings[3]) + "Hz;");
                              MeasuredPowerValue = (Math.Abs((ChannelPower - MeasuredPowerValue)) > (Math.Abs(channelPower - Convert.ToDouble(resultStrings[22])))) ? Convert.ToDouble(resultStrings[22]) : MeasuredPowerValue;
                             StrChannelMeasurementsCh2[iteration] += "," + resultStrings[1] + "," + resultStrings[3] + "," + measuredPowerBeforeDPD;
                             if (iteration <= 7)
                             {
                                 E6680InsturmentTrx3.SelectInstScreen("SEQ");
                             }
                             else
                             {
                                 E6680InsturmentTrx4.SelectInstScreen("SEQ");
                             }
                             ChannelPowerOk = ChannelPowerLimit > Math.Abs((ChannelPower - MeasuredPowerValue));
                             EVMOK = EVMLimit >= Convert.ToDouble(resultStrings[1]);
                             FREQERROK = fREQErrorLimit >= Math.Abs(Convert.ToDouble(resultStrings[3]));
                             AttemptNumber++;
                             /////////////////////////////////////////////////////////////
                             // MRURjioReportCls.Measurements += StrChannelMeasurements[iteration] + "," + resultStrings[1] + "," + resultStrings[3] + ";";
                             if ((ChannelPowerOk&& ACLR_R1OK && ACLR_L2OK && ACLR_R1OK && ACLR_R2OK && FREQERROK && EVMOK) || AttemptNumber > 2)
                             {
                                 break;
                             }
                             else
                             {
                                 continue;
                             }                           
                         }
                         DSACommand = dsaConstruction.GenerateCommand(iteration, HexValues[iteration]);
                         MRU_DUT.DR49CH2executeCALDSAScripts(DSACommand, "rjInitialConfiguration Completed");
                         //TapThread.Sleep(10000);
                         for (int j = 0; j < 2; j++)
                         {
                             do
                             {
                                 TapThread.Sleep(300);
                                 //  E6680Insturment.SetRFInputPort((iteration+1)%9);
                                 resultStrings = ((iteration <= 7) ? E6680InsturmentTrx3.ReadSequencerPower() : E6680InsturmentTrx4.ReadSequencerPower());
                                 /// resultStrings = E6680Insturment.measureModulationRead();
                                 MeasuredPowerValue = Convert.ToDouble(resultStrings[13]);
                                 //  MeasuredPowerValue = Convert.ToDouble(resultStrings[22]);

                             } while (MeasuredPowerValue < -5 || TapThread.Current.AbortToken.IsCancellationRequested);
                         }
                         ACPValues = new double[4] { Convert.ToDouble(resultStrings[67]), Convert.ToDouble(resultStrings[69]), Convert.ToDouble(resultStrings[71]), Convert.ToDouble(resultStrings[73]) };
                         MeasuredPowerValue += (CableLosses[iteration] * -1);
                         measuredPowerBeforeDPD = MeasuredPowerValue;
                         Log.Info("CH2 during Chain : " + iteration + " Cal Measured power value :" + MeasuredPowerValue);
                     }
                     // ToDo: Add test case code.
                     RunChildSteps(); //If the step supports child steps.

                     // If no verdict is used, the verdict will default to NotSet.
                     // You can change the verdict using UpgradeVerdict() as shown below.
                     // UpgradeVerdict(Verdict.Pass);
                 }
             }
             catch (Exception)
             {
                 CCDUServer.loopBreak = true;
                 throw;
             }

             Log.Info("CH2 Total Cal time : " + stopwathCh2.ElapsedMilliseconds / 1000 + " Seconds.");
             // MRU_DUT.stopReceiveEvent();
             UpgradeVerdict(Verdict.Pass);
         }

         //private string GenerateCommand(int portNumber, int HexNumber)
         //{
         //    string DSAValues = string.Empty;
         //    for (int iteration = 0; iteration < 16; iteration++)
         //    {
         //        if (iteration == portNumber)
         //        {
         //            DSAValues += $" 0x{HexNumber:X}";
         //        }
         //        else
         //        {
         //            DSAValues += " 0x7F";
         //        }
         //    }
         //    return "rj-dsa-init 16" + DSAValues + " 16 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 2 1200 1611";
         //}
         private static void readDSA_CableLossFile(string dSA_CableLossFile, out string[] hexValues, out double[] cableLosses)
         {
             hexValues = new string[16];
             cableLosses = new double[16];
             try
             {
                 var strValues = File.ReadAllLines(dSA_CableLossFile);
                 if (strValues.Count() != 16)
                 {
                     throw new Exception("Invalid numbers of dsavalues on file.");
                 }
                 for (int iteration = 0; iteration < strValues.Length; iteration++)
                 {
                     hexValues[iteration] = strValues[iteration].Split(',')[0];
                     cableLosses[iteration] = double.Parse(strValues[iteration].Split(',')[1]);
                 }
             }
             catch (Exception ex)
             {
                 Log.Error("DSA_ Attenuation File reading issue :" + ex.Message);
                 throw;
             }
         }


         public override void PostPlanRun()
         {
             if (!RjioReportCls.reportGenerated)
             {
                 RjioReportCls.reportGenerated = true;
                 RjioReportCls MRURjioReportCls = new RjioReportCls();
                 MRURjioReportCls.ProdID = DR21_ReadInfo.ProdID;
                 MRURjioReportCls.MACID1 = DR21_ReadInfo.MAC1;
                 MRURjioReportCls.MACID2 = DR21_ReadInfo.MAC2;
                 MRURjioReportCls.MACID3 = DR21_ReadInfo.MAC3;
                 MRURjioReportCls.MACID4 = DR21_ReadInfo.MAC4;
                 MRURjioReportCls.PcbSerialNumber = DR21_ReadInfo.PCBserialNumber;
                 MRURjioReportCls.ProductSerialNumber = DR21_ReadInfo.ProductSerialNumber;
                 MRURjioReportCls.TestStartTime = DR21Login.TestPlanStartTime;
                 MRURjioReportCls.TestEndTime = DateTime.Now.ToLongTimeString();
                 MRURjioReportCls.TotalTestTime = (DateTime.Now - DR21Login.testplanStartTime_dateTime).TotalMinutes.ToString();
                 MRURjioReportCls.SWVersion = DR21Login.softwareVersion;
                 MRURjioReportCls.testStage = DR21Login.testStage;
                 MRURjioReportCls.EMPID = DR21Login.EmpID;

                 foreach (var item in RjioMRU.TestSteps.CalibrationStep_CH1.StrChannelMeasurementsCh1)
                 {
                     MRURjioReportCls.Measurements += item + ";";
                 }
                 MRURjioReportCls.Measurements += ";";
                 foreach (var item in CalibrationStep_CH2.StrChannelMeasurementsCh2)
                 {
                     MRURjioReportCls.Measurements += item + ";";
                 }
                 Results.Publish<RjioReportCls>("Report", MRURjioReportCls);
                 Log.Info("Measurements : " + MRURjioReportCls.Measurements);

             }
         }
         public override void PrePlanRun()
         {
             RjioReportCls.reportGenerated = false;
         }
     }
     */
    public class GeneralFunctions
    {
        public string GenerateCommand(int portNumber, int HexNumber)
        {
            if (HexNumber < 0)
            {
                HexNumber = 0;
            }
            string DSAValues = string.Empty;
            for (int iteration = 0; iteration < 16; iteration++)
            {
                if (iteration == portNumber)
                {
                    DSAValues += $" 0x{HexNumber:X}";
                }
                else
                {
                    DSAValues += " 0x7F";
                }
            }
            return "rj-dsa-init 16" + DSAValues + " 16 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 2 1150 1611";
        }
    }

}