using System;
using Microsoft.SPOT;
using System.Threading;
using SecretLabs.NETMF.Hardware;
using Microsoft.SPOT.Hardware;

namespace ShotBru
{
    public class KeyPad
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

        private void TimerCallback(object state)
        {
            int analogValue = 0;
            // read in loop to debounce switch
            for (int i = 0; i < 3; i++)
            {
                analogValue = keyPadInput.Read();
                if (i < 3) Thread.Sleep(10);
            }

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
            if (analogValue < 900)
            {
                if (analogValue < 750)
                {
                    if (analogValue < 580)
                    {
                        if (analogValue < 480)
                        {
                            if (analogValue < 300)
                            {
                                if (analogValue < 200)
                                {
                                    keyPressed = Key.Menu;
                                }
                                else { keyPressed = Key.Select; }
                            }
                            else { keyPressed = Key.Left; }
                        }
                        else { keyPressed = Key.Down; }
                    }
                    else { keyPressed = Key.Right; }
                }
                else { keyPressed = Key.Up; }
            }
            return keyPressed;
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
