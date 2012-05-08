using System;
using Microsoft.SPOT;
using ShotBru.Models;

namespace ShotBru.Modes.Settings
{
    public abstract class Setting
    {
        internal readonly ShotModel model;

        public Setting(ShotModel model)
        {
            this.model = model;
        }

        public abstract string FormatDisplay();

        public abstract void AdjustUp();

        public abstract void AdjustDown();
    }
}
