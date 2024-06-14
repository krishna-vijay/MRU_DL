using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace RjioMRU
{
    [Display("E6680A", Group: "RjioMRU", Description: "Insert a description here")]
    public class EXM_E6680A : ScpiInstrument
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public EXM_E6680A()
        {
            Name = "EXM";
            // ToDo: Set default values for properties / settings.
        }

        /// <summary>
        /// Open procedure for the instrument.
        /// </summary>
        public override void Open()
        {

            base.IoTimeout = 31000;
            base.Open();
            // TODO:  Open the connection to the instrument here

            //if (!IdnString.Contains("Instrument ID"))
            //{
            //    Log.Error("This instrument driver does not support the connected instrument.");
            //    throw new ArgumentException("Wrong instrument type.");
            // }

        }

        internal List<string> GetWaveformList()
        {
            var waveforms = ScpiQuery("SOUR:RAD:ARB:FCAT?").Split(',').ToList(); ;
            waveforms.RemoveAt(0);
            return waveforms;
        }

        /// <summary>
        /// Close procedure for the instrument.
        /// </summary>
        public override void Close()
        {
            // TODO:  Shut down the connection to the instrument here.
            base.Close();
        }
        public void RecallStateFile(string stateFilePath)
        {
            ScpiCommand(":MMEMory:LOAD:STATe \"" + stateFilePath + "\"");
        }

        public void RecallSCPFile(string ScpFilePath)
        {
            //MMEM:LOAD:EVM:SET ALL,"C:\TEMP\Rjio_MRU file_V2.scp"
            ScpiCommand("MMEM:LOAD:EVM:SET ALL,\"" + ScpFilePath + "\"");
        }

        public void SSBAutoDetect(bool autoDetect)
        {
            ScpiCommand(":EVM:CCAR0:PROF:SSB:AUTO " + (autoDetect ? "ON" : "OFF"));
        }
        public void PDCCHAutoDetect(bool autoDetect)
        {//:EVM:CCAR0:PROF:PDCC:AUTO OFF
            ScpiCommand(":EVM:CCAR0:PROF:PDCC:AUTO " + (autoDetect ? "ON" : "OFF"));
        }

        public void RecallStateFile_SCreen(string stateFilePath)
        {
            ScpiCommand(":MMEMory:LOAD:SCON \"" + stateFilePath + "\"");
        }
        public void RestoreScreenDefaults()
        {
            ScpiCommand(":SYST:DEF SCReen");
        }
        public void TechModeSelect(ModeSelect techMode)
        {
            ScpiCommand(":INSTrument:SELect " + Enum.GetName(typeof(ModeSelect), techMode));
        }
        public void CreateNewScreen()
        {
            ScpiCommand(":INSTrument:SCReen:CREate");
        }
        public void renameScreen(string screenName)
        {
            ScpiCommand(":INSTrument:SCReen:REName \"" + screenName + "\"");
        }
        public void SelectMeasurements(LTEAFDD_MEASUREMENT_Type measurementType)
        {
            ScpiCommand("INIT:" + Enum.GetName(typeof(LTEAFDD_MEASUREMENT_Type), measurementType));
        }

        public void SetFrequency(double frequency)
        {
            ScpiCommand(":CCAR:REF " + frequency);
        }
        public void seqSourceFrequency(double frequency)
        {
            ScpiCommand(":SOUR:FREQ " + frequency);
        }
        public void SetBandwidth(LTE_BANDWIDTH lteBandwidth)
        {
            ScpiCommand(":RADio:STANdard:PRESet " + Enum.GetName(typeof(LTE_BANDWIDTH), lteBandwidth));
        }

        public void SetExternalPowerLoss(double powerLossindB)
        {
            ScpiCommand(":SENSe:CORRection:BTS:RF:GAIN " + powerLossindB.ToString());
        }

        public void SetACPNoiseCorrection(bool On)
        {
            ScpiCommand("SENSE:ACP:CORR:NOIS:AUTO " + (On ? "1" : "0"));
        }
        public void MeasureContinues(bool isContinues)
        {
            ScpiCommand(":INITiate:CONTinuous " + (isContinues ? "1" : "0"));
        }
        public void InitiateMeasurement()
        {
            ScpiQuery(":INITiate:IMM;*OPC?;");
        }

        public string[] measureModulation()
        {
            string returnValue;
            try
            {
                returnValue = ScpiQuery(":FETCh:EVM?");
            }
            catch (Exception)
            {
                returnValue = string.Empty;
                 
            }
            
            return returnValue.Split(',');
        }
        public string[] measureModulationRead()
        {
            string returnValue;
            try
            {
                ScpiCommand(":SENSE:POW:RF:RANGE:OPT IMM");
                returnValue = ScpiQuery(":READ:EVM?");
            }
            catch (Exception)
            {

                returnValue =string.Empty;
            }
             
            // Log.Info("Modulation Values : " + returnValue);
            return returnValue.Split(',');
        }

        public string[] measureACP()
        {
            string returnValue = string.Empty;
            try
            {
                ScpiCommand(":SENSE:POW:RF:RANGE:OPT IMM");
                returnValue = ScpiQuery(":READ:ACP?");
            }
            catch (Exception)
            {
                returnValue = string.Empty;
                 
            }
            
            return returnValue.Split(',');
        }


        public void ARBState(bool putOn)
        {
            ScpiCommand(":SOURce:RADio:ARB:STATe " + (putOn ? "ON" : "OFF"));
        }

        public void SetARBSampleRate(double sampleRate)
        {
            ScpiCommand(":SOURce:RADio:ARB:SCLock:RATE " + sampleRate.ToString());
        }

        public void SetPulseRFBlank(PulseRFBlankEnum marker)
        {
            ScpiCommand(":SOURce:RADio:ARB:MDEStination:PULSe " + Enum.GetName(typeof(PulseRFBlankEnum), marker));
        }
        public void SelectWaveform(string waveform)
        {
            ScpiCommand(":SOURce:RADio:ARB:WAVeform \"" + waveform + "\"");
        }

        public void SetTriggerType(TriggerTypeEnum triggerType)
        {
            ScpiCommand(":SOURce:RADio:ARB:TRIGger:TYPE " + Enum.GetName(typeof(TriggerTypeEnum), triggerType));
        }
        public void SelectInstScreen(string ScreenName)
        {
            ScpiCommand(":INSTrument:SCReen:SELect \"" + ScreenName + "\"");
        }
        public void FreqRefInput(FreqRefInputEnum freqRefInput)
        {
            ScpiCommand(":SENSe:ROSCillator:SOURce:TYPE " + Enum.GetName(typeof(FreqRefInputEnum), freqRefInput));
        }

        public void SetRFPowerRange(double range)
        {
            ScpiCommand(":SENSe:POWer:RF:RANGe " + range.ToString());
        }

        public void TriggerSource(TriggerSourceEnum triggersource)
        {
            ScpiCommand(":TRIG:EVM:SOUR " + Enum.GetName(typeof(TriggerSourceEnum), triggersource));
        }
        
        public void ACPTriggerSource(TriggerSourceEnum triggersource)
        {
            ScpiCommand(":TRIG:ACP:SEQ:SOUR " + Enum.GetName(typeof(TriggerSourceEnum), triggersource));
        }

        //public void SetGeteOffOn(bool putOn)
        //{
        //    ScpiCommand(":SENSe:SWEep:EGATe:STATe " + (putOn ? "ON" : "OFF"));
        //}
        public void SetGateOffOn(bool putOn)
        {
            ScpiCommand(":SENSe:SWEep:EGATe:STATe " + (putOn ? "ON" : "OFF"));
        }

        public void SetGateSource(string GateSource)
        {
            ScpiCommand(":SENSe:SWEep:EGATe:SOURce " + GateSource);
        }

        public void SetGateDelay(double GateDelay)
        {
            ScpiCommand(":SENSe:SWEep:EGATe:DELay " + GateDelay);
        }

        public void SetGateLength(double GateLength)
        {
            ScpiCommand(":SENSe:SWEep:EGATe:LENGth " + GateLength);
        }
        public void SetRFInputPort(int portNumber)
        {
            ScpiCommand(":SENSe:FEED:RF:PORT:INPut RFIO" + portNumber.ToString());
        }
        public void GlobalCenterFrequency(bool On)
        {
            ScpiCommand(":INST:COUP:FREQ:CENT " + (On ? "ALL" : "NONE"));
        }

        public void RBAutoDetect(bool On, int carrierNumber)
        {
            ScpiCommand(":SENS:EVM:CCAR" + carrierNumber + ":PROF:AUTO " + (On ? "ON" : "OFF"));
        }

        public void RxContinuesTrigger(ContinuesTrigger ct)
        {

            string command = string.Empty;
            switch (ct)
            {
                case ContinuesTrigger.Free_Run:
                    command = "FREE";
                    break;
                case ContinuesTrigger.Trigger_Plus_Run:
                    command = "TRIGGER";
                    break;
                case ContinuesTrigger.Reset_Plus_Run:
                    command = "RESET";
                    break;
                default:
                    break;
            }
            ScpiCommand(":SOURCE:RADIO:ARB:TRIGGER:TYPE:CONT:TYPE " + command);
        }

        public void RxTriggerSource(ArbTriggerSource arbTrigger)
        {
            ScpiCommand(":SOURCE:RADIO:ARB:TRIGGER:SOUR " + Enum.GetName(typeof(ArbTriggerSource), arbTrigger));
        }
        public void RxSyncToTriggerSource(bool SysctoSource)
        {
            ScpiCommand(":SOURCE:RADIO:ARB:TRIGGER:SYNC:STATE " + (SysctoSource ? "ON" : "OFF"));
        }
        public void RxARBTrigExtDelay(double externalDelay)
        {
            ScpiCommand(":SOURce:RADio:ARB:TRIGger:SOURce:EXTernal:DELay " + externalDelay.ToString());
            ScpiCommand(":SOUR:RAD:ARB:TRIG:EXT:DEL:STAT ON");
        }
        public void RxExtTrigPolarity(polority trigPolority)
        {
            ScpiCommand(":SOURce:RADio:ARB:TRIGger:SOURce:EXTernal:SLOPe " + Enum.GetName(typeof(polority), trigPolority));
        }

        public void RxARBRms(double rmsValue)
        {
            ScpiCommand(":SOURce:RADio:ARB:RMS " + rmsValue.ToString());
        }
        public void RxOutputPort(RfOutputPorts outputPorts)
        {
            ScpiCommand(":SENSe:FEED:RF:PORT:OUTPut " + Enum.GetName(typeof(RfOutputPorts), outputPorts));
        }
        public void RxOutputOnOFf(bool OutputOn)
        {
            ScpiCommand(":OUTPut:STATe " + (OutputOn ? "ON" : "OFF"));
        }

        public void RxSourcePower(double powerValue)
        {
            ScpiCommand(":SOUR:POW " + powerValue.ToString());
        }

        public void RxFrequency(double rxfrequency)
        {
            ScpiCommand(":SOUR:FREQ " + rxfrequency.ToString());
        }

        public void TxradioDirection(Direction rDirection)
        {
            ScpiCommand(":SENS:RAD:STAN:DIR " + Enum.GetName(typeof(Direction), rDirection));
        }

        public void TxRefLevel(int window, int value)
        {
            ScpiCommand(":DISPLAY:EVM:WIND" + window + ":Y:RLEV " + value.ToString());
        }

        public void ACPRefValue(double value)
        {
            ScpiCommand(":DISP:ACP:VIEW:WIND:TRAC:Y:SCAL:RLEV " + value);
        }

        public void TxScalePerDivision(int window, double value)
        {
            ScpiCommand("DISP:EVM:TRAC" + window + ":Y:PDIV " + value.ToString());
        }
        public void DecodePBCHBitsNone()
        {
            ScpiCommand(":EVM:CCAR0:DEC:PBCH NONE");
        }
        public void DecodePDCCHBitsNone()
        {
            ScpiCommand(":EVM:CCAR0:DEC:PDCC NONE");
        }
        public void DecodePDSCHBitsNone()
        {
            ScpiCommand(":EVM:CCAR0:DEC:PDSC NONE");
        }

        public void SetupSequencer()
        {
            ScpiCommand(":INSTrument:SELect SEQAN;:DISP:LSEQ:VIEW RES;:LSEQ:ACQ1:SET:RAD:STAN NR5G;:LSEQ:ACQ1:SET:RAD:BAND N78;:LSEQ:LIST:SET:CNFR 636666;:SENSe:LSEQuencer:ACQuire1:ASTep1:SETup:MBITmap 257;");
        }
        public void SequenceExpectedPower(double ExpectedPower)
        {
            string command = ":SENSe:LSEQuencer:ACQuire1:LIST:SETup:EPOWer " + ExpectedPower;
            ScpiCommand(command);
        }

        public void SequencePeakPower(double PeakPower)
        {
            string command = ":SENSe:LSEQuencer:ACQuire1:SETup:PPOWer " + PeakPower;
            ScpiCommand(command);
        }

        internal void RecallRegistry(int v)
        {
            ScpiCommand(" *RCL " + v);
        }

        public void TrackEVM()
        {
            ScpiCommand(":EVM:CCAR0:TRAC:MODE RSD;:EVM:CCAR0:TRAC:AMPL 1;:EVM:CCAR0:TRAC:PHAS 1;:EVM:CCAR0:TRAC:TIM 1;:EVM:CCAR0:EQU:TRA RSD;:EVM:CCAR0:EQU:TRA:TBAS MINTerval");
        }
        /*The parameters are:
        <enum> - specifies the Radio Standard for the Acquisition. ->NR5G
        <enum> - specifies the Radio Band for the Acquisition. -> N78
        <enum> - specifies the DeviceType for the Acquisition. ->BTS
        <real> - specifies the Frequency or Channel Number for the Acquisition. The channel number and frequency are combined to one parameter. 
            If the radio band is set to NONE, this value is interpreted as a frequency value in Hz. If the radio band is set to a valid band, this value is interpreted as a channel number. ->channelNumber
        <integer> - specifies the Number of Averages for the Acquisition. ->1
        <ampl> - specifies the peak expected power in dB for the Acquisition. ->PeakPower
        <enum> - used to specify the Instrument Gain for the Acquisition. ->ZERO
        <time> - specifies the Transition Time for the Acquisition. ->1.700000E-03
        <time> - specifies the duration of the Acquisition. ->1.000000E-03
        <enum> - specifies the Input Trigger Type for the Acquisition. -> RFB
        <ampl> - specifies the Input Trigger Level for the Acquisition. ->inputTriggerLevel
        <time> - specifies the Input Trigger Delay for the Acquisition. ->0.000000000E+00
        <enum> - specifies the Acquisition Output Trigger for the Acquisition. ->NONE
        <relative ampl> - used to specify the dB value of Instrument Gain when Instrument Gain is set to LOW. It is Electronic Attenuation in fact.
            If Instrument Gain is not set to LOW, this value is ignored. This value is optional. If it is not set, the default value is used.->inputTriggerLevel
        <enum> - used to specify the step Multiport Adapter Input path selection. ->RFIO0
        <enum> - used to specify the step Multiport Adapter Preamplify On/Off. ->0
        <enum> - specify the Integration type of current acquisition. this is for Span extension and Range extension. ->NORM
        <enum> - specify the acquisition step input path selection. ->RFIO1
        */
        public void SequenceAcquireSetup(string channelNumber = "636666", double PeakPower = 3.400000E+01, int inputPort = 1,double inputTriggerLevel=5)
        {
            string radioStandard = "NR5G";
            string radioband = "N78";
            string DeviceType = "BTS";
            string numberofAverages = "1";
            string instrumentGain = "ZERO";
            string transisionTime = "1.700000E-03";
            string durationofAcq = "1.000000E-03";
            string triggerType = "RFB";
            string triggerDelay = "0.000000000E+00";
            string triggerOutput = "NONE";
            string multiportInputPath = "RFIO"+inputPort;
            string preamp = "0";
            string integrationType = "NORM";
            string acqInputPath= "RFIO" + inputPort;
            ScpiCommand(":SENSe:LSEQuencer:ACQuire1:SETup "+radioStandard+","+radioband+","+DeviceType+"," + channelNumber + ","+numberofAverages+"," + PeakPower.ToString() + "," +
                ""+instrumentGain+","+transisionTime+","+durationofAcq+","+triggerType+","+ inputTriggerLevel .ToString()+ ","+ triggerDelay + ","+triggerOutput+","+
                inputTriggerLevel.ToString()+ ","+multiportInputPath+","+preamp+","+integrationType+","+acqInputPath);
        }

       
        public string[] ReadSequencerPower()
        {
            string ReturnValue = string.Empty;
            try
            {
                ReturnValue = ScpiQuery("read:LSEQuencer?");
            }
            catch (Exception)
            {

                ReturnValue = string.Empty;
            }
           
            return ReturnValue.Split(','); ;//Convert.ToDouble(ReturnValue[13]);
        }

        public void TriggerBurstSelect()
        {
            ScpiCommand(":SENSe:LSEQuencer:ACQuire1:SETup:TRIGger:INPut RFB");
        }
        public void TriggerLevel(double level)
        {
            ScpiCommand(":SENSe:LSEQuencer:ACQuire1:SETup:TRIGger:INPut:LEVel " + level.ToString());
        }


        public void ViewOnlySummary()
        {
            ScpiCommand(":DISPlay:EVM:VIEW:SELect NRES");
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////trials
        /// </summary>
        public string trialGetSeqPower()
        {
            var ReturnValue = ScpiQuery(":INSTrument:SCReen:SELect \"SEQ\";:READ:LSEQuencer?");
            return ReturnValue;//Convert.ToDouble(ReturnValue[13]);
        }

        public string TrailmeasureModulation()
        {
            var returnValue = ScpiQuery(":INSTrument:SCReen:SELect \"EVM\";:READ:EVM?");
            return returnValue;
        }

        public void ACP_GATESource_ABs_Trig_Level(double level)
        {
            ScpiCommand(":TRIG:SEQ:RFB:LEV:ABS " + level);
        }
        #region Enums

        public enum RfOutputPorts
        {
            RFOut, RFIO1, RFIO2, RFIO3, RFIO4, RFHD, RFFD, A1, A2, A3, B1, B2, B3, IFIO1, IFIO2, GEN, TR, RRHhRFHDp, IFOutn, IFHDn, NONE
        }
        public enum polority
        {
            POSitive, NEGative

        }
        public enum ArbTriggerSource
        {
            KEY,
            BUS,
            EXTERNAL1,
            EXTERNAL2,
            PXI
        }
        public enum ContinuesTrigger
        {
            Free_Run,
            Trigger_Plus_Run,
            Reset_Plus_Run
        }
        public enum NR_BANDWIDTH
        {
            B5M, B10M, B15M, B20M, B25M, B30M, B40M, B50M, B60M, B70M, B80M, B90M, B100M, B200M, B400M
        }
        public enum SCSValues
        {
            SCS15K, SCS30K, SCS60K, SCS120K
        }

        public enum EAutoFreqOffset
        {
            OFF, ACRA100K, ACRA15K, ACRA60K, CARA100K, CARA15K, CARA60K
        }

        public enum ModeSelect
        {
            LTEAFDD, LTEATDD, NR5G, SEQAN
        }
        public enum LTEAFDD_MEASUREMENT_Type
        {
            EVM, CEVM, ACP
        }
        public enum ReadFetch
        {
            READ, FETCh
        }
        public enum Direction
        {
            DLINK, ULINK
        }
        public enum LTE_BANDWIDTH
        {
            B1M4, B3M, B5M, B10M, B15M, B20M
        }
        public enum IF_TypeFP
        {
            B10M, B25M, B40M, Wideband
        }
        public enum DMRsConfig
        {
            TYPE1,
            TYPE2
        }
        public enum DMRsMapping
        {
            TYPEA,
            TYPEB
        }
        public enum MCSTable
        {
            TABLe1,
            TABLe2,
            TABLe3
        }
        public enum LO_Mixing
        {
            NORMal,
            ALTernate
        }
        public enum AnalysisStartBoundary
        {
            FRAMe,
            SUB
        }

        public enum Demod_Spectrum
        {
            NORMal,
            INVert
        }

        public enum PulseRFBlankEnum
        {
            NONE,
            M1,
            M2,
            M3,
            M4
        }
        public enum TriggerTypeEnum
        {
            CONTinuous,
            SINGle,
            SADVance
        }

        public enum FreqRefInputEnum
        {
            INTernal,
            EXTernal,
            SENSe,
            PULSe
        }
        public enum TriggerSourceEnum
        {
            EXTernal1, EXTernal2, EXTernal3, IMMediate, LINE, FRAMe, RFBurst, VIDeo, TV, PXI, INTernal
        }
        #endregion Enums

    }
}
