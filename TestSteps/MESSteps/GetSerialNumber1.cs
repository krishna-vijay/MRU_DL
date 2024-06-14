// Author: MyName
// Copyright:   Copyright 2024 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using Newtonsoft.Json;
using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RjioMRU.TestSteps.MESSteps
{
    [Display("GetSerialNumber1", Group: "RjioMRU.TestSteps.MESSteps", Description: "Insert a description here")]
    public class GetSerialNumber1 : TestStep
    {
        #region Settings
        ClsMES _MESSerialNumber = null;
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public GetSerialNumber1()
        {
            // ToDo: Set default values for properties / settings.
        }

        public ClsMES MESSerialNumber { get => _MESSerialNumber; set => _MESSerialNumber = value; }

        public override void Run()
        {
            var serialnumber = MESSerialNumber.GetSerialNumber1();
            Log.Info(serialnumber.Result);

            string json = serialnumber.Result;
            Root root = JsonConvert.DeserializeObject<Root>(json);

            foreach (var component in root.data)
            {
                Console.WriteLine($"Serial Number: {component.serial_number}");
                Console.WriteLine($"Parent Part Number: {component.parent_part_number}");
                Console.WriteLine($"Component ID: {component.component_id}");
            }

            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
        public class Component
        {
            public int serial_key { get; set; }
            public string serial_number { get; set; }
            public int part_key { get; set; }
            public string parent_part_number { get; set; }
            public string component_id { get; set; }
            // ... other properties
        }

        public class Root
        {
            public List<Component> data { get; set; }
        }
    }
}
