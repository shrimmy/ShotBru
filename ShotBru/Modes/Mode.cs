using System;
using Microsoft.SPOT;
using ShotBru.Modes.Settings;
using ShotBru.Models;

namespace ShotBru.Modes
{
    public abstract class Mode
    {
        internal readonly ShotModel model;
        internal bool isEnabled = false;
        internal string line1 = "";
        internal string line2 = "";
        private readonly Setting[] settings;
        private int currentSetting;

        public Mode(ShotModel model, Setting[] settings)
        {
            //if (settings == null || settings.Length == 0)
            //    throw new ArgumentNullException("settings");

            this.model = model;
            this.settings = settings;
            currentSetting = 0;
        }

        public string Line1 { get { return line1; } }
        public string Line2 
        { 
            get 
            {
                if (!model.IsExecuting)
                {
                    if (settings != null && settings.Length > 0)
                    {
                        return settings[currentSetting].FormatDisplay();
                    }
                }
                return line2; 
            } 
        }

        public virtual void Show()
        {
            model.DataChanged += new ShotModel.DataChangedHandler(OnDataChanged);
        }

        public virtual void Hide()
        {
            model.DataChanged -= new ShotModel.DataChangedHandler(OnDataChanged);
        }

        protected virtual void OnDataChanged()
        {
            //if (settings != null && settings.Length > 0)
            //{
            //    if (!model.IsExecuting)
            //    {
            //        line2 = settings[currentSetting].FormatDisplay();
            //    }
            //}
        }

        public virtual void RouteKeyPressed(Key key)
        {
            Debug.Print(line1.Trim() + ": Key(" + key + ")");
            if (key == Key.Select)
            {
                //line2 = "";
                model.TriggerCount = 0;
                model.IsPaused = false;
                model.IsExecuting = true;
            }

            if (settings != null && settings.Length > 0)
            {
                // inrement or decrement setting
                if (key == Key.Up || key == Key.Down)
                {
                    if (key == Key.Up)
                        settings[currentSetting].AdjustUp();
                    else
                        settings[currentSetting].AdjustDown();
                }

                // handle navigation to next or previous setting
                if (key == Key.Left || key == Key.Right)
                {
                    if (key == Key.Right)
                    {
                        currentSetting++;
                        if (currentSetting >= settings.Length) { currentSetting = 0; }
                    }
                    else
                    {
                        currentSetting--;
                        if (currentSetting < 0) { currentSetting = settings.Length - 1; }
                    }

                    // refresh the display
                    OnDataChanged();
                }
            }
        }

        public virtual void Reset()
        {
            // implementing class will need to descide what to do here
            currentSetting = 0;
            OnDataChanged();
        }
    }
}