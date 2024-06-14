using System;
using System.Collections.Generic;
using System.IO;
using OpenTap;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using System.Data.Common;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;


namespace RjioMRU
{
    [Display("Rjio MRU Summary Report", Group: "MRU Test cases", Description: "Insert a description here")]
    public class RjioMRUExcelReportSummary : ResultListener
    {
        #region ReportStaticVariales
        static object printObject = new object();
        static int RowNumber = 0;
        //static Boolean DoSaveReportSUM = false;
        static string currentReportFolderSUM = string.Empty;//This report folder holds the date month and year in ddMMMYYYY as a reporting folder , if assigns the value in preplan run.

        public static string TestStepNameSum;
        //  private string templetepath;
        private string tempFileName;

        #endregion

        #region Settings
        //Microsoft.Office.Interop.Excel.Application xlApp;
        //Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
        //Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;

        private string templetepathSUM;

        string reportName = string.Empty;
        public string ReportName { get => reportName; set => reportName = value; }

        bool creteCSV = false;
        bool useGenericExcel = false;
        bool excel_pdf = false;

        bool generatePDF = false;
        [Display("Create PDF", Group: "Report Details", Description: "If checked generates pdf with all report generated", Order: 1)]
        public bool GeneratePDF { get => generatePDF; set => generatePDF = value; }


        string reportPath = string.Empty;
        [Display("Excel/Pdf report path", Group: "Report Details", Description: "Enter the Excel and Pdf report path", Order: 2)]
        [DirectoryPath()]
        public string ReportPath { get => reportPath; set => reportPath = value; }

        [Display("Excel summary Report Templete", Group: "Report Details", Description: "Select the Summary Report templete", Order: 4)]
        [FilePath(fileExtension: "xlsx")]
        public string TempletepathSUM { get { return templetepathSUM; } set { templetepathSUM = System.IO.Path.GetFullPath(value); } }// = "C:\\Program Files\\Keysight\\Test Automation\\Packages\\Rjio\\Report.xlsx";

        [Display("Create CSV only", Order: 10, Description: "On Selecting this CSV report will only be created")]
        public bool CreateCSV { get => creteCSV; set => creteCSV = value; }

        [Display("Generic Spreadsheet", Order: 15, Description: "")]
        public bool UseGenericExcel { get => useGenericExcel; set => useGenericExcel = value; }

        [Display("Excel format", Order: 20, Description: "")]
        public bool Excel_pdf { get => excel_pdf; set => excel_pdf = value; }

        //public static object PrintObject { get => printObject; set => printObject = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RjioMRUExcelReportSummary()
        {
            Rules.Add(() => !string.IsNullOrEmpty(ReportPath), "Please enter ESD executable location", nameof(ReportPath));
            Rules.Add(() => System.IO.Directory.Exists(ReportPath), "Plese Check the application location", nameof(ReportPath));
            Name = "Rjio Report";
            // ToDo: Set default values for properties / settings.
        }

        //private void WriteReportData(ResultTable result)
        //{
        //    //DoSaveReportSUM = true;
        //    RowNumber = 1;
        //    for (int iteration = 0; iteration < result.Columns.Length; iteration++)
        //    {
        //        switch (result.Columns[iteration].Name)
        //        {
        //            case "Ch1Measurements":
        //            case "Ch2Measurements":
        //            case "Ch3Measurements":
        //            case "Measurements":
        //                int column = 1;
        //                foreach (var item in ((string[])result.Columns[iteration].Data)[0].Split(';'))
        //                {
        //                    foreach (var item1 in item.Split(','))
        //                    {
        //                        xlWorkSheet.Cells[RowNumber, column++] = item1;
        //                    }
        //                    column = 1;
        //                    RowNumber++;
        //                }
        //                break;
        //            default:
        //                xlWorkSheet.Cells[RowNumber, 1] = result.Columns[iteration].Data.GetValue(0);

        //                break;
        //        }
        //        RowNumber++;

        //        ////Cell Color 
        //        //var columnHeadingsRange = xlWorkSheet.Range[xlWorkSheet.Cells[RowNumber, 1], xlWorkSheet.Cells[RowNumber, 2]];

        //        //columnHeadingsRange.Interior.Color = XlRgbColor.rgbSkyBlue;

        //        //columnHeadingsRange.Font.Color = XlRgbColor.rgbWhite;
        //    }



