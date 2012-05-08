using System;
using Microsoft.SPOT;
using System.Text;
using ShotBru.Modes.Settings;
using ShotBru.Models;

namespace ShotBru.Modes.Settings
{
    class ThresholdSetting : Setting
    {
        private string text = "${0} ${1} ${2}  ${3}";
        private StringBuilder sb = new StringBuilder(16);

        public ThresholdSetting(ShotModel model)
            : base(model)
        { }

        public override string FormatDisplay()
        {
            sb.Clear();
            sb.Append(text);
            sb.Replace("${0}", Utility.ZeroFill(model.SensorValue.ToString(), 4));
            sb.Replace("${1}", model.TriggerOnRisingEdge ? ">" : "<");
            sb.Replace("${2}", Utility.ZeroFill(model.Threshold.ToString(), 4));
            sb.Replace("${3}", model.IsTriggered ? "*" : " ");
            return sb.ToString();
        }

        public override void AdjustUp()
        {
            model.Threshold += 10;
        }

        public override void AdjustDown()
        {
            model.Threshold -= 10;
        }
    }
}
