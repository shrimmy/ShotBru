using System;
using Microsoft.SPOT;
using System.Threading;
using SecretLabs.NETMF.Hardware;
using Microsoft.SPOT.Hardware;

namespace ShotBru
{
    public class KeyPad : IDisposable
    {
        private Timer timer;
        private AnalogInput keyPadInput;
        private Key lastKey;
        private Key currentKey;

        public event KeyPressedEventHandler KeyPressed;

        public KeyPad(Cpu.Pin pin)
        {
            currentKey = Key.None;
            keyPadInput = new AnalogInput(pin);
            timer = new Timer(new TimerCallback(TimerCallback), null, 100, 100);
        }

        public Key CurrentKey { get { return currentKey; } }

        private void TimerCallback(object state)
        {
            // no need to debounce the switch if we are just sampling
            int analogValue = keyPadInput.Read();

            // figure out which key was pressed
            currentKey = GetKeyPressed(analogValue);

            // only raise the event if the key has changed
            if (currentKey != lastKey)
            {
                lastKey = currentKey;
                // dont fire the event if key = none
                if (KeyPressed != null && currentKey != Key.None)
                    KeyPressed(currentKey);
            }
        }

        private Key GetKeyPressed(int analogValue)
        {
            Key keyPressed = Key.None;
            if (analogValue <= 900 && analogValue > 750) { keyPressed = Key.Up; }
            else if (analogValue <= 750 && analogValue > 580) { keyPressed = Key.Right; }
            else if (analogValue <= 580 && analogValue > 480) { keyPressed = Key.Down; }
            else if (analogValue <= 480 && analogValue > 300) { keyPressed = Key.Left; }
            else if (analogValue <= 300 && analogValue > 200) { keyPressed = Key.Select; }
            else if (analogValue <= 200) { keyPressed = Key.Menu; }

            return keyPressed;
        }

        public void Dispose()
        {
            keyPadInput.Dispose();
        }
    }

    public delegate void KeyPressedEventHandler(Key key);

    public enum Key
    {
        None,
        Menu,
        Select,
        Left,
        Right,
        Up,
        Down
    }
}
