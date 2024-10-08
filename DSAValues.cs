﻿using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RjioMRU
{
    [Display("DSAHexValues", Description: "Add a description here")]
    public class DSACHexValues : ComponentSettings<DSACHexValues>
    {
        private string[] hexValuesCh1 = new string[16] { "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F" };
        private string[] hexValuesCh2 = new string[16] { "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F", "0x7F" };

        [Display("Hex valus for Channel1", Description: "HexValuesCh1")]
        public string[] HexValuesCh1 { get => hexValuesCh1; set => hexValuesCh1 = value; }
        [Display("Hex valus for Channel2", Description: "HexValuesCh2")]
        public string[] HexValuesCh2 { get => hexValuesCh2; set => hexValuesCh2 = value; }
    }
    public class DSAValues
    {
        private string decimalVaues;
        private string hexValues;
        //public string[] DecimalVaues = new string[16] { "31", "31", "31", "31", "31", "31", "31", "31", "31", "31", "31", "31", "31", "31", "31", "31" };
        //public string[] HexValues = new string[16] { "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F" }; 
        //public string DecimalVaues = new string[16] { "31", "31", "31", "31", "31", "31", "31", "31", "31", "31", "31", "31", "31", "31", "31", "31" };
        //public string HexValues = new string[16] { "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F", "1F" };
        public DSAValues(string decimalVaues, string hexValues)
        {
            DecimalVaues = decimalVaues;
            this.HexValues = hexValues;
        }
        public string DecimalVaues { get => decimalVaues; set => decimalVaues = value; }
        public string HexValues { get => hexValues; set => hexValues = value; }
    }
}

