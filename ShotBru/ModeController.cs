using System;
using Microsoft.SPOT;
using ShotBru.Modes;

namespace ShotBru
{
    public class ModeController
    {
        private readonly Mode[] mode;
        private int currentIndex = 0;

        public ModeController(Mode[] modes)
        {
            if (modes == null)
                throw new ArgumentNullException("modes");
            if (modes.Length == 0)
                throw new ArgumentException("Need at least one mode", "modes");

            this.mode = modes;
        }

        public Mode Current
        {
            get { return mode[currentIndex]; }
        }

        public void ResetCurrent()
        {
            Current.Reset();
        }

        public void Next()
        {
            Current.Hide();
            // TODO: let the mode know that we are navigating away from it

            currentIndex++;
            if (currentIndex >= mode.Length)
                currentIndex = 0;

            // enable the new mode
            Current.Show();
        }
    }
}
