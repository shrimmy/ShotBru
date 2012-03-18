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
        private Thread triggerThread;
        private AnalogInput analogInput;
        private OutputPort power;
        private int currentValue;

        public event TriggerEventHandler Triggered;

        public Sensor(Cpu.Pin analogInputPin, Cpu.Pin powerPin)
        {
            isTriggered = false;
            currentValue = 0;
            isPaused = true;
            threshold = 510;
            analogInput = new AnalogInput(analogInputPin);
            power = new OutputPort(powerPin, true);
            // power up the sensor
            power.Write(false);
            triggerThread = new Thread(new ThreadStart(ReadAnalogValue));
            triggerThread.Start();
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

        public void Start()
        {
            isTriggered = false;
            isPaused = false;
        }

        public void Dispose()
        {
            power.Write(true);
            triggerThread.Suspend();
            analogInput.Dispose();
            power.Dispose();
        }

        private void ReadAnalogValue()
        {
            while (true)
            {
                currentValue = analogInput.Read();

                if (currentValue > threshold)
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
                else
                {
                    isTriggered = false;
                }

                Thread.Sleep(10); // 10mS loop
            }
        }

    }

    public delegate void TriggerEventHandler(int value);
}
