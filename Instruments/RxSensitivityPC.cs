using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Renci.SshNet;
using System.Threading;
using System.Diagnostics;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace RjioODSC.Instruments
{
    [Display("RxSensitivity", Group: "RjioODSC", Description: "RxSensitivity,")]
    public class RxSensitivityPC : Instrument
    {
        #region Settings
        private static SshClient _sshConnection;
        private static ShellStream _shellStream;
        private delegate void UpdateTextCallback(string message);
        static bool WriteCD = false;
        static bool WriteCOmmand = false;
        //  static bool writeLine = false;

        string rxServerAddress = "10.33.131.143";
        string sensitivityServerUsername = "techie";
        string sensitivityServerPassword = "iltwat";

        [Display("Rx Sensitivity IP", "Enter Rx sensitivit PC IP Address", Order: 1)]
        public string RxServerAddress { get => rxServerAddress; set => rxServerAddress = value; }

        [Display("RxPC User Name", "Enter Rx sensitivit PC User name", Order: 5)]
        public string SensitivityServerUsername { get => sensitivityServerUsername; set => sensitivityServerUsername = value; }


        [Display("RxPC Password", "Enter Rx sensitivit PC Password", Order: 10)]
        public string SensitivityServerPassword { get => sensitivityServerPassword; set => sensitivityServerPassword = value; }



        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public RxSensitivityPC()
        {
            Name = "RxSensitivity";
            _sshConnection = new SshClient(RxServerAddress, SensitivityServerUsername, SensitivityServerPassword);


            // ToDo: Set default values for properties / settings.
        }

        /// <summary>
        /// Open procedure for the instrument.
        /// </summary>
        public override void Open()
        {
            try
            {
                _sshConnection.Connect();


            }
            catch (Exception)
            {

                throw;
            }


        }
        public string GetRxSensitivity(string RACCARDIP, int SectorID, int PRBNumber, int portID, double setPower, string logName_SerialNumber)
        {
            WriteCD = false;
            WriteCOmmand = false;
            _shellStream = _sshConnection.CreateShellStream("test", 80, 60, 800, 600, 65536);
            string rxSensitivity = string.Empty;
            var threadStart = new Thread(() => { rxSensitivity = RecvSshData(RACCARDIP, SectorID, PRBNumber, portID, setPower, logName_SerialNumber); });
            //  Thread thread = new Thread(threadStart);
            //threadStart.Start();
            threadStart.IsBackground = true;
            threadStart.Start();
            threadStart.Join();
            return rxSensitivity;
        }

        /// <summary>
        /// Close procedure for the instrument.
        /// </summary>
        public override void Close()
        {
            try
            {
                _sshConnection.Disconnect();
            }
            catch (Exception)
            {

                throw;
            }

           // _sshConnection.Dispose();
            // TODO:  Shut down the connection to the instrument here.
            //  base.Close();
        }


        public string RecvSshData(string RACCARDIP, int SectorID, int PRBNumber, int portID, double setPower, string logName_SerialNumber)
        {
            string rxsensitivity = string.Empty;
            Stopwatch sp = new Stopwatch();
            sp.Reset();
            sp.Start();
            while (true)
            {
                try
                {
                    if (_shellStream != null && _shellStream.DataAvailable)
                    {
                        _shellStream.Flush();
                        string data = _shellStream.Read();
                        //if (writeLine)
                        //{
                        // _shellStream.WriteLine(" ");
                        //   writeLine = false;
                        // }
                        if (!WriteCD)
                        {
                            _shellStream.WriteLine("cd VSG_Testing/");
                            Thread.Sleep(1000);
                            WriteCD = true;
                        }
                        if (!WriteCOmmand)
                        {
                            _shellStream.WriteLine("./TriggerVSGCapture_RCTM1_Offset.sh " + RACCARDIP + " " + SectorID + " " + PRBNumber + " "+portID + " " + setPower + "_" + logName_SerialNumber);
                            Thread.Sleep(1000);
                            WriteCOmmand = true;
                        }
                        Console.WriteLine(data);
                        var splitData = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                        for (int iteration = 0; iteration < splitData.Length; iteration++)
                        {
                            Log.Info(splitData[iteration]);
                            if (splitData[iteration].Contains("Antenna,PRB,MCS,RSSI,SNR,TimingOffset,CRCFail"))

                            {
                               return rxsensitivity = splitData[iteration + 1];
                                
                            }
                        }
                    }
                    if (sp.ElapsedMilliseconds > 120000)
                    {
                        return rxsensitivity = "Timeout";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                Thread.Sleep(200);
            }
        }
    }
}
