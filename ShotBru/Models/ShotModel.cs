using System;
using Microsoft.SPOT;

namespace ShotBru.Models
{
    public class ShotModel
    {
        public delegate void DataChangedHandler();
        public event DataChangedHandler DataChanged;

        private int sensorValue;
        private int threshold;
        private bool triggerOnRisingEdge;
        private bool isAutoReset;
        private bool isExecuting;
        private int autoResetDelay;
        private int triggerDelay;
        private bool isTriggered;
        private bool isPaused;
        private int triggerCount;

        public ShotModel()
        {
            sensorValue = 0;
            threshold = 510;
            triggerOnRisingEdge = true;
            isAutoReset = false;
            isExecuting = false;
            autoResetDelay = 5;
            triggerDelay = 0;
            isPaused = true;
        }

        public bool IsExecuting
        {
            get { return isExecuting; }
            set
            {
                if (isExecuting != value)
                {
                    isExecuting = value;
                    RaiseDataChanged();
                }
            }
        }

        public bool IsPaused
        {
            get { return isPaused; }
            set
            {
                if (value != isPaused)
                {
                    isPaused = value;
                    RaiseDataChanged();
                }
            }
        }
            

        public int SensorValue
        {
            get { return sensorValue; }
            set 
            {
                if (value != sensorValue)
                {
                    sensorValue = value;
                    RaiseDataChanged();
                }
            }
        }

        public int Threshold
        {
            get { return threshold; }
            set
            {
                if (value != threshold)
                {
                    threshold = value;

                    if (threshold < 0) { threshold = 0; }
                    if (threshold > 1020) { threshold = 1020; }

                    RaiseDataChanged();
                }
            }
        }
        
        public bool TriggerOnRisingEdge
        {
            get { return triggerOnRisingEdge; }
            set
            {
                if (value != triggerOnRisingEdge)
                {
                    triggerOnRisingEdge = value;
                    RaiseDataChanged();
                }
            }
        }

        public bool IsAutoReset
        {
            get { return isAutoReset; }
            set
            {
                if (value != isAutoReset)
                {
                    isAutoReset = value;
                    RaiseDataChanged();
                }
            }
        }

        public int AutoResetDelay
        {
            get { return autoResetDelay; }
            set
            {
                if (value != autoResetDelay)
                {
                    autoResetDelay = value;
                    RaiseDataChanged();
                }
            }
        }

        public int TriggerDelay
        {
            get { return triggerDelay; }
            set
            {
                if (value != triggerDelay)
                {
                    triggerDelay = value;
                    RaiseDataChanged();
                }
            }
        }

        public bool IsTriggered
        {
            get { return isTriggered; }
            set
            {
                if (value != isTriggered)
                {
                    isTriggered = value;
                    RaiseDataChanged();
                }
            }
        }

        public int TriggerCount
        {
            get { return triggerCount; }
            set
            {
                if (value != triggerCount)
                {
                    triggerCount = value;
                    RaiseDataChanged();
                }
            }
        }

        private void RaiseDataChanged()
        {
            if (DataChanged != null)
                DataChanged();
        }
    }

}
