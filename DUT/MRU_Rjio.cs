// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files..

//#define  NORMAL 
using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Net.NetworkInformation;
using System.Globalization;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.ObjectModel;
using System.Runtime.Remoting.Channels;
using System.Windows.Forms;
using System.Net;

namespace RjioMRU
{
    [Display("MRU_Rjio", Group: "RjioMRU", Description: "Add a description here")]
    public class MRU_Rjio : Dut
    {
        public MRU_Rjio()
        {
            Name = "RjioMRU";
            //l2ComObj.DataReceived += new SerialDataReceivedEventHandler(L2ComObj_DataReceived);
            // ToDo: Set default values for properties / settings.
        }
        public override void Open()
        {
            DR21ComObj = new SerialPort(DR21ComPort, 115200, Parity.None, 8, StopBits.One);
            DR21ComObj.ReadTimeout = 30000;
            DR21ComObj.Handshake = Handshake.XOnXOff;
            DR21ComObj.RtsEnable = true;
            DR21ComObj.Open();


            DR49Ch1ComObj = new SerialPort(DR49Ch1ComPort, 115200, Parity.None, 8, StopBits.One);
            DR49Ch1ComObj.ReadTimeout = 3000000;
            DR49Ch1ComObj.Handshake = Handshake.XOnXOff;
            DR49Ch1ComObj.RtsEnable = true;
            DR49Ch1ComObj.Open();


            DR49Ch2ComObj = new SerialPort(DR49Ch2ComPort, 115200, Parity.None, 8, StopBits.One);
            DR49Ch2ComObj.ReadTimeout = 3000000;
            DR49Ch2ComObj.Handshake = Handshake.XOnXOff;
            DR49Ch2ComObj.RtsEnable = true;
            DR49Ch2ComObj.Open();


            base.Open();


            // TODO: establish connection to DUT here
        }
        /// <summary>
        /// Closes the connection made to the DUT represented by this class
        /// </summary>

        public override void Close()
        {
            DR21ComObj.Close();
            DR21ComObj.Dispose();
            DR21ComObj = null;

            DR49Ch1ComObj.Close();
            DR49Ch1ComObj.Dispose();
            DR49Ch1ComObj = null;

            DR49Ch2ComObj.Close();
            DR49Ch2ComObj.Dispose();
            DR49Ch2ComObj = null;

            // TODO: close connection to DUT
            base.Close();
        }
        #region Settings
        string dR21ComPort = "COM13";
        string dR49Ch1ComPort = "COM12";
        string dR49Ch2ComPort = "COM11";

        SerialPort DR21ComObj;
        SerialPort DR49Ch1ComObj;
        SerialPort DR49Ch2ComObj;

        public string bootMode = string.Empty;

        Thread l1ComThread;
        Thread dPDComThread;


        public bool StartMeasurements;

        [Display("DR21 COM Port", Description: "DR21 Port Number")]
        public string DR21ComPort { get => dR21ComPort; set => dR21ComPort = value; }
        [Display("DR49 CH1 COM Port", Description: "DR49 Ch1 Port Number")]
        public string DR49Ch1ComPort { get => dR49Ch1ComPort; set => dR49Ch1ComPort = value; }
        [Display("DR49 CH2 COM Port", Description: "DR49 Ch2 Port Number")]
        public string DR49Ch2ComPort { get => dR49Ch2ComPort; set => dR49Ch2ComPort = value; }

        // ToDo: Add property here for each parameter the end user should be able to change.
        #endregion

        /// <summary>
        /// Initializes a new instance of this DUT class.
        /// </summary>


        /// <summary>
        /// Opens a connection to the DUT represented by this class
        /// </summary>

        #region DR49Ch1Functions
        public bool DR49CH1executeScripts(string sendScript, string validateScript)
        {
            int attemptNumber = 1;
            Log.Debug("49DR CH1 Functions");
            string returnValue;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            stopwatch.Restart();

            DR49Ch1ComObj.ReadExisting();
            DR49Ch1ComObj.WriteLine(sendScript);
            if (!string.IsNullOrEmpty(validateScript))
            {
                returnValue = string.Empty;
                do
                {
#if NORMAL
                    TapThread.Sleep(100);
#else
                    TapThread.Sleep(100);
#endif
                    returnValue = DR49Ch1ComObj.ReadExisting();
                    if (!string.IsNullOrEmpty(returnValue))
                    {
                        foreach (var item in returnValue.Split('\r'))
                        {
                            Log.Info("DR49 Ch1 Commands:" + item);

                        }
                        if (returnValue.Contains("configuration failed"))
                        {
                            return false;
                        }

                    }
                    if (stopwatch.ElapsedMilliseconds > 10000 && attemptNumber < 2)
                    {
                        attemptNumber++;
                        DR49Ch1ComObj.WriteLine(sendScript);
                        stopwatch.Restart();

                    }
                    else if (stopwatch.ElapsedMilliseconds > 10000 && attemptNumber >= 2)
                    {
                        return false;
                    }
                } while (!returnValue.Contains(validateScript));
            }
            return true;
        }

        public bool DR49CH1executeCALDSAScripts(string sendScript, string validateScript)
        {
            Log.Debug("49DR CH1 Functions");
            Stopwatch sw = Stopwatch.StartNew();
            string returnValue;
            int attemptNumber = 1;

            DR49Ch1ComObj.ReadExisting();
            DR49Ch1ComObj.WriteLine(sendScript);
            if (!string.IsNullOrEmpty(validateScript))
            {
                returnValue = string.Empty;
                do
                {
#if NORMAL
                    TapThread.Sleep(100);
#else
                    TapThread.Sleep(100);
#endif
                    returnValue = DR49Ch1ComObj.ReadExisting();
                    if (!string.IsNullOrEmpty(returnValue))
                    {
                        foreach (var item in returnValue.Split('\r'))
                        {
                            Log.Info("DR49 Ch1 Commands:" + item);

                        }
                        if (returnValue.Contains("configuration failed") || returnValue.Contains("Initialization Failed"))//17:20:46.791  RjioMRU      DR49 Ch1 Commands:                             for DAC connected with R Channel MUX CH0

                        {
                            DR49Ch1ComObj.WriteLine(sendScript);
                            sw.Restart();
                        }
                    }
                    if (sw.ElapsedMilliseconds > 5000 && attemptNumber < 2)
                    {
                        attemptNumber++;
                        DR49Ch1ComObj.WriteLine(sendScript);
                        sw.Restart();
                    }
                    else if (sw.ElapsedMilliseconds > 5000 && attemptNumber >= 2)
                    {
                        return false;
                    }
                } while (!returnValue.Replace("\n", string.Empty).Contains(validateScript));
            }
            return true;
        }


        internal void DR49CH1Jjio_DPD_InitRun(int chainNumber)
        {
            Log.Debug("49DR CH1 Functions");
            //stopReceiveEvent();
            int flag = 0;
            string returnValue = string.Empty;
            //DR49Ch1ComObj.WriteLine("rjio_dpd_init.sh");
            DR49Ch1ComObj.WriteLine("cfr_dpd_init.sh 100");
            do
            {
                returnValue = DR49Ch1ComObj.ReadExisting();
                if (!string.IsNullOrEmpty(returnValue.Trim()))
                {
                    Log.Info("DPD INIT CH1 for Chain :" + chainNumber + " :" + returnValue);
                }

                TapThread.Sleep(100);
                if (flag == 0 && (returnValue.Replace("\n", string.Empty).Contains("down") || returnValue.Replace("\n", string.Empty).Contains("environment") || returnValue.Replace("\n", string.Empty).Contains("~#")))

                {
                    flag = 1;

                }
                else if (flag == 1 && (returnValue.Replace("\n", string.Empty).Contains("down") || returnValue.Replace("\n", string.Empty).Contains("environment") || returnValue.Replace("\n", string.Empty).Contains("~#")))
                {
                    flag = 2;
                }
                if (returnValue.Contains("CFR/Rescalers/DPD init done"))
                {
                    break;
                }


            } while (flag != 2 || TapThread.Current.AbortToken.IsCancellationRequested);
            //startReceiveEvent();
        }

        public void Dr49_CH1_ControlC()
        {
            Log.Debug("49DR CH1 Functions");
            DR49Ch1ComObj.WriteLine("\x03");
        }
        public void Dr21_ControlC()
        {
            Log.Debug("21DR Functions");
            DR21ComObj.WriteLine("\x03");
        }

        public bool login49drCh1(string username, String password)
        {
            Log.Debug("49DR CH1 Functions");
            int count = 0;
            Stopwatch sp = Stopwatch.StartNew();
            sp.Reset();
            sp.Restart();
            string readValue = string.Empty;
            DR49Ch1ComObj.WriteLine(Environment.NewLine);
            //while (true)
            //{

            //   
            //}
            do
            {
                readValue = DR49Ch1ComObj.ReadExisting();
                //byte[] bytes = Encoding.Default.GetBytes(readValue);
                //readValue = Encoding.UTF8.GetString(bytes);
                // readValue= readValue.Replace('\r', char.MinValue);
                foreach (var sentance in readValue.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(sentance))
                    {
                        Log.Info($"Login 49DR Ch1 : {sentance}");
                        count = 0;
                    }
                    else
                    {
                        TapThread.Sleep(1000);
                        count++;
                        if (count > 35)
                        {
                            DR49Ch1ComObj.WriteLine(Environment.NewLine);
                            Log.Info($"Login 49DR Ch1 : Enter hit");
                            count = 0;
                        }
                    }
                }

                if (readValue.Contains("login:"))
                {
                    DR49Ch1ComObj.WriteLine(username);
                }
                if (readValue.Contains("Password:"))
                {
                    DR49Ch1ComObj.WriteLine(password);
                }

                if (readValue.Contains("~#"))
                {
                    Log.Info("Login 49DR Ch1 : Already Logged in ");
                    return true;
                }

            } while (!readValue.Contains("~#") || TapThread.Current.AbortToken.IsCancellationRequested);




            //do
            //{
            //    readValue = DR49Ch1ComObj.ReadExisting();
            //    if (!string.IsNullOrEmpty(readValue))
            //        Log.Info($"Login 49DR Ch1 :{readValue}");

            //} while (!readValue.Contains("Password:") || TapThread.Current.AbortToken.IsCancellationRequested);


            //DR49Ch1ComObj.WriteLine(password);
            //do
            //{
            //    readValue = DR49Ch1ComObj.ReadExisting();
            //    if (!string.IsNullOrEmpty(readValue))
            //    {
            //        Log.Info($"Login 49DR Ch1 :{readValue}");
            //        count = 0;
            //    }
            //    else
            //    {
            //        TapThread.Sleep(1000);
            //        count++;
            //        if (count > 35)
            //        {
            //            DR49Ch1ComObj.WriteLine(Environment.NewLine);
            //            Log.Info($"Login 49DR Ch1 : Enter hit");
            //            count = 0;
            //        }
            //    }