        //    // xlApp.Cells[rowNumber, columnNumber + iterationCOlumn] = result.Columns[iterationCOlumn].Name;
        //}
        public override void OnTestStepRunCompleted(TestStepRun stepRun)
        {

            //add test table header...........................
            //Add handling code for test step run completed.
            //stepRun.TestStepName;
        }
        public override void OnTestPlanRunCompleted(TestPlanRun planRun, Stream logStream)
        {
            //if (Excel_pdf)
            //{


            //    try
            //    {
            //        //xlApp.Quit();
            //        Marshal.ReleaseComObject(xlWorkSheet);
            //        //}
            //        //else
            //        //{
            //        //    xlWorkBook.Close(false, Missing.Value, Missing.Value);
            //        //    xlApp.Quit();
            //        //}
            //        Marshal.ReleaseComObject(xlWorkBook);
            //        //Marshal.ReleaseComObject(xlApp);
            //        GC.Collect();
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Error("Exception thrown while closing report objects." + ex.Message);
            //        throw;
            //    }
            //    GC.Collect();
            //    TapThread.Sleep(1000);
            //}

            //Add handling for test plan run completed.
        }
        public override void Open()
        {
            base.Open();
            //Add resource open code.
        }
        public override void Close()
        {
            //Add resource close code.
            base.Close();
        }
        //public void OnTestPlanRunStartCreateExcelObject()
        //{
        //    var proccessss = Process.GetProcessesByName("Excel"); ;
        //    int proccessCount = proccessss.Length;
        //    if (proccessCount > 1)
        //    {
        //        //foreach (var process in Process.GetProcessesByName("Excel"))
        //        //{
        //        //    process.Kill();

        //        //}
        //        for (int i = 1; i < proccessCount; i++)
        //        {
        //            proccessss[i].Kill();
        //        }
        //        xlApp = (Application)Marshal.GetActiveObject("Excel.Application");
        //    }
        //    else if (proccessCount == 1)
        //    {
        //        xlApp = (Application)Marshal.GetActiveObject("Excel.Application");
        //    }
        //    else
        //    {
        //        xlApp = new Microsoft.Office.Interop.Excel.Application();


        //    }
        //    if (xlApp == null)
        //    {
        //        throw (new Exception("Excel Open Execption"));
        //        //System.Windows.MessageBox.Show("Excel is not properly installed!!");
        //        //return;
        //    }
        //    xlApp.Visible = false;
        //}
        public override void OnTestStepRunStart(TestStepRun stepRun)
        {
            TestStepNameSum = stepRun.TestStepName;
        }
        public override void OnResultPublished(Guid stepRun, ResultTable result)
        {
            try
            {


                if (CreateCSV)
                {
                    Log.Info("Report Generation CSV Called");
                    WriteToCSV(result);
                }
                //if (Excel_pdf)
                //{
                //    Log.Info("Reprot generation Excel called");
                //    OnTestPlanRunStartCreateExcelObject();
                //    MruCCDUServerClient(result);

                //}
                if (UseGenericExcel)
                {
                    Log.Info("Report generatin Spread sheet lignt called:");
                    WriteToSpreadSheetLight(result);
                }
            }
            catch (Exception ex)
            {
                Log.Info("Report generation Error:" + ex.Message);

            }

        }

        private void WriteToSpreadSheetLight(ResultTable result)
        {
            try
            {


                using (SLDocument sl = new SLDocument(TempletepathSUM, "Sheet1"))
                {



                    RowNumber = 1;
                    for (int iteration = 0; iteration < result.Columns.Length; iteration++)
                    {
                        switch (result.Columns[iteration].Name)
                        {
                            case "Ch1Measurements":
                            case "Ch2Measurements":
                            case "Ch3Measurements":
                            case "Measurements":
                                int column = 1;
                                foreach (var item in ((string[])result.Columns[iteration].Data)[0].Split(';'))
                                {
                                    foreach (var item1 in item.Split(','))
                                    {
                                        // xlWorkSheet.Cells[RowNumber, column++] = item1;
                                        sl.SetCellValue(RowNumber, column++, item1);
                                    }
                                    column = 1;
                                    RowNumber++;
                                }
                                break;
                            default:
                                try
                                {
                                    if (result.Columns[iteration].Data.GetValue(0) != null)
                                    {
                                        sl.SetCellValue(RowNumber, 1, result.Columns[iteration].Data.GetValue(0).ToString());
                                    }
                                    else
                                    {
                                        sl.SetCellValue(RowNumber, 1, string.Empty);
                                    }
                                }
                                catch (Exception)
                                {


                                }




                                break;
                        }
                        RowNumber++;
                    }
                    sl.SaveAs(Path.Combine(ReportPath, (string.IsNullOrEmpty(ReportName) ? "" : ReportName) + "_" + DateTime.Now.ToShortDateString().Replace('/', '_') + "-" + DateTime.Now.ToLongTimeString().Replace(':', '_') + result.Name) + ".xlsx");
                    Log.Info("Report saved in :" + Path.Combine(ReportPath, (string.IsNullOrEmpty(ReportName) ? "" : ReportName) + "_" + DateTime.Now.ToShortDateString().Replace('/', '_') + "-" + DateTime.Now.ToLongTimeString().Replace(':', '_') + result.Name) + ".xlsx");
                }

            }
            catch (Exception ex)
            {
                Log.Error("Report Generation Error :" + ex.Message);

            }
        }

