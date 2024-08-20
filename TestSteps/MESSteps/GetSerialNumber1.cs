// Author: MyName
// Copyright:   Copyright 2024 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using Newtonsoft.Json;
using OpenTap;
using RjioMRU.MES;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RjioMRU
{
    [Display("MES PRODUCT INFORMATION", Group: "RjioMRU.TestSteps.MESSteps", Description: "Insert a description here")]
    public class MesCheckToStart : TestStep
    {
        #region Settings
        ClsMES mesResource = null;
        Input<string> serialNumber;//= string.Empty;
        //string serialNumberByUser = string.Empty;
        string productID = string.Empty;
        string macID = string.Empty;
        string pcbSerialNumber_HSTB = string.Empty;
        string rfbSerialNumber = string.Empty;
        #endregion

        public MesCheckToStart()
        {
            //SerialnumberByUser = "JITSAF1LIMRU00006";
            Serialnumber = new Input<string>();
        }

        public ClsMES MesResource { get => mesResource; set => mesResource = value; }

        [Output]
        [Display("Serial Number", Order: 2)]
        public Input<string> Serialnumber { get => serialNumber; set => serialNumber = value; }

        [Output]
        [Display("MAC ID", Order: 4)]
        public string MacID { get => macID; set => macID = value; }

        [Output]
        [Display("Product id", Order: 6)]
        public string ProductID { get => productID; set => productID = value; }

        [Output]
        [Display("PCB Serial Number(HSTB)", Order: 8)]
        public string PCBSerialNumber_HSTB { get => pcbSerialNumber_HSTB; set => pcbSerialNumber_HSTB = value; }

        [Output]
        [Display("RFB Serial Number", Order: 10)]
        public string RFBSerialNumber { get => rfbSerialNumber; set => rfbSerialNumber = value; }

        public override void Run()

        {
            try
            {
                var componentDataObj = MesResource.GetMesInformationResponse(Serialnumber.Value);
                Component[] componentArray = new Component[5];

                for (int i = 0; i < componentDataObj.Result.data.Count; i++)
                {
                    componentArray[i] = componentDataObj.Result.data[i];
                }

                MacID = componentArray[(int)mesSelectoin.MacEnum].component_id.ToString();
                ProductID = componentArray[(int)mesSelectoin.productIDEnum].component_id.ToString();
                pcbSerialNumber_HSTB = componentArray[(int)mesSelectoin.PCBSerialNumber_hstbEnum].component_id.ToString();
                RFBSerialNumber = componentArray[(int)mesSelectoin.rffeEnum].component_id.ToString();


                if (componentDataObj.Result.success)
                {
                    UpgradeVerdict(Verdict.Pass);
                }
                else
                {
                    UpgradeVerdict(Verdict.Fail);
                    this.PlanRun.MainThread.Abort();
                }
            }

            catch (Exception ex)
            {

                Log.Error("Error in MesCheckToStart, please check MES Connection: " + ex.Message);
                UpgradeVerdict(Verdict.Error);
            }

            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", serialNumber.Value, " ", "EQ", serialNumber.Value, "Bool");
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", MacID, " ", "EQ", MacID, "Bool");
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", ProductID, " ", "EQ", ProductID, "Bool");
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", pcbSerialNumber_HSTB, " ", "EQ", pcbSerialNumber_HSTB, "Bool");
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", RFBSerialNumber, " ", "EQ", RFBSerialNumber, "Bool");
            RunChildSteps(); //If the step supports child steps.
        }
    }
    public enum mesSelectoin
    {
        MacEnum = 0,
        productIDEnum = 1,
        PCBSerialNumber_hstbEnum = 3, rffeEnum = 4
    }
}

public class InputBox : Form
{
    private TextBox textBox;
    private Button okButton;

    public string InputValue => textBox.Text;

    public InputBox(string title, string prompt)
    {
        InitializeComponents(title, prompt);
    }

    private void InitializeComponents(string title, string prompt)
    {
        this.BringToFront();
        this.TopMost = true;
        this.WindowState = FormWindowState.Normal;
        this.TopLevel = true;
        this.Activate();
        this.Focus();
        //textBox.Select();
        this.Text = title;
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Size = new System.Drawing.Size(400, 250);

        Label promptLabel = new Label() { Left = 20, Top = 20, Text = prompt, AutoSize = true };
        this.Controls.Add(promptLabel);
        textBox = new TextBox() { Left = 20, Top = 40, Width = 240 };
        this.Controls.Add(textBox);
        textBox.Select();
        okButton = new Button() { Text = "OK", Left = 190, Width = 70, Top = 70, DialogResult = DialogResult.OK };
        this.Controls.Add(okButton);
        this.AcceptButton = okButton;
    }
}
