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
        HttpClient client;

        static string BaseURL;
        string authenticationEncoded = string.Empty;
        string username = string.Empty;
        string password = string.Empty;
        [Display("User name", Description: "Enter the username", Order: 0)]
        public string Username { get => username; set => username = value; }
        [Display("Password", Description: "Enter the password", Order: 1)]
        public string Password { get => password; set => password = value; }

        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        #region Constructors
        public ClsMES()
        {

            // ToDo: Set default values for properties / settings.
        }
        #endregion Constructors

        #region Methods

        public void OpenMESConnection()
        {
            authenticationEncoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(Username + ":" + Password));
            BaseURL = "http://42qconduituat2.42-q.com:18003";// "http://" + base.VisaAddress + ":8733";
            handler = new HttpClientHandler()
            {
                Proxy = new WebProxy(BaseURL),
                UseProxy = true,
            };
            client = new HttpClient(handler);
            isMESConnectionDone = true;

        }

        public async Task<MesResponseData> GetDataFrom(string serialNumber)
        {
            MesResponseData rep = new MesResponseData();

            try
            {
                if (!isMESConnectionDone)
                {
                    OpenMESConnection();
                }
                string route = $"https://production.42-q.com/mes-api/p5599dc1/units/{serialNumber}/children";
                Log.Debug("Command Sent->" + route);
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(route),
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    MES.Response2.ResponseItem[] response2 = JsonConvert.DeserializeObject<MES.Response2.ResponseItem[]>(body);

                    foreach (var resp in response2)
                    {
                        if (resp.ref_designator.Equals("MAC2", StringComparison.InvariantCultureIgnoreCase))
                        {
                            rep.MacAddress = resp.component_id;
                        }
                        else if (resp.ref_designator.Equals("PRODUCT CODE", StringComparison.InvariantCultureIgnoreCase))
                        {
                            rep.ProductCode = resp.component_id;
                        }
                        else if (resp.ref_designator.Equals("SACN 70341", StringComparison.InvariantCultureIgnoreCase))
                        {
                            rep.HSTB = resp.component_id;
                        }
                        else if (resp.ref_designator.Equals("SCAN MRURF PCBA", StringComparison.InvariantCultureIgnoreCase))
                        {
                            rep.RFFE = resp.component_id;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return rep;
        }

        public async Task<string> GetSerialNumber()
        {
            try
            {
                if (!isMESConnectionDone)
                {
                    OpenMESConnection();
                }
                string SerialNumber = string.Empty;
                string route = "/conduit";
                Log.Debug("Command Sent->" + route);
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(BaseURL + route),
                    Headers =
    {
        { "User-Agent", "insomnia/9.2.0" },
    },
                    Content = new StringContent("{\n\"version\": \"1.0\",\n\"source\": {\n\"client_id\": \"p5599dc1uat\",\n\"employee\": \"62153666\",\n\"password\": \"\",\n\"workstation\": {\n\"type\": \"Device\",\n\"station\": \" 539 \"\n}\n},\n\"refresh_unit\": true,\n\"token\": \"\",\n\"keep_alive\": false,\n\"single_transaction\": false,\n\"options\": {\n\"skip_data\": [\n\"defects\",\n\" comments\",\n\"components\",\n\"attributes\"\n]\n},\n\"transactions\": [\n{\n\"unit\": {\n\"unit_id\": \"JITSAF1FKMRU00006\",\n\"part_number\": \"\",\n\"revision\": \"\"\n}\n}\n]\n}")
                    {
                        Headers =
        {
            ContentType = new MediaTypeHeaderValue("application/json")
        }
                    }
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    SerialNumber = body;
                }

                return SerialNumber;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<string> GetSerialNumber1()
        {
            try
            {
                if (!isMESConnectionDone)
                {
                    OpenMESConnection();
                }
                string SerialNumber = string.Empty;
                string route = "/conduit";
                Log.Debug("Command Sent->" + route);
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(BaseURL + route),
                    Headers =
                    {
                        { "User-Agent", "insomnia/9.2.0" },
                    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    SerialNumber = body;
                }

                return SerialNumber;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> GetTestRunStatus(string testBenchID, int channelID)
        {
            try
            {
                if (!isMESConnectionDone)
                {
                    OpenMESConnection();
                }
                bool testrunningStatus = false;
                string route = "/Design_time_addresses/ESD2RemoteServiceActive/testbench/" + testBenchID + "/channel/" + channelID + "/test-finished";
                Log.Debug("Command Sent->" + route);
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(BaseURL + route),
                    Headers = { { "Authorization", "Basic " + authenticationEncoded + "" }, },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    testrunningStatus = Convert.ToBoolean(body);
                }

                return testrunningStatus;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GetChannelState()
        {
            try
            {
                if (!isMESConnectionDone)
                {
                    OpenMESConnection();
                }
                string testrunningStatus = string.Empty;
                string route = "/Design_time_addresses/ESD2RemoteServiceActive/channel";
                Log.Debug("Command Sent->" + route);
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(BaseURL + route),
                    Headers =
    {
         { "Authorization", "Basic "+authenticationEncoded+"" },
    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    testrunningStatus = body;

                }

                return testrunningStatus;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<string> GetGlobelVariables(string route)
        {
            Log.Debug("Command Sent->" + route);
            try
            {
                if (!isMESConnectionDone)
                {
                    OpenMESConnection();
                }

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(BaseURL + route),
                    Headers = { { "Authorization", "Basic " + authenticationEncoded }, },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    return body;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        //not required as per Teja
        //        public async Task<bool> SleepCurrentMeasureExecuteTest(string route, int delay, string testBenchID, int channelID, string esdFileName, string esdFilePath, string batteryFileName, string batteryFilePath, string globalVar1, string globalVar2)
        //        {
        //            Log.Debug("Coomand sent->" + route);
        //            try
        //            {
        //                if (!isESDConnectionDone)
        //                {
        //                    OpenESDConnection();
        //                }
        //                var request = new HttpRequestMessage
        //                {
        //                    Method = HttpMethod.Post,
        //                    RequestUri = new Uri(BaseURL + route),
        //                    Headers =
        //    {
        //        { "Authorization", "Basic "+authenticationEncoded+"" },
        //    },
        //                    Content = new StringContent("[\n  {\n    \"testbenchId\": \"" + testBenchID + "\",\n    \"channelId\": " + channelID + ", \n    \"files\": [{\n\"localPath\": " +
        //                "\"" + esdFileName + "\",\n\t\t\t\"downloadUrl\": \"" + esdFilePath + "\",\n\"authToken\": \"\"\n},{\n" +
        //                "\"localPath\": \"" + batteryFileName + "\",\n\"downloadUrl\": \"" + batteryFilePath + "\", \n\"authToken\": \"\"\n }],\n\"lockBy\": \"TestUser\",\n" +
        //                "\"globalVars\": [{\"name\":\"enable_DIO11_Req\",\"value\":\"" + globalVar1 + "\"}],\n\"reportPath\": \"\", \n \"reportAction\": \"1\", \n" +
        //                " \"reportExportFormat\": -1,\n \"reportInfo\":[{\n \"group\":\"\",\n \"name\":\"\",\n \"value\":\"\"\n }]\n  }\n]")
        //                    {
        //                        Headers =
        //        {
        //            ContentType = new MediaTypeHeaderValue("application/json")
        //        }
        //                    }
        //                };
        //                using (var response = await client.SendAsync(request))
        //                {
        //                    response.EnsureSuccessStatusCode();
        //                    // var body = await response.Content.ReadAsStringAsync();
        //                    return response.IsSuccessStatusCode;
        //                }

        //            }
        //            catch (Exception)
        //            {

        //                throw;
        //            }

        //        }
        //        public async Task<bool> BMSWakeupTest(string route, string testBenchID, int channelID, string esdFileName, string esdFilePath, string batteryFileName, string batteryFilePath, string globalVar1)
        //        {
        //            Log.Debug("Coomand sent->" + route);
        //            try
        //            {
        //                if (!isESDConnectionDone)
        //                {
        //                    OpenESDConnection();
        //                }
        //                var request = new HttpRequestMessage
        //                {
        //                    Method = HttpMethod.Post,
        //                    RequestUri = new Uri(BaseURL + route),
        //                    Headers =
        //    {
        //        { "Authorization", "Basic "+authenticationEncoded+"" },
        //    },
        //                    Content = new StringContent("[\n  {\n    \"testbenchId\": \"" + testBenchID + "\",\n    \"channelId\": " + channelID + ", \n    \"files\": [{\n\"localPath\": " +
        //                "\"" + esdFileName + "\",\n\t\t\t\"downloadUrl\": \"" + esdFilePath + "\",\n\"authToken\": \"\"\n},{\n" +
        //                "\"localPath\": \"" + batteryFileName + "\",\n\"downloadUrl\": \"" + batteryFilePath + "\", \n\"authToken\": \"\"\n }],\n\"lockBy\": \"TestUser\",\n" +
        //                "\"globalVars\": [{\"name\":\"enable_DIO11_Req\",\"value\":\"" + globalVar1 + "\"}],\n\"reportPath\": \"\", \n \"reportAction\": \"1\", \n" +
        //                " \"reportExportFormat\": -1,\n \"reportInfo\":[{\n \"group\":\"\",\n \"name\":\"\",\n \"value\":\"\"\n }]\n  }\n]")
        //                    {
        //                        Headers =
        //        {
        //            ContentType = new MediaTypeHeaderValue("application/json")
        //        }
        //                    }
        //                };
        //                using (var response = await client.SendAsync(request))
        //                {
        //                    response.EnsureSuccessStatusCode();
        //                    // var body = await response.Content.ReadAsStringAsync();
        //                    return response.IsSuccessStatusCode;
        //                }
        //            }
        //            catch (Exception)
        //            {

        //                throw;
        //            }
        //        }

        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <param name="route"></param>
        //        /// <param name="testBenchID"></param>
        //        /// <param name="channelID"></param>
        //        /// <param name="esdFileName"></param>
        //        /// <param name="esdFilePath"></param>
        //        /// <param name="batteryFileName"></param>
        //        /// <param name="batteryFilePath"></param>
        //        /// <returns></returns>
        //        /* var client = new HttpClient();
        //        var request = new HttpRequestMessage
        //        {
        //            Method = HttpMethod.Post,
        //            RequestUri = new Uri("http://10.14.7.100:8733/Design_time_addresses/ESD2RemoteServiceActive/TestExecution"),
        //            Headers =
        //    {
        //        { "Authorization", "Basic QWN0aXZlOm1lbWw=" },
        //    },
        //            Content = new StringContent("[\n  {\n    \"testbenchId\": \"DE58B10194\",\n    \"channelId\": 1, \n    \"files\": [{\n                                             \"localPath\": \"test1.esd\",\n\t\t\t\"downloadUrl\": \"C:\\\\Users\\\\administrator\\\\Documents\\\\Scienlab\\\\ESD\\\\TestDocuments\\\\test1.esd\",\n                                             \"authToken\": \"\"\n                              },{\n                                             \"localPath\": \"MEML-PARI_BATTERY.battery\",\n                                             \"downloadUrl\": \"C:\\\\Users\\\\administrator\\\\Documents\\\\Scienlab\\\\ESD\\\\Batteries\\\\MEML-PARI_BATTERY.battery\",   \n                                             \"authToken\": \"\"\n                              }],\n                              \"lockBy\": \"TestUser\",\n                              \"globalVars\": [ ],\n                              \"reportPath\": \"\", \n                              \"reportAction\": \"1\", \n                              \"reportExportFormat\": -1,\n                              \"reportInfo\":[{\n                                             \"group\":\"\",\n                                             \"name\":\"\",\n                                             \"value\":\"\"\n                              }]\n  }\n]")
        //            {
        //                Headers =
        //        {
        //            ContentType = new MediaTypeHeaderValue("application/json")
        //        }
        //            }
        //        };
        //using (var response = await client.SendAsync(request))
        //{
        //    response.EnsureSuccessStatusCode();
        //    var body = await response.Content.ReadAsStringAsync();
        //    Console.WriteLine(body);
        //}*/

        public async Task<bool> TestExecuteWithoutGlobalVariables(string route, string testBenchID, int channelID, string esdFileName, string esdFilePath, string batteryFileName, string batteryFilePath)
        {
            Log.Debug("Coomand sent->" + route);
            try
            {
                if (!isMESConnectionDone)
                {
                    OpenMESConnection();
                }
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(BaseURL + route),
                    Headers = { { "Authorization", "Basic " + authenticationEncoded + "" }, },
                    Content = new StringContent("[\n  {\n    \"testbenchId\": \"" + testBenchID + "\",\n    \"channelId\": " + channelID + ", \n    \"files\": [{\n\"localPath\": " +
                "\"" + esdFileName + "\",\n\t\t\t\"downloadUrl\": \"" + esdFilePath.Replace(@"\", @"\\") + "\",\n\"authToken\": \"\"\n},{\n" + "\"localPath\": \"" + batteryFileName + "\",\n\"downloadUrl\": \"" + batteryFilePath.Replace(@"\", @"\\") + "\", \n\"authToken\": \"\"\n }],\n\"lockBy\": \"TestUser\",\n" +
                "\"globalVars\": \"\",\n\"reportPath\": \"\", \n \"reportAction\": \"1\", \n" + " \"reportExportFormat\": -1,\n \"reportInfo\":[{\n \"group\":\"\",\n \"name\":\"\",\n \"value\":\"\"\n }]\n  }\n]")
                    { Headers = { ContentType = new MediaTypeHeaderValue("application/json") } }

                };
                Log.Debug("Request URI : " + request.RequestUri.ToString());
                Log.Debug("Headers : " + request.Headers.ToString());
                Log.Debug("Content : " + request.Content.ToString());
                using (var response = await client.SendAsync(request))
                {

                    response.EnsureSuccessStatusCode();

                    // var body = await response.Content.ReadAsStringAsync();
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception EX)
            {

                Log.Error(EX.Message);
                throw;
            }
        }


        public async Task<bool> TestExecuteWith_1_GlobalVariable(string route, string testBenchID, int channelID, string esdFileName, string esdFilePath, string batteryFileName, string batteryFilePath, string globalVariableName, string globalVar1)
        {
            Log.Debug("Coomand sent->" + route);
            try
            {
                if (!isMESConnectionDone)
                {
                    OpenMESConnection();
                }
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(BaseURL + route),
                    Headers =
    {
        { "Authorization", "Basic "+authenticationEncoded+"" },
    },
                    Content = new StringContent("[\n  {\n    \"testbenchId\": \"" + testBenchID + "\",\n    \"channelId\": " + channelID + ", \n    \"files\": [{\n\"localPath\": " +
                "\"" + esdFileName + "\",\n\t\t\t\"downloadUrl\": \"" + esdFilePath.Replace(@"\", @"\\") + "\",\n\"authToken\": \"\"\n},{\n" +
                "\"localPath\": \"" + batteryFileName + "\",\n\"downloadUrl\": \"" + batteryFilePath.Replace(@"\", @"\\") + "\", \n\"authToken\": \"\"\n }],\n\"lockBy\": \"TestUser\",\n" +
                "\"globalVars\": [{\"name\":\"" + globalVariableName + "\",\"value\":\"" + globalVar1 + "\"}],\n\"reportPath\": \"\", \n \"reportAction\": \"1\", \n" +
                " \"reportExportFormat\": -1,\n \"reportInfo\":[{\n \"group\":\"\",\n \"name\":\"\",\n \"value\":\"\"\n }]\n  }\n]")
                    {
                        Headers =
        {
            ContentType = new MediaTypeHeaderValue("application/json")
        }
                    }
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    // var body = await response.Content.ReadAsStringAsync();
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        public async Task<bool> TestAbort(string testBenchID, int channelID)
        {
            // Log.Debug("Coomand sent->" + route);
            try
            {
                if (!isMESConnectionDone)
                {
                    OpenMESConnection();
                }
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(BaseURL + "testbench/" + testBenchID + "/channel/" + channelID + "/control"),
                    Headers =
    {
        { "Authorization", "Basic "+authenticationEncoded+"" },
    },
                    Content = new StringContent("[\n  {\n    \"controlCommand\": \"" + 3 + "\" }\n]")
                    {
                        Headers =
        {
            ContentType = new MediaTypeHeaderValue("application/json")
        }
                    }
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    // var body = await response.Content.ReadAsStringAsync();
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }




        public async Task<bool> T15Recycle(string testBenchID, int channelID)
        {
            try
            {
                if (!isMESConnectionDone)
                {
                    OpenMESConnection();
                }
                string route = "/Design_time_addresses/ESD2RemoteServiceActive/TestExecution";
                string esdFileName = "ESD_TEST_STEP_7.esd";
                string esdFilePath = "C:\\\\Users\\\\vijkrish.KEYSIGHT\\\\Documents\\\\Scienlab\\\\ESD\\\\TestDocuments\\\\ESD_TEST_STEP_7.esd";
                string batteryFileName = "MEML_S210.battery";
                string batteryFilePath = "C:\\\\Users\\\\administrator\\\\Documents\\\\Scienlab\\\\ESD\\\\Batteries\\\\MEML_S210.battery";
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(BaseURL + route),
                    Headers =
    {
        { "Authorization", "Basic "+authenticationEncoded+"" },
    },
                    Content = new StringContent("[\n  {\n    \"testbenchId\": \"" + testBenchID + "\",\n    \"channelId\": " + channelID + ", \n    \"files\": [{\n\"localPath\": " +
                    "\"" + esdFileName + "\",\n\t\t\t\"downloadUrl\": \"" + esdFilePath + "\",\n\"authToken\": \"\"\n},{\n" +
                    "\"localPath\": \"" + batteryFileName + "\",\n\"downloadUrl\": \"" + batteryFilePath + "\", \n\"authToken\": \"\"\n }],\n\"lockBy\": \"TestUser\",\n" +
                    "\"globalVars\": \"\",\n\"reportPath\": \"\", \n \"reportAction\": \"1\", \n" +
                    " \"reportExportFormat\": -1,\n \"reportInfo\":[{\n \"group\":\"\",\n \"name\":\"\",\n \"value\":\"\"\n }]\n  }\n]")
                    {
                        Headers =
        {
            ContentType = new MediaTypeHeaderValue("application/json")
        }
                    }
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    // var body = await response.Content.ReadAsStringAsync();
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        public async Task<string> GetVersion(string route)
        {
            Log.Debug("Coomand sent->" + route);
            try
            {
                if (!isMESConnectionDone)
                {
                    OpenMESConnection();
                }

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(BaseURL + route),
                    Headers = { { "Authorization", "Basic " + authenticationEncoded }, },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    return body;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// possible combinations of status catagories and name
        /// Idle,idle
        /// Error,Deactivation,
        /// Testing  RunSequence
        /// Processing UploadData
        /// Suspended Suspend
        /// Suspended WaitContinue
        /// Processing FinishDataCollection
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetESDSTatus()
        {
            if (!isMESConnectionDone)
            {
                OpenMESConnection();
            }
            string route = "/Design_time_addresses/ESD2RemoteServiceActive/channel";
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(BaseURL + route),
                Headers =
    {
        { "Authorization", "Basic "+ authenticationEncoded },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                return body;
            }
        }

        #endregion Methods

        #region Implementations
        /// <summary>
        /// Open procedure for the instrument.
        /// </summary>
        public override void Open()
        {

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
            isMESConnectionDone = false;
            client.Dispose();
            client = null;
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
}
