using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace RjioMRU.Reporting
{
    [Display("tempResults", Group: "RjioMRU", Description: "Insert a description here")]
    public class tempResults : ResultListener
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public tempResults()
        {
            Name = "MyRes";
            // ToDo: Set default values for properties / settings.
        }

        public override void OnTestPlanRunStart(TestPlanRun planRun)
        {
            //Add handling code for test plan run start.
        }

        public override void OnTestStepRunStart(TestStepRun stepRun)
        {
            //Add handling code for test step run start.
        }

        public override void OnResultPublished(Guid stepRun, ResultTable result)
        {
            // Add handling code for result data.
            OnActivity();
        }

        public override void OnTestStepRunCompleted(TestStepRun stepRun)
        {
            //Add handling code for test step run completed.
        }

        public override void OnTestPlanRunCompleted(TestPlanRun planRun, Stream logStream)
        {
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
    }
}