            //} while (!readValue.Contains("~#") || TapThread.Current.AbortToken.IsCancellationRequested);
            return true;
        }

        public bool dr49ch1oran_modem_initializationcheck(int waittimems, string validataionScript)
        {
            Log.Debug("49DR CH1 Functions");
            int count = 0;
            bool ORANInit = false;
            string readValue = string.Empty;
            do
            {
                TapThread.Sleep(100);
                readValue = DR49Ch1ComObj.ReadExisting();
                if (readValue.Contains(validataionScript))
                {
                    ORANInit = true;
                }
                //byte[] bytes = Encoding.Default.GetBytes(readValue);
                //readValue = Encoding.UTF8.GetString(bytes);
                // readValue= readValue.Replace('\r', char.MinValue);
                foreach (var sentance in readValue.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(sentance))
                    {
                        Log.Info($"49DR  Ch1 ORAN Check : {sentance}");
                        count = 0;
                    }
                    else
                    {
                        TapThread.Sleep(1000);
                        count++;
                        if (count > waittimems)
                        {
                            DR49Ch1ComObj.WriteLine(Environment.NewLine);
                            Log.Info($"ORAN Check 49DR CH1 : Enter hit");
                            count = 0;
                        }
                    }
                }


            } while (!readValue.Contains(validataionScript) || TapThread.Current.AbortToken.IsCancellationRequested);
            DR49Ch1ComObj.WriteLine(Environment.NewLine);
            return ORANInit;
        }

        public bool Dr49_CH1_DPD_Measurement(int channelNumber, out double Txvalue, out double RxValue)
        {
            Log.Debug("49DR CH1 Functions");
            int count = 0;
            Stopwatch sp = new Stopwatch();
            sp.Reset();
            sp.Restart();
            Txvalue = 0;
            RxValue = 0;
            DR49Ch1ComObj.WriteLine("\x03");
            // DR49Ch1ComObj.WriteLine(Environment.NewLine);

            bool Read_Capture_Power_Meter = false;
            //string command = (channelNumber <= 7 ? "dpd1-debug" : "dpd2-debug");
            string command = (channelNumber <= 7 ? "dpd_dbg_host_app 0xa0060000" : "dpd_dbg_host_app 0xa00e0000");
            channelNumber = channelNumber % 8;
            DR49Ch1ComObj.WriteLine(command);
            string readValue = string.Empty;
            do
            {
                TapThread.Sleep(200);
                readValue = DR49Ch1ComObj.ReadExisting();
                if (!string.IsNullOrEmpty(readValue.Trim()))
                {
                    foreach (var line in readValue.Split('\n'))
                    {
                        Log.Info("DPD Meas Ch1 :" + line);
                    }
                    sp.Restart();
                }
                else
                {
                    if (sp.ElapsedMilliseconds > 10000)
                    {
                        sp.Restart();
                        DR49Ch1ComObj.WriteLine("\x03");
                        TapThread.Sleep(200);
                        DR49Ch1ComObj.WriteLine("\x03");
                        TapThread.Sleep(1000);
                        DR49Ch1ComObj.ReadExisting();
                        readValue = string.Empty;
                        DR49Ch1ComObj.WriteLine(command);
                    }
                }

                ///DPD HOST Example Application MAIN MENU
                if (readValue.Replace("\n", string.Empty).Contains("Selection:") && (
                    readValue.Replace("\n", string.Empty).Contains("Open DPD Host Interface") ||
                    readValue.Contains("DFE System Reset") ||
                    readValue.Contains("")))
                {
                    if (!Read_Capture_Power_Meter)
                    {
                        DR49Ch1ComObj.WriteLine("1");
                    }
                    else
                    {
                        DR49Ch1ComObj.WriteLine("2");
                    }
                }




                if (readValue.Replace("\n", string.Empty).Contains("DPD User Interface Base Address: [0xa0060000]" )|| readValue.Replace("\n", string.Empty).Contains("DPD User Interface Base Address: [0xa4060000]"))
                {
                    DR49Ch1ComObj.WriteLine(Environment.NewLine);
                }

                if ((readValue.Replace("\n", string.Empty).Contains("Application SUB MENU") ||
                    readValue.Contains("Display DPD build settings") ||
                    readValue.Contains("Restore default DPD parameters") ||
                    readValue.Contains("Display DPD parameters") ||
                    readValue.Contains("Detect Alignment Parameters") ||
                    readValue.Contains("Detect Alignment Parameters") ||
                    readValue.Contains("Read Capture Power Meter Measurement") ||
                    readValue.Contains("Set modified DPD parameters") ||
                    readValue.Contains("Test PAPR and Test Power meter") ||
                    readValue.Contains("Test Pre-Distortion") ||
                    readValue.Contains("DCL Options") ||
                    readValue.Contains("DPD Monitors and Diagnostics") ||
                    readValue.Contains("Reset Coefficients") ||
                    readValue.Contains("Enable Output") ||
                    readValue.Contains("Disable Output") ||
                    readValue.Contains("DPD ON") ||
                    readValue.Contains("DPD OFF") ||
                    readValue.Contains("Export Data to text file") ||
                    readValue.Contains("Exit to Application MAIN MENU")) &&
                    readValue.Replace("\n", string.Empty).Contains("Selection:"))
                {
                    if (!Read_Capture_Power_Meter)
                    {
                        string TxRxValues = string.Empty;
                        string txStr = string.Empty;
                        string rxStr = string.Empty;
                        TapThread.Sleep(200);
                        DR49Ch1ComObj.ReadExisting();
                        DR49Ch1ComObj.WriteLine("6");
                        do
                        {
                            TxRxValues += DR49Ch1ComObj.ReadExisting().Replace("\n", string.Empty);
                            //TapThread.Sleep(200);
                        } while (!TxRxValues.Replace("\n", string.Empty).Contains("Sub Menu Selection:"));
                        ///////////////////////////////////////////////
                        foreach (var txRxLines in TxRxValues.Split('\r'))
                        {
                            Log.Info("DPD Meaure Ch2 :" + txRxLines);
                        }
                        foreach (var sentance in TxRxValues.Split('\r'))
                        {
                            if (sentance.Contains("tx_pwr_dbfs:") && sentance.Contains("portnum " + channelNumber.ToString()))
                            {
                                txStr = sentance.Substring(sentance.IndexOf("tx_pwr_dbfs:") + "tx_pwr_dbfs:".Length).Trim().Split(' ')[0];
                                Console.WriteLine($"{sentance}");
                            }
                            if (sentance.Contains("rx_pwr_dbfs:") && sentance.Contains("portnum " + channelNumber.ToString()))
                            {
                                rxStr = sentance.Substring(sentance.IndexOf("rx_pwr_dbfs:") + "rx_pwr_dbfs:".Length).Trim().Split(' ')[0];
                                Console.WriteLine($"{sentance}");
                            }
                        }

                        //Console.WriteLine(txStr);
                        //Console.WriteLine(rxStr);
                        //if (string.IsNullOrEmpty(txStr) || string.IsNullOrEmpty(rxStr))
                        //{
                        //    throw new Exception("Tx or Rx not found");
                        //}
                        if (!string.IsNullOrEmpty(txStr))
                        {
                            Txvalue = double.Parse(txStr);
                            RxValue = double.Parse(rxStr);
                        }
                        ///////////////////////////////////////////////
                        //  extractTxRx(channelNumber, out txvalue, out RxValue, TxRxValues);
                        Read_Capture_Power_Meter = true;
                        DR49Ch1ComObj.WriteLine("99");
                    }
                }

                if (readValue.Contains("Exiting program"))
                {
                    DR49Ch1ComObj.WriteLine(Environment.NewLine);
                    break;
                }

                if (readValue.Replace("\n", string.Empty).Contains("Failed to open DPD Host Interface"))
                {
                    DR49Ch1ComObj.WriteLine("\x03");
                    TapThread.Sleep(500);
                    DR49Ch1ComObj.WriteLine("\x03");
                    TapThread.Sleep(1000);
                    DR49Ch1ComObj.WriteLine(command);
                    Read_Capture_Power_Meter = false;
                    Log.Info("DPD Meas Repeting for Failed to open DPD Host Interface");
                    //throw new Exception("DPD Host interface open failed");
                }
                if (readValue.Replace("\n", string.Empty).Contains("Error! Wrong selection"))
                {
                    DR49Ch1ComObj.WriteLine("\x03");
                    TapThread.Sleep(500);
                    DR49Ch1ComObj.ReadExisting();
                    readValue = string.Empty;
                    Log.Info("DPD Meas Repeting for Error! Wrong selection");
                    DR49Ch1ComObj.WriteLine(command);

                }

            } while (true);
            //startReceiveEvent();
            return true;
            //Log.Debug("49DR CH1 Functions");
            //// stopReceiveEvent();
            //int count = 0;
            //txvalue = 0;
            //RxValue = 0;
            //DR49Ch1ComObj.WriteLine("\x03");
            //Stopwatch sw = Stopwatch.StartNew();
            //// DR49Ch1ComObj.WriteLine(Environment.NewLine);

            //bool Read_Capture_Power_Meter = false;
            //string command = (channelNumber <= 7 ? "dpd1-debug" : "dpd2-debug");
            //channelNumber = channelNumber % 8;
            //DR49Ch1ComObj.WriteLine(command);
            //string readValue = string.Empty;
            //do
            //{
            //    TapThread.Sleep(200);
            //    readValue = DR49Ch1ComObj.ReadExisting();
            //    if (!string.IsNullOrEmpty(readValue.Trim()))
            //    {
            //        foreach (var line in readValue.Split('\n'))
            //        {
            //            Log.Info("DPD Meas CH1 " + line);
            //        }
            //        sw.Restart();
            //    }
            //    else
            //    {
            //        if (sw.ElapsedMilliseconds > 10000)
            //        {
            //            DR49Ch1ComObj.WriteLine("\x03");
            //            TapThread.Sleep(100);
            //            DR49Ch1ComObj.ReadExisting();
            //            readValue = string.Empty;
            //            DR49Ch1ComObj.WriteLine(command);
            //            sw.Restart();
            //        }

            //    }


            //    if (readValue.Replace("\n", string.Empty).Contains("Selection:") && readValue.Replace("\n", string.Empty).Contains("[1] Open DPD Host Interface"))
            //    {
            //        if (!Read_Capture_Power_Meter)
            //        {
            //            DR49Ch1ComObj.WriteLine("1");

            //        }
            //        else
            //        {
            //            DR49Ch1ComObj.WriteLine("2");
            //        }
            //    }


            //    if (readValue.Replace("\n", string.Empty).Contains("Failed to open DPD Host Interface"))
            //    {
            //        DR49Ch1ComObj.WriteLine("\x03");
            //        TapThread.Sleep(100);
            //        DR49Ch1ComObj.WriteLine(command);
            //        Read_Capture_Power_Meter = false;
            //        Log.Info("DPD Meas Repeating for Failed to open DPD Host Interface");
            //        //throw new Exception("DPD Host interface open failed");
            //    }
            //    if (readValue.Replace("\n", string.Empty).Contains("Error! Wrong selection"))
            //        DR49Ch1ComObj.WriteLine("\x03");
            //        TapThread.Sleep(100);
            //        DR49Ch1ComObj.ReadExisting();
            //        readValue = string.Empty;
            //        DR49Ch1ComObj.WriteLine(command);
            //        Log.Info("DPD Meas Repeating for Error! Wrong selection");
            //    }

            //    if (readValue.Replace("\n", string.Empty).Contains("DPD User Interface Base Address: [0xa4000000]:") || readValue.Replace("\n", string.Empty).Contains("DPD User Interface Base Address: [0xa4060000]"))
            //    {
            //        DR49Ch1ComObj.WriteLine(Environment.NewLine);

            //    }

            //    if (readValue.Replace("\n", string.Empty).Contains("Application SUB MENU") && readValue.Replace("\n", string.Empty).Contains("Selection:"))
            //    {

            //        if (!Read_Capture_Power_Meter)
            //        {
            //            string TxRxValues = string.Empty;
            //            string txStr = string.Empty;
            //            string rxStr = string.Empty;
            //            TapThread.Sleep(100);
            //            DR49Ch1ComObj.WriteLine("6");

            //            do
            //            {


            //                TxRxValues += DR49Ch1ComObj.ReadExisting().Replace("\n", string.Empty);
            //                //TapThread.Sleep(200);

            //            } while (!TxRxValues.Replace("\n", string.Empty).Contains("Sub Menu Selection:"));
            //            ///////////////////////////////////////////////
            //            foreach (var txrxValuesLine in TxRxValues.Split('\r'))
            //            {
            //                Log.Info("DPD Meas Ch1 :" + TxRxValues);
            //            }


            //            foreach (var sentance in TxRxValues.Split('\n'))

            //            {
            //                if (sentance.Contains("tx_pwr_dbfs:") && sentance.Contains("portnum " + channelNumber
            //                    .ToString()))
            //                {
            //                    txStr = sentance.Substring(sentance.IndexOf("tx_pwr_dbfs:") + "tx_pwr_dbfs:".Length).Trim().Split(' ')[0];

            //                    Console.WriteLine($"{sentance}");
            //                }
            //                if (sentance.Contains("rx_pwr_dbfs:") && sentance.Contains("portnum " + channelNumber.ToString()))
            //                {
            //                    rxStr = sentance.Substring(sentance.IndexOf("rx_pwr_dbfs:") + "rx_pwr_dbfs:".Length).Trim().Split(' ')[0];
            //                    Console.WriteLine($"{sentance}");
            //                }
            //            }

            //            //Console.WriteLine(txStr);
            //            //Console.WriteLine(rxStr);
            //            //if (string.IsNullOrEmpty(txStr) || string.IsNullOrEmpty(rxStr))
            //            //{
            //            //    throw new Exception("Tx or Rx not found");
            //            //}
            //            if (!string.IsNullOrEmpty(txStr))
            //            {
            //                txvalue = double.Parse(txStr);
            //                RxValue = double.Parse(rxStr);
            //            }

            //            ///////////////////////////////////////////////



            //            //  extractTxRx(channelNumber, out txvalue, out RxValue, TxRxValues);

            //            Read_Capture_Power_Meter = true;

            //            DR49Ch1ComObj.WriteLine("99");
            //        }
            //    }


            //    if (readValue.Contains("Exiting program"))
            //    {
            //        DR49Ch1ComObj.WriteLine(Environment.NewLine);
            //        break;
            //    }

            //} while (true);
            ////startReceiveEvent();
            //return true;
        }
        public bool Dr49_CH1_DPD_TillDCLRun(int channelNumber)
        {
            bool StartDone = false;


            bool ResetCoefficientsdone = false;
            bool DCLRunDOne = false;
            bool DCLOptions_MenuDone = false;
            bool DCLExitDone = false;

            Log.Debug("49DR CH1 Functions");
            int count = 0;
            Stopwatch sp = new Stopwatch();
            sp.Reset();
            sp.Restart();

            DR49Ch1ComObj.WriteLine("\x03");
            // DR49Ch1ComObj.WriteLine(Environment.NewLine);

            bool Read_Capture_Power_Meter = false;
            string command = (channelNumber <= 7 ? "dpd_dbg_host_app 0xa0060000" : "dpd_dbg_host_app 0xa00e0000");
            channelNumber = channelNumber % 8;
            DR49Ch1ComObj.WriteLine(command);
            string readValue = string.Empty;
            do
            {
                TapThread.Sleep(200);
                readValue = DR49Ch1ComObj.ReadExisting();
                if (!string.IsNullOrEmpty(readValue.Trim()))
                {
                    foreach (var line in readValue.Split('\n'))
                    {
                        Log.Info("DPD Meas Ch1 :" + line);
                    }
                    sp.Restart();
                }

                if (sp.ElapsedMilliseconds > 10000)
                {
                    sp.Restart();
                    DR49Ch1ComObj.WriteLine("\x03");
                    TapThread.Sleep(1000);
                    DR49Ch1ComObj.ReadExisting();
                    readValue = string.Empty;
                    DR49Ch1ComObj.WriteLine(command);
                }


                ///DPD HOST Example Application MAIN MENU
                if (readValue.Replace("\n", string.Empty).Contains("on:") && (
                    readValue.Replace("\n", string.Empty).Contains("Open DPD Host Interface") ||
                    readValue.Contains("DFE System Reset")))
                {
                    if (!StartDone)
                    {
                        DR49Ch1ComObj.WriteLine("1");
                        StartDone = true;
                    }
                    else
                    {
                        DR49Ch1ComObj.WriteLine("2");
                        StartDone = false;
                    }
                }






                if (readValue.Replace("\n", string.Empty).Contains("DPD User Interface Base Address: [0xa4000000]:") || readValue.Replace("\n", string.Empty).Contains("DPD User Interface Base Address: [0xa4060000]"))
                {
                    DR49Ch1ComObj.WriteLine(Environment.NewLine);
                }




                if ((readValue.Replace("\n", string.Empty).Contains("Application SUB MENU") ||
                    readValue.Contains("Display DPD build settings") ||
                    readValue.Contains("Restore default DPD parameters") ||
                    readValue.Contains("Display DPD parameters") ||
                    readValue.Contains("Detect Alignment Parameters") ||
                    readValue.Contains("Detect Alignment Parameters") ||
                    readValue.Contains("Read Capture Power Meter Measurement") ||
                    readValue.Contains("Set modified DPD parameters") ||
                    readValue.Contains("Test PAPR and Test Power meter") ||
                    readValue.Contains("Test Pre-Distortion") ||
                    readValue.Contains("[10] DCL Options") ||
                    readValue.Contains("DPD Monitors and Diagnostics") ||
                    readValue.Contains("[12] Reset Coefficients") ||
                    readValue.Contains("Enable Output") ||
                    readValue.Contains("Disable Output") ||
                    readValue.Contains("DPD ON") ||
                    readValue.Contains("DPD OFF") ||
                    readValue.Contains("Export Data to text file") ||
                    readValue.Contains("Exit to Application MAIN MENU")) &&
                    readValue.Replace("\n", string.Empty).Contains("Selection:"))
                {
                    if (!ResetCoefficientsdone)
                    {
                        DR49Ch1ComObj.ReadExisting();
                        DR49Ch1ComObj.WriteLine("12");
                        ResetCoefficientsdone = true;
                    }
                    if (ResetCoefficientsdone && !DCLOptions_MenuDone)
                    {
                        DR49Ch1ComObj.WriteLine("10");
                        DCLOptions_MenuDone = true;
                    }
                    //if (!Read_Capture_Power_Meter)
                    //{
                    //    string TxRxValues = string.Empty;
                    //    string txStr = string.Empty;
                    //    string rxStr = string.Empty;
                    //    TapThread.Sleep(200);
                    //    DR49Ch1ComObj.ReadExisting();
                    //    DR49Ch1ComObj.WriteLine("6");
                    //    do
                    //    {
                    //        TxRxValues += DR49Ch1ComObj.ReadExisting().Replace("\n", string.Empty);
                    //        //TapThread.Sleep(200);
                    //    } while (!TxRxValues.Replace("\n", string.Empty).Contains("Sub Menu Selection:"));
                    //    ///////////////////////////////////////////////
                    //    //foreach (var txRxLines in TxRxValues.Split('\r'))
                    //    //{
                    //    //    Log.Info("DPD Meaure Ch2 :" + txRxLines);
                    //    //}
                    //    //foreach (var sentance in TxRxValues.Split('\r'))
                    //    //{
                    //    //    if (sentance.Contains("tx_pwr_dbfs:") && sentance.Contains("portnum " + channelNumber.ToString()))
                    //    //    {
                    //    //        txStr = sentance.Substring(sentance.IndexOf("tx_pwr_dbfs:") + "tx_pwr_dbfs:".Length).Trim().Split(' ')[0];
                    //    //        Console.WriteLine($"{sentance}");
                    //    //    }
                    //    //    if (sentance.Contains("rx_pwr_dbfs:") && sentance.Contains("portnum " + channelNumber.ToString()))
                    //    //    {
                    //    //        rxStr = sentance.Substring(sentance.IndexOf("rx_pwr_dbfs:") + "rx_pwr_dbfs:".Length).Trim().Split(' ')[0];
                    //    //        Console.WriteLine($"{sentance}");
                    //    //    }
                    //    //}

                    //    //Console.WriteLine(txStr);
                    //    //Console.WriteLine(rxStr);
                    //    //if (string.IsNullOrEmpty(txStr) || string.IsNullOrEmpty(rxStr))
                    //    //{
                    //    //    throw new Exception("Tx or Rx not found");
                    //    //}
                    //    //if (!string.IsNullOrEmpty(txStr))
                    //    //{
                    //    //    Txvalue = double.Parse(txStr);
                    //    //    RxValue = double.Parse(rxStr);
                    //    //}
                    //    ///////////////////////////////////////////////
                    //    //  extractTxRx(channelNumber, out txvalue, out RxValue, TxRxValues);
                    //    Read_Capture_Power_Meter = true;
                    //    DR49Ch1ComObj.WriteLine("99");
                    //}
                }

                if (readValue.Replace("\n", string.Empty).Contains("on:") && (
                    readValue.Contains("[8] Port 7") ||
                    readValue.Contains("[7] Port 6") ||
                    readValue.Contains("[6] Port 5") ||
                    readValue.Contains("[5] Port 4") ||
                    readValue.Contains("[4] Port 3")))
                {
                    DR49Ch1ComObj.WriteLine(channelNumber.ToString());
                }


                if (readValue.Replace("\n", string.Empty).Contains("on:") && (
                    readValue.Contains("DCL Options MENU") ||
                    readValue.Contains("[0] Run DCL") ||
                    readValue.Contains("[1] Exit DCL") ||
                    readValue.Contains("[2] Reset DCL") ||
                    readValue.Contains("[3] DCL get skip port") ||
                    readValue.Contains("[4] DCL set skip port")))
                {
                    if (!DCLRunDOne)
                    {
                        DR49Ch1ComObj.WriteLine("0");
                        DCLRunDOne = true;
                    }

                }




                if (readValue.Contains("Exiting program"))
                {
                    DR49Ch1ComObj.WriteLine(Environment.NewLine);
                    break;
                }

                if (readValue.Replace("\n", string.Empty).Contains("Failed to open DPD Host Interface"))
                {
                    DR49Ch1ComObj.WriteLine("\x03");
                    TapThread.Sleep(100);
                    DR49Ch1ComObj.WriteLine(command);
                    Read_Capture_Power_Meter = false;
                    Log.Info("DPD Meas Repeting for Failed to open DPD Host Interface");
                    //throw new Exception("DPD Host interface open failed");
                }
                if (readValue.Replace("\n", string.Empty).Contains("Error! Wrong selection"))
                {
                    DR49Ch1ComObj.WriteLine("\x03");
                    TapThread.Sleep(100);
                    DR49Ch1ComObj.ReadExisting();
                    readValue = string.Empty;
                    Log.Info("DPD Meas Repeting for Error! Wrong selection");
                    DR49Ch1ComObj.WriteLine(command);

                }

            } while (true);
            //startReceiveEvent();
            return true;


        }
        public void dPdExit(int dpdport)
        {
            if (dpdport == 0)
            {
                return;
            }
            //ResetCoefficientsdone = false;
            //DCLRunDOne = false;

            string DpDReadValue = string.Empty;
            string MainHeader = string.Empty;
            string subHeader = string.Empty;
            while (true)
            {
#if NORMAL
                TapThread.Sleep(500);
#else
                TapThread.Sleep(100);
#endif
                DpDReadValue = DR49Ch1ComObj.ReadExisting();
                foreach (var item in DpDReadValue.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(DpDReadValue))
                        Log.Info("DPD Reset :" + item);
                    if (item.Contains("**  DPD HOST Example Application MAIN MENU  **"))
                    {
                        MainHeader = "**  DPD HOST Example Application MAIN MENU  **";
                    }
                    if (item.Contains("DPD User Interface Base Address: [0xa4000000]:"))
                    {
                        MainHeader = "DPD User Interface Base Address: [0xa4000000]:";
                    }
                    if (item.Contains("**  Reset Coefficients MENU  **"))
                    {
                        MainHeader = "**  Reset Coefficients MENU  **";
                    }
                    if (item.Contains("**  DPD HOST Example Application SUB MENU  **"))
                    {
                        MainHeader = "**  DPD HOST Example Application SUB MENU  **";
                    }
                    if (item.Contains("**  DCL Options MENU  **"))
                    {
                        MainHeader = "**  DCL Options MENU  **";
                    }
                    if (item.Contains("Menu Selection:"))
                    {
                        subHeader = "Menu Selection:";
                    }
                    if (item.Contains("Sub Menu Selection:"))
                    {
                        subHeader = "Sub Menu Selection:";
                    }
                }

                if (MainHeader == "**  DPD HOST Example Application SUB MENU  **" && subHeader == "Sub Menu Selection:")
                {
                    DR49Ch1ComObj.WriteLine("10");
                }


                if (MainHeader == "**  DCL Options MENU  **" && subHeader == "Menu Selection:")
                {
                    DR49Ch1ComObj.WriteLine((1).ToString());

                    //DCLExitDone = true;
                    break;
                }
                DR49Ch1ComObj.WriteLine(String.Empty);
                //Log.Info(dPDComObj.ReadLine());

                DR49Ch1ComObj.DiscardInBuffer();
                DR49Ch1ComObj.DiscardOutBuffer();
            }
        }
        private void DR49Ch1ComObj_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Log.Debug("49DR CH1 Functions");
            var returnValue = DR49Ch1ComObj.ReadExisting();
            if (!string.IsNullOrEmpty(returnValue))
            {
                foreach (var item in returnValue.Split('\n'))
                {
                    Log.Info(item);
                }
                if (returnValue.Contains("out of range"))
                {
                    DR49Ch1ComObj.WriteLine("rj-dsa-init 16 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 0x7F 16 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 0x0f 2 1200 1611");
                }
            }
        }


        public void startReceiveEvent()
        {
            Log.Debug("49DR CH1 Functions");
            DR49Ch1ComObj.DataReceived += DR49Ch1ComObj_DataReceived;
        }
        public void stopReceiveEvent()
        {
            Log.Debug("49DR CH1 Functions");
            DR49Ch1ComObj.DataReceived -= DR49Ch1ComObj_DataReceived;
        }


        public string Dr49_Ch1_checkYoctoVersion(string commandScript)
        {
            string yoctoVersion = string.Empty;
            DR49Ch1ComObj.ReadExisting();
            DR49Ch1ComObj.WriteLine(commandScript);
            TapThread.Sleep(100);
            var returnValue = DR49Ch1ComObj.ReadExisting();
            if (!string.IsNullOrEmpty(returnValue))
            {
                foreach (var item in returnValue.Split('\n'))
                {
                    Log.Info("DR49 Ch1 Yokto version :" + item);

                }

            }
            yoctoVersion = returnValue.Split('\n')[1].Split(':')[1].Trim();
            return yoctoVersion;
        }

        public string Dr49_Ch1_checkSlot(string commandScript)
        {
            string Slot = string.Empty;
            DR49Ch1ComObj.ReadExisting();
            DR49Ch1ComObj.WriteLine(commandScript);
            TapThread.Sleep(100);
            var returnValue = DR49Ch1ComObj.ReadExisting();
            if (!string.IsNullOrEmpty(returnValue))
            {
                foreach (var item in returnValue.Split('\n'))
                {
                    Log.Info("DR49 Ch1 slot :" + item);

                }

            }
            Slot = returnValue.Split('\n')[1].Split('=')[1].Trim();
            return Slot;
        }



        //        public bool Dr49_CH1_DPD_reset(int channelNumber, out double Txvalue, out double RxValue)
        //        {
        //            Log.Debug("49DR CH1 Functions");
        //            int count = 0;
        //            Stopwatch sp = new Stopwatch();
        //            sp.Reset();
        //            sp.Restart();
        //            Txvalue = 0;
        //            RxValue = 0;
        //            DR49Ch1ComObj.WriteLine("\x03");
        //            // DR49Ch1ComObj.WriteLine(Environment.NewLine);

        //            bool Read_Capture_Power_Meter = false;
        //            bool Reset_Coefficients_Done = false;
        //            bool DCL_Options = false;
        //            bool Run_DCL = false;
        //            bool Exit_DCL = false;


        //            string command = (channelNumber <= 7 ? "dpd1-debug" : "dpd2-debug"

        //                channelNumber = channelNumber % 8;
        //            DR49Ch1ComObj.WriteLine(command);
        //            string readValue = string.Empty;
        //            do
        //            {
        //                TapThread.Sleep(200);
        //                readValue = DR49Ch1ComObj.ReadExisting();
        //                if (!string.IsNullOrEmpty(readValue.Trim()))
        //                {
        //                    foreach (var line in readValue.Split('\n'))
        //                    {
        //                        Log.Info("DPD Meas Ch2 :" + line);
        //                    }
        //                    sp.Restart();
        //                }
        //                else
        //                {
        //                    if (sp.ElapsedMilliseconds > 10000)
        //                    {
        //                        sp.Restart();
        //                        DR49Ch1ComObj.WriteLine("\x03");
        //                        TapThread.Sleep(100);
        //                        DR49Ch1ComObj.ReadExisting();
        //                        readValue = string.Empty;
        //                        DR49Ch1ComObj.WriteLine(command);
        //                    }
        //                }

        //                ///DPD HOST Example Application MAIN MENU
        //                if (readValue.Replace("\n", string.Empty).Contains("Selection:") && (
        //                    readValue.Replace("\n", string.Empty).Contains("Open DPD Host Interface") ||
        //                    readValue.Contains("DFE System Reset") ||
        //                    readValue.Contains("")))
        //                {
        //                    if (!Read_Capture_Power_Meter)
        //                    {
        //                        DR49Ch1ComObj.WriteLine("1");
        //                    }
        //                    else
        //                    {
        //                        DR49Ch1ComObj.WriteLine("2");
        //                    }
        //                }


        //                if (readValue.Replace("\n", string.Empty).Contains("DPD Us

        //er Interface Base Address: [0xa4000000]:") || readValue.Replace("\n", string.Empty).Contains("DPD User Interface Base Address: [0xa4060000]"))
        //                {
        //                    DR49Ch1ComObj.WriteLine(Environment.NewLine);
        //                }

        //                if ((readValue.Replace("\n", string.Empty).Contains("Application SUB MENU") ||
        //                    readValue.Contains("Display DPD build settings") ||
        //                    readValue.Contains("Restore default DPD parameters") ||
        //                    readValue.Contains("Display DPD parameters") ||
        //                    readValue.Contains("Detect Alignment Parameters") ||
        //                    readValue.Contains("Detect Alignment Parameters") ||
        //                    readValue.Contains("Read Capture Power Meter Measurement") ||
        //                    readValue.Contains("Set modified DPD parameters") ||
        //                    readValue.Contains("Test PAPR and Test Power meter") ||
        //                    readValue.Contains("Test Pre-Distortion") ||
        //                    readValue.Contains("DCL Options") ||
        //                    readValue.Contains("DPD Monitors and Diagnostics") ||
        //                    readValue.Contains("Reset Coefficients") ||
        //                    readValue.Contains("Enable Output") ||
        //                    readValue.Contains("Disable Output") ||
        //                    readValue.Contains("DPD ON") ||
        //                    readValue.Contains("DPD OFF") ||
        //                    readValue.Contains("Export Data to text file") ||
        //                    readValue.Contains("Exit to Application MAIN MENU")) &&
        //                    readValue.Replace("\n", string.Empty).Contains("Selection:"))
        //                {
        //                    if (!Read_Capture_Power_Meter)
        //                    {
        //                        Read_Capture_Power_Meter = true;
        //                         string txStr = string.Empty;
        //                        string rxStr = string.Empty;
        //                        TapThread.Sleep(200);
        //                        DR49Ch1ComObj.ReadExisting();
        //                        DR49Ch1ComObj.WriteLine("6");
        //                        do
        //                        {
        //                            TxRxValues += DR49Ch1ComObj.ReadExisting().Replace("\n", string.Empty);
        //                            //TapThread.Sleep(200);
        //                        } while (!TxRxValues.Replace("\n", string.Empty).Contains("Sub Menu Selection:"));
        //                        ///////////////////////////////////////////////
        //                        foreach (var txRxLines in TxRxValues.Split('\r'))
        //                        {
        //                            Log.Info("DPD Meaure Ch2 :" + txRxLines);
        //                        }
        //                        foreach (var sentance in TxRxValues.Split('\r'))
        //                        {
        //                            if (sentance.Contains("tx_pwr_dbfs:") && sentance.Contains("portnum " + channelNumber.ToString()))
        //                            {
        //                                txStr = sentance.Substring(sentance.IndexOf("tx_pwr_dbfs:") + "tx_pwr_dbfs:".Length).Trim().Split(' ')[0];
        //                                Console.WriteLine($"{sentance}");
        //                            }
        //                            if (sentance.Contains("rx_pwr_dbfs:") && sentance.Contains("portnum " + channelNumber.ToString()))
        //                            {
        //                                rxStr = sentance.Substring(sentance.IndexOf("rx_pwr_dbfs:") + "rx_pwr_dbfs:".Length).Trim().Split(' ')[0];
        //                                Console.WriteLine($"{sentance}");
        //                            }
        //                        }

        //                        //Console.WriteLine(txStr);
        //                        //Console.WriteLine(rxStr);
        //                        //if (string.IsNullOrEmpty(txStr) || string.IsNullOrEmpty(rxStr))
        //                        //{
        //                        //    throw new Exception("Tx or Rx not found");
        //                        //}
        //                        if (!string.IsNullOrEmpty(txStr))
        //                        {
        //                            Txvalue = double.Parse(txStr);
        //                            RxValue = double.Parse(rxStr);
        //                        }
        //                        ///////////////////////////////////////////////
        //                        //  extractTxRx(channelNumber, out txvalue, out RxValue, TxRxValues);
        //                        DR49Ch1ComObj.WriteLine("99");
        //                    }
        //                        Read_Capture_Power_Meter = true;

        //                    DR49Ch1ComObj.WriteLine(Environment.NewLine);
        //                    break;
        //                }

        //                if (readValue.Replace("\n", string.Empty).Contains("Failed to open DPD Host Interface"))
        //                {
        //                    DR49Ch1ComObj.WriteLine("\x03");
        //                    TapThread.Sleep(100);
        //                    DR49Ch1ComObj.WriteLine(command);
        //                    Read_Capture_Power_Meter = false;
        //                    Log.Info("DPD Meas Repeting for Failed to open DPD Host Interface");
        //                    //throw new Exception("DPD Host interface open failed");
        //                }
        //                if (readValue.Replace("\n", string.Empty).Contains("Error! Wrong selection"))
        //                {
        //                    DR49Ch1ComObj.WriteLine("\x03");
        //                    TapThread.Sleep(100);
        //                    DR49Ch1ComObj.ReadExisting();
        //                    readValue = string.Empty;
        //                    Log.Info("DPD Meas Repeting for Error! Wrong selection");
        //                    DR49Ch1ComObj.WriteLine(command);

        //                }

        //            } while (true);
        //        }



        #endregion DR49Ch1Functions

        #region DR49Ch2Functions
        public bool DR49CH2executeScripts(string sendScript, string validateScript)
        {
            int attemptNumber = 1;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            stopwatch.Restart();
            Log.Debug("49DR CH2 Functions");
            string returnValue;
            DR49Ch2ComObj.WriteLine(sendScript);
            if (!string.IsNullOrEmpty(validateScript))
            {
                returnValue = string.Empty;
                do
                {
#if NORMAL
                    TapThread.Sleep(100);
#else
                    TapThread.Sleep(100);
#endif
                    returnValue = DR49Ch2ComObj.ReadExisting();
                    if (!string.IsNullOrEmpty(returnValue))
                    {
                        foreach (var item in returnValue.Split('\r'))
                        {
                            Log.Info("DR49 Ch2 Commands:" + item);

                        }

                    }
                    if (returnValue.Contains("configuration failed"))
                    {
                        return false;
                    }
                    if (stopwatch.ElapsedMilliseconds > 10000 && attemptNumber < 2)
                    {
                        attemptNumber++;
                        DR49Ch2ComObj.WriteLine(sendScript);
                        stopwatch.Restart();

                    }
                    else if (stopwatch.ElapsedMilliseconds > 10000 && attemptNumber >= 2)
                    {
                        return false;
                    }
                } while (!returnValue.Contains(validateScript));
            }
            return true;
        }

        public bool DR49CH2executeCALDSAScripts(string sendScript, string validateScript)
        {
            Log.Debug("49DR CH2 Functions");
            string returnValue;
            int attemptNumber = 1;
            Stopwatch sw = Stopwatch.StartNew();
            DR49Ch2ComObj.ReadExisting();
            DR49Ch2ComObj.WriteLine(sendScript);
            if (!string.IsNullOrEmpty(validateScript))
            {
                returnValue = string.Empty;
                do
                {
#if NORMAL
                    TapThread.Sleep(100);
#else
                    TapThread.Sleep(100);
#endif
                    returnValue = DR49Ch2ComObj.ReadExisting();
                    if (!string.IsNullOrEmpty(returnValue))
                    {
                        foreach (var item in returnValue.Split('\r'))
                        {
                            Log.Info("DR49 Ch2 Commands:" + item);

                        }
                        if (returnValue.Contains("configuration failed") || returnValue.Contains("Initialization Failed"))//17:20:46.791  RjioMRU      DR49 Ch1 Commands:                             for DAC connected with R Channel MUX CH0

                        {
                            DR49Ch2ComObj.WriteLine(sendScript);
                            sw.Restart();
                        }
                    }
                    if (sw.ElapsedMilliseconds > 5000 && attemptNumber < 2)
                    {
                        attemptNumber++;
                        DR49Ch2ComObj.WriteLine(sendScript);
                        sw.Restart();
                    }
                    else if (sw.ElapsedMilliseconds > 5000 && attemptNumber >= 2)
                    {
                        return false;
                    }
                } while (!returnValue.Contains(validateScript));
            }
            return true;
        }
        internal void DR49CH2Jjio_DPD_InitRun(int chainNumber)
        {
            Log.Debug("49DR CH2 Functions");
            //stopReceiveEvent();
            int flag = 0;
            string returnValue = string.Empty;
            DR49Ch2ComObj.WriteLine("cfr_dpd_init.sh 100");
            do
            {
                returnValue = DR49Ch2ComObj.ReadExisting();
                if (!string.IsNullOrEmpty(returnValue.Trim()))
                {
                    Log.Info("DPD INIT CH2 for Chain : " + chainNumber + " is : " + returnValue);
                }

                TapThread.Sleep(100);
                if (flag == 0 && (returnValue.Replace("\n", string.Empty).Contains("down") || returnValue.Replace("\n", string.Empty).Contains("environment") || returnValue.Replace("\n", string.Empty).Contains("~#")))

                {
                    flag = 1;

                }
                else if (flag == 1 && (returnValue.Replace("\n", string.Empty).Contains("down") || returnValue.Replace("\n", string.Empty).Contains("environment") || returnValue.Replace("\n", string.Empty).Contains("~#")))
                {
                    flag = 2;
                }


            } while (flag != 2 || TapThread.Current.AbortToken.IsCancellationRequested);
            //startReceiveEvent();
        }
        public void Dr49_CH2_ControlC()
        {
            Log.Debug("49DR CH2 Functions");
            DR49Ch2ComObj.WriteLine("\x03");
        }

        public bool login49drCh2(string username, String password)
        {
            Log.Debug("49DR CH2 Functions");
            int count = 0;
            Stopwatch sp = Stopwatch.StartNew();
            sp.Reset();
            sp.Restart();
            string readValue = string.Empty;
            DR49Ch2ComObj.WriteLine(Environment.NewLine);
            //while (true)
            //{

            //   
            //}
            do
            {
                readValue = DR49Ch2ComObj.ReadExisting();

                // readValue= readValue.Replace('\r', char.MinValue);
                foreach (var sentance in readValue.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(sentance))
                    {
                        Log.Info($"Login 49DR Ch2 :{sentance}");
                        count = 0;
                    }
                    else
                    {
                        TapThread.Sleep(1000);
                        count++;
                        if (count > 35)
                        {
                            DR49Ch2ComObj.WriteLine(Environment.NewLine);
                            Log.Info($"Login 49DR Ch2 : Enter hit");
                            count = 0;
                        }
                    }
                }




                if (readValue.Contains("login:"))
                {
                    DR49Ch2ComObj.WriteLine(username);

                }

                if (readValue.Contains("Password:"))
                {
                    DR49Ch2ComObj.WriteLine(password);

                }
                if (readValue.Contains("~#"))
                {
                    Log.Info("Login 49DR Ch2 : Already Logged in ");
                    return true;
                }

            } while (!readValue.Contains("~#") || TapThread.Current.AbortToken.IsCancellationRequested);




            //do
            //{
            //    readValue = DR49Ch2ComObj.ReadExisting();
            //    if (!string.IsNullOrEmpty(readValue))
            //        Log.Info($"Login 49DR Ch2 : {readValue}");

            //} while (!readValue.Contains("Password:") || TapThread.Current.AbortToken.IsCancellationRequested);


            //DR49Ch2ComObj.WriteLine(password);
            //do
            //{
            //    readValue = DR49Ch2ComObj.ReadExisting();
            //    if (!string.IsNullOrEmpty(readValue))
            //    {
            //        Log.Info($"Login 49DR Ch2 : {readValue}");
            //        count = 0;
            //    }
            //    else
            //    {
            //        TapThread.Sleep(1000);
            //        count++;
            //        if (count > 35)
            //        {
            //            DR49Ch2ComObj.WriteLine(Environment.NewLine);
            //            Log.Info($"Login 49DR Ch2 : Enter hit");
            //            count = 0;
            //        }
            //    }

            //} while (!readValue.Contains("~#") || TapThread.Current.AbortToken.IsCancellationRequested);
            return true;
        }

        public bool dr49ch2oran_modem_initializationcheck(int waittimems, string validataionScript)
        {
            Log.Debug("49DR CH2 Functions");
            int count = 0;
            bool ORANInit = false;
            string readValue = string.Empty;
            do
            {
                TapThread.Sleep(100);
                readValue = DR49Ch2ComObj.ReadExisting();
                if (readValue.Contains(validataionScript))
                {
                    ORANInit = true;
                }
                //byte[] bytes = Encoding.Default.GetBytes(readValue);
                //readValue = Encoding.UTF8.GetString(bytes);
                // readValue= readValue.Replace('\r', char.MinValue);
                foreach (var sentance in readValue.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(sentance))
                    {
                        Log.Info($"49DR  Ch2 ORAN Check : {sentance}");
                        count = 0;
                    }
                    else
                    {
                        TapThread.Sleep(1000);
                        count++;
                        if (count > waittimems)
                        {
                            DR49Ch2ComObj.WriteLine(Environment.NewLine);
                            Log.Info($"ORAN Check 49DR CH2 : Enter hit");
                            count = 0;
                        }
                    }
                }


            } while (!readValue.Contains(validataionScript) || TapThread.Current.AbortToken.IsCancellationRequested);
            DR49Ch2ComObj.WriteLine(Environment.NewLine);
            return ORANInit;

        }

        public bool Dr49_CH2_DPD_Measurement(int channelNumber, out double Txvalue, out double RxValue)
        {
            Log.Debug("49DR CH2 Functions");
            int count = 0;
            Stopwatch sp = new Stopwatch();
            sp.Reset();
            sp.Restart();
            Txvalue = 0;
            RxValue = 0;
            DR49Ch2ComObj.WriteLine("\x03");
            // DR49Ch1ComObj.WriteLine(Environment.NewLine);

            bool Read_Capture_Power_Meter = false;
            string command = (channelNumber <= 7 ? "dpd_dbg_host_app 0xa0060000" : "dpd_dbg_host_app 0xa00e0000");
            channelNumber = channelNumber % 8;
            DR49Ch2ComObj.WriteLine(command);
            string readValue = string.Empty;
            do
            {
                TapThread.Sleep(200);
                readValue = DR49Ch2ComObj.ReadExisting();
                if (!string.IsNullOrEmpty(readValue.Trim()))
                {
                    foreach (var line in readValue.Split('\n'))
                    {
                        Log.Info("DPD Meas Ch2 :" + line);
                    }
                    sp.Restart();
                }

                if (sp.ElapsedMilliseconds > 10000)
                {
                    sp.Restart();
                    DR49Ch2ComObj.WriteLine("\x03");
                    TapThread.Sleep(500);
                    DR49Ch2ComObj.WriteLine("\x03");
                    TapThread.Sleep(1000);
                    DR49Ch2ComObj.ReadExisting();
                    readValue = string.Empty;
                    DR49Ch2ComObj.WriteLine(command);
                }


                ///DPD HOST Example Application MAIN MENU
                if (readValue.Replace("\n", string.Empty).Contains("Selection:") && (
                    readValue.Replace("\n", string.Empty).Contains("Open DPD Host Interface") ||
                    readValue.Contains("DFE System Reset") ||
                    readValue.Contains("")))
                {
                    if (!Read_Capture_Power_Meter)
                    {
                        DR49Ch2ComObj.WriteLine("1");
                    }
                    else
                    {
                        DR49Ch2ComObj.WriteLine("2");
                    }
                }




                if (readValue.Replace("\n", string.Empty).Contains("DPD User Interface Base Address: [0xa4000000]:") || readValue.Replace("\n", string.Empty).Contains("DPD User Interface Base Address: [0xa4060000]"))
                {
                    DR49Ch2ComObj.WriteLine(Environment.NewLine);
                }

                if ((readValue.Replace("\n", string.Empty).Contains("Application SUB MENU") ||
                    readValue.Contains("Display DPD build settings") ||
                    readValue.Contains("Restore default DPD parameters") ||
                    readValue.Contains("Display DPD parameters") ||
                    readValue.Contains("Detect Alignment Parameters") ||
                    readValue.Contains("Detect Alignment Parameters") ||
                    readValue.Contains("Read Capture Power Meter Measurement") ||
                    readValue.Contains("Set modified DPD parameters") ||
                    readValue.Contains("Test PAPR and Test Power meter") ||
                    readValue.Contains("Test Pre-Distortion") ||
                    readValue.Contains("DCL Options") ||
                    readValue.Contains("DPD Monitors and Diagnostics") ||
                    readValue.Contains("Reset Coefficients") ||
                    readValue.Contains("Enable Output") ||
                    readValue.Contains("Disable Output") ||
                    readValue.Contains("DPD ON") ||
                    readValue.Contains("DPD OFF") ||
                    readValue.Contains("Export Data to text file") ||
                    readValue.Contains("Exit to Application MAIN MENU")) &&
                    readValue.Replace("\n", string.Empty).Contains("Selection:"))
                {
                    if (!Read_Capture_Power_Meter)
                    {
                        string TxRxValues = string.Empty;
                        string txStr = string.Empty;
                        string rxStr = string.Empty;
                        TapThread.Sleep(200);
                        DR49Ch2ComObj.ReadExisting();
                        DR49Ch2ComObj.WriteLine("6");
                        do
                        {
                            TxRxValues += DR49Ch2ComObj.ReadExisting().Replace("\n", string.Empty);
                            //TapThread.Sleep(200);
                        } while (!TxRxValues.Replace("\n", string.Empty).Contains("Sub Menu Selection:"));
                        ///////////////////////////////////////////////
                        foreach (var txRxLines in TxRxValues.Split('\r'))
                        {
                            Log.Info("DPD Meaure Ch2 :" + txRxLines);
                        }
                        foreach (var sentance in TxRxValues.Split('\r'))
                        {
                            if (sentance.Contains("tx_pwr_dbfs:") && sentance.Contains("portnum " + channelNumber.ToString()))
                            {
                                txStr = sentance.Substring(sentance.IndexOf("tx_pwr_dbfs:") + "tx_pwr_dbfs:".Length).Trim().Split(' ')[0];
                                Console.WriteLine($"{sentance}");
                            }
                            if (sentance.Contains("rx_pwr_dbfs:") && sentance.Contains("portnum " + channelNumber.ToString()))
                            {
                                rxStr = sentance.Substring(sentance.IndexOf("rx_pwr_dbfs:") + "rx_pwr_dbfs:".Length).Trim().Split(' ')[0];
                                Console.WriteLine($"{sentance}");
                            }
                        }

                        //Console.WriteLine(txStr);
                        //Console.WriteLine(rxStr);
                        //if (string.IsNullOrEmpty(txStr) || string.IsNullOrEmpty(rxStr))
                        //{
                        //    throw new Exception("Tx or Rx not found");
                        //}
                        if (!string.IsNullOrEmpty(txStr))
                        {
                            Txvalue = double.Parse(txStr);
                            RxValue = double.Parse(rxStr);
                        }
                        ///////////////////////////////////////////////
                        //  extractTxRx(channelNumber, out txvalue, out RxValue, TxRxValues);
                        Read_Capture_Power_Meter = true;
                        DR49Ch2ComObj.WriteLine("99");
                    }
                }

                if (readValue.Contains("Exiting program"))
                {
                    DR49Ch2ComObj.WriteLine(Environment.NewLine);
                    break;
                }

                if (readValue.Replace("\n", string.Empty).Contains("Failed to open DPD Host Interface"))
                {
                    DR49Ch2ComObj.WriteLine("\x03");
                    TapThread.Sleep(500);
                    DR49Ch2ComObj.WriteLine("\x03");
                    TapThread.Sleep(1000);
                    DR49Ch2ComObj.WriteLine(command);
                    Read_Capture_Power_Meter = false;
                    Log.Info("DPD Meas Repeting for Failed to open DPD Host Interface");
                    //throw new Exception("DPD Host interface open failed");
                }
                if (readValue.Replace("\n", string.Empty).Contains("Error! Wrong selection"))
                {
                    DR49Ch2ComObj.WriteLine("\x03");
                    TapThread.Sleep(500);
                    DR49Ch2ComObj.WriteLine("\x03");
                    TapThread.Sleep(1000);
                    DR49Ch2ComObj.ReadExisting();
                    readValue = string.Empty;
                    Log.Info("DPD Meas Repeting for Error! Wrong selection");
                    DR49Ch2ComObj.WriteLine(command);

                }

            } while (true);
            //startReceiveEvent();
            return true;
        }

        private void extractTxRx(int portNumber, out double tx, out double rx, string inputValue)
        {
            Log.Debug("49DR CH2 Functions");
            string txStr = string.Empty;
            string rxStr = string.Empty;
            //string inputValue = "--- GET_POWER_RESULTS: cmd_status = 2 for portnum 0\r\n---    tx_pwr_dbfs:        -11.036421 dBFS for portnum 0\r\n---    rx_pwr_dbfs:        -50.220133 dBFS for portnum 0\r\n---    measurement length: 8000 samples for portnum 0\r\n---    measurement port:   0 for portnum 0\r\n\r\n--- GET_POWER_RESULTS: cmd_status = 2 for portnum 1\r\n---    tx_pwr_dbfs:        -11.036421 dBFS for portnum 1\r\n---    rx_pwr_dbfs:        -43.847057 dBFS for portnum 1\r\n---    measurement length: 8000 samples for portnum 1\r\n---    measurement port:   1 for portnum 1\r\n\r\n--- GET_POWER_RESULTS: cmd_status = 2 for portnum 2\r\n---    tx_pwr_dbfs:        -11.036421 dBFS for portnum 2\r\n---    rx_pwr_dbfs:        -45.071089 dBFS for portnum 2\r\n---    measurement length: 8000 samples for portnum 2\r\n---    measurement port:   2 for portnum 2\r\n\r\n--- GET_POWER_RESULTS: cmd_status = 2 for portnum 3\r\n---    tx_pwr_dbfs:        -11.036421 dBFS for portnum 3\r\n---    rx_pwr_dbfs:        -40.598550 dBFS for portnum 3\r\n---    measurement length: 8000 samples for portnum 3\r\n---    measurement port:   3 for portnum 3\r\n\r\n--- GET_POWER_RESULTS: cmd_status = 2 for portnum 4\r\n---    tx_pwr_dbfs:        -11.036421 dBFS for portnum 4\r\n---    rx_pwr_dbfs:        -43.489300 dBFS for portnum 4\r\n---    measurement length: 8000 samples for portnum 4\r\n---    measurement port:   4 for portnum 4\r\n\r\n--- GET_POWER_RESULTS: cmd_status = 2 for portnum 5\r\n---    tx_pwr_dbfs:        -11.036421 dBFS for portnum 5\r\n---    rx_pwr_dbfs:        -39.648527 dBFS for portnum 5\r\n---    measurement length: 8000 samples for portnum 5\r\n---    measurement port:   5 for portnum 5\r\n\r\n--- GET_POWER_RESULTS: cmd_status = 2 for portnum 6\r\n---    tx_pwr_dbfs:        -11.036421 dBFS for portnum 6\r\n---    rx_pwr_dbfs:        -40.942294 dBFS for portnum 6\r\n---    measurement length: 8000 samples for portnum 6\r\n---    measurement port:   6 for portnum 6\r\n\r\n--- GET_POWER_RESULTS: cmd_status = 2 for portnum 7\r\n---    tx_pwr_dbfs:        -11.152796 dBFS for portnum 7\r\n---    rx_pwr_dbfs:        -42.652290 dBFS for portnum 7\r\n---    measurement length: 8000 samples for portnum 7\r\n---    measurement port:   7 for portnum 7";
            foreach (var sentance in inputValue.Split('\n'))

            {
                if (sentance.Contains("tx_pwr_dbfs:") && sentance.Contains("portnum " + portNumber.ToString()))
                {
                    txStr = sentance.Substring(sentance.IndexOf("tx_pwr_dbfs:") + "tx_pwr_dbfs:".Length).Trim().Split(' ')[0];

                    Console.WriteLine($"{sentance}");
                }
                if (sentance.Contains("rx_pwr_dbfs:") && sentance.Contains("portnum " + portNumber.ToString()))
                {
                    rxStr = sentance.Substring(sentance.IndexOf("rx_pwr_dbfs:") + "rx_pwr_dbfs:".Length).Trim().Split(' ')[0];
                    Console.WriteLine($"{sentance}");
                }


            }
            Console.WriteLine(txStr);
            Console.WriteLine(rxStr);
            if (string.IsNullOrEmpty(txStr) || string.IsNullOrEmpty(rxStr))
            {
                throw new Exception("Tx or Rx not found");
            }
            tx = double.Parse(txStr);
            rx = double.Parse(rxStr);
        }

        public string Dr49_Ch2_checkYoctoVersion(string commandScript)
        {
            string yoctoVersion = string.Empty;
            DR49Ch2ComObj.ReadExisting();
            DR49Ch2ComObj.WriteLine(commandScript);
            TapThread.Sleep(100);
            var returnValue = DR49Ch2ComObj.ReadExisting();
            if (!string.IsNullOrEmpty(returnValue))
            {
                foreach (var item in returnValue.Split('\n'))
                {
                    Log.Info("DR49 Ch2 Yokto version :" + item);

                }
            }
            yoctoVersion = returnValue.Split('\n')[1].Split(':')[1].Trim();
            return yoctoVersion;
        }

        public string Dr49_Ch2_checkSlot(string commandScript)
        {
            string slot = string.Empty;
            DR49Ch2ComObj.ReadExisting();
            DR49Ch2ComObj.WriteLine(commandScript);
            TapThread.Sleep(100);
            var returnValue = DR49Ch2ComObj.ReadExisting();
            if (!string.IsNullOrEmpty(returnValue))
            {
                foreach (var item in returnValue.Split('\n'))
                {
                    Log.Info("DR49 Ch2 slot:" + item);

                }
            }
            slot = returnValue.Split('\n')[1].Split('=')[1].Trim();
            return slot;
        }



        #endregion DR49Ch2Functions

        #region DR21Functions

        public bool Dr21PingTest(int NoofPingsRequested, string ipaddress, string validateScript)
        {
            Stopwatch sw = new Stopwatch();
            int attemptNumber = 1;
            sw.Restart();
            string returnValue;
            DR21ComObj.WriteLine("ping -c " + NoofPingsRequested + " " + ipaddress);

            do
            {
                //returnValue = DR21ComObj.ReadExisting();
                //if (!string.IsNullOrEmpty(returnValue))
                //{
                //    Log.Info(returnValue);
                //}
                //if (returnValue.Contains(validateScript))
                //{
                //    return true;
                //}
                returnValue = DR21ComObj.ReadExisting().Replace('\r', char.MinValue);
                if (!string.IsNullOrEmpty(returnValue))
                {
                    Log.Info(returnValue);
                }
                if (sw.ElapsedMilliseconds > 10000 && attemptNumber < 2)
                {
                    attemptNumber++;
                    DR21ComObj.WriteLine("ping -c " + NoofPingsRequested + " " + ipaddress);
                    sw.Restart();
                }
                else if (sw.ElapsedMilliseconds > 10000 && attemptNumber >= 2)
                {
                    return false;
                }
                foreach (var sentance in returnValue.Split('\n'))
                {
                    if (returnValue.Contains(validateScript))
                    {
                        return true;
                    }
                }

            } while (!returnValue.Contains("~#"));



            return false;
        }
        
        
        public void Dr21known_hostsCommand()
        {
            
            DR21ComObj.WriteLine("rm -rf /root/.ssh/known_hosts");             
        }

        public bool login21dr(string username, String password)
        {
            int count = 0;
            Stopwatch sp = Stopwatch.StartNew();
            sp.Reset();
            sp.Restart();
            string readValue = string.Empty;
            DR21ComObj.WriteLine(Environment.NewLine);
            //while (true)
            //{

            //   
            //}
            do
            {
                readValue = DR21ComObj.ReadExisting();

                // readValue= readValue.Replace('\r', char.MinValue);
                foreach (var sentance in readValue.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(sentance))
                    {
                        Log.Info($"Login 21DR : {sentance}");
                        count = 0;
                    }
                    else
                    {
                        TapThread.Sleep(1000);
                        count++;
                        if (count > 10)
                        {
                            DR21ComObj.WriteLine(Environment.NewLine);
                            Log.Info($"Login 21DR  : Enter hit");
                            count = 0;
                        }
                    }
                }
                if (readValue.Contains("login:"))
                {
                    DR21ComObj.WriteLine(username);
                }
                if (readValue.Contains("Password:"))
                {
                    DR21ComObj.WriteLine(password);
                }

                if (readValue.Contains("~#") )
                {
                    Log.Info("Login 21DR  : Already Logged in ");
                    return true;
                }

            } while (!readValue.Contains("~#") || TapThread.Current.AbortToken.IsCancellationRequested);



            //DR21ComObj.WriteLine(username);
            //do
            //{
            //    readValue = DR21ComObj.ReadExisting();
            //    if (!string.IsNullOrEmpty(readValue))
            //        Log.Info($"Login 21DR  :{readValue}");
            //    else
            //    {
            //        TapThread.Sleep(1000);
            //        count++;
            //        if (count > 35)
            //        {
            //            DR21ComObj.WriteLine(Environment.NewLine);
            //            Log.Info($"Login 21DR  : Enter hit");
            //            count = 0;
            //        }
            //    }

            //} while (!readValue.Contains("Password:") || TapThread.Current.AbortToken.IsCancellationRequested);


            //DR21ComObj.WriteLine(password);
            //do
            //{
            //    readValue = DR21ComObj.ReadExisting();
            //    if (!string.IsNullOrEmpty(readValue))
            //    {
            //        Log.Info($"Login 21DR  :{readValue}");
            //        count = 0;
            //    }
            //    else
            //    {
            //        TapThread.Sleep(1000);
            //        count++;
            //        if (count > 35)
            //        {
            //            DR21ComObj.WriteLine(Environment.NewLine);
            //            Log.Info($"Login 21DR  : Enter hit");
            //            count = 0;
            //        }
            //    }

            //} while (!readValue.Contains("~#") || TapThread.Current.AbortToken.IsCancellationRequested);
            return true;
        }

        public bool Dr21MRUIpChange(string ethInterface, string IPAddress)
        {
            DR21ComObj.WriteLine("ifconfig " + ethInterface + " " + IPAddress + " up");
            return true;
        }


        public bool dr21oran_modem_initializationcheck(int waittimems, string validationScript)
        {
            int count = 0;
            bool ORANInit = false;
            string readValue = string.Empty;
            do
            {
                TapThread.Sleep(100);
                readValue = DR21ComObj.ReadExisting();
                if (readValue.Contains(validationScript))
                {
                    ORANInit = true;
                }
                //byte[] bytes = Encoding.Default.GetBytes(readValue);
                //readValue = Encoding.UTF8.GetString(bytes);
                // readValue= readValue.Replace('\r', char.MinValue);
                foreach (var sentance in readValue.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(sentance))
                    {
                        Log.Info($"21DR Oran Check : {sentance}");
                        count = 0;
                    }
                    else
                    {
                        TapThread.Sleep(1000);
                        count++;
                        if (count > waittimems)
                        {
                            DR21ComObj.WriteLine(Environment.NewLine);
                            Log.Info($"ORAN Check 21Dr : Enter hit");
                            count = 0;
                        }
                    }
                }


            } while (!readValue.Contains(validationScript) || TapThread.Current.AbortToken.IsCancellationRequested);
            DR21ComObj.WriteLine(Environment.NewLine);
            return ORANInit;
        }



        public bool dr21PTPInitEstablishedncheck(int waittimems, string validationScript)
        {
            Stopwatch sw = Stopwatch.StartNew();
            bool ORANInit = false;
            string readValue = string.Empty;
            DR21ComObj.WriteLine(Environment.NewLine);
            do
            {
                TapThread.Sleep(100);
                readValue = DR21ComObj.ReadExisting();
                if (!string.IsNullOrEmpty( readValue.Trim()))
                {
                    Log.Info(readValue);
                }
               
                if (readValue.Contains(validationScript)||readValue.Contains("detected established!"))
                {
                    ORANInit = true;
                    break;
                }
                //byte[] bytes = Encoding.Default.GetBytes(readValue);
                //readValue = Encoding.UTF8.GetString(bytes);
                // readValue= readValue.Replace('\r', char.MinValue);
                if ((sw.ElapsedMilliseconds / 1000) > waittimems)
                {
                    Log.Error("Timeout");
                    ORANInit = false;
                    break;
                }


            } while (!readValue.Contains(validationScript) || TapThread.Current.AbortToken.IsCancellationRequested);
            DR21ComObj.WriteLine(Environment.NewLine);
            return ORANInit;
        }

        public bool dr21ORANStatusCheck()
        {
            string totalRx = string.Empty;
            string readValue = string.Empty;
            string ontimePackets = string.Empty;
            DR21ComObj.WriteLine("xorif-app -c 'clear fhi_stats'");

            Log.Info("21DR ORAN Status :" + DR21ComObj.ReadExisting());
            DR21ComObj.WriteLine("xorif-app -c 'get fhi_stats 0'");
            // Log.Info("21DR ORAN Status :" + DR21ComObj.ReadExisting());
            TapThread.Sleep(100);
            readValue = DR21ComObj.ReadExisting();
            if (!string.IsNullOrEmpty(readValue))
            {
                foreach (var readLines in readValue.Split('\n'))
                {
                    Log.Info(readLines);
                    if (readLines.Contains("oran_rx_total_c"))
                    {
                        totalRx = readLines.Split('=')[1];
                    }
                    if (readLines.Contains("oran_rx_on_time_c"))
                    {
                        ontimePackets = readLines.Split('=')[1];

                    }
                }
            }
            if (!string.IsNullOrEmpty(totalRx) && !string.IsNullOrEmpty(ontimePackets))
            {
                if (Convert.ToDouble(totalRx) == Convert.ToDouble(ontimePackets))
                {
                    return true;
                }
            }


            return false;
        }


        public bool Dr21PTPSyncStatusCheck()
        {
            string totalRx = string.Empty;
            string readValue = string.Empty;
            string ontimePackets = string.Empty;
            DR21ComObj.WriteLine("loglvl-cli -i");

            Log.Info("21DR PTPSync Status :" + DR21ComObj.ReadExisting());
            DR21ComObj.WriteLine("tail -f /var/log/messages | grep ptp");
            // Log.Info("21DR ORAN Status :" + DR21ComObj.ReadExisting());
            TapThread.Sleep(500);
            readValue = DR21ComObj.ReadExisting();
            //            Oct  9 02:06:09 ptp4l: [861.543] master offset          2 s2 freq    +147 path delay      1034
            double ptpTime = -999;
            foreach (var item in readValue.Split('\n'))
            {
                if (item.Contains("master offset"))
                {
                    //Error due to handling large value , which could not be fit in int16/ converting to int32
                    ptpTime = Convert.ToInt32(item.Substring(item.IndexOf("master offset") + 13, "          2".Length).Trim());
                }
                if (Math.Abs(ptpTime) < 100)
                {
                    break;
                }
            }
            Dr21_ControlC();

            if (Math.Abs(ptpTime) < 100)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool Dr21GetEepromInfo(out string MAC1, out string MAC2, out string MAC3, out string MAC4, out string ProductSerialNumber, out string PCBSerialNumber, out string ProdID)
        {
            MAC1 = string.Empty; MAC2 = string.Empty; MAC3 = string.Empty; MAC4 = string.Empty; ProductSerialNumber = string.Empty; PCBSerialNumber = string.Empty; ProdID = string.Empty;
            DR21ComObj.WriteLine("hstb-m-eeprom -read_info");
            TapThread.Sleep(100);
            var returnValue = DR21ComObj.ReadExisting();
            Log.Info("MAC and Other Details : " + returnValue);
            if (returnValue.Contains("MAC1 :"))
            {
                MAC1 = returnValue.Substring(returnValue.IndexOf("MAC1 :") + "MAC1 :".Length + 1, returnValue.IndexOf("mac2 crc") - (returnValue.IndexOf("MAC1 :") + "MAC1 :".Length + 1));
            }
            if (returnValue.Contains("MAC2 :"))
            {
                MAC2 = returnValue.Substring(returnValue.IndexOf("MAC2 :") + "MAC2 :".Length + 1, returnValue.IndexOf("mac3 crc") - (returnValue.IndexOf("MAC2 :") + "MAC2 :".Length + 1));
            }

            if (returnValue.Contains("MAC3 :"))
            {
                MAC3 = returnValue.Substring(returnValue.IndexOf("MAC3 :") + "MAC3 :".Length + 1, returnValue.IndexOf("mac4 crc") - (returnValue.IndexOf("MAC3 :") + "MAC3 :".Length + 1));
            }
            if (returnValue.Contains("MAC4 :"))
            {
                MAC4 = returnValue.Substring(returnValue.IndexOf("MAC4 :") + "MAC4 :".Length + 1, returnValue.IndexOf("prod serial") - (returnValue.IndexOf("MAC4 :") + "MAC4 :".Length + 1));
            }
            if (returnValue.Contains("PROD_SERIAL_NUMBER :"))
            {
                ProductSerialNumber = returnValue.Substring(returnValue.IndexOf("PROD_SERIAL_NUMBER :") + "PROD_SERIAL_NUMBER :".Length + 1, returnValue.IndexOf("pcb serial no") - (returnValue.IndexOf("PROD_SERIAL_NUMBER :") + "PROD_SERIAL_NUMBER :".Length + 1));
            }
            if (returnValue.Contains("PCB_SERIAL_NUMBER :"))
            {
                PCBSerialNumber = returnValue.Substring(returnValue.IndexOf("PCB_SERIAL_NUMBER :") + "PCB_SERIAL_NUMBER :".Length + 1, returnValue.IndexOf("prod id") - (returnValue.IndexOf("PCB_SERIAL_NUMBER :") + "PCB_SERIAL_NUMBER :".Length + 1));
            }
            if (returnValue.Contains("PROD_ID :"))
            {
                foreach (var item in returnValue.Split('\n'))
                {
                    if (item.Contains("PROD_ID :"))
                    {
                        ProdID = item.Split(':')[1];
                        break;
                    }
                }
                //ProdID = returnValue.Substring(returnValue.IndexOf("PROD_ID :") + "PROD_ID :".Length + 1, returnValue.IndexOf("app ver") - (returnValue.IndexOf("PROD_ID :") + "PROD_ID :".Length + 1));
            }
            return true;
        }
        
        public bool Dr21MAC_SLNO_PRODID_Provisioning(string macID, string serialNumber, string prodID   )
        {
            DR21ComObj.WriteLine("hstb-m-eeprom -upd_mac2 " + macID + " -upd_product_serial_no " + serialNumber + " -upd_product_id " + prodID + "");
            Log.Info("Completed MAC write on MRU.");
            return true;
        }
        internal void PTPSyncCheck()
        {
            string readValue = string.Empty;
            do
            {
                //TapThread.Sleep(100);
                //readValue = DR21ComObj.ReadExisting();
                //if (readValue.Contains(validationScript))
                //{
                //    ORANInit = true;
                //}
                ////byte[] bytes = Encoding.Default.GetBytes(readValue);
                ////readValue = Encoding.UTF8.GetString(bytes);
                //// readValue= readValue.Replace('\r', char.MinValue);
                //foreach (var sentance in readValue.Split('\n'))
                //{
                //    if (!string.IsNullOrEmpty(sentance))
                //    {
                //        Log.Info($"21DR Oran Check : {sentance}");
                //        count = 0;
                //    }
                //    else
                //    {
                //        TapThread.Sleep(1000);
                //        count++;
                //        if (count > waittimems)
                //        {
                //            DR21ComObj.WriteLine(Environment.NewLine);
                //            Log.Info($"ORAN Check 21Dr : Enter hit");
                //            count = 0;
                //        }
                //    }
                //}
                TapThread.Sleep(1000);

            } while (true);
        }


        public string Dr21_checkYoctoVersion(string commandScript)
        {
            string yoctoVersion = string.Empty;
            DR21ComObj.ReadExisting();
            DR21ComObj.WriteLine(commandScript);
            TapThread.Sleep(500);
            var returnValue = DR21ComObj.ReadExisting();

            if (!string.IsNullOrEmpty(returnValue))
            {
                foreach (var item in returnValue.Split('\n'))
                {
                    Log.Info("DR21 Yokto version :" + item);

                }

            }
            yoctoVersion = returnValue.Split('\n')[1].Split(':')[1].Trim();
            return yoctoVersion;
        }
        public string Dr21_checkSlot(string commandScript)
        {
            string Slot = string.Empty;
            DR21ComObj.ReadExisting();
            DR21ComObj.WriteLine(commandScript);
            TapThread.Sleep(500);
            var returnValue = DR21ComObj.ReadExisting();
            if (!string.IsNullOrEmpty(returnValue))
            {
                foreach (var item in returnValue.Split('\n'))
                {
                    Log.Info("DR21 slot :" + item);

                }

            }
            Slot = returnValue.Split('\n')[1].Split('=')[1].Trim();
            return Slot;
        }


        public string Dr21RunExtractCommandScripts(string commandScript)
        {
            DR21ComObj.WriteLine(commandScript);
            TapThread.Sleep(1000);
            return DR21ComObj.ReadExisting();
        }


        public void Dr21RebootMRUAll()
        {
            DR21ComObj.WriteLine("reboot-mru");
            string tempStr = string.Empty;  
            Stopwatch   stopwatch = Stopwatch.StartNew();

            do
            {
                tempStr = DR21ComObj.ReadExisting();
                TapThread.Sleep(200);
                if (tempStr.Contains("logout"))
                {
                    break;
                }
                if ((stopwatch.ElapsedMilliseconds/1000)>50)
                {
                    throw new Exception("Timeout");
                    
                }

            } while (true);
        }

        internal void Dr49_CH1_WriteDSAToEEPROM(int[] hexValues)
        {
            DR49Ch1ComObj.ReadExisting();
            string command4EEPROM_DSA = "rj-rfeeprom-updater -upd_dsa_tx " + hexValues[0] + "," + hexValues[1] + "," + hexValues[2] + "," + hexValues[3] + "," + hexValues[4] + "," + hexValues[5] + "," + hexValues[6] + "," + hexValues[7] + "," + hexValues[8] + "," + hexValues[9] + "," + hexValues[10] + "," + hexValues[11] + "," + hexValues[12] + "," + hexValues[13] + "," + hexValues[14] + "," + hexValues[15] + " -upd_dsa_fb 0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f -upd_dac0 0x47E - nupd_dac1 0x64B -upd_fw_ver 1.0 -upd_hw_ver B -upd_prv_valid";
            Log.Info(command4EEPROM_DSA);
            DR49Ch1ComObj.WriteLine(command4EEPROM_DSA);

            
        }
        internal void Dr49_CH2_WriteDSAToEEPROM(int[] hexValues)
        {
            DR49Ch1ComObj.ReadExisting();
            string command4EEPROM_DSA = "rj-rfeeprom-updater -upd_dsa_tx " + hexValues[0] + "," + hexValues[1] + "," + hexValues[2] + "," + hexValues[3] + "," + hexValues[4] + "," + hexValues[5] + "," + hexValues[6] + "," + hexValues[7] + "," + hexValues[8] + "," + hexValues[9] + "," + hexValues[10] + "," + hexValues[11] + "," + hexValues[12] + "," + hexValues[13] + "," + hexValues[14] + "," + hexValues[15] + " -upd_dsa_fb 0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f,0x0f -upd_dac0 0x47E - nupd_dac1 0x64B -upd_fw_ver 1.0 -upd_hw_ver B -upd_prv_valid";
            Log.Info(command4EEPROM_DSA);
            DR49Ch1ComObj.WriteLine(command4EEPROM_DSA);

            
        }
        #endregion DR21Functions




    }
}