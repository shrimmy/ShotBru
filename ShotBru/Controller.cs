using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using MicroLiquidCrystal;

namespace ShotBru
{
    public class Controller
    {
        #region Local Varaibles
        // variables
        private ShotModel model;
        private Timer displayTimer;

        // pin defenitions
        private Cpu.Pin keyPad_IN = Pins.GPIO_PIN_A0;
        private Cpu.Pin sensor1_Input = Pins.GPIO_PIN_A1;
        //private Cpu.Pin sensor1_Tip = Pins.GPIO_PIN_A2;
        private Cpu.Pin sensor1_Power = Pins.GPIO_PIN_D0;
        private Cpu.Pin camera1_Shutter = Pins.GPIO_PIN_D1;
        private Cpu.Pin lcd_RS = Pins.GPIO_PIN_D2;
        private Cpu.Pin lcd_EN = Pins.GPIO_PIN_D3;
        private Cpu.Pin lcd_D4 = Pins.GPIO_PIN_D4;
        private Cpu.Pin lcd_D5 = Pins.GPIO_PIN_D5;
        private Cpu.Pin lcd_D6 = Pins.GPIO_PIN_D6;
        private Cpu.Pin lcd_D7 = Pins.GPIO_PIN_D7;

        // drivers
        private KeyPad keyPad;
        private Lcd lcd;
        private Camera camera1;
        private Sensor sensor1;
        #endregion

        public void Start()
        {
            Initialize();

            Thread.Sleep(Timeout.Infinite);
        }

        private void Initialize()
        {
            lcd = new Lcd(new GpioLcdTransferProvider(lcd_RS, lcd_EN, lcd_D4, lcd_D5, lcd_D6, lcd_D7));
            lcd.Begin(16, 2);
            DisplayLine("Shot Bru (V1)");
            DisplayLine("Initializing...", 1);
            // wait for 1 second, not necessary
            Thread.Sleep(1000);

            model = new ShotModel
            {
                CurrentRunMode = RunMode.Setup,
                CurrentSensorMode = SensorMode.Interval
            };

            keyPad = new KeyPad(keyPad_IN);
            keyPad.KeyPressed += new KeyPressedEventHandler(keyPad_KeyPressed);

            // setup the display timer to fire every 250mS,
            displayTimer = new Timer(new TimerCallback(displayTimer_Fired), model, 250, 250);

            sensor1 = new Sensor(sensor1_Input, sensor1_Power);
            sensor1.Triggered += new TriggerEventHandler(sensor1_Triggered);
            sensor1.Start();

            camera1 = new Camera(camera1_Shutter);
        }

        /// <summary>
        /// Displays a line on the lcd display. Empty space will be added to the end of the line to remove any previous text
        /// </summary>
        /// <param name="line">Text to display</param>
        /// <param name="row">Optional row number</param>
        private void DisplayLine(string line, int row = 0)
        {
            lcd.SetCursorPosition(0, row);
            lcd.Write(Utility.PadEndWithSpace(line, 16));
        }

        void displayTimer_Fired(object state)
        {
            // debug for now, just show sensor reading, threashold & isTriggered
            DisplayLine("S:" + Utility.Pad(sensor1.Value, 4) + " T:" + Utility.Pad(sensor1.Threshold, 4) + " " + (sensor1.IsTriggered ? "*" : " "), 1);
        }

        void sensor1_Triggered(int value)
        {
            // Test code that will take a photo when the sensor is triggered, pause for 5 seconds & reactivate the sensor
            camera1.TakePhoto();
            DisplayLine("Camera Triggered");
            Thread.Sleep(4000);
            DisplayLine("Activating...");
            Thread.Sleep(1000);
            DisplayLine("Shot Bru (V1)");
            sensor1.Start();
        }

        void keyPad_KeyPressed(Key key)
        {
            if (key == Key.Menu)
            {
                if (model.CurrentRunMode == RunMode.Setup)
                {
                    // select the next sensor mode
                    switch (model.CurrentSensorMode)
                    {
                        case SensorMode.Interval:
                            model.CurrentSensorMode = SensorMode.Laser;
                            break;
                        case SensorMode.Laser:
                            model.CurrentSensorMode = SensorMode.Sound;
                            break;
                        case SensorMode.Sound:
                            model.CurrentSensorMode = SensorMode.Motion;
                            break;
                        case SensorMode.Motion:
                            model.CurrentSensorMode = SensorMode.Interval;
                            break;
                    }
                }
            }
        }
    }
}
