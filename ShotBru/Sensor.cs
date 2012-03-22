using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;
using SecretLabs.NETMF.Hardware;

namespace ShotBru
{
    public class Sensor : IDisposable
    {
        private int threshold;
        private bool isPaused;
        private bool isTriggered;
        private Thread thread;
        private AnalogInput analogInput;
        private OutputPort power;
        private int currentValue;
        private TriggerType triggerType;

        public event TriggerEventHandler Triggered;

        public Sensor(Cpu.Pin analogInputPin, Cpu.Pin powerPin)
        {
            isTriggered = false;
            currentValue = 0;
            isPaused = true;
            threshold = 550;
            triggerType = TriggerType.Above;
            analogInput = new AnalogInput(analogInputPin);
            power = new OutputPort(powerPin, true);
            // power up the sensor
            power.Write(false);
            thread = new Thread(new ThreadStart(ContinuousRead));
            thread.Start();
        }

        public int Value
        {
            get { return currentValue; }
        }

        public int Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }

        public bool IsPaused
        {
            get { return isPaused; }
        }

        public bool IsTriggered
        {
            get { return isTriggered; }
        }

        public TriggerType TriggerType
        {
            get { return triggerType; }
            set { triggerType = value; }
        }

        public void Start()
        {
            isTriggered = false;
            isPaused = false;
        }

        public void Dispose()
        {
            power.Write(true);
            thread.Abort();
            analogInput.Dispose();
            power.Dispose();
        }

        private void ContinuousRead()
        {
            while (true)
            {
                ReadAnalogValue();
                Thread.Sleep(10);
            }
        }

        private void ReadAnalogValue()
        {
            currentValue = analogInput.Read();

            int difference = threshold - currentValue;
            isTriggered = false;
            if (triggerType == ShotBru.TriggerType.Above)
            {
                if (difference < 0)
                    SetTrigger();
            }
            else
            {
                if (difference > 0)
                    SetTrigger();
            }
        }

        private void SetTrigger()
        {
            isTriggered = true;

            // fire the event if not in the paused state
            if (!isPaused)
            {
                isPaused = true;
                if (Triggered != null)
                    Triggered(currentValue);
            }
        }
    }

    public delegate void TriggerEventHandler(int value);
}
