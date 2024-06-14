// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;

using DocumentFormat.OpenXml.Spreadsheet;

namespace RjioMRU
{
    //[Display("TrailSpreadSheetLightEx", Group: "RjioMRU", Description: "Insert a description here")]
    //public class TrailSpreadSheetLightEx : TestStep
    //{
    //    #region Settings
    //    // ToDo: Add property here for each parameter the end user should be able to change
    //    #endregion

    //    public TrailSpreadSheetLightEx()
    //    {
    //        // ToDo: Set default values for properties / settings.
    //    }

    //    public override void Run()
    //    {

    //        SLDocument sl = new SLDocument("C:\\Program Files\\Keysight\\Test Automation\\Packages\\Debug\\MRUReportTemplete.xlsx", "Sheet1");

    //        // set a boolean at "A1"
    //        sl.SetCellValue("A1", true);

    //        // set at row 2, columns 1 through 20, a value that's equal to the column index
    //        for (int i = 1; i <= 30; ++i)
    //            for (int j = 1; j < 30; j++)
    //            {
    //                sl.SetCellValue(i, j, (i*j).ToString());
    //            }


    //        // set the value of PI
    //        sl.SetCellValue("B3", 3.14159);

    //        // set the value of PI at row 4, column 2 (or "B4") in string form.
    //        // use this when you already have numeric data in string form and don't
    //        // want to parse it to a double or float variable type
    //        // and then set it as a value.
    //        // Note that "3,14159" is invalid. Excel (or Open XML) stores numerals in
    //        // invariant culture mode. Frankly, even "1,234,567.89" is invalid because
    //        // of the comma. If you can assign it in code, then it's fine, like so:
    //        // double fTemp = 1234567.89;
    //        sl.SetCellValueNumeric(4, 2, "3.14159");

    //        // normal string data
    //        sl.SetCellValue("C6", "This is at C6!");

    //        // typical XML-invalid characters are taken care of,
    //        // in particular the & and < and >
    //        sl.SetCellValue("I6", "Dinner & Dance costs < $10");

    //        // this sets a cell formula
    //        // Note that if you want to set a string that starts with the equal sign,
    //        // but is not a formula, prepend a single quote.
    //        // For example, "'==" will display 2 equal signs
    //        sl.SetCellValue(7, 3, "=SUM(A2:T2)");

    //        // if you need cell references and cell ranges *really* badly, consider the SLConvert class.
    //        sl.SetCellValue(SLConvert.ToCellReference(7, 4), string.Format("=SUM({0})", SLConvert.ToCellRange(2, 1, 2, 20)));

    //        // dates need the format code to be displayed as the typical date.
    //        // Otherwise it just looks like a floating point number.
    //        sl.SetCellValue("C8", new DateTime(3141, 5, 9));
    //        SLStyle style = sl.CreateStyle();
    //        style.FormatCode = "d-mmm-yyyy";
    //        sl.SetCellStyle("C8", style);

    //        sl.SetCellValue(8, 6, "I predict this to be a significant date. Why, I do not know...");

    //        sl.SetCellValue(9, 4, 456.123789);
    //        // we don't have to create a new SLStyle because
    //        // we only used the FormatCode property
    //        style.FormatCode = "0.000%";
    //        sl.SetCellStyle(9, 4, style);

    //        sl.SetCellValue(9, 6, "Perhaps a phenomenal growth in something?");

    //        sl.SaveAs("HelloWorld" + DateTime.Now.ToShortTimeString().Replace(':', '_') + ".xlsx");
    //        // ToDo: Add test case code.
    //        RunChildSteps(); //If the step supports child steps.

    //        // If no verdict is used, the verdict will default to NotSet.
    //        // You can change the verdict using UpgradeVerdict() as shown below.
    //        // UpgradeVerdict(Verdict.Pass);
    //    }
    //}
}
