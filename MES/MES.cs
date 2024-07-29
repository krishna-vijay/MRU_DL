using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using OpenTap;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Diagnostics;
using System.Net.Http.Headers;
using DocumentFormat.OpenXml.InkML;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace RjioMRU
{
    [Display("MES", Group: "RjioMRU", Description: "Insert a description here")]
    public class ClsMES : ScpiInstrument
    {
        #region Settings
        public static bool isMESConnectionDone = false;
        HttpClientHandler handler;
        HttpClient client = null;

        string mESInfoUrl;
        string authenticationEncoded = string.Empty;
        string clientID = string.Empty;
        string employee = string.Empty;
        string station = string.Empty;
        //string unit_id = string.Empty;
        [Display("Client ID", Description: "Enter the Client ID", Order: 0)]
        public string ClientID { get => clientID; set => clientID = value; }
        [Display("Employee", Description: "Enter the employee ID", Order: 1)]
        public string Employee { get => employee; set => employee = value; }
        [Display("Station", Description: "Enter the Station ID", Order: 2)]
        public string Station { get => station; set => station = value; }
        //[Display("ExtraParameter", Description: "Enter the Station ID", Order: 2)]
        //public string Unit_id { get => unit_id; set => unit_id = value; }
        [Display("MES Information Server URL")]
        public string MESInfoUrl { get => mESInfoUrl; set => mESInfoUrl = value; }
        [Display("Result Sever URL", Description: "Enter Result server URL", Order: 3)]
        public string ResultServerURL { get => resultServerURL; set => resultServerURL = value; }
       

        string resultServerURL = string.Empty;


        #region MES_CVS Group

        [Display("CSV Storage Folder", Description: "Enter the CSV Storage Folder", Group: "CSV Informations", Order: 4)]
        [DirectoryPath()]
        public string CsvStorageFolder { get => csvStorageFolder; set => csvStorageFolder = value; }

        string equipmentNumber = string.Empty;
        [Display("Equipment Number", Description: "Enter the Equipment Number",Group:"CSV Informations", Order: 5)]
        public string EquipmentNumber { get => equipmentNumber; set => equipmentNumber = value; }
        string csvStorageFolder = string.Empty;
        
        //string groupName = string.Empty;
        //[Display("Group Name", Description: "Enter the Group Name", Group: "CSV Informations", Order: 6)]
        //public string GroupName { get => groupName; set => groupName = value; }


        string slot = string.Empty;
        [Display("Slot", Description: "Enter the Slot", Group: "CSV Informations", Order: 7)]
        public string Slot { get => slot; set => slot = value; }

        string credentials = string.Empty;
        [Display("Credentials", Description: "Enter the Credentials", Group: "CSV Informations", Order: 8)]
        public string Credentials { get => credentials; set => credentials = value; }

        OperationCodes operationMode = OperationCodes.PRODUCTION;
        [Display("Operation Mode", Description: "Enter the Operation Mode", Group: "CSV Informations", Order: 9)]
        public OperationCodes OperationMode { get => operationMode; set => operationMode = value; }

        string sequenceID = string.Empty;
        [Display("Sequence ID", Description: "Enter the Sequence ID", Group: "CSV Informations", Order: 10)]
        public string SequenceID { get => sequenceID; set => sequenceID = value; }

        string overallDefectCode = string.Empty;
        [Display("Overall Defect Code", Description: "Enter the Overall Defect Code", Group: "CSV Informations", Order: 11)]
        public string OverallDefectCode { get => overallDefectCode; set => overallDefectCode = value; }
        

        string partNumber = string.Empty;
        [Display("Part Number",Description:"Enter the Part Number",Group: "CSV Informations",Order:12)]
        public string PartNumber { get => partNumber; set => partNumber = value; }

        string equipment_ID = string.Empty;
        [Display("Equipment ID",Description:"Enter equipment ID",Group: "CSV Informations", Order: 13)]
        public string Equipment_ID { get => equipment_ID; set => equipment_ID = value; }


        #endregion MES_CVS Group
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        #region Constructors
        public ClsMES()
        {
            ClientID = "p5599dc1uat";
            Employee = "62153666";
            Station = "539";
            //unit_id = "JITSAF1FKMRU00006";




        }
        #endregion Constructors

        #region Methods

        public void OpenMESConnection()
        {

        }

        //public async Task<MesResponseData> GetDataFromMac_ProductID(string serialNumber)
        //{
        //    MesResponseData rep = new MesResponseData();

        //    try
        //    {
        //        if (!isMESConnectionDone)
        //        {
        //            OpenMESConnection();
        //        }
        //        //http://42qconduituat2.42-q.com:18003/conduit //[by Naresh eamil]
        //        string route = $"https://production.42-q.com/mes-api/p5599dc1/units/{serialNumber}/children";
        //        Log.Debug("Command Sent->" + route);
        //        var client = new HttpClient();
        //        var request = new HttpRequestMessage
        //        {
        //            Method = HttpMethod.Get,
        //            RequestUri = new Uri(route),
        //        };

        //        using (var response = await client.SendAsync(request))
        //        {
        //            response.EnsureSuccessStatusCode();
        //            var body = await response.Content.ReadAsStringAsync();
        //            Response2.ResponseItem[] response2 = JsonConvert.DeserializeObject<Response2.ResponseItem[]>(body);
        //            //write an array to deserialize json object


        //            foreach (var resp in response2)
        //            {
        //                if (resp.ref_designator.Equals("MAC2", StringComparison.InvariantCultureIgnoreCase))
        //                {
        //                    rep.MacAddress = resp.component_id;
        //                }
        //                else if (resp.ref_designator.Equals("PRODUCT CODE", StringComparison.InvariantCultureIgnoreCase))
        //                {
        //                    rep.ProductCode = resp.component_id;
        //                }
        //                else if (resp.ref_designator.Equals("SACN 70341", StringComparison.InvariantCultureIgnoreCase))
        //                {
        //                    rep.HSTB_SerialNumber = resp.component_id;
        //                }
        //                else if (resp.ref_designator.Equals("SCAN MRURF PCBA", StringComparison.InvariantCultureIgnoreCase))
        //                {
        //                    rep.RFFE_SerialNumber = resp.component_id;
        //                }
        //            }
        //        }
        //        Log.Info("MES -> MAC :" + rep.MacAddress + " Product Code :" + rep.ProductCode);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //    return rep;
        //}

    
        public async Task<ComponentData> GetMesInformationResponse(string serialNumberByUser)
        {           
            ComponentData componentData = new ComponentData();
            try
            {
                var clientHandler = new HttpClientHandler
                {
                    UseCookies = false,
                };
                var client = new HttpClient(clientHandler);
                string PathWithSerialNo = MESInfoUrl + "/mes-api/p5599dc1/units/" + serialNumberByUser + "/children";
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(PathWithSerialNo),
                    Headers =
                    {
                        { "User-Agent", "insomnia/9.2.0" },
                    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    componentData = JsonConvert.DeserializeObject<ComponentData>(body);
                    Log.Info("Success: " + componentData.success);
                    Log.Info("Message: " + componentData.message);
                    Log.Info("Message body received from MES : {0}", body);
                    foreach (var component in componentData.data)
                    {
                        Log.Info("Serial Key: " + component.serial_key);
                        Log.Info("Serial Number: " + component.serial_number);
                        Log.Info("Part Key: " + component.part_key);
                        Log.Info("Parent Part Number: " + component.parent_part_number);
                        Log.Info("Component ID: " + component.component_id);
                        Log.Info("Component Part Key: " + component.component_part_key);
                        Log.Info("Component Part Number: " + component.component_part_number);
                        Log.Info("Ref Designator: " + component.ref_designator);
                        Log.Info("Removed: " + component.removed);
                        Log.Info("Level: " + component.level);
                        Log.Info("Path: " + component.path);
                        Log.Info("Cycle: " + component.cycle);
                    }             
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in getting MES data from server: " + ex.Message);
            }
            return componentData;
        }
               
        
        
        public async Task<bool> SingleSerialFlowCheck(string serverURL,string client_id= "p5599dc1uat",string employeeID= "62153666",string serialNumber = "JITSAF1LIMRU00006")
        {
            ComponentData componentData = new ComponentData();
            try
            {
                var clientHandler = new HttpClientHandler
                {
                    UseCookies = false,
                };
                var client = new HttpClient(clientHandler);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(serverURL),
                    Headers ={
                        { "User-Agent", "insomnia/9.2.0" } , },
                   // Content = new StringContent("{\r\n\"version\": \"1.0\",\r\n\"source\": {\r\n\"client_id\": \""+ client_id + "\",\r\n\"employee\": \""+ employeeID + "\",\r\n\"password\": \"\",\r\n\"workstation\": {\r\n\"type\": \"Device\",\r\n\"station\": \"903\"\r\n}\r\n},\r\n\"refresh_unit\": true,\r\n\"token\": \"\",\r\n\"keep_alive\": false,\r\n\"single_transaction\": false,\r\n\"options\": {\r\n\"skip_data\": [\r\n\"defects\",\r\n\"comments\",\r\n\"components\",\r\n\"attributes\"\r\n]\r\n},\r\n\"transactions\": [\r\n{\r\n\"unit\": {\r\n\"unit_id\": \""+serialNumber+"\",\r\n\"part_number\": \"\",\r\n\"revision\": }\r\n}\r\n]\r\n}")
                
                   // Content = new StringContent("{\r\n\"version\": \"1.0\",\r\n\"source\": {\r\n\"client_id\": \"p5599dc1uat\",\r\n\"employee\": \"62153666\",\r\n\"password\": \"\",\r\n\"workstation\": {\r\n\"type\": \"Device\",\r\n\"station\": \" 539 \"\r\n}\r\n},\r\n\"refresh_unit\": true,\r\n\"token\": \"\",\r\n\"keep_alive\": false,\r\n\"single_transaction\": false,\r\n\"options\": {\r\n\"skip_data\": [\r\n\"defects\",\r\n\" comments\",\r\n\"components\",\r\n\"attributes\"\r\n]\r\n},\r\n\"transactions\": [\r\n{\r\n\"unit\": {\r\n\"unit_id\": \"JITSAF1FKMRU00006\",\r\n\"part_number\": \"\",\r\n\"revision\": \"\"\r\n}\r\n}\r\n]\r\n}")
                    Content = new StringContent("{\r\n\"version\": \"1.0\",\r\n\"source\": {\r\n\"client_id\": \""+ client_id + "\",\r\n\"employee\": \"\"+ employeeID + \"\",\r\n\"password\": \"\",\r\n\"workstation\": {\r\n\"type\": \"Device\",\r\n\"station\": \" 539 \"\r\n}\r\n},\r\n\"refresh_unit\": true,\r\n\"token\": \"\",\r\n\"keep_alive\": false,\r\n\"single_transaction\": false,\r\n\"options\": {\r\n\"skip_data\": [\r\n\"defects\",\r\n\" comments\",\r\n\"components\",\r\n\"attributes\"\r\n]\r\n},\r\n\"transactions\": [\r\n{\r\n\"unit\": {\r\n\"unit_id\": \""+serialNumber+"\",\r\n\"part_number\": \"\",\r\n\"revision\": \"\"\r\n}\r\n}\r\n]\r\n}")


                };

                /*{
"version": "1.0",
"source": {
"client_id": "p5599dc1uat",
"employee": "62153666",
"password": "",
"workstation": {
"type": "Device",
"station": " 539 "
}
},
"refresh_unit": true,
"token": "",
"keep_alive": false,
"single_transaction": false,
"options": {
"skip_data": [
"defects",
" comments",
"components",
"attributes"
]
},
"transactions": [
{
"unit": {
"unit_id": "JITSAF1FKMRU00006",
"part_number": "",
"revision": ""
}
}
]
}*/
                ///Client ID, Employee, Station, Unit_id are the parameters to be passed in the request body
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Log.Info(body);
                    //body status ok return true else false
                }
            }
            catch (Exception)
            {

                throw;
            }
            return true; // return true if EnsureSuccessStatusCode(); is success else return false.
            /*
             {
                "success": true,
                "message": "",
                "data": [
                    {
                        "serial_key": 2095440,
                        "serial_number": "JITSAF1LIMRU00006",
                        "part_key": 375,
                        "parent_part_number": "LFIRIL051-7470089-A_1.1",
                        "component_id": "A0:73:FC:00:BF:9A",
                        "component_part_key": 0,
                        "ref_designator": "MAC2",
                        "removed": 0,
                        "level": -1,
                        "path": "{A0:73:FC:00:BF:9A}",
                        "cycle": false
                    },
                    {
                        "serial_key": 2095440,
                        "serial_number": "JITSAF1LIMRU00006",
                        "part_key": 375,
                        "parent_part_number": "LFIRIL051-7470089-A_1.1",
                        "component_id": "JITSAF1MRU01",
                        "component_part_key": 0,
                        "ref_designator": "PRODUCT CODE",
                        "removed": 0,
                        "level": -1,
                        "path": "{JITSAF1MRU01}",
                        "cycle": false
                    },
                    {
                        "serial_key": 2095440,
                        "serial_number": "JITSAF1LIMRU00006",
                        "part_key": 375,
                        "parent_part_number": "LFIRIL051-7470089-A_1.1",
                        "component_id": "MRRFBD22480003",
                        "component_part_key": 371,
                        "component_part_number": "LFIRIL002-7470340-B_1.0-SA",
                        "ref_designator": "SCAN 70340",
                        "removed": 0,
                        "level": -1,
                        "path": "{MRRFBD22480003}",
                        "cycle": false
                    },
                    {
                        "serial_key": 2095440,
                        "serial_number": "JITSAF1LIMRU00006",
                        "part_key": 375,
                        "parent_part_number": "LFIRIL051-7470089-A_1.1",
                        "component_id": "JITSAMRUIIHSB00022",
                        "component_part_key": 355,
                        "component_part_number": "LFIRIL002-7470341-B_1.4",
                        "ref_designator": "SACN 70341",
                        "removed": 0,
                        "level": -1,
                        "path": "{JITSAMRUIIHSB00022}",
                        "cycle": false
                    },
                    {
                        "serial_key": 2084161,
                        "serial_number": "MRRFBD22480003",
                        "part_key": 371,
                        "parent_part_number": "LFIRIL002-7470340-B_1.0-SA",
                        "component_id": "JITSAMRUHIMRB00018",
                        "component_part_key": 337,
                        "component_part_number": "LFIRIL002-7470340-B_1.0",
                        "ref_designator": "SCAN MRURF PCBA",
                        "removed": 0,
                        "level": -2,
                        "path": "{MRRFBD22480003,JITSAMRUHIMRB00018}",
                        "cycle": false
                    }
                ]
            }
            */

            // change the above JSON to a JSON array (e.g. [1,2,3])


        }



        #endregion Methods

        #region Implementations
        /// <summary>
        /// Open procedure for the instrument.
        /// </summary>
        public override void Open()
        {

            //MES_CSV.GroupName = GroupName;
            MES_CSV.Equipment_Number = EquipmentNumber;
            MES_CSV.Slot = Slot;
            MES_CSV.Credentials = Credentials;
            MES_CSV.Operation_Mode =Enum.GetName(typeof(OperationCodes),OperationMode);
            MES_CSV.SequenceID = SequenceID;
            MES_CSV.Overall_Defect_Code = OverallDefectCode;

            MES_CSV.MES_CSV_FilePath = CsvStorageFolder;
            MES_CSV.PART_Number = PartNumber;
            MES_CSV.Equipment_ID = Equipment_ID;
            //authenticationEncoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(Username + ":" + Password));
            //BaseURL = "http://" + base.VisaAddress + ":8733";
            //handler = new HttpClientHandler()
            //{
            //    Proxy = new WebProxy(BaseURL),
            //    UseProxy = true,
            //};
            //client = new HttpClient(handler);

            isMESConnectionDone = false;
        }

        /// <summary>
        /// Close procedure for the instrument.
        /// </summary>
        public override void Close()
        {
            //isMESConnectionDone = false;
            //client.Dispose();
            //client = null;
            // TODO:  Shut down the connection to the instrument here.
            base.Close();
        }
        #endregion Implementations

        #region Structures
        public struct ReslutCls
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Unit { get; set; }
            public string Value { get; set; }

        }
        #endregion Structures
    }

    public class ComponentData
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<Component> data { get; set; }
    }

    public class Component
    {
        
        public int serial_key { get; set; }
        public string serial_number { get; set; }
        public int part_key { get; set; }
        public string parent_part_number { get; set; }
        public string component_id { get; set; }
        public int component_part_key { get; set; }
        public string component_part_number { get; set; }
        public string ref_designator { get; set; }
        public int removed { get; set; }
        public int level { get; set; }
        public string path { get; set; }
        public bool cycle { get; set; }
    }
    public enum OperationCodes
    {
        PRODUCTION,ENGINEERING,CALIBRATION,SKIPPING,TESTING
    }
}




