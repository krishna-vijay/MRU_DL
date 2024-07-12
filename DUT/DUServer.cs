using OpenTap;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;



//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace RjioMRU
{
    [Display("CCDUServer", Group: "RjioMRU", Description: "Insert a description here")]
    public class CCDUServer : ScpiInstrument
    {
        #region Settings
        public static object ServerLockObject { get; set; }
        public static bool loopBreak = false;
        public static string linkInteraface = "ens7f1";
        string ccduServerIp = "192.168.1.4";
        string ccduUserName = "root";
        string ccduPassword = "root123";
        SshClient MruCCDUServerClient { get; set; }


        SshClient PTP_CCDUServerClient { get; set; }
        SshClient PHC_CCDUServerClient { get; set; }
        SshClient L1_CCDUServerClient { get; set; }
        SshClient TestMac_CCDUServerClient { get; set; }
        SshClient CarrierAggrigationClient { get; set; }


        ShellStream MruCCDUServerDataStream { get; set; }
        ShellStream PTPCCDUServerDataStream { get; set; }
        ShellStream PHCCCDUServerDataStream { get; set; }
        ShellStream L1_CCDUServerDataStream { get; set; }
        ShellStream TestMacCCDUServerDataStream { get; set; }
        ShellStream CarrierAggrigationStream { get; set; }

        [Display("Server IP", Order: 0, Description: "Enter Server IP Address")]
        public string CcduServerIp { get => ccduServerIp; set => ccduServerIp = value; }
        [Display("DU Server User name", Order: 5, Description: "Enter DU Server user name")]
        public string CcduUserName { get => ccduUserName; set => ccduUserName = value; }
        [Display("DU Server Password", Order: 10, Description: "Enter DU Server password")]
        public string CcduPassword { get => ccduPassword; set => ccduPassword = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public CCDUServer()
        {

            Name = "AB#Server";
            ServerLockObject = new object();
            // ToDo: Set default values for properties / settings.
        }

        /// <summary>
        /// Open procedure for the instrument.
        /// </summary>
        public override async void Open()
        {

            MruCCDUServerClient = new SshClient(CcduServerIp, CcduUserName, CcduPassword);
            PTP_CCDUServerClient = new SshClient(CcduServerIp, CcduUserName, CcduPassword);
            PHC_CCDUServerClient = new SshClient(CcduServerIp, CcduUserName, CcduPassword);
            L1_CCDUServerClient = new SshClient(CcduServerIp, CcduUserName, CcduPassword);
            TestMac_CCDUServerClient = new SshClient(CcduServerIp, CcduUserName, CcduPassword);
            CarrierAggrigationClient = new SshClient(CcduServerIp, CcduUserName, CcduPassword);

            bool val;
            var thread = new Thread(() =>    {
                val = MruCCDUServerClientConnectFunction().Result; // Publish the return value
    });
            thread.Start();
            
            var thread1 = new Thread(() =>    {
                val = PTP_CCDUServerClientConnectFuction().Result; // Publish the return value
    });
            thread1.Start();
            
            var thread2 = new Thread(() =>    {
                val = PHC_CCDUServerClientConnectFunction().Result; // Publish the return value
    });
            thread2.Start();
            
            var thread3 = new Thread(() =>    {
                val = L1_CCDUServerClientConnectionFunction().Result; // Publish the return value
    });
            thread3.Start();
            
            var thread4 = new Thread(() =>    {
                val = TestMac_CCDUServerClientConnectionFunction().Result; // Publish the return value
    });
            thread4.Start();
            
            var thread5 = new Thread(() =>    {
                val = CarrierAggrigationClientConnectionFunction().Result; // Publish the return value
    });
            thread5.Start();


            thread.Join();  
            thread1.Join();
            thread2.Join(); 
            thread3.Join();
            thread4.Join();
            thread5.Join();


            //Thread t1 = new Thread(new ThreadStart(MruCCDUServerClientConnectFunction()));




            //var val = await Task.Run(() => MruCCDUServerClientConnectFunction());
            //var val1 = await Task.Run(() => PTP_CCDUServerClientConnectFuction());
            //var val2 = await Task.Run(() => PHC_CCDUServerClientConnectFunction());
            //var val3 = await Task.Run(() => L1_CCDUServerClientConnectionFunction());
            //var val4 = await Task.Run(() => TestMac_CCDUServerClientConnectionFunction());
            //var val5 = await Task.Run(() => CarrierAggrigationClientConnectionFunction());

            //while (true)
            //{
            //    if (val && val1 && val2 && val3 && val4 && val5 )
            //    {
            //        break;
            //    }
            //    TapThread.Sleep(1000);
            //}
           // MruCCDUServerClientConnectFunction();
            //-------------------------------------------------------------------------------------------------------------
            //PTP_CCDUServerClientConnectFuction();
            ////-------------------------------------------------------------------------------------------------------------
            //PHC_CCDUServerClientConnectFunction();
            ////-------------------------------------------------------------------------------------------------------------
            //L1_CCDUServerClientConnectionFunction();
            //////-------------------------------------------------------------------------------------------------------------
            //TestMac_CCDUServerClientConnectionFunction();
            ////-------------------------------------------------------------------------------------------------------------
            //CarrierAggrigationClientConnectionFunction();
            //-------------------------------------------------------------------------------------------------------------
            try
            {
                MruCCDUServerDataStream = MruCCDUServerClient.CreateShellStream("test", 80, 60, 800, 600, 65536);
                MruCCDUServerDataStream.WriteLine(Environment.NewLine);
            }
            catch (SshConnectionException ex)
            {

                Log.Error("CCDU Create shell error :" + ex.Message); throw;
            }
            try
            {
                PTPCCDUServerDataStream = PTP_CCDUServerClient.CreateShellStream("test1", 80, 60, 800, 600, 65536);
                PTPCCDUServerDataStream.WriteLine(Environment.NewLine);
                //PTPCCDUServerDataStream.ReadTimeout = 2000;
            }
            catch (SshConnectionException ex)
            {

                Log.Error("PTP Streem Create shell error :" + ex.Message); throw;
            }
            try
            {
                PHCCCDUServerDataStream = PHC_CCDUServerClient.CreateShellStream("test2", 80, 60, 800, 600, 65536);
                PTPCCDUServerDataStream.WriteLine(Environment.NewLine);
            }
            catch (SshConnectionException ex)
            {

                Log.Error("PHC Stream Create shell error :" + ex.Message); throw;
            }
            try
            {
                L1_CCDUServerDataStream = L1_CCDUServerClient.CreateShellStream("test3", 80, 60, 800, 600, 65536);
                L1_CCDUServerDataStream.WriteLine(Environment.NewLine);
            }
            catch (SshConnectionException ex)
            {

                Log.Error("L1 Stream Create shell error :" + ex.Message); throw;
            }
            try
            {
                TestMacCCDUServerDataStream = TestMac_CCDUServerClient.CreateShellStream("test4", 80, 60, 800, 600, 65536);
                TestMacCCDUServerDataStream.WriteLine(Environment.NewLine);
            }
            catch (SshConnectionException ex)
            {

                Log.Error("TestMac stream Create shell error :" + ex.Message); throw;
            }
            try
            {
                CarrierAggrigationStream = MruCCDUServerClient.CreateShellStream("test", 80, 60, 800, 600, 65536);
                CarrierAggrigationStream.WriteLine(Environment.NewLine);
            }
            catch (SshConnectionException ex)
            {

                Log.Error("CCDU Create shell error :" + ex.Message); throw;
            }

            //base.Open();
            // TODO:  Open the connection to the instrument here

            //if (!IdnString.Contains("Instrument ID"))
            //{
            //    Log.Error("This instrument driver does not support the connected instrument.");
            //    throw new ArgumentException("Wrong instrument type.");
            // }
        }

        private async Task<bool> CarrierAggrigationClientConnectionFunction()
        {
            try
            {
                CarrierAggrigationClient.Connect();
                Log.Info("Hurrah Connected to CA  Server");
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("CA Connection error :" + ex.Message);
                throw;
            }

            catch (SocketException ex) { Log.Error("CA Connection error :" + ex.Message); throw; }
            catch (SshAuthenticationException ex) { Log.Error("CA Connection error :" + ex.Message); throw; }
            catch (SshConnectionException ex) { Log.Error("CA Connection error :" + ex.Message); throw; }
            catch (ProxyException ex) { Log.Error("CA Connection error :" + ex.Message); throw; }
            return true;
        }

        private async Task<bool> TestMac_CCDUServerClientConnectionFunction()
        {
            try
            {
                TestMac_CCDUServerClient.Connect();
                Log.Info("Hurrah Connected to TestMac Client");
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("TestMac Client Connection error :" + ex.Message);
                throw;
            }

            catch (SocketException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (SshAuthenticationException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (SshConnectionException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (ProxyException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            return true;
        }

        private async Task<bool> L1_CCDUServerClientConnectionFunction()
        {
            try
            {
                L1_CCDUServerClient.Connect();
                Log.Info("Hurrah Connected to L1 Client");
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("L1 Client Connection error :" + ex.Message);
                throw;
            }

            catch (SocketException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (SshAuthenticationException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (SshConnectionException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (ProxyException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            return true;
        }

        private async Task<bool> PHC_CCDUServerClientConnectFunction()
        {
            try
            {
                PHC_CCDUServerClient.Connect();
                Log.Info("Hurrah Connected to PHC Client");
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("PHC Client Connection error :" + ex.Message);
                throw;
            }

            catch (SocketException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (SshAuthenticationException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (SshConnectionException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (ProxyException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            return true;
        }

        private async Task<bool> PTP_CCDUServerClientConnectFuction()
        {
            try
            {
                PTP_CCDUServerClient.Connect();
                Log.Info("Hurrah Connected to PTP_ServerClient");
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("PTP Client Connection error :" + ex.Message);
                throw;
            }

            catch (SocketException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (SshAuthenticationException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (SshConnectionException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (ProxyException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            return true;
        }

        private async Task<bool> MruCCDUServerClientConnectFunction()
        {
            try
            {
                MruCCDUServerClient.Connect();
                Log.Info("Hurrah Connected to CCDU Server");
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("CCDU Connection error :" + ex.Message);
                throw;
            }

            catch (SocketException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (SshAuthenticationException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (SshConnectionException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            catch (ProxyException ex) { Log.Error("CCDU Connection error :" + ex.Message); throw; }
            return true;
        }

        /// <summary>
        /// Close procedure for the instrument.
        /// </summary>
        public override void Close()
        {
            MruCCDUServerClient.Disconnect();
            MruCCDUServerClient.Dispose();
            MruCCDUServerClient = null;
            MruCCDUServerDataStream.Dispose();
            MruCCDUServerDataStream = null;

            PTP_CCDUServerClient.Disconnect();
            PTP_CCDUServerClient.Dispose();
            PTP_CCDUServerClient = null;
            PTPCCDUServerDataStream.Dispose();
            PTPCCDUServerDataStream = null;


            PHC_CCDUServerClient.Disconnect();
            PHC_CCDUServerClient.Dispose();
            PHC_CCDUServerClient = null;
            PHCCCDUServerDataStream.Dispose();
            PHCCCDUServerDataStream = null;

            L1_CCDUServerClient.Disconnect();
            L1_CCDUServerClient.Dispose();
            L1_CCDUServerClient = null;
            L1_CCDUServerDataStream.Dispose();
            L1_CCDUServerDataStream = null;

            TestMac_CCDUServerClient.Disconnect();
            TestMac_CCDUServerClient.Dispose();
            TestMac_CCDUServerClient = null;
            TestMacCCDUServerDataStream.Dispose();
            TestMacCCDUServerDataStream = null;


            CarrierAggrigationClient.Disconnect();
            CarrierAggrigationClient.Dispose();
            CarrierAggrigationClient = null;
            CarrierAggrigationStream.Dispose();
            CarrierAggrigationStream = null;
            // TODO:  Shut down the connection to the instrument here.
            base.Close();
        }


        #region Functions
        public bool BasicVerification(string BasicverificationCommand, out string linkInterface)
        {
            linkInterface = string.Empty;
            List<string> ServerInterfaces = new List<string>();
            ServerInterfaces.Clear();
            WritetoSSH(Environment.NewLine);
            WritetoSSH(BasicverificationCommand);
            var readValue = ReadFromSSH();
            Log.Info("Raw Read from Command :" + BasicverificationCommand + " ->" + readValue);
            foreach (var line in readValue.Split('\n'))
            {
                string strInterface = string.Empty;
                Log.Info("Basic Verification :" + line);
                if (line.Contains("1000"))
                {
                    Log.Info("Basic Verification : Interfaces:" + (strInterface = line.Split(':')[1]));
                    ServerInterfaces.Add(strInterface);
                }
            }

            foreach (var Iface in ServerInterfaces)
            {
                WritetoSSH("ethtool " + Iface);
                Log.Info("Basic Verification : " + (readValue = ReadFromSSH()));
                if (readValue.Contains("25000Mb/s"))
                {
                    Log.Info("Found the 25Gbps interface");
                    linkInterface = Iface;
                    return true;
                }
            }

            return false;
        }

        public bool ChangeInterfceIP(string interfaceName, string ipAddress)
        {
            if (string.IsNullOrEmpty(interfaceName.Trim()))
            {
                return false;
            }
            WritetoSSH("ifconfig " + interfaceName + " " + ipAddress + " up");
            return true;
        }

        public bool Interface_testIPverification(string interfaceName, string IPaddress)
        {
            WritetoSSH("ifconfig " + interfaceName);
            var ReadValue = ReadFromSSH();
            bool IpThere = ReadValue.Contains(IPaddress);
            if (!IpThere)
            {
                WritetoSSH("ifconfig " + interfaceName + " " + IPaddress + " up");
                var retrunValue = ReadFromSSH();
            }
            return true;
        }



        public void PtpTaBCommandExecute(bool KeepTrace)
        {
            if (string.IsNullOrEmpty(CCDUServer.linkInteraface.Trim()))
            {
                CCDUServer.linkInteraface = "ens7f1";
            } 
           // int counter = 0;

            //ptp4l -i p4p3 -m -f /configs/ptp4l-master.conf
            // PTPCCDUServerDataStream.WriteLine(Environment.NewLine);
            string command = "ptp4l -i " + CCDUServer.linkInteraface + " -m -f /configs/ptp4l-master.conf";
           // command = "ptp4l -i  p4p3 -m -f /configs/ptp4l-master.conf";
            Log.Info($"PtpTaBCommandExecute : {command}");
            PTPCCDUServerDataStream.WriteLine(command);
            //TapThread.Sleep(4000);
            try
            {
                do
                {
                    lock (ServerLockObject)
                    {
                        var readValue = PTPCCDUServerDataStream.Read();
                        if (!string.IsNullOrEmpty(readValue))
                        {

                            foreach (var Readed in readValue.Split('\n'))
                            {
                                if (KeepTrace)
                                {
                                    Log.Info("PTP Command in while:" + Readed);
                                }
                                
                            }
                        }
                    }
                    lock (ServerLockObject)
                    {
                        if (CCDUServer.loopBreak)
                            break;
                    }
                    TapThread.Sleep(100);
                } while (!TapThread.Current.AbortToken.IsCancellationRequested);
            }
            catch (Exception ex)
            {
                Log.Info("Loop executiong error \" PtpTaBCommandExecute\" : " + ex.Message + " :" + ex.Source);
                throw;
            }
            // return true;
        }

        public void PhCTaBCommandExecute(bool keepTrace, string Command1, string Command2)
        {
           // int counter = 0;
            PHCCCDUServerDataStream.WriteLine(Environment.NewLine);
           
            PHCCCDUServerDataStream.WriteLine(Command1);
            Log.Info("PHC Command :" + Command1);
            Log.Info("PHC Read Back :" + PHCCCDUServerDataStream.Read());
            //./phc2sys -s p4p3 -O -37 -m
            //string command2phc = "./phc2sys -s " + CCDUServer.linkInteraface + " -O -37 -m";
            Log.Info($"PHC Command2 : {Command2}");
            PHCCCDUServerDataStream.WriteLine(Command2); ;
            //Log.Info("PHC Read Back :" + PHCCCDUServerDataStream.Read());
            do
            {
                lock (ServerLockObject)
                {
                    var readValue = PHCCCDUServerDataStream.Read();
                    if (!string.IsNullOrEmpty(readValue.Trim()))
                    {
                        if (keepTrace)
                        Log.Info("PHC Command in while:" + readValue);
                    }
                }

                lock (ServerLockObject)
                {
                    if (CCDUServer.loopBreak)
                        break;
                }
                TapThread.Sleep(200);
            } while (!TapThread.Current.AbortToken.IsCancellationRequested);


            // return true;
        }


        public void L1TaBCommandExecute(bool KeepTrace,string command1)
        {
            // int counter = 0;
            //string command1 = "cd /home/macro-gnb/working_dir/pkg_HiPhy_22.11.00.12_Xml_v5.2.5_Os_CentOs/custom-sw/ccdu/scripts/l1/";
            L1_CCDUServerDataStream.WriteLine(command1);
            Log.Info("L1 Command :" + command1);
            //Log.Info("PHC Read Back :" + PHCCCDUServerDataStream.Read());
            //./phc2sys -s p4p3 -O -37 -m
            string command2 = "./l1_entry_tdd.sh";
            Log.Info($"L1 Command2 : {command2}");
            L1_CCDUServerDataStream.WriteLine(command2); ;
            //Log.Info("PHC Read Back :" + PHCCCDUServerDataStream.Read());
            do
            {
                lock (ServerLockObject)
                {
                    var readValue = L1_CCDUServerDataStream.Read();
                    if (!string.IsNullOrEmpty(readValue.Trim()))
                    {
                        foreach (var readLines in readValue.Split('\n'))
                        {
                            if (KeepTrace)
                            {
                                Log.Info("L1 Command in while:" + readLines);
                            }
                           
                        }

                    }
                    if (readValue.Contains("welcome to application console"))
                    {
                        break;
                    }
                }
                //Log.Info(counter++.ToString());
                lock (ServerLockObject)
                {
                    if (CCDUServer.loopBreak)
                        break;
                }

                TapThread.Sleep(100);
            } while (true);


            // return true;
        }


        public void TestMacTaBCommandExecute(bool KeepTrace,string command1, string command2 )
        {
            // int counter = 0;
           // string command1 = "cd /home/macro-gnb/working_dir/testmac_pkg_22.11.00.03";
            TestMacCCDUServerDataStream.WriteLine(command1);
            Log.Info("TestMac Command :" + command1);
            //Log.Info("PHC Read Back :" + PHCCCDUServerDataStream.Read());
            //./phc2sys -s p4p3 -O -37 -m
            //string command2 = "./testmac_entry.sh";
            Log.Info($"TestMac Command2 : {command2}");
            TestMacCCDUServerDataStream.WriteLine(command2); ;
            //Log.Info("PHC Read Back :" + PHCCCDUServerDataStream.Read());
            do
            {
                lock (ServerLockObject)
                {
                    var readValue = TestMacCCDUServerDataStream.Read();
                    if (!string.IsNullOrEmpty(readValue.Trim()))
                    {
                        foreach (var readlines in readValue.Split('\n'))
                        {
                            if (KeepTrace)
                            {
                                Log.Info("TestMac Command in while:" + readlines);
                            }
                           
                        }
                    }
                    if (readValue.Contains("welcome to application console"))
                    {
                        break;
                    }
                }
                //Log.Info(counter++.ToString());
                lock (ServerLockObject)
                {
                    if (CCDUServer.loopBreak)
                        break;
                }

                TapThread.Sleep(100);
            } while (!TapThread.Current.AbortToken.IsCancellationRequested);


            // return true;
        }



        public void startDLTest(bool KeepTrace,string command,string command1)
        {
            TestMacCCDUServerDataStream.WriteLine(Environment.NewLine);
            //string commamd = "phystart 4 0 0";
            Log.Info("DL Test Command : " + command);
            TestMacCCDUServerDataStream.WriteLine(command);
            var readValue = TestMacCCDUServerDataStream.Read();
            Log.Info("TestMac Reply: " + readValue);


           // string command1 = "run 1 2 1 100 1433";
            TestMacCCDUServerDataStream.WriteLine(command1);

            do
            {
                lock (ServerLockObject)
                {
                    readValue = TestMacCCDUServerDataStream.Read();
                    if (!string.IsNullOrEmpty(readValue.Trim()))
                    {
                        foreach (var readlines in readValue.Split('\n'))
                        {
                            if (KeepTrace)
                            {
                                Log.Info("TestMac Test in while:" + readlines);
                            }
                           
                        }
                    }

                }
                //Log.Info(counter++.ToString());
                lock (ServerLockObject)
                {
                    if (CCDUServer.loopBreak)
                        break;
                }

                TapThread.Sleep(100);
            } while (!TapThread.Current.AbortToken.IsCancellationRequested);

        }

        public void CarrierAggrigation(string command1 = "export LD_LIBRARY_PATH=/custom-sw/thirdparty/usr/lib64/:/custom-sw/thirdparty/usr/lib/:/usr/local/bin/:/custom-sw/thirdparty/usr/local/ssl/lib/", string command2 = "./netopeer2-cli", string command3 = "connect --host=192.168.1.2 --port=1830 --login=root", string refTag = "Type your password:", string password = "root", string command4 = "listen", string command4RefTag = "cmd_listen: Already connected to", string command5 = "edit-config --target running --config=/root/uplane_test_xml_for_release_2.9.0_ACTIVE_1.xml --defop merge", string command6 = "rm -rf /root/.ssh/known_hosts")
        {
            bool warningMessage = false;
            Stopwatch  sw = Stopwatch.StartNew();
           // string command1 = "export LD_LIBRARY_PATH=/custom-sw/thirdparty/usr/lib64/:/custom-sw/thirdparty/usr/lib/:/usr/local/bin/:/custom-sw/thirdparty/usr/local/ssl/lib/";
            CarrierAggrigationStream.WriteLine(command1);
            TapThread.Sleep(1000);
            Log.Info("CA Command sent : export LD_LIBRARY_PATH=/custom-sw/thirdparty/usr/lib64/:/custom-sw/thirdparty/usr/lib/:/usr/local/bin/:/custom-sw/thirdparty/usr/local/ssl/lib/");
            string readConsole = string.Empty;

            CarrierAggrigationStream.WriteLine(command2 + "\n");
            TapThread.Sleep(1000);
            Log.Info("Command2 Sent: ./netopeer2-cli");
            do
            {
                readConsole = CarrierAggrigationStream.Read();
                foreach (var item in readConsole.Split('\n'))
                {
                    Log.Info(readConsole);
                }
                if (readConsole.Contains(">"))
                {
                    break;
                }
                TapThread.Sleep(100);
                if (sw.ElapsedMilliseconds > 10000)
                {
                    throw new Exception("Timeout");
                }
            } while (readConsole.Contains(">"));

           
            CarrierAggrigationStream.WriteLine(command3 + "\n");
            TapThread.Sleep(3000);
            Log.Info("Command3 Sent: connect --host=192.168.1.2 --port=1830 --login=root");
            TapThread.Sleep(100);
            sw.Reset();
            sw.Restart();
            do
            {
                readConsole = CarrierAggrigationStream.Read();
                foreach (var item in readConsole.Split('\n'))
                {
                    Log.Info(readConsole);
                }
                if (readConsole.Contains(refTag))
                {
                    break;
                }
                if (readConsole.Contains("(yes/no)?"))
                {
                    CarrierAggrigationStream.WriteLine("yes");
                }
                else if(readConsole.Contains("WARNING:"))
                {
                    warningMessage = true;
                    break;
                }
                //if (readConsole.Contains("login=rootnc ERROR: Remote host key changed"))
                //{
                //    Log.Info("Error during CA login=rootnc ERROR: Remote host key changed");
                //    Log.Info("Command6 Sent:rm -rf /root/.ssh/known_hosts");
                //    CarrierAggrigationStream.WriteLine(command6 + "\n");
                //    TapThread.Sleep(3000);
                //}
                TapThread.Sleep(1000);
                if (sw.ElapsedMilliseconds>20000)
                {
                    Log.Info("Timeout (20 seconds) occured during Carrier Aggrigation Functions");
                    //bool SendIfCommandOnce = true;
                    //if(SendIfCommandOnce)
                    //{
                    //    SendIfCommandOnce = false;
                    //    CarrierAggrigationStream.WriteLine(command6 + "\n");
                    //    TapThread.Sleep(3000);
                    //    Log.Info("Command6 Sent:rm -rf /root/.ssh/known_hosts");
                    //}
                    break;
                }

                //if (sw.ElapsedMilliseconds > 25000)
                //{
                //    Log.Info("Timeout (20 seconds) occured even after issuing command6 : {0}", command6);
                //    break;
                //}
                Log.Info("CA functions : " + readConsole);
            } while (!readConsole.Contains(refTag));

            if (!warningMessage)
            {

                TapThread.Sleep(2000);
                CarrierAggrigationStream.WriteLine(password);
                TapThread.Sleep(2000);
                CarrierAggrigationStream.WriteLine("");
                sw.Restart();
                TapThread.Sleep(100);
              
            }
            readConsole = string.Empty;
            do
            {
                readConsole = CarrierAggrigationStream.Read();
                foreach (var item in readConsole.Split('\n'))
                {
                    Log.Info(readConsole);
                }
                if (readConsole.Contains(">"))
                {
                    break;
                }
                TapThread.Sleep(100);
                if (sw.ElapsedMilliseconds > 10000)
                {
                    throw new Exception("Timeout");
                }
                Log.Info("CA Functions Waiting for > " + readConsole);
            } while (readConsole.Contains(">"));
            TapThread.Sleep(1000);
            CarrierAggrigationStream.WriteLine(command4);
            readConsole = string.Empty;
            sw.Restart();
            TapThread.Sleep(1000);
            do
            {
                readConsole = CarrierAggrigationStream.Read();
                foreach (var item in readConsole.Split('\n'))
                {
                    Log.Info(readConsole);
                }
                if (readConsole.Contains(command4RefTag) || readConsole.Contains("OK"))
                {
                    break;
                }
                TapThread.Sleep(100);
                if (sw.ElapsedMilliseconds > 10000)
                {
                    throw new Exception("Timeout");
                }
            } while (readConsole.Contains(command4RefTag)||readConsole.Contains("OK"));
            TapThread.Sleep(1000);
            CarrierAggrigationStream.WriteLine(command5);
            readConsole = string.Empty;
            sw.Restart();
            TapThread.Sleep(1000);
            do
            {
                readConsole = CarrierAggrigationStream.Read();
                TapThread.Sleep(100);
                Log.Info(readConsole);
                if (sw.ElapsedMilliseconds > 10000)
                {
                    throw new Exception("Timeout");
                }
            } while (readConsole.Contains("OK"));
            TapThread.Sleep(100);
        }
        
        
        
        
        private void WritetoSSH(string command)
        {
            // _shellStream.WriteLine("");        
            MruCCDUServerDataStream.WriteLine(command);
            Thread.Sleep(100);

        }
        private string ReadFromSSH()
        {
            string data = string.Empty;
            // _shellStream.Flush();
            return data = MruCCDUServerDataStream.Read();
        }

        public void RunVirutalFunctions(String VirtualFunctionCommand,string VirtualFunctionCommand2,string VirtualFunctionCommand3)
        {
            WritetoSSH(VirtualFunctionCommand);
            Log.Info    ("Virtual Functions command sent "+VirtualFunctionCommand);
            WritetoSSH(VirtualFunctionCommand2);
            Log.Info("Virtual Functions command sent " + VirtualFunctionCommand3);
            WritetoSSH(VirtualFunctionCommand3);
            Log.Info("Virtual Functions command sent " + VirtualFunctionCommand3);
        }

        public void CCDU_CA_known_hostsCommand()
        {
            WritetoSSH("rm -rf /root/.ssh/known_hosts");

        }

        #endregion Functions
    }
}
