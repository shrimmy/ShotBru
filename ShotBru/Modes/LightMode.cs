using System;
using Microsoft.SPOT;
using System.Text;
using ShotBru.Models;
using ShotBru.Modes.Settings;

namespace ShotBru.Modes
{
    class LightMode : Mode
    {
        private string executing = "Armed: ${0}  ${1}";
        private StringBuilder sb;

        public LightMode(ShotModel model)
            : base(model, new Setting[] { new TriggerEdgeSetting(model), new ThresholdSetting(model), new TriggerDelaySetting(model), new AutoResetSetting(model), new AutoResetDelaySetting(model) })
        {
            line1 = "Light Trigger";
            sb = new StringBuilder(16);
        }

        protected override void OnDataChanged()
        {
            if (model.IsExecuting)
            {
                // display running mode
                sb.Clear();
                sb.Append(executing);
                sb.Replace("${0}", Utility.ZeroFill(model.SensorValue.ToString(), 4));
                sb.Replace("${1}", Utility.ZeroFill(model.TriggerCount.ToString(), 3));
                line2 = sb.ToString();
            }
            else
            {
                base.OnDataChanged();
            }
        }

    }
}
