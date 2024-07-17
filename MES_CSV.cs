using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2013.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RjioMRU
{
    public static class MES_CSV
    {
        public static List<string> MRU_MES_List = new List<string>();
        public static string GroupName { get; set; }
        public static string Equipment_ID { get; set; }

        public static string Slot { get; set; }
        public static string Credentials { get; set; }
        public static string Operation_Mode  { get; set; }
        public static string SequenceID { get; set;}
        public static string Overall_Defect_Code { get; set; }

        public static string MES_CSV_FilePath { get; set; }

        public static void UpdateMESCSV_Parametric_List(string GroupName, string Test_Step_Name, string Test_Step_Status, string Low_Limit, String Measured_Value, string High_Limit, string Limit_Type, string Expected_Value, string Unit_Of_Measure)
        {
            string str = GroupName + "," +
                Test_Step_Name + "," +
                Test_Step_Status + "," +
                Low_Limit + "," +
                Measured_Value + "," +
                High_Limit + "," +
                Limit_Type + "," +
                Expected_Value + "," +
                Unit_Of_Measure;
            MRU_MES_List.Add(str.ToString());
        }
        public enum LimitType
        {
            EQ, NE, GT, LT, GE, LE, GTLT, GELE, GTLE, GELT,
            LTGT, LEGE, LTGE, LEGT
        }
        public static void UpdateHeader(string SerialNumber, string PartNumber, string Equipment_ID, string Slot, string Credentials, string Result, string Operation_Mode, string Test_Start_DateTime, string Test_Stop_DateTime, string Test_Sequence_ID, string Overall_Defect_Code)
        {
            string str = SerialNumber + "," +
                PartNumber + "," +
                Equipment_ID + "," +
                Slot + "," +
                Credentials + "," +
                Result + "," +
                Operation_Mode + "," +
                Test_Start_DateTime + "," +
                Test_Stop_DateTime + "," +
                Test_Sequence_ID + "," +
                Overall_Defect_Code;

            MRU_MES_List.Insert(0, str.ToString());
        }

        public static void WrteMESCSVFile()
        {
            string csv = string.Join(Environment.NewLine, MRU_MES_List);
            File.WriteAllText(MES_CSV_FilePath, csv);
        }
    }
}
