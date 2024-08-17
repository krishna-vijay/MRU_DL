﻿// Author: MyName
// Copyright:   Copyright 2024 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using Microsoft.Office.Interop.Excel;
using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RjioMRU.TestSteps.MESSteps
{
    [Display("Single Serial Fail ", Group: "RjioMRU.TestSteps.MESSteps", Description: "Insert a description here")]
    public class SingleSerialFail : TestStep
    {
        #region Settings
        ClsMES mesResource = null;
       // string serialNumber = "JITSAF1LIMRU00006";
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public SingleSerialFail()
        {
            Input<string> SerialNumber = new Input<string>();
            Input<string> ServerURL = new Input<string>();
            Input<string> ClientID = new Input<string>();
            Input<string> EmployeeID = new Input<string>();
            Input<string> stageID = new Input<string>();
            // ToDo: Set default values for properties / settings.
        }

        public ClsMES MesResource { get => mesResource; set => mesResource = value; }
        Input<string> serialNumber;
        public Input<string> SerialNumber { get => serialNumber; set => serialNumber = value; }

        public Input<string> ServerURL { get; set; } //= "http://42qconduituat2.42-q.com:18003/conduit";
        public Input<string> ClientID { get; set; }// = "p5547dc2_uat";
        public Input<string> EmployeeID { get; set; }// = "62153666";

        public Input<string> stageID { get; set; }


        public override void Run()
        {
            InputBox inputBox = new InputBox("Barcore Reader", "Please SCAN the QR/ Barcode");
            if (inputBox.ShowDialog() == DialogResult.OK)
            {
                string inputValue = inputBox.InputValue;
                
            }
            else
            {
                this.PlanRun.MainThread.Abort();
                Log.Info("User has cancelled the Barcode SCAN's operation");
            }



            var value1 = MesResource.SingleSerialFail(ServerURL.Value, ClientID.Value, EmployeeID.Value, SerialNumber.Value, stageID.Value);
            Log.Info("Single Serial Fail:  " + value1.Result.ToString());
            // ToDo: Add test case code.
            if (value1.Result)
            {
                UpgradeVerdict(Verdict.Pass);
            }
            else
            {
                UpgradeVerdict(Verdict.Fail);
            }
            MES_CSV.UpdateMESCSV_Parametric_List((MES_CSV.GroupName++).ToString(), this.StepRun.TestStepName, Verdict.ToString(), " ", this.Verdict == Verdict.Pass ? "TRUE" : "FALSE", "", "EQ", "TRUE", "Bool");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }

    /*
     return value 
    {"source":{"agent":"p5599dc1uat","client_id":"p5599dc1uat","employee":"62153666","password":"","workstation":{"station":" 539 ","type":"Device"}},"status":{"code":"OK","message":""},"transaction_responses":[{"command_responses":null,"scanned_unit":{"status":{"code":"OK","message":""},"unit":{"part_number":"LFIRIL051-7470089-B_1.3","quantity":"1","revision":"","unit_id":"JITSAF1FKMRU00006"},"unit_info":{"active_hold_type":"","assembly_count":0,"attr_def_count":0,"auto_commands":null,"c_level_key":null,"client_id":"p5599dc1uat","command_template":[],"container_config":null,"container_quantity":0,"container_serial_key":null,"defect_seq":0,"description":"5G NR MRU SKU1 with PDC design RevB_1.3","detailed_unit_set":null,"employee_key":8167,"fail_to_loc_key":null,"finisher_executed":"","item_num":0,"loc_ts":"2024-06-07 16:15:04.548311+05:30","location_key":622,"long_workstation":"5F18 OVER THE AIR TST-1","lot_serial_key":null,"mtl_count":0,"order_line_key":null,"part_family":null,"part_key":1042,"part_number":"LFIRIL051-7470089-B_1.3","pass_fail_seq":1,"pass_to_loc_key":null,"pass_to_location":null,"pass_to_process":null,"process_key":577,"process_name":"018 OVER THE AIR TST","quantity":1,"revision":"","route_key":514,"scanning_location_key":622,"scanning_process_key":577,"scanning_process_name":"018 OVER THE AIR TST","scanning_template":[],"scanning_workstation":"5F18 OVER THE AIR TST-1","serial_key":3596933,"serial_number":"JITSAF1FKMRU00006","sfdc_key":1,"ship_notify_key":null,"shop_order_key":6168,"shop_order_number":"SFDC_VALID-089-B_1.3","short_workstation":"5F18","sit_range_key":null,"unit_elements":{"attributes":[],"comments":[],"components":[],"defects":[]},"unit_set":["JITSAF1FKMRU00006"],"unit_status":"On The Line","unit_status_key":20,"unit_type":0,"uom":null,"user_defined":"","work_order_key":null}},"status":{"code":"OK","message":""}}],"version":"1.0"}

     */
}