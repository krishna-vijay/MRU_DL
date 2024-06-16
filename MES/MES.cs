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

namespace RjioMRU.MES
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
           // authenticationEncoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(Username + ":" + Password));
            BaseURL = "http://production.42-q.com:18003";// "http://" + base.VisaAddress + ":8733";
            handler = new HttpClientHandler()
            {
                Proxy = new WebProxy(BaseURL),
                UseProxy = true,
            };
            client = new HttpClient(handler);
            isMESConnectionDone = true;

        }

        public async Task<MesResponseData> GetDataFromMac_ProductID(string serialNumber)
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
                    Response2.ResponseItem[] response2 = JsonConvert.DeserializeObject<Response2.ResponseItem[]>(body);

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
                            rep.HSTB_SerialNumber = resp.component_id;
                        }
                        else if (resp.ref_designator.Equals("SCAN MRURF PCBA", StringComparison.InvariantCultureIgnoreCase))
                        {
                            rep.RFFE_SerialNumber = resp.component_id;
                        }
                    }
                }
                Log.Info("MES -> MAC :" + rep.MacAddress + " Product Code :" + rep.ProductCode);
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
