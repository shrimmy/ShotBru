using System;
using Microsoft.SPOT;
using System.Text;
using ShotBru.Models;

namespace ShotBru.Modes.Settings
{
    class TriggerEdgeSetting : Setting
    {
        private string text = "Rising? ${0}";
        private StringBuilder sb = new StringBuilder(16);

        public TriggerEdgeSetting(ShotModel model)
            : base(model)
        { }

        public override string FormatDisplay()
        {
            sb.Clear();
            sb.Append(text);
            sb.Replace("${0}", model.TriggerOnRisingEdge ? "yes" : "no");
            return sb.ToString();
        }

        public override void AdjustUp()
        {
            model.TriggerOnRisingEdge = !model.TriggerOnRisingEdge;
        }

        public override void AdjustDown()
        {
            model.TriggerOnRisingEdge = !model.TriggerOnRisingEdge;
        }
    }
}
