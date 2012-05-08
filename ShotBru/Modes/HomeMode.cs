using System;
using Microsoft.SPOT;
using ShotBru.Models;
using ShotBru.Modes.Settings;

namespace ShotBru.Modes
{
    class HomeMode : Mode
    {
        public HomeMode(ShotModel model)
            : base(model, new Setting[] { })
        {
            line1 = "Shot Bru (V1.0)";
            line2 = " >> press menu";
        }
    }
}
