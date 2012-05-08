using System;
using Microsoft.SPOT;
using System.Text;
using ShotBru.Models;

namespace ShotBru.Modes.Settings
{
    class AutoResetDelaySetting : Setting
    {
        private string text = "ResetDelay ${0}s";
        private StringBuilder sb = new StringBuilder(16);

        public AutoResetDelaySetting(ShotModel model)
            : base(model)
        { }


        public override string FormatDisplay()
        {
            sb.Clear();
            sb.Append(text);
            sb.Replace("${0}", Utility.ZeroFill(model.AutoResetDelay.ToString(), 2));
            return sb.ToString();
        }

        public override void AdjustUp()
        {
            if (model.AutoResetDelay >= 60)
                model.AutoResetDelay = 0;
            else
                model.AutoResetDelay++;
        }

        public override void AdjustDown()
        {
            if (model.AutoResetDelay <= 0)
                model.AutoResetDelay = 60;
            else
                model.AutoResetDelay--;
        }
    }
}
