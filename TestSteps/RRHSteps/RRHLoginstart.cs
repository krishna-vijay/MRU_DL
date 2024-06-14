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
using RjioODSC;

namespace RjioODSC
{
    [Display("RRHLoginstart", Group: "RjioODSC", Description: "Insert a description here")]
    public class RRHLoginstart : TestStep
    {
        #region Settings
        RRH_DUT rrh_DUT;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RRHLoginstart()
        {
            Rrh_DUT = new RRH_DUT();
            // ToDo: Set default values for properties / settings.
        }
        [Display("RRH DUT")]
        public RRH_DUT Rrh_DUT { get => rrh_DUT; set => rrh_DUT = value; }

        public override void Run()
        {
            Rrh_DUT.loginStart();
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("RRH Execute script", Group: "RjioODSC", Description: "Insert a description here")]
    public class RRHExecuteScript : TestStep
    {
        #region Settings
        RRH_DUT rrh_DUT;
        // ToDo: Add property here for each parameter the end user should be able to change
        string sendScript;
        string compareString;
        #endregion

        public RRHExecuteScript()
        {
            Rrh_DUT = new RRH_DUT();
            // ToDo: Set default values for properties / settings.
        }
        [Display("RRH DUT")]
        public RRH_DUT Rrh_DUT { get => rrh_DUT; set => rrh_DUT = value; }
        [Display("Send Script",Order:1,Description:"Enter the command to ODSC")]
        public string SendScript { get => sendScript; set => sendScript = value; }
        [Display("Compare String",Description:"Enter the string to end the command execution",Order:5)]
        public string CompareString { get => compareString; set => compareString = value; }

        public override void Run()
        {
            Rrh_DUT.executeScript(SendScript, CompareString);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("Set RRH Frequency", Group: "RjioODSC", Description: "Insert a description here")]
    public class SetRRHFrequency : TestStep
    {
        #region Settings
        double dutFrequency;
        RRH_DUT rrh_DUT;
        int timeOut = 300;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SetRRHFrequency()
        {
            Rrh_DUT = new RRH_DUT();
            // ToDo: Set default values for properties / settings.
        }
        [Display("Select RRH Port", Description: "Select RRH Port number", Order: 1)]
        public RRH_DUT Rrh_DUT { get => rrh_DUT; set => rrh_DUT = value; }

        [Display("DUT Frequency", Description: "Enter DUT Frequency In MHz", Order: 2)]
        [Unit("Hz", true)]
        public double DutFrequency { get => dutFrequency; set => dutFrequency = value; }


        [Display("Timeout in seconds", Description: "Enter timeout in seconds")]
        [Unit("S")]
        public int TimeOut { get => timeOut; set => timeOut = value; }


        public override void Run()
        {
            Rrh_DUT.SetFrequency(DutFrequency / 1000, TimeOut);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("Set RRH Power", Group: "RjioODSC", Description: "Insert a description here")]
    public class SetRRHPower : TestStep
    {
        #region Settings
        double dutPower = 460;
        int antPort = 0;
        int antPath = 0;
        RRH_DUT rrh_DUT;
        int timeOut = 300;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SetRRHPower()
        {
            Rrh_DUT = new RRH_DUT();
            // ToDo: Set default values for properties / settings.
        }
        [Display("Select RRH Port", Description: "Select RRH Port number", Order: 1)]
        public RRH_DUT Rrh_DUT { get => rrh_DUT; set => rrh_DUT = value; }

        [Display("Antenna Port", Description: "Enter antenna port 0 or 1", Order: 2)]
        public int AntPort { get => antPort; set => antPort = value; }


        [Display("Antenna Path", Description: "Enter antenna Path 0 to 3", Order: 3)]
        public int AntPath { get => antPath; set => antPath = value; }


        [Display("DUT Power", Description: "Enter DUT Power In dBm", Order: 4)]
        [Unit("dBm", true)]
        public double DutPower { get => dutPower; set => dutPower = value; }


