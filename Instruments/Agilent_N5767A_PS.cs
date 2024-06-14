﻿using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace RjioMRU.Instruments
{
    [Display("Agilent_N5767A_PS", Group: "RjioMRU.Instruments", Description: "Insert a description here")]
    public class Agilent_N5767A_PS : ScpiInstrument
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public Agilent_N5767A_PS()
        {
            Name = "Agilent N5767A PS";
            // ToDo: Set default values for properties / settings.
        }

        /// <summary>
        /// Open procedure for the instrument.
        /// </summary>
        public override void Open()
        {

            base.Open();
            // TODO:  Open the connection to the instrument here

            //if (!IdnString.Contains("Instrument ID"))
            //{
            //    Log.Error("This instrument driver does not support the connected instrument.");
            //    throw new ArgumentException("Wrong instrument type.");
            // }

        }

        public void OnPowerSupply(bool WantToOn)
        {
            ScpiCommand("OUTP " + (WantToOn ? "1" : "0"));
        }

        public bool ISPowerSupplyOn()
        {
            return ScpiQuery<bool>("OUTP?");
        }

        /// <summary>
        /// Close procedure for the instrument.
        /// </summary>
        public override void Close()
        {
            // TODO:  Shut down the connection to the instrument here.
            base.Close();
        }
    }
}
