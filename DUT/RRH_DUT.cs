// Author: MyName
// Copyright:   Copyright 2022 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files..

using Ivi.Visa;
using Keysight.Visa;
using OpenTap;
using Renci.SshNet;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace RjioODSC
{
    [Display("RRH_DUT", Group: "RjioODSC", Description: "Add a description here")]
    public class RRH_DUT : Dut
    {
        #region Settings
        public bool StartMeasurements = false;
        static bool _continue;

        static string message = string.Empty;
        public SerialSession session2L2;
        public SerialSession session2L1;
        IMessageBasedFormattedIO ioL2;
        IMessageBasedFormattedIO ioL1;
        string l1fileName = "C:/L1Log.txt";
        string l2fileName = "C:/L2Log.txt";
         
        public Thread L1Thread;

        string comportNumberL2 = "COM0";
        string comportNumberL1 = "COM2";
        ComType dUTCommunication = ComType.SSH;


        [Display("DUT Com Type", Group: "Communication Seletion", Description: "Select DUT Communication Type", Order: 0)]
        public ComType DUTCommunication { get => dUTCommunication; set => dUTCommunication = value; }

        [EnabledIf("DUTCommunication", ComType.RS232, HideIfDisabled = true)]
        [Display("DUT Comport L2", Description: "Enter DUT Comport Number for L2", Group: "RS232", Order: 2)]
        public string ComportNumberL2 { get => comportNumberL2; set => comportNumberL2 = value; }

        [EnabledIf("DUTCommunication", ComType.RS232, HideIfDisabled = true)]
        [Display("DUT Comport L1", Description: "Enter DUT Comport Number for L1", Group: "RS232", Order: 5)]
        public string ComportNumberL1 { get => comportNumberL1; set => comportNumberL1 = value; }

        
        #region SSH
        private static SshClient _sshConnection;
        private static ShellStream _shellStream;
        private delegate void UpdateTextCallback(string message);
        static bool WriteCD = false;
        static bool WriteCOmmand = false;


        string rRhAddress = "172.16.81.2";
        string rRHUsername = "root";
        string rRHPassword = "swtn100tj";
        [EnabledIf("DUTCommunication", ComType.SSH, HideIfDisabled = true)]
        [Display("RRH IP", "Enter RRH IP Address", Group: "SSH Setting", Order: 3)]

        public string RRHIPAddress { get => rRhAddress; set => rRhAddress = value; }
        [EnabledIf("DUTCommunication", ComType.SSH, HideIfDisabled = true)]
        [Display("RRH User Name", "Enter RRH User Name", Group: "SSH Setting", Order: 5)]

        public string RRHUserName { get => rRHUsername; set => rRHUsername = value; }

        [EnabledIf("DUTCommunication", ComType.SSH, HideIfDisabled = true)]
        [Display("RRH Password", "Enter RRH Password", Group: "SSH Setting", Order: 10)]

        public string RRHPassword { get => rRHPassword; set => rRHPassword = value; }
        #endregion SSH

        // ToDo: Add property here for each parameter the end user should be able to change.
        #endregion


        /// <summary>
        /// Initializes a new instance of this DUT class.
        /// </summary>
        public RRH_DUT()
        {
            File.Delete(l1fileName);
            File.Delete(l2fileName);
            Name = "RRH";
            _sshConnection = new SshClient(RRHIPAddress, RRHUserName, RRHPassword);
            // ToDo: Set default values for properties / settings.
        }

        /// <summary>
        /// Opens a connection to the DUT represented by this class
        /// </summary>
        public override void Open()
        {
            //base.Open();
            if (DUTCommunication == ComType.SSH)
            {
                try
                {
                    if (!_sshConnection.IsConnected)
                    {
                        _sshConnection.Connect();
                        _shellStream = _sshConnection.CreateShellStream("test", 80, 60, 800, 600, 65536);
                    }
               
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                session2L2 = new SerialSession(ComportNumberL2, AccessModes.None, 300000)
                {
                    BaudRate = 115200,
                    DataBits = 8,
                    FlowControl = SerialFlowControlModes.XOnXOff,
                    Parity = SerialParity.None,
                    ReadTermination = SerialTerminationMethod.TerminationCharacter,
                    TerminationCharacterEnabled = true,
                    TimeoutMilliseconds = 300000,
                    WriteTermination = SerialTerminationMethod.TerminationCharacter,
                    TerminationCharacter = Convert.ToByte('\n')

                };
               ioL2= session2L2.FormattedIO;

                StartMeasurements = false;
                //session2L1 = new SerialSession(ComportNumberL1, AccessModes.None, 300000)
                //{
                //    BaudRate = 115200,
                //    DataBits = 8,
                //    FlowControl = SerialFlowControlModes.XOnXOff,
                //    Parity = SerialParity.None,
                //    ReadTermination = SerialTerminationMethod.TerminationCharacter,
                //    TerminationCharacterEnabled = true,
                //    TimeoutMilliseconds = 300000,
                //    WriteTermination = SerialTerminationMethod.TerminationCharacter,
                //    TerminationCharacter = 10

                //};
                //ioL1 = session2L1.FormattedIO;
                //string L1Results = string.Empty;
                //L1Thread = new Thread(() => { L1Results = L1ThreadFunction(ioL1); });
                ////  Thread thread = new Thread(threadStart);
                ////threadStart.Start();
                //L1Thread.IsBackground = true;
                //L1Thread.Start();

            }


            // TODO: establish connection to DUT here
        }


        public string L1ThreadFunction(IMessageBasedFormattedIO l1ThreadIO)
        {
            string l1ReadValue = string.Empty;
            while (true)
            {
                Thread.Sleep(10);
                
                Log.Info(l1ReadValue = l1ThreadIO.ReadLine());
                File.AppendAllText(l1fileName,l1ReadValue);
                if (l1ReadValue.Contains("++++++++++++++++ 1+++++++++++++++++"))
                {
                    StartMeasurements = true;
                }
            }
            return "";
        }
        
        /// <summary>
        /// Closes the connection made to the DUT represented by this class
        /// </summary>
        public override void Close()
        {

            if (DUTCommunication == ComType.SSH)
            {
                try
                {
                    _sshConnection.Disconnect();
                    _shellStream.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                session2L2.Clear();
                session2L2.Dispose();
            }


            // TODO: close connection to DUT
            base.Close();
        }

        public void SetFrequency(double frequency, int timeout)
        {
            WriteCD = false;
            string freqCommand = "Frequency 1 " + frequency.ToString();

            if (DUTCommunication == ComType.SSH)
            {
                string returnValue = string.Empty;
                WritetoSSH(freqCommand);
                Stopwatch sp = new Stopwatch();
                sp.Reset();
                sp.Start();
                do
                {
                    returnValue = ReadFromSSH();
                    TapThread.Sleep(1000);
                    if (sp.ElapsedMilliseconds > timeout * 1000)
                    {
                        if (System.Windows.Forms.MessageBox.Show("Waited for 1 min, Continue for 1 more second?", "Timeout", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {
                            sp.Reset();
                            sp.Start();
                        }
                        else
                        {
                            returnValue = "INVALID";
                            break;
                        }
                    }
                } while (!returnValue.ToUpper().Contains("FREQUENCY " + frequency + " OK"));
            }
            else
            {

                string returnValue = string.Empty;
                ioL2.DiscardBuffers();

                ioL2.WriteLine(freqCommand);
                Stopwatch sp = new Stopwatch();
                sp.Reset();
                sp.Start();
                do
                {
                    returnValue = ioL2.ReadLine();
                    Log.Info(returnValue);
                    TapThread.Sleep(500);
                    if (returnValue.Contains("Frequency 2120000 OK"))
                    {
                        ioL2.WriteLine(freqCommand);
                    }
                    if (sp.ElapsedMilliseconds > timeout * 1000)
                    {
                        if (System.Windows.Forms.MessageBox.Show("Waited for 1 min, Continue for 1 more second?", "Timeout", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {
                            sp.Reset();
                            sp.Start();
                        }
                        else
                        {
                            returnValue = "INVALID";
                            break;
                        }
                    }
                } while (!returnValue.ToUpper().Contains("FREQUENCY " + frequency + " OK"));
            }
        }

        public void SetPower(int port, int path, double power, int timeout)
        {
            string TxPowerCommand = "Txpower " + port + " " + path + " " + power;

            if (DUTCommunication == ComType.SSH)
            {
                string returnValue = string.Empty;
                WritetoSSH(TxPowerCommand);
                Stopwatch sp = new Stopwatch();
                sp.Reset();
                sp.Start();
                do
                {
                    returnValue = ReadFromSSH();
                    TapThread.Sleep(1000);
                    if (sp.ElapsedMilliseconds > timeout * 1000)
                    {
                        if (System.Windows.Forms.MessageBox.Show("Waited for 1 min, Continue for 1 more second?", "Timeout", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {
                            sp.Reset();
                            sp.Start();
                        }
                        else
                        {
                            returnValue = "INVALID";
                            break;
                        }
                    }
                } while (!returnValue.Contains("Txpower " + path + " " + power + " OK"));
            }
            else
            {
                string returnValue = string.Empty;
                ioL2.DiscardBuffers();
                ioL2.WriteLine(TxPowerCommand);
                Stopwatch sp = new Stopwatch();
                sp.Reset();
                sp.Start();
                do
                {
                    returnValue = ioL2.ReadLine();
                    Log.Info(returnValue);
                    TapThread.Sleep(500);
                    if (sp.ElapsedMilliseconds > timeout * 1000)
                    {
                        if (System.Windows.Forms.MessageBox.Show("Waited for 1 min, Continue for 1 more second?", "Timeout", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {
                            sp.Reset();
                            sp.Start();
                        }
                        else
                        {
                            returnValue = "INVALID";
                            break;
                        }
                    }
                } while (!returnValue.Contains("Txpower " + path + " " + power + " OK"));
            }

        }

        public void SetLNAOnOff(bool On)
        {
            string TxLNACommand = On ? "devmem 0xff230174 32 0xff" : "devmem 0xff230174 32 0x00";
            if (DUTCommunication == ComType.SSH)
            {
                WritetoSSH(TxLNACommand);  
            }
            else
            {
                ioL2.WriteLine("exit");
                ioL2.DiscardBuffers();
                ioL2.WriteLine(TxLNACommand);
            }
        }

        public void SetPAMOnOff(bool On)
        {
            string TxLNACommand = On ? "devmem 0xff230054 32 0xff" : "devmem 0xff230054 32 0x00";
            if (DUTCommunication == ComType.SSH)
            {
                WritetoSSH(TxLNACommand);
            }
            else
            {
                ioL2.WriteLine("exit");
                ioL2.DiscardBuffers();
                ioL2.WriteLine(TxLNACommand);
            }
        }

        public void SetAdrvresetDpd(int path)
        {
            string SetAdrvresetDpdCommand = "AdrvresetDpd " + path;
            if (DUTCommunication == ComType.SSH)
            {
                WritetoSSH(SetAdrvresetDpdCommand);
            }
            else
            {
                string returnValue = string.Empty;
                ioL2.DiscardBuffers();
                ioL2.WriteLine(SetAdrvresetDpdCommand);
                TapThread.Sleep(500);
            }

        }

        public void SetTxStart(int port, int path, int timeout)
        {
            string TxPowerCommand = "Txstart " + port + " " + path;
            if (DUTCommunication == ComType.SSH)
            {
                string returnValue = string.Empty;
                WritetoSSH(TxPowerCommand);
                Stopwatch sp = new Stopwatch();
                sp.Reset();
                sp.Start();
                do
                {
                    returnValue = ReadFromSSH();
                    TapThread.Sleep(1000);
                    if (sp.ElapsedMilliseconds > timeout * 1000)
                    {
                        if (System.Windows.Forms.MessageBox.Show("Waited for 1 min, Continue for 1 more second?", "Timeout", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {
                            sp.Reset();
                            sp.Start();
                        }
                        else
                        {
                            returnValue = "INVALID";
                            break;
                        }
                    }
                } while (!(returnValue.Contains("Txstart " + path + " NG") || returnValue.Contains("Status is illegal(Already started)") || returnValue.Contains("Txstart " + path + " OK")));
            }
            else
            {
                string returnValue = string.Empty;
                ioL2.DiscardBuffers();
                ioL2.WriteLine(TxPowerCommand);
                Stopwatch sp = new Stopwatch();
                sp.Reset();
                sp.Start();
                do
                {
                    returnValue = ioL2.ReadLine();
                    Log.Info(returnValue);
                    TapThread.Sleep(500);
                    if (sp.ElapsedMilliseconds > timeout * 1000)
                    {
                        if (System.Windows.Forms.MessageBox.Show("Waited for 1 min, Continue for 1 more second?", "Timeout", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {
                            sp.Reset();
                            sp.Start();
                        }
                        else
                        {
                            returnValue = "INVALID";
                            break;
                        }
                    }
                } while (!(returnValue.Contains("Txstart " + path + " NG") || returnValue.Contains("Status is illegal(Already started)") || returnValue.Contains("Txstart " + path + " OK")));
            }
        }

        public void SetTxAllStop(int port, int timeout)
        {
            string TxPowerCommand = "TxStopAll " + port;
            if (DUTCommunication == ComType.SSH)
            {
                string returnValue = string.Empty;
                WritetoSSH(TxPowerCommand);
                Stopwatch sp = new Stopwatch();
                sp.Reset();
                sp.Start();
                do
                {
                    returnValue = ReadFromSSH();
                    TapThread.Sleep(1000);
                    if (sp.ElapsedMilliseconds > timeout * 1000)
                    {
                        if (System.Windows.Forms.MessageBox.Show("Waited for 1 min, Continue for 1 more second?", "Timeout", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {
                            sp.Reset();
                            sp.Start();
                        }
                        else
                        {
                            returnValue = "INVALID";
                            break;
                        }
                    }
                } while (!returnValue.Contains("TxstopAll OK"));

            }
            else
            {
                string returnValue = string.Empty;
                ioL2.DiscardBuffers();
                ioL2.WriteLine(TxPowerCommand);
                do
                {
                    returnValue = ioL2.ReadLine();
                    Log.Info(returnValue);
                    TapThread.Sleep(500);
                } while (!returnValue.Contains("TxstopAll OK"));
            }

        }

        public void SetTxAllStart(int port, int timeout)
        {
            string TxPowerCommand = "TxStartAll " + port;
            if (DUTCommunication == ComType.SSH)
            {
                string returnValue = string.Empty;
                WritetoSSH(TxPowerCommand);
                Stopwatch sp = new Stopwatch();
                sp.Reset();
                sp.Start();
                do
                {
                    returnValue = ReadFromSSH();
                    TapThread.Sleep(1000);
                    if (sp.ElapsedMilliseconds > timeout * 1000)
                    {
                        if (System.Windows.Forms.MessageBox.Show("Waited for 1 min, Continue for 1 more second?", "Timeout", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {
                            sp.Reset();
                            sp.Start();
                        }
                        else
                        {
                            returnValue = "INVALID";
                            break;
                        }
                    }
                } while (!returnValue.Contains("TxstartAll OK"));
            }
            else
            {


                string returnValue = string.Empty;
                ioL2.DiscardBuffers();
                ioL2.WriteLine(TxPowerCommand);
                do
                {
                    returnValue = ioL2.ReadLine();
                    Log.Info(returnValue);
                    TapThread.Sleep(500);
                } while (!returnValue.Contains("TxstartAll OK"));
            }

        }

        public void executeScript(string sendScript,string compareExecutionEnd)
        {
            string TxPowerCommand = String.Empty;
            if (DUTCommunication == ComType.SSH)
            {
                WritetoSSH(string.Empty);
                TxPowerCommand = "export CLISH_PATH=/usr/sbin/tejas/cfg/cli-xml-files/\r\n";
                WritetoSSH(TxPowerCommand);
                TxPowerCommand = "export LD_LIBRARY_PATH=/usr/sbin/tejas/lib/\r\n";
                WritetoSSH(TxPowerCommand);
                TxPowerCommand = "/usr/sbin/tejas/./cli_zync";
                WritetoSSH(TxPowerCommand);
                WritetoSSH(string.Empty);
                foreach (var item in ReadFromSSH().Split('\n'))
                {
                    Log.Info(item);
                }
            }
            else
            {
                TxPowerCommand = sendScript;
               // TxPowerCommand = "export CLISH_PATH=/usr/sbin/tejas/cfg/cli-xml-files/\r\n";
                ioL2.DiscardBuffers();
                ioL2.WriteLine(TxPowerCommand);
                //TapThread.Sleep(1000);
                //TxPowerCommand = "export LD_LIBRARY_PATH=/usr/sbin/tejas/lib/\r\n";
                //ioL2.DiscardBuffers();
                //ioL2.WriteLine(TxPowerCommand);
                //TapThread.Sleep(1000);
                //TxPowerCommand = "/usr/sbin/tejas/./cli_zync";
                //ioL2.DiscardBuffers();
                //ioL2.WriteLine(TxPowerCommand);
                string readValue = string.Empty;
                if (!string.IsNullOrEmpty(compareExecutionEnd.Trim()))
                {
                    do
                    {
                        TapThread.Sleep(200);
                        Log.Info(readValue = ioL2.ReadLine());
                    } while (!readValue.Contains(compareExecutionEnd));

                }
            }
        }

        public void loginStart()
        {
            if (DUTCommunication == ComType.SSH)
            {

            }
            else
            {
                Thread readThread = new Thread(TryLogin);
                _continue = true;
                readThread.Start();
                readThread.Join();
            }
        }


        public string GetRRHInformations()
        {
            string TxPowerCommand = "cat /etc/tejas/log/.dbg/persistent_info";
            WritetoSSH(TxPowerCommand);
            return ReadFromSSH();
        }
        public void TryLogin()
        {
            ioL2.WriteLine();

            while (_continue)
            {
                Thread.Sleep(100);
                try
                {
                
                    message = ioL2.ReadLine();
                    // Console.WriteLine(message);
                    Log.Info(message);
                    //if (!startLoginPrompt)
                    //{
                     
                    if (message.Contains("odsc3-ibtb-sku2-board login:"))
                    {
                        ioL2.Write("root");
                        //ioL2.WriteLine();
                        // Console.WriteLine("///////////////////Root");
                    }
                    
                    else if (message.Contains("Password:"))
                    {
                        ioL2.Write("root");
                        //ioL2.WriteLine();
                        //ioL2.WriteLine(Environment.NewLine);
                    }
                    else if (message.Contains("root@rel_2_a:/#"))
                    {                        
                        break;
                    }
                    else if (message.Trim() == ">")
                    {
                        _continue = false;
                        break;
                        #region MyRegion

                        //if (prompt > 10)
                        //{


                        //    if (!TxPower1DOne)
                        //    {
                        //        _serialPort.WriteLine("Txpower 1 0 460");
                        //        var Dutresponse = _serialPort.ReadLine();
                        //        TxPower1DOne = true;
                        //    }
                        //    if (TxPower1DOne & !TxPower2DOne)
                        //    {
                        //        _serialPort.WriteLine("Txpower 1 1 460");
                        //        var Dutresponse = _serialPort.ReadLine();
                        //        TxPower2DOne = true;
                        //    }
                        //    if (TxPower1DOne & TxPower2DOne & !TxPower3DOne)
                        //    {
                        //        _serialPort.WriteLine("Txpower 1 2 460");
                        //        var Dutresponse = _serialPort.ReadLine();
                        //        TxPower3DOne = true;
                        //    }
                        //    if (TxPower1DOne & TxPower2DOne & TxPower3DOne & !TxPower4DOne)
                        //    {
                        //        _serialPort.WriteLine("Txpower 1 3 460");
                        //        var Dutresponse = _serialPort.ReadLine();
                        //        TxPower4DOne = true;
                        //    }
                        //    Thread.Sleep(1000);
                        //}
                        //else
                        //{
                        //    prompt++;
                        //    Thread.Sleep(100);
                        //    _serialPort.WriteLine("\r\n");
                        //}
                        #endregion

                    }
                    //File.AppendAllText(l2fileName, message);
                    //else if (message.Contains("Login incorrect") || message.Contains("PetaLinux 2017.4 rel_2_a /dev/ttyPS0"))
                    //{
                    //    _serialPort.WriteLine(Environment.NewLine);
                    //}
                    // }
                }
                catch (TimeoutException) { }
            }
        }

       

        public enum ComType
        {
            RS232,
            SSH
        }


        public string ReceiveSend(string command, string expectedValue, int timeout)
        {
            // string rxsensitivity = string.Empty;
            Stopwatch sp = new Stopwatch();
            sp.Reset();
            sp.Start();
            while (true)
            {
                try
                {

                    _shellStream.Flush();
                    string data = _shellStream.Read();
                    Console.WriteLine(data);

                    if (!WriteCD)
                    {
                        _shellStream.WriteLine(command);
                        Thread.Sleep(1000);
                        WriteCD = true;
                    }

                    var splitData = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    for (int iteration = 0; iteration < splitData.Length; iteration++)
                    {
                        Log.Info(splitData[iteration]);
                        if (splitData[iteration].Contains(expectedValue))

                        {
                            return expectedValue;

                        }
                    }

                    if (sp.ElapsedMilliseconds > timeout * 1000)
                    {
                        return expectedValue = "Timeout";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                Thread.Sleep(200);
            }
        }
        private void WritetoSSH(string command)
        {
            // _shellStream.WriteLine("");        
            _shellStream.WriteLine(command);
            Thread.Sleep(100);

        }
        private string ReadFromSSH()
        {
            string data = string.Empty;
            // _shellStream.Flush();
            return data = _shellStream.Read();
        }
    }
}