        [Display("Timeout in seconds", Description: "Enter timeout in seconds", Order: 5)]
        [Unit("S")]
        public int TimeOut { get => timeOut; set => timeOut = value; }


        public override void Run()
        {
            Rrh_DUT.SetPower(AntPort, AntPath, DutPower, TimeOut);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("Set RRH Tx Start", Group: "RjioODSC", Description: "Insert a description here")]
    public class SetRRHTxStart : TestStep
    {
        #region Settings

        int antPort = 0;
        int antPath = 0;
        RRH_DUT rrh_DUT;
        int timeOut = 300;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SetRRHTxStart()
        {
            Rrh_DUT = new RRH_DUT();
            // ToDo: Set default values for properties / settings.
        }
        [Display("Select RRH Port", Description: "Select RRH Port number", Order: 1)]
        public RRH_DUT Rrh_DUT { get => rrh_DUT; set => rrh_DUT = value; }

        [Display("Antenna Port", Description: "Enter antenna port 0 or 1", Order: 2)]
        public int AntPort { get => antPort; set => antPort = value; }


        [Display("Antenna Path", Description: "Enter antenna Path 0 to 3", Order: 3)]
        public int AntPath { get => antPath; set => antPath = value; }





        [Display("Timeout in seconds", Description: "Enter timeout in seconds", Order: 5)]
        [Unit("S")]
        public int TimeOut { get => timeOut; set => timeOut = value; }


