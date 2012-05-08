using System;
using Microsoft.SPOT;
using System.Text;
using ShotBru.Models;

namespace ShotBru.Modes.Settings
{
    class TriggerDelaySetting : Setting
    {
        private string text = "TriggerDelay ${0}s";
        private StringBuilder sb = new StringBuilder(16);

        public TriggerDelaySetting(ShotModel model)
            : base(model)
        { }


        public override string FormatDisplay()
        {
            sb.Clear();
            sb.Append(text);
            sb.Replace("${0}", Utility.ZeroFill(model.TriggerDelay.ToString(), 2));
            return sb.ToString();
        }

        public override void AdjustUp()
        {
            if (model.TriggerDelay >= 60)
                model.TriggerDelay = 0;
            else
                model.TriggerDelay++;
        }

        public override void AdjustDown()
        {
            if (model.TriggerDelay <= 0)
                model.TriggerDelay = 60;
            else
                model.TriggerDelay--;
        }
    }
}
