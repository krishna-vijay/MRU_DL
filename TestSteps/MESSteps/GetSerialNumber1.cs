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
    [Display("MES Check To Start", Group: "RjioMRU.TestSteps.MESSteps", Description: "Insert a description here")]
    public class MesCheckToStart : TestStep
    {
        #region Settings
        ClsMES mesResource = null;
        string serialNumber = string.Empty;
        string serialNumberByUser = string.Empty;
        string productID = string.Empty;
        string macID = string.Empty;
        string hstbID = string.Empty;
        string rffeID = string.Empty;
        #endregion

        public MesCheckToStart()
        {
            SerialnumberByUser = "JITSAF1LIMRU00006";
        }

        public ClsMES MesResource { get => mesResource; set => mesResource = value; }

        [Display("Enter Serial Number", Order: 1)]
        public string SerialnumberByUser { get => serialNumberByUser; set => serialNumberByUser = value; }

        [Output]
        [Display("Serial Number", Order: 2)]
        public string Serialnumber { get => serialNumber; set => serialNumber = value; }

        [Output]
        [Display("Product Number", Order: 3)]
        public string ProductID { get => productID; set => productID = value; }

        [Output]
        [Display("MAC ID", Order: 4)]
        public string MacID { get => macID; set => macID = value; }

        [Output]
        [Display("HSTB ID", Order: 5)]
        public string HstbID { get => hstbID; set => hstbID = value; }

        [Output]
        [Display("RFFE ID", Order: 6)]
        public string RffeID { get => rffeID; set => rffeID = value; }

        public override void Run()

        {
            try
            {
                InputBox inputBox = new InputBox("Barcore Reader", "Please SCAN the QR/ Barcode");
                if (inputBox.ShowDialog() == DialogResult.OK)
                {
                    string inputValue = inputBox.InputValue;
                    SerialnumberByUser = inputValue;
                }
                else
                {
                    this.PlanRun.MainThread.Abort();
                    Log.Info("User has cancelled the Barcode SCAN's operation");
                }


                var componentDataObj = MesResource.GetMesInformationResponse(SerialnumberByUser);
                Component[] componentArray = new Component[5];

                for (int i = 0; i < componentDataObj.Result.data.Count; i++)
                {
                    componentArray[i] = componentDataObj.Result.data[i];
                }

                Serialnumber = Serialnumber;
                MacID = componentArray[(int)mesSelectoin.MacEnum].component_id.ToString();
                ProductID = componentArray[(int)mesSelectoin.productIDEnum].component_id.ToString();
                HstbID = componentArray[(int)mesSelectoin.hstbEnum].component_id.ToString();
                RffeID = componentArray[(int)mesSelectoin.rffeEnum].component_id.ToString();


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
                MessageBox.Show($"The operator has scanned an incorrect barcode or needs to check the MES database.");
                Log.Error("Error in MesCheckToStart, please check MES Connection: " + ex.Message);
                UpgradeVerdict(Verdict.Error);
            }


            MES_CSV.UpdateMESCSV_Parametric_List(MES_CSV.GroupName, this.StepRun.TestStepName  , Verdict.ToString(),"NA", SerialnumberByUser, "NA", "NA", "NA", "NA");
            RunChildSteps(); //If the step supports child steps.
        }
    }
    public enum mesSelectoin
    {
        MacEnum =0,
        productIDEnum=1,
        hstbEnum=3, rffeEnum=4
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