        public override void Run()
        {
            Rrh_DUT.SetTxStart(AntPort, AntPath, TimeOut);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Set RRH Tx All Stop", Group: "RjioODSC", Description: "Insert a description here")]
    public class SetRRHTxAllStop : TestStep
    {
        #region Settings

        int antPort = 0;
        // int antPath = 0;
        RRH_DUT rrh_DUT;
        int timeOut = 300;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SetRRHTxAllStop()
        {
            Rrh_DUT = new RRH_DUT();
            // ToDo: Set default values for properties / settings.
        }
        [Display("Select RRH ", Description: "Select RRH Port number", Order: 1)]
        public RRH_DUT Rrh_DUT { get => rrh_DUT; set => rrh_DUT = value; }

        [Display("Antenna Port", Description: "Enter antenna port 0 or 1", Order: 2)]
        public int AntPort { get => antPort; set => antPort = value; }



        [Display("Timeout in seconds", Description: "Enter timeout in seconds", Order: 5)]
        [Unit("S")]
        public int TimeOut { get => timeOut; set => timeOut = value; }


        public override void Run()
        {
            Rrh_DUT.SetTxAllStop(AntPort, TimeOut);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("Set RRH Tx All Start", Group: "RjioODSC", Description: "Insert a description here")]
    public class SetRRHTxAllStart : TestStep
    {
        #region Settings

        int antPort = 0;
        // int antPath = 0;
        RRH_DUT rrh_DUT;
        int timeOut = 300;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SetRRHTxAllStart()
        {
            Rrh_DUT = new RRH_DUT();
            // ToDo: Set default values for properties / settings.
        }
        [Display("Select RRH ", Description: "Select RRH Port number", Order: 1)]
        public RRH_DUT Rrh_DUT { get => rrh_DUT; set => rrh_DUT = value; }

        [Display("Antenna Port", Description: "Enter antenna port 0 or 1", Order: 2)]
        public int AntPort { get => antPort; set => antPort = value; }



        [Display("Timeout in seconds", Description: "Enter timeout in seconds", Order: 5)]
        [Unit("S")]
        public int TimeOut { get => timeOut; set => timeOut = value; }


        public override void Run()
        {
            Rrh_DUT.SetTxAllStart(AntPort, TimeOut);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("RRH AdrvresetDpd", Group: "RjioODSC", Description: "Insert a description here")]
    public class SetRRHAdrvrsetDpd : TestStep
    {
        #region Settings
        //double dutPower;
        //int antPort = 0;
        int antPath = 0;
        RRH_DUT rrh_DUT;
        int timeOut = 300;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SetRRHAdrvrsetDpd()
        {
            Rrh_DUT = new RRH_DUT();
            // ToDo: Set default values for properties / settings.
        }
        [Display("Select RRH Port ", Description: "Select RRH Port number", Order: 1)]
        public RRH_DUT Rrh_DUT { get => rrh_DUT; set => rrh_DUT = value; }

        [Display("Antenna Path", Description: "Enter antenna Path 0 to 3", Order: 3)]
        public int AntPath { get => antPath; set => antPath = value; }

        [Display("Timeout in seconds", Description: "Enter timeout in seconds", Order: 5)]
        [Unit("S")]
        public int TimeOut { get => timeOut; set => timeOut = value; }


        public override void Run()
        {
            Rrh_DUT.SetAdrvresetDpd(AntPath);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("RRH LNA ON/OFF", Group: "RjioODSC", Description: "Insert a description here")]
    public class SetRRHLnaOn: TestStep
    {
        #region Settings
        bool lnaOn = true;
     
        RRH_DUT rrh_DUT;
        int timeOut = 300;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SetRRHLnaOn()
        {
            Rrh_DUT = new RRH_DUT();
            // ToDo: Set default values for properties / settings.
        }
        [Display("Select RRH Port ", Description: "Select RRH Port number", Order: 1)]
        public RRH_DUT Rrh_DUT { get => rrh_DUT; set => rrh_DUT = value; }
       

        [Display("LNA On?",Description:"Check to On LNA",Order:5)]
        public bool LnaOn { get => lnaOn; set => lnaOn = value; }



        public override void Run()
        {
            Rrh_DUT.SetLNAOnOff(LnaOn);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }


    [Display("RRH PAM ON/OFF", Group: "RjioODSC", Description: "Insert a description here")]
    public class SetRRHPAMOn : TestStep
    {
        #region Settings
        bool pamOn = true;

        RRH_DUT rrh_DUT;
        int timeOut = 300;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SetRRHPAMOn()
        {
            Rrh_DUT = new RRH_DUT();
            // ToDo: Set default values for properties / settings.
        }
        [Display("Select RRH Port ", Description: "Select RRH Port number", Order: 1)]
        public RRH_DUT Rrh_DUT { get => rrh_DUT; set => rrh_DUT = value; }


        [Display("PAM On?", Description: "Check to On LNA", Order: 5)]
        public bool PAMOn { get => pamOn; set => pamOn = value; }



        public override void Run()
        {
            Rrh_DUT.SetPAMOnOff(PAMOn);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }



    [Display("RRH Information", Group: "RjioODSC", Description: "Insert a description here")]
    public class RRHInformation: TestStep
    {
        #region Settings
        

        
 
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RRHInformation()
        {
           
            // ToDo: Set default values for properties / settings.
        }
       


        


        public override void Run()
        {

           
            
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
    [Display("RRH Report Header Details", Group: "RjioODSC", Description: "Insert a description here")]
    public class ReportHeaderDetails : TestStep
    {
        #region Settings
        RRH_DUT rrh_DUT;
        Reporting.TejasReportHeader reportHeaderDetail;

        [Display("Select RRH Port ", Description: "Select RRH Port number", Order: 1)]
        public RRH_DUT Rrh_DUT { get => rrh_DUT; set => rrh_DUT = value; }

        string swoNumber = "TEJSWO222300188-TCS";
        [Display("SWO Number", Description: "Please Enter SWO Number", Order: 2)]
        public string SwoNumber { get => swoNumber; set => swoNumber = value; }

        string rRHPartNumber = string.Empty;
        private string RRHPartNumber { get => rRHPartNumber; set => rRHPartNumber = value; }
        string rRHCardNumber = string.Empty;
        private string RRHCardNumber { get => rRHCardNumber; set => rRHCardNumber = value; }

        string rfPCPartNumber = string.Empty;
        private string RfPCPartNumber { get => rfPCPartNumber; set => rfPCPartNumber = value; }
        string rfPCCardNumber = string.Empty;
        private string RfPCCardNumber { get => rfPCCardNumber; set => rfPCCardNumber = value; }



        string pam1PartNumber = string.Empty;
        private string Pam1PartNumber { get => pam1PartNumber; set => pam1PartNumber = value; }
        string pam1CardNumber = string.Empty;
        private string Pam1CardNumber { get => pam1CardNumber; set => pam1CardNumber = value; }



        string pam2PartNumber = string.Empty;
        private string Pam2PartNumber { get => pam2PartNumber; set => pam2PartNumber = value; }
        string pam2CardNumber = string.Empty;
        private string Pam2CardNumber { get => pam2CardNumber; set => pam2CardNumber = value; }



        string pam3PartNumber = string.Empty;
        private string Pam3PartNumber { get => pam3PartNumber; set => pam3PartNumber = value; }
        string pam3CardNumber = string.Empty;
        private string Pam3CardNumber { get => pam3CardNumber; set => pam3CardNumber = value; }



        string pam4PartNumber = string.Empty;
        private string Pam4PartNumber { get => pam4PartNumber; set => pam4PartNumber = value; }
        string pam4CardNumber = string.Empty;
        private string Pam4CardNumber { get => pam4CardNumber; set => pam4CardNumber = value; }


        string lnaPartNumber = string.Empty;
        private string LnaPartNumber { get => lnaPartNumber; set => lnaPartNumber = value; }

        string lnaCardNumber = string.Empty;
        private string LnaCardNumber { get => lnaCardNumber; set => lnaCardNumber = value; }

        string softwareVersion = string.Empty;
        private string SoftwareVersion { get => softwareVersion; set => softwareVersion = value; }

        string operatorName = string.Empty;
        string approverName = string.Empty;
        string testedDate = string.Empty;
        private string TestedDate { get => testedDate; set => testedDate = value; }

        string approvedDate = string.Empty;
        string testStation = string.Empty;
        string testLocation = string.Empty;

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public ReportHeaderDetails()
        {
            Rrh_DUT = new RRH_DUT();
            reportHeaderDetail = new Reporting.TejasReportHeader();
            // ToDo: Set default values for properties / settings.
        }

        

        [Display("Operator Name", Description: "Please enter Operator Name", Order: 5)]
        public string OperatorName { get => operatorName; set => operatorName = value; }
        [Display("Approver Name", Description: "Please enter Approver Name", Order: 10)]
        public string ApproverName { get => approverName; set => approverName = value; }

        [Display("Approved date", Description: "Please enter Approved date", Order: 15)]

        public string ApprovedDate { get => approvedDate; set => approvedDate = value; }

        [Display("Test Station", Description: "Please enter test station name", Order: 25)]
        public string TestStation { get => testStation; set => testStation = value; }

        [Display("Test Location", Description: "Pleaes enter test location", Order: 25)]

        public string TestLocation { get => testLocation; set => testLocation = value; }
       
        

        public override void Run()
        {

            var rrhInfo = Rrh_DUT.GetRRHInformations();
            foreach (var rrh in rrhInfo.Split('\n'))
            {
                if (rrh.Contains("RFPC Part Number         :"))
                {
                    RfPCPartNumber = rrh.Substring("RFPC Part Number         :".Length + 1).Trim();
                }
                if (rrh.Contains("RFPC Serial Number       :"))
                {
                    RfPCCardNumber = rrh.Substring("RFPC Serial Number       :".Length + 1);
                }
                if (rrh.Contains("SW version	:")) 
                {
                    SoftwareVersion = rrh.Substring("SW version	:".Length + 1);
                }
                if (rrh.Contains("Comp Serial Number       :")){
                    RRHCardNumber = rrh.Substring("Comp Serial Number       :".Length + 1);
                }
                if (rrh.Contains("Comp Part Number         :"))
                {
                    RRHPartNumber = rrh.Substring("Comp Part Number         :".Length + 1).Trim();
                }

                if (rrh.Contains("PAM 0 Serial Number	:")) 
                {
                    Pam1CardNumber = rrh.Substring("PAM 0 Serial Number	:".Length + 1);
                }

                if (rrh.Contains("PAM 0 TPN		:"))
                {
                    Pam1PartNumber = rrh.Substring("PAM 0 TPN		:".Length + 1).Trim();
                }

                if (rrh.Contains("PAM 1 Serial Number	:"))
                {
                    Pam2CardNumber = rrh.Substring("PAM 1 Serial Number	:".Length + 1);
                }

                if (rrh.Contains("PAM 1 TPN		:"))
                {
                    Pam2PartNumber = rrh.Substring("PAM 1 TPN		:".Length + 1).Trim();
                }



                if (rrh.Contains("PAM 2 Serial Number     :"))
                {
                    pam3CardNumber = rrh.Substring("PAM 2 Serial Number     :".Length + 1);
                }

                if (rrh.Contains("PAM 2 TPN               :"))
                {
                    Pam3PartNumber = rrh.Substring("PAM 2 TPN               :".Length + 1);
                }

                if (rrh.Contains("PAM 3 Serial Number     :"))
                {
                    pam4CardNumber = rrh.Substring("PAM 3 Serial Number     :".Length + 1);
                }

                if (rrh.Contains("PAM 3 TPN               :"))
                {
                    Pam4PartNumber = rrh.Substring("PAM 3 TPN               :".Length + 1);
                }

                if (rrh.Contains("LNA   Serial Number	:"))
                {
                    LnaCardNumber = rrh.Substring("LNA   Serial Number	:".Length + 1);
                }

                if (rrh.Contains("LNA   TPN		:"))
                {
                    LnaPartNumber = rrh.Substring("LNA   TPN		:".Length + 1).Trim();
                }

                Log.Info(rrh);
            }
            reportHeaderDetail.SwoNumber = SwoNumber;
            reportHeaderDetail.TestLocation = TestLocation;

            reportHeaderDetail.TejasPartNumberRRH = RRHPartNumber;
            reportHeaderDetail.TejasCARDNumberRRH = RRHCardNumber;

            // reportHeaderDetail.TejasPartNuberRFPC = RfPCNumber;
            reportHeaderDetail.TejasPartNumberPAM1 = Pam1PartNumber;
            reportHeaderDetail.TejasCardNumberPAM1 = Pam1CardNumber;

            reportHeaderDetail.TejasPartNumberPAM2 = Pam2PartNumber;
            reportHeaderDetail.TejasCardNumberPAM2 = Pam2CardNumber;


            reportHeaderDetail.TejasPartNumberPAM3 = Pam3PartNumber;
            reportHeaderDetail.TejasCardNumberPAM3 = Pam3CardNumber;

            reportHeaderDetail.TejasPartNumberPAM4 = Pam4PartNumber;
            reportHeaderDetail.TejasCardNumberPAM4 = pam4CardNumber;

            reportHeaderDetail.TejasPartNumberLNA = LnaPartNumber;
            reportHeaderDetail.TejasCardNumberLNA = LnaCardNumber;

            reportHeaderDetail.SoftwaareVersion = SoftwareVersion;
            reportHeaderDetail.OperatorName = OperatorName;
            reportHeaderDetail.ShipmentApprover = ApproverName;
            reportHeaderDetail.TestedDate = TestedDate;
            reportHeaderDetail.ApprovedDate = ApprovedDate;
            reportHeaderDetail.TestStationName = TestStation;

            Results.Publish(reportHeaderDetail);
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
