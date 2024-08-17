// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using RjioMRU;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace RjioMRU
{
    [Display("CCDU File Copy to 21DR", Group: "RjioMRU", Description: "Insert a description here")]
    public class FileCopyfromCCDU_21DR : TestStep
    {
        /* 
        scp /root/MRUV2_21_37/SKU1-MRU-FW-PKG-2.21.37-FACT-1.3-Sanmina/binaries.tar.gz root@192.168.1.2:/home/root/
The authenticity of host '192.168.1.2 (192.168.1.2)' can't be established.
ECDSA key fingerprint is SHA256:cY+BS1XUY83h1Q8RR/YYFHIntdykt59ugx111WUgKzQ.
ECDSA key fingerprint is MD5:94:b1:1a:4f:13:10:00:68:76:8e:9e:ee:4a:98:ff:ed.
Are you sure you want to continue connecting (yes/no)? yes
Warning: Permanently added '192.168.1.2' (ECDSA) to the list of known hosts.
root@192.168.1.2's password:

         */

        private string command = "scp /root/MRUV2_21_37/SKU1-MRU-FW-PKG-2.21.37-FACT-1.3-Sanmina/binaries.tar.gz root@192.168.1.2:/home/root/";

        private CCDUServer cCDUServerobj;

        #region Settings
        [Display("CCDU Server", Order: 0, Description: "Select Server")]
        public CCDUServer CCDUServerobj { get => cCDUServerobj; set => cCDUServerobj = value; }
        [Display(" Command to CCDU", Order: 1, Description: "server command  ")]
        public string CommandforCCDU { get => command; set => command = value; }



        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public FileCopyfromCCDU_21DR()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            var FileCopyReturn = CCDUServerobj.BasicCCDUCommands(CommandforCCDU);

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.
            if (FileCopyReturn)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }        

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }





    [Display("CCDU Basic Verification", Group: "RjioMRU", Description: "Insert a description here")]
    public class CCDUBasicVerification : TestStep
    {
        private string basicverificationCommand = "ip link show";
        private string varificationInterface1 = "ens1f0";
        private string varificationInterface2 = "ens1f1";
        private string varificationInterface3 = "ens1f2";
        private string varificationInterface4 = "ens1f3";
        private CCDUServer cCDUServerobj;

        #region Settings
        [Display("CCDU Server", Order: 0, Description: "Select Server")]
        public CCDUServer CCDUServerobj { get => cCDUServerobj; set => cCDUServerobj = value; }
        [Display("Basic verification command", Order: 2, Description: "server command to basic verification")]
        public string BasicverificationCommand { get => basicverificationCommand; set => basicverificationCommand = value; }
        [Display("Interface1 name", Order: 5, Description: "Enter interface1 validation name")]
        public string VarificationInterface1 { get => varificationInterface1; set => varificationInterface1 = value; }
        [Display("Interface2 name", Order: 5, Description: "Enter interface2 validation name")]
        public string VarificationInterface2 { get => varificationInterface2; set => varificationInterface2 = value; }
        [Display("Interface3 name", Order: 5, Description: "Enter interface3 validation name")]
        public string VarificationInterface3 { get => varificationInterface3; set => varificationInterface3 = value; }
        [Display("Interface4 name", Order: 5, Description: "Enter interface4 validation name")]
        public string VarificationInterface4 { get => varificationInterface4; set => varificationInterface4 = value; }


        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public CCDUBasicVerification()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            var basicVarificationStatus = CCDUServerobj.BasicVerification(BasicverificationCommand, out CCDUServer.linkInteraface);
            if (!basicVarificationStatus)
            {
                TapThread.Sleep(100);
                basicVarificationStatus = CCDUServerobj.BasicVerification(BasicverificationCommand, out CCDUServer.linkInteraface);
            }
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            if (basicVarificationStatus)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, this.Verdict.ToString(), " ", this.Verdict== Verdict.Pass?"TRUE":"FALSE",  " ", "EQ", "TRUE", "Bool");
            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("CCDU Link Interface IP Change", Group: "RjioMRU", Description: "Insert a description here")]
    public class CheckLinkDetection : TestStep
    {
        private string ipAddress = "192.168.1.1";
        private string speedCheckString = "speed 25000Mb/s,";
        private string linkDetecetedString = "Link detected";

        private CCDUServer cCDUServerobj;

        #region Settings
        [Display("CCDU Server", Order: 0, Description: "Select Server")]
        public CCDUServer CCDUServerobj { get => cCDUServerobj; set => cCDUServerobj = value; }
        [Display("Link interface IP", Order: 2, Description: "server command to basic verification")]
        public string IpAddress { get => ipAddress; set => ipAddress = value; }




        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public CheckLinkDetection()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            var ipChangeStatus = CCDUServerobj.ChangeInterfceIP(CCDUServer.linkInteraface, ipAddress);
            if (!ipChangeStatus)
            {
                TapThread.Sleep(100);
                ipChangeStatus = CCDUServerobj.ChangeInterfceIP(CCDUServer.linkInteraface, ipAddress);
            }
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            if (ipChangeStatus)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, this.Verdict.ToString(), " ", ipChangeStatus.ToString(),"", "EQ", "TRUE", "Bool");
            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("CCDU Interface Test Verification", Group: "RjioMRU", Description: "Insert a description here")]
    public class interfaecTestVerificaiton : TestStep
    {
        private string interfaceName = "ens1f1";
        private string ipAddress = "192.168.1.2";
        private string linkDetecetedString = "Link detected";

        private CCDUServer cCDUServerobj;

        #region Settings
        [Display("CCDU Server", Order: 0, Description: "Select Server")]
        public CCDUServer CCDUServerobj { get => cCDUServerobj; set => cCDUServerobj = value; }

        [Display("Ip Address", Order: 5, Description: "IP address ")]
        public string IpAddress { get => ipAddress; set => ipAddress = value; }


        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public interfaecTestVerificaiton()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            var PingTestStatus = CCDUServerobj.Interface_testIPverification(CCDUServer.linkInteraface, IpAddress);
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
            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("PTP Tab ptp4l Command", Group: "RjioMRU", Description: "Insert a description here")]
    public class PTP_Tabptp4lCommand : TestStep
    {
        private string interfaceName = "ens7f1";
        private string ipAddress = "192.168.1.30";
        private string linkDetecetedString = "Link detected";
        private bool KeepTrace = false;

        private CCDUServer cCDUServerobj;

        #region Settings
        [Display("CCDU Server", Order: 0, Description: "Select Server")]
        public CCDUServer CCDUServerobj { get => cCDUServerobj; set => cCDUServerobj = value; }
        [Display("Enable Trace", Order: 10, Description: "Check for Trace Enable.")]
        public bool KeepTrace1 { get => KeepTrace; set => KeepTrace = value; }

        //[Display("Interface IP",Order:20,Description:"Enter ip of the interface ")]
        //public string IpAddress { get => ipAddress; set => ipAddress = value; }

        //[Display("Interface Name", Order: 2, Description: "Interface Name")]
        //public string InterfaceName { get => interfaceName; set => interfaceName = value; }
        //[Display("Ip Address", Order: 5, Description: "IP address ")]
        //public string IpAddress { get => ipAddress; set => ipAddress = value; }


        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public PTP_Tabptp4lCommand()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            Thread PTPthread = new Thread(() => CCDUServerobj.PtpTaBCommandExecute(KeepTrace1));
            PTPthread.IsBackground = false;
            PTPthread.Start();
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            UpgradeVerdict(Verdict.Pass);
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, this.Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", " ","EQ", "TRUE", "Bool");

        }
    }

    [Display("PHc Tab phc2sys Command", Group: "RjioMRU", Description: "Insert a description here")]
    public class PHC_Tabphc2sysCommand : TestStep
    {
        private string interfaceName = "p4p3";
        private string ipAddress = "192.168.1.30";
        private string linkDetecetedString = "Link detected";
        private bool KeepTracing = false;
        private string command1 = "cd /custom-sw/thirdparty/usr/sbin/";
        private string command2phc = "./phc2sys -s " + CCDUServer.linkInteraface + " -O -37 -m";

        private CCDUServer cCDUServerobj;

        #region Settings
        [Display("CCDU Server", Order: 0, Description: "Select Server")]
        public CCDUServer CCDUServerobj { get => cCDUServerobj; set => cCDUServerobj = value; }
        [Display("Enable Trace", Order: 10, Description: "Check for Trace Enable.")]
        public bool KeepTracing1 { get => KeepTracing; set => KeepTracing = value; }
        [Display("Command1 ", Order: 15, Description: "Enter the first command")]

        public string Command1 { get => command1; set => command1 = value; }

        [Display("Command2", Order: 20, Description: "Enter second command")]
        public string Command2phc { get => command2phc; set => command2phc = value; }


        //[Display("Interface Name", Order: 2, Description: "Interface Name")]
        //public string InterfaceName { get => interfaceName; set => interfaceName = value; }
        //[Display("Ip Address", Order: 5, Description: "IP address ")]
        //public string IpAddress { get => ipAddress; set => ipAddress = value; }


        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public PHC_Tabphc2sysCommand()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            Command2phc = "./phc2sys -s " + CCDUServer.linkInteraface + " -O -37 -m";
            Thread PHCthread = new Thread(() => CCDUServerobj.PhCTaBCommandExecute(KeepTracing1, Command1, Command2phc));
            PHCthread.IsBackground = true;
            PHCthread.Start();
            // var PingTestStatus = CCDUServerobj.PhCTaBCommandExecute(CCDUBasicVerification.linkInteraface);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps

            //if (PingTestStatus)
            //{
            UpgradeVerdict(Verdict.Pass);
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");


            //}
            //else
            //{
            //    UpgradeVerdict(Verdict.Fail);
            //}
            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }

    }


    [Display("L1 Tab phc2sys Command", Group: "RjioMRU", Description: "Insert a description here")]
    public class L1_Tabphc2sysCommand : TestStep
    {


        private CCDUServer cCDUServerobj;
        private bool KeepTrace = false;
        private string command1 = "cd /home/macro-gnb/working_dir/pkg_HiPhy_22.11.00.12_Xml_v5.2.5_Os_CentOs/custom-sw/ccdu/scripts/l1/";

        #region Settings
        [Display("CCDU Server", Order: 0, Description: "Select Server")]
        public CCDUServer CCDUServerobj { get => cCDUServerobj; set => cCDUServerobj = value; }
        [Display("Enable Trace", Order: 10, Description: "Check for Trace Enable.")]
        public bool KeepTrace1 { get => KeepTrace; set => KeepTrace = value; }

        [Display("Command 1", Order: 15, Description: "Enter command1 script")]
        public string Command1 { get => command1; set => command1 = value; }

        //[Display("Interface Name", Order: 2, Description: "Interface Name")]
        //public string InterfaceName { get => interfaceName; set => interfaceName = value; }
        //[Display("Ip Address", Order: 5, Description: "IP address ")]
        //public string IpAddress { get => ipAddress; set => ipAddress = value; }


        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public L1_Tabphc2sysCommand()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            bool returnValue = false;
            Thread L1_thread = new Thread(() => CCDUServerobj.L1TaBCommandExecute(KeepTrace1, Command1));
            L1_thread.IsBackground = true;
            L1_thread.Start();
            // var PingTestStatus = CCDUServerobj.PhCTaBCommandExecute(CCDUBasicVerification.linkInteraface);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps

            //if (PingTestStatus)
            //{

            UpgradeVerdict(Verdict.Pass);
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, this.Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", " ", "EQ", "TRUE", "Bool");

            //}
            //else
            //{
            //    UpgradeVerdict(Verdict.Fail);
            //}
            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }

    }
    [Display("TestMac Tab phc2sys Command", Group: "RjioMRU", Description: "Insert a description here")]
    public class TestMac_Tabphc2sysCommand : TestStep
    {

        private string command1 = "cd /home/macro-gnb/working_dir/testmac_pkg_22.11.00.03";
        private string command2 = "./testmac_entry.sh";
        private CCDUServer cCDUServerobj;
        private bool KeepTracing = false;
        #region Settings
        [Display("CCDU Server", Order: 0, Description: "Select Server")]
        public CCDUServer CCDUServerobj { get => cCDUServerobj; set => cCDUServerobj = value; }
        [Display("Enable Trace", Order: 10, Description: "Check for Trace Enable.")]
        public bool KeepTracing1 { get => KeepTracing; set => KeepTracing = value; }
        [Display("Command1", Order: 15, Description: "Enter the command1 script")]
        public string Command1 { get => command1; set => command1 = value; }
        [Display("Command2", Order: 20, Description: "Enter the command2 script")]
        public string Command2 { get => command2; set => command2 = value; }

        int timeout = 10;
        [Display("Time out", Order: 30, Description: "Enter the timeout in seconds")]
        public int Timeout { get => timeout; set => timeout = value; }


        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public TestMac_Tabphc2sysCommand()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            bool resultStatus = false;
            Thread TestMac_thread = new Thread(() => resultStatus = CCDUServerobj.TestMacTaBCommandExecute(KeepTracing1, Command1, Command2, Timeout));
            TestMac_thread.IsBackground = true;
            TestMac_thread.Start();
            // var PingTestStatus = CCDUServerobj.PhCTaBCommandExecute(CCDUBasicVerification.linkInteraface);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps
                             //if (resultStatus)
                             //{

            UpgradeVerdict(Verdict.Pass);
            //}
            //else
            //{
            // UpgradeVerdict(Verdict.Fail);
            // }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, this.Verdict.ToString(), " ", resultStatus.ToString(), "", "EQ", "TRUE", "Bool");

        }

    }

    [Display("TestMac Start DL Testing", Group: "RjioMRU", Description: "Insert a description here")]
    public class TestMac_TABStartDLTest : TestStep
    {


        private CCDUServer cCDUServerobj;
        private string commamd = "phystart 4 0 0";
        private string command1 = "run 1 2 1 100 1433";
        #region Settings
        private bool KeepTracing = false;
        [Display("CCDU Server", Order: 0, Description: "Select Server")]
        public CCDUServer CCDUServerobj { get => cCDUServerobj; set => cCDUServerobj = value; }

        [Display("Enable Trace", Order: 10, Description: "Check for Trace Enable.")]
        public bool KeepTracing1 { get => KeepTracing; set => KeepTracing = value; }
        [Display("Command", Order: 15, Description: "Enter the command script")]
        public string Commamd { get => commamd; set => commamd = value; }

        [Display("command1", Order: 20, Description: "ENter the command1 script")]
        public string Command1 { get => command1; set => command1 = value; }


        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public TestMac_TABStartDLTest()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            Thread TestMacTest_thread = new Thread(() => CCDUServerobj.startDLTest(KeepTracing1, Commamd, Command1));
            TestMacTest_thread.IsBackground = true;
            TestMacTest_thread.Start();
            //CCDUServerobj.startDLTest();
            // var PingTestStatus = CCDUServerobj.PhCTaBCommandExecute(CCDUBasicVerification.linkInteraface);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps

            UpgradeVerdict(Verdict.Pass);
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, this.Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", " ", "EQ", "TRUE", "Bool");
        }

    }


    [Display("Close all Plugins", Group: "RjioMRU", Description: "Insert a description here")]
    public class CloseAllPlugins : TestStep
    {


        public CloseAllPlugins()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {

            lock (CCDUServer.ServerLockObject)
            {
                CCDUServer.loopBreak = true;
            }
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            UpgradeVerdict(Verdict.Pass);
            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Loop Break False", Group: "RjioMRU", Description: "Insert a description here")]
    public class LoopBreakFalse : TestStep
    {


        public LoopBreakFalse()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {

            lock (CCDUServer.ServerLockObject)
            {
                CCDUServer.loopBreak = false;
            }
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.


            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Run Virtual Functions", Group: "RjioMRU", Description: "Insert a description here")]
    public class RunVirtualFunctions : TestStep
    {
        private CCDUServer cCDUServerobj;
        string command1 = string.Empty;
        string command2 = string.Empty;
        string command3 = string.Empty;
        public CCDUServer CCDUServerobj { get => cCDUServerobj; set => cCDUServerobj = value; }
        [Display("Virtual Function script1", Order: 1, Description: "Enter virtual function script1")]

        public string Command1 { get => command1; set => command1 = value; }
        [Display("Virtual Function script2", Order: 5, Description: "Enter virtual function script2")]

        public string Command2 { get => command2; set => command2 = value; }
        [Display("Virtual Function script3", Order: 10, Description: "Enter virtual function script3")]
        public string Command3 { get => command3; set => command3 = value; }

        public RunVirtualFunctions()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            bool returnValue = false;
            try
            {
                returnValue = CCDUServerobj.RunVirutalFunctions(Command1, Command2, Command3);

            }
            catch (Exception)
            {

                throw;
            }


            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.


            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, this.Verdict.ToString(), " ", returnValue.ToString(), "","EQ", "TRUE", "Bool");
        }
    }

    [Display("Carrier Aggrigation Functions", Group: "RjioMRU", Description: "Insert a description here")]
    public class RunCAFunctions : TestStep
    {


        /*
         string command1 = "export LD_LIBRARY_PATH=/custom-sw/thirdparty/usr/lib64/:/custom-sw/thirdparty/usr/lib/:/usr/local/bin/:/custom-sw/thirdparty/usr/local/ssl/lib/", string command2 = "./netopeer2-cli", string command3 = "connect --host=192.168.1.2 --port=1830 --login=root", string refTag = "Type your password:", string password = "root", string command4 = "listen", string command4RefTag = "cmd_listen: Already connected to", string command5 = "edit-config --target running --config=/root/uplane_test_xml_for_release_2.9.0_ACTIVE_1.xml --defop merge"
         */
        private CCDUServer cCDUServerobj;
        string command1 = "export LD_LIBRARY_PATH=/custom-sw/thirdparty/usr/lib64/:/custom-sw/thirdparty/usr/lib/:/usr/local/bin/:/custom-sw/thirdparty/usr/local/ssl/lib/";
        string command2 = "./netopeer2-cli";
        string command3 = "connect --host=192.168.1.2 --port=1830 --login=root";
        string refTag = "Type your password:";
        string password = "root";
        string command4 = "listen";
        string command4RefTag = "cmd_listen: Already connected to";
        string command5 = "edit-config --target running --config=/root/uplane_test_xml_for_release_2.9.0_ACTIVE_1.xml --defop merge";
        string command6 = "rm -rf /root/.ssh/known_hosts"; // if failed only

        public CCDUServer CCDUServerobj { get => cCDUServerobj; set => cCDUServerobj = value; }
        [Display("CA script1", Order: 1, Description: "Enter CA function script1")]

        public string ExportCommand { get => command1; set => command1 = value; }
        [Display("CA script2", Order: 5, Description: "Enter CA function script2")]

        public string netopeer2_cli_Command2 { get => command2; set => command2 = value; }
        [Display("CA script3", Order: 10, Description: "Enter CA function script3")]
        public string ConnectCommand3 { get => command3; set => command3 = value; }

        [Display("REF Tag", Order: 10, Description: "Enter Reg Tag script3")]
        public string ConsolePasswordPrompt { get => refTag; set => refTag = value; }

        [Display("Password", Order: 10, Description: "Enter Password")]
        public string Password { get => password; set => password = value; }

        [Display("CA script4", Order: 10, Description: "Enter CA function script4")]
        public string Command4 { get => command4; set => command4 = value; }

        [Display("Ref Tag for Command4", Order: 10, Description: " ")]
        public string Command4RefTag { get => command4RefTag; set => command4RefTag = value; }

        [Display("CA script5", Order: 10, Description: "Enter CA function script5")]
        public string Command5 { get => command5; set => command5 = value; }


        [Display("CA script6", Order: 10, Description: "Enter CA function script6")]
        public string Command6 { get => command6; set => command6 = value; }

        public RunCAFunctions()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            bool CAResult = false;
            string readConsole = string.Empty;
            bool warning = false;
            Stopwatch sp = Stopwatch.StartNew();
            try
            {
                CCDUServerobj.ExportCOmmandExecution(ExportCommand, netopeer2_cli_Command2, out readConsole, out sp);
                readConsole = CCDUServerobj.ConnectCommand(ConnectCommand3, ConsolePasswordPrompt, ref warning, sp);
                CCDUServerobj.PasswordIfPrompts(Password, warning, sp);
                readConsole = CCDUServerobj.waitforConsole(command6, sp);
                readConsole = CCDUServerobj.ListenCommand(command4, command4RefTag, sp);
                TapThread.Sleep(1000);
                readConsole = CCDUServerobj.Edit_configCommand(command5, sp);
                if (readConsole.Contains("OK") || readConsole.Contains("0K"))
                {
                    CAResult = true;
                }
                else
                {
                    CAResult = false;
                }

                //CAResult = CCDUServerobj.CarrierAggrigation(ExportCommand, netopeer2_cli_Command2, ConnectCommand3, ConsolePasswordPrompt, Password, Command4, Command4RefTag, Command5, Command6);
            }
            catch (Exception ex)
            {
                Log.Error("Error during Carrier Aggrigation Functions: {0}", ex);
                UpgradeVerdict(Verdict.Error);
            }
            finally
            {
                RunChildSteps(); //Turn off the power supply
            }
            if (CAResult)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, this.Verdict.ToString(),"", CAResult.ToString(),"", "EQ", "TRUE", "Bool");

            //UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("rm -rf /root/.ssh/known_hosts ", Group: "RjioMRU", Description: "Insert a description here")]
    public class RunCAKnownHostCommand : TestStep
    {


        /*
         string command1 = "export LD_LIBRARY_PATH=/custom-sw/thirdparty/usr/lib64/:/custom-sw/thirdparty/usr/lib/:/usr/local/bin/:/custom-sw/thirdparty/usr/local/ssl/lib/", string command2 = "./netopeer2-cli", string command3 = "connect --host=192.168.1.2 --port=1830 --login=root", string refTag = "Type your password:", string password = "root", string command4 = "listen", string command4RefTag = "cmd_listen: Already connected to", string command5 = "edit-config --target running --config=/root/uplane_test_xml_for_release_2.9.0_ACTIVE_1.xml --defop merge"
         */
        private CCDUServer cCDUServerobj;


        public CCDUServer CCDUServerobj { get => cCDUServerobj; set => cCDUServerobj = value; }
        [Display("CA script1", Order: 1, Description: "Enter CA function script1")]


        public RunCAKnownHostCommand()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            CCDUServerobj.CCDU_CA_known_hostsCommand();
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.


            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }

}
