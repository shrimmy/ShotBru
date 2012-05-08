using System;
using Microsoft.SPOT;
using System.Text;
using ShotBru.Models;

namespace ShotBru.Modes.Settings
{
    class AutoResetSetting : Setting
    {
        private string text = "AutoReset? ${0}";
        private StringBuilder sb = new StringBuilder(16);

        public AutoResetSetting(ShotModel model)
            : base(model)
        { }

        public override string FormatDisplay()
        {
            sb.Clear();
            sb.Append(text);
            sb.Replace("${0}", model.IsAutoReset ? "yes" : "no");
            return sb.ToString();
        }

        public override void AdjustUp()
        {
            model.IsAutoReset = !model.IsAutoReset;
        }

        public override void AdjustDown()
        {
            model.IsAutoReset = !model.IsAutoReset;
        }
    }
}