        private void WriteToCSV(ResultTable result)
        {
            for (int iteration = 0; iteration < result.Columns.Length; iteration++)
            {
                switch (result.Columns[iteration].Name)
                {
                    case "Ch1Measurements":
                    case "Ch2Measurements":
                    case "Ch3Measurements":
                    case "Measurements":
                        int column = 1;
                        foreach (var item in ((string[])result.Columns[iteration].Data)[0].Split(';'))
                        {
                            File.WriteAllText(Path.Combine(ReportPath, (string.IsNullOrEmpty(ReportName) ? "" : ReportName) + "_" + DateTime.Now.ToShortDateString().Replace('/', '_') + "-" + DateTime.Now.ToLongTimeString().Replace(':', '_') + result.Name) + ".csv", item + Environment.NewLine);
                            //column = 1;
                            // RowNumber++;
                        }
                        break;
                    default:
                        // xlWorkSheet.Cells[RowNumber, 1] = result.Columns[iteration].Data.GetValue(0);

                        break;
                }
                RowNumber++;

                ////Cell Color 
                //var columnHeadingsRange = xlWorkSheet.Range[xlWorkSheet.Cells[RowNumber, 1], xlWorkSheet.Cells[RowNumber, 2]];

                //columnHeadingsRange.Interior.Color = XlRgbColor.rgbSkyBlue;

                //columnHeadingsRange.Font.Color = XlRgbColor.rgbWhite;
            }

        }

        //private void MruCCDUServerClient(ResultTable result)
        //{
        //    lock (printObject)
        //    {



        //        //if (xlApp == null)
        //        //{
        //        //    //System.Windows.MessageBox.Show("Excel is not properly installed!!");
        //        //    return;
        //        //}
        //        //xlApp.Visible = false;
        //        tempFileName = Path.Combine(Path.GetTempPath() + DateTime.Now.ToLongTimeString().Replace(':', '_') + "_" + Path.GetFileName(TempletepathSUM));
        //        if (File.Exists(tempFileName))
        //        {
        //            File.Delete(tempFileName);
        //        }
        //        File.Copy(TempletepathSUM, tempFileName);
        //        xlWorkBook = xlApp.Workbooks.Open(tempFileName);
        //        // xlWorkBook = xlApp.Workbooks.Open(TempletepathSUM);
        //        xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2);
        //        RowNumber++;
        //        WriteReportData(result);
        //        try
        //        {
        //            if (xlWorkBook == null || xlWorkSheet == null)
        //            {
        //                //xlApp.Quit();
        //                //Marshal.ReleaseComObject(xlApp);
        //                //GC.Collect();
        //                return;
        //            }
        //            else
        //            {
        //                xlWorkBook.SaveAs(Path.Combine(ReportPath, (string.IsNullOrEmpty(ReportName) ? "" : ReportName) + "_" + DateTime.Now.ToShortDateString().Replace('/', '_') + "-" + DateTime.Now.ToLongTimeString().Replace(':', '_') + result.Name) + ".xlsx", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        //                Log.Info("Report saved at : " + Path.Combine(ReportPath, (string.IsNullOrEmpty(ReportName) ? "" : ReportName) + "_" + DateTime.Now.ToShortDateString().Replace('/', '_') + "-" + DateTime.Now.ToLongTimeString().Replace(':', '_') + result.Name) + ".xlsx");
        //                if (GeneratePDF)
        //                {
        //                    xlWorkBook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, Path.Combine(ReportPath, (string.IsNullOrEmpty(ReportName) ? "" : ReportName) + "_" + DateTime.Now.ToShortDateString().Replace('/', '_') + "-" + DateTime.Now.ToLongTimeString().Replace(':', '_')));
        //                }
        //                xlWorkBook.Close(true, Missing.Value, Missing.Value);
        //            }
        //            //if (DoSaveReportSUM)
        //            //{

        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error("Exception thrown while closing report objects." + ex.Message);
        //            throw;
        //        }

        //        // TapThread.Sleep(100);
        //    }
        //}
    }


}
